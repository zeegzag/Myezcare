CREATE PROCEDURE [dbo].[DeletePreference]     
 @PreferenceType varchar(100)=NULL,       
 @PreferenceName varchar(1000)=null,      
 @IsDeleted int=-1,      
 @SortExpression NVARCHAR(100),        
 @SortType NVARCHAR(10),      
 @FromIndex INT,      
 @PageSize INT,      
 @ListOfIdsInCsv varchar(300),      
 @IsShowList bit,      
 @loggedInID BIGINT      
AS      
BEGIN      
      
 IF(LEN(@ListOfIdsInCsv)>0)      
 BEGIN        
   UPDATE Preferences SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() WHERE PreferenceID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))           
  END      
      
 IF(@IsShowList=1)      
 BEGIN      
  EXEC GetPreferenceList @PreferenceType,@PreferenceName,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize      
 END      
END 
