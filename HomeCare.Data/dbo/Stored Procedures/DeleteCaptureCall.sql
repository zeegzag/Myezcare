    
 CREATE PROCEDURE [dbo].[DeleteCaptureCall]        
 @Id NVARCHAR(max),             
 @IsDeleted BIGINT = -1,              
 @SortExpression NVARCHAR(100),                          
 @SortType NVARCHAR(10),                        
 @FromIndex INT,                        
 @PageSize INT,        
 @IsShowList bit,                    
 @loggedInID BIGINT                    
AS                    
BEGIN                    
                    
 IF(LEN(@Id)>0)                    
 BEGIN                      
   UPDATE CaptureCall SET IsDeleted=1 ,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() WHERE Id in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@Id))                       
  END                     
                    
 IF(@IsShowList=1)                    
 BEGIN                    
 EXEC GetCaptureCallList @IsDeleted = '0', @SortExpression = 'Id', @SortType = 'DESC', @FromIndex = '1', @PageSize = '50'                    
 END                    
END 