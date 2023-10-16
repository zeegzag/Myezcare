--EXEC DeleteDDMaster @SortExpression = 'ItemType', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @ItemType = '1', @IsDeleted = '1', @ListOfIdsInCsv = '4,17,33,35', @IsShowList = 'True', @loggedInID = '1'  
CREATE PROCEDURE [dbo].[DeleteDDMaster]    
 @ItemType VARCHAR(100) = NULL,                      
 @Title nvarchar(2000) = NULL,    
 @Value nvarchar(2000) = NULL,    
 @IsDeleted BIGINT = -1,        
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
   
 Declare @TempUsedDDMasterInPSM int=0;  
 Declare @TempUsedDDMasterInVT int=0;  
  
 IF EXISTS(SELECT 1 FROM PayorServiceCodeMapping PSM Inner Join DDMaster DM on DM.DDMasterID = PSM.CareType AND DM.IsDeleted=0   
   WHERE CareType in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv)))  
 BEGIN  
  SELECT -1 AS ResultId  
 END  
 ELSE IF EXISTS(SELECT 1 FROM VisitTasks VT Inner Join DDMaster DM on DM.DDMasterID = VT.CareType AND DM.IsDeleted=0  
    WHERE CareType in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv)))    
 BEGIN  
  SELECT -1 AS ResultId  
 END  
 ELSE IF EXISTS(SELECT 1 FROM VisitTasks VT Inner Join DDMaster DM on DM.DDMasterID = VT.VisitType AND DM.IsDeleted=0   
    WHERE VisitType in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv)))    
 BEGIN  
  SELECT -1 AS ResultId  
 END  
 ELSE  
 BEGIN  
  UPDATE DDMaster SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,  
  UpdatedDate=GETUTCDATE()   
  WHERE DDMasterID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))  
    
  SELECT 1 AS ResultId  
 END  
  
 IF(@IsShowList=1)                  
 BEGIN                  
  EXEC GetDDMasterList @ItemType,@Title,@Value,@IsDeleted,@SortExpression,@SortType,@FromIndex,@PageSize      
 END    
   
  END                  
END