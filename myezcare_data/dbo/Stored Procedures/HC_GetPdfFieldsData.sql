CREATE proc [dbo].[HC_GetPdfFieldsData]
(
	@AdminDBName NVARCHAR(1000)='',
	@Id INT,
	@TypeId TINYINT
)
AS
BEGIN
	--TypeId { 1= Employee, 2 = Referral
	/*
	exec HC_GetPdfFieldsData 'test',3,1
	*/

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
			from [Local_Admin].dbo.PDFFieldMappings


			DECLARE @rows int=0,@i int=1,@tblName NVARCHAR(100) = '',@Query NVARCHAR(MAX)=''

			select @rows = count(distinct TblName) from @Tbl


			while(@i<=@rows)
			begin
				SELECT @tblName = TblName from @Tbl where RowId = @i


				declare @tblColumn as table( columnName nvarchar(200))

				insert into @tblColumn
				select DBFieldName from [Local_Admin].dbo.PDFFieldMappings where TableName = @tblName

				DECLARE @cols NVARCHAR(MAX)='',@colStrType nvarchar(max)=''

				select @cols = STUFF((SELECT ',' + QUOTENAME(DBFieldName) 
								from [Local_Admin].dbo.PDFFieldMappings
								WHERE TableName = @tblName
								group by DBFieldName
						FOR XML PATH(''), TYPE
						).value('.', 'NVARCHAR(MAX)') 
					,1,1,'')


				select @colStrType = STUFF((SELECT ',' + 'cast('+QUOTENAME(DBFieldName)+' as nvarchar(1000)) as '+DBFieldName 
								from [Local_Admin].dbo.PDFFieldMappings
								WHERE TableName = @tblName
								group by DBFieldName
						FOR XML PATH(''), TYPE
						).value('.', 'NVARCHAR(MAX)') 
					,1,1,'')

				declare @pkColumn nvarchar(1000)

				SELECT @pkColumn=Col.Column_Name from 
					INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, 
					INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
				WHERE 
					Col.Constraint_Name = Tab.Constraint_Name
					AND Col.Table_Name = Tab.Table_Name
					AND Constraint_Type = 'PRIMARY KEY'
					AND Col.Table_Name = @tblName
	
				declare @internalQuery nvarchar(max) = ''


				SELECT @internalQuery= 'SELECT DBFieldName,DBValue FROM
				(
				SELECT '+@colStrType+' FROM '+@tblName+' where '+@pkColumn+'='+cast(@Id as varchar(100))+'
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
			inner join [Local_Admin].dbo.PDFFieldMappings tblMp ON tblMp.DBFieldName = mp.DBFieldName AND tblMp.TableName = mp.TableName

			select * from @tblMappings


	END

END
