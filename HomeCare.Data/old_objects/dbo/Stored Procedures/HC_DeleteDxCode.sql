
    
CREATE PROCEDURE [dbo].[HC_DeleteDxCode]    
 @DXCodeName varchar(100)=NULL,     
 @DXCodeWithoutDot varchar(100)=null,    
 @Description VARCHAR(500)= NULL,    
 @DXCodeShortName VARCHAR(20)= NULL,    
 @EffectiveFrom DATE= NULL,    
 @EffectiveTo DATE= NULL,    
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
   UPDATE DXCodes SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() WHERE DXCodeID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))         
  END    
    
 IF(@IsShowList=1)    
 BEGIN    
  EXEC GetDxCodeList @DXCodeName,@DXCodeWithoutDot,@Description,@DXCodeShortName,@EffectiveFrom,@EffectiveTo,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize    
 END    
END    

