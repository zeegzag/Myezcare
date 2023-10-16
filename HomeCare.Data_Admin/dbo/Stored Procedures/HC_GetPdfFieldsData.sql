CREATE proc [dbo].[HC_GetPdfFieldsData]
(
	--@AdminDBName NVARCHAR(1000)='',
	@Id INT,
	@TypeId TINYINT,
	@OrganizationID INT
)

AS
BEGIN
	--TypeId { 1= Employee, 2 = Referral
	/*
	exec HC_GetPdfFieldsData 'test',3,1
	*/

	DECLARE @DBName NVARCHAR(1000)

	SELECT 
		@DBName = DBName
	FROM Organizations WHERE OrganizationID = @OrganizationID

	DECLARE @REFERRAL_TYPE TINYINT= 1, @EMP_TYPE TINYINT=2

	--IF( @TypeId = @REFERRAL_TYPE)
	BEGIN
		
			declare @tblMappings as table
			(
				DBFieldName nvarchar(500),
				DBValue nvarchar(500),
				TableName NVARCHAR(500),
				PDFFieldName nvarchar(500)
			)



			DECLARE @Tbl as table
			(
				RowId INT IDENTITY(1,1),
				TblName nvarchar(max)
			)

			insert into @Tbl
			(
				TblName
			)
			select distinct 
				TableName
			from dbo.PDFFieldMappings


			DECLARE @rows int=0,@i int=1,@tblName NVARCHAR(100) = '',@Query NVARCHAR(MAX)=''

			select @rows = count(distinct TblName) from @Tbl


			while(@i<=@rows)
			begin
				SELECT @tblName = TblName from @Tbl where RowId = @i


				declare @tblColumn as table( columnName nvarchar(200))

				insert into @tblColumn
				select DBFieldName from dbo.PDFFieldMappings where TableName = @tblName

				DECLARE @cols NVARCHAR(MAX)='',@colStrType nvarchar(max)=''

				select @cols = STUFF((SELECT ',' + QUOTENAME(DBFieldName) 
								from dbo.PDFFieldMappings
								WHERE TableName = @tblName
								group by DBFieldName
						FOR XML PATH(''), TYPE
						).value('.', 'NVARCHAR(MAX)') 
					,1,1,'')


				select @colStrType = STUFF((SELECT ',' + 'cast('+QUOTENAME(DBFieldName)+' as nvarchar(1000)) as '+DBFieldName 
								from dbo.PDFFieldMappings
								WHERE TableName = @tblName
								group by DBFieldName
						FOR XML PATH(''), TYPE
						).value('.', 'NVARCHAR(MAX)') 
					,1,1,'')

				declare @pkColumn nvarchar(1000)

				DECLARE @CurrentDBName NVARCHAR(1000)= db_name();

				declare @tempQuery nvarchar(max) = 'use '+@DBName+';';

				
				DECLARE @TempQueryPk NVARCHAR(MAX)  

				declare @pkTable as table(PKCol nvarchar(100))

				SET @TempQueryPk ='use '+@DBName+'; SELECT Col.Column_Name 
				from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab
				INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col ON Col.Constraint_Name = Tab.Constraint_Name
					AND Col.Table_Name = Tab.Table_Name
					AND Col.Table_Name = '''+@tblName+'''
				INNER JOIN INFORMATION_SCHEMA.TABLES tbl ON tbl.TABLE_NAME = Tab.TABLE_NAME
				WHERE
					Constraint_Type = ''PRIMARY KEY'''

				--PRINT @TempQueryPk

				INSERT INTO @pkTable
				exec sp_executesql @TempQueryPk

				select @pkColumn =PKCol from @pkTable
									
	
				declare @internalQuery nvarchar(max) = ''


				SELECT @internalQuery= 'SELECT DBFieldName,DBValue FROM
				(
				SELECT '+@colStrType+' FROM '+@DBName+'.dbo.'+@tblName+' where '+@pkColumn+'='+cast(@Id as varchar(100))+'
				) P
				UNPIVOT
				(
					DBValue FOR DBFieldName IN ('+@cols+')
				) pvt';

				print @internalQuery

				INSERT INTO @tblMappings
				(
					DBFieldName,
					DBValue
				)
				exec sp_executesql @internalQuery;

				update @tblMappings
				set TableName = @tblName
				where TableName is null
	

				SET @i = @i+1
			end

			UPDATE mp
			set PDFFieldName = tblMp.PDFFieldName
			from @tblMappings mp 
			inner join dbo.PDFFieldMappings tblMp ON tblMp.DBFieldName = mp.DBFieldName AND tblMp.TableName = mp.TableName

			select * from @tblMappings


	END

END