DECLARE @DDMasterTypeID INT;
DECLARE @SortOrder INT;
SELECT @DDMasterTypeID = COALESCE(MAX(DDMasterTypeID),0) + 1 FROM lu_DDMasterTypes;
SELECT @SortOrder = COALESCE(MAX(SortOrder),0) + 1 FROM lu_DDMasterTypes;

INSERT INTO [dbo].[lu_DDMasterTypes]
           ([DDMasterTypeID]
           ,[Name]
           ,[SortOrder]
           ,[IsDisplayValue]
           ,[ParentID])
     VALUES
           (@DDMasterTypeID
           ,'Taxonomy Code'
           ,@SortOrder
           ,1
           ,0)