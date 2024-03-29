--CreatedBy: Jemin
--CreatedDate: 19 June 2020
--Description: For Get Delete CaptureCall
 CREATE PROCEDURE [dbo].[DeleteCaptureCall]    
 @Id int,          
 @IsDeleted BIGINT = -1,          
 @SortExpression NVARCHAR(100),                      
 @SortType NVARCHAR(10),                    
 @FromIndex INT,                    
 @PageSize INT,    
 @IsShowList bit,                
 @loggedInID BIGINT                
AS                
BEGIN                
                
 IF(@Id > 0)                
 BEGIN                  
   UPDATE CaptureCall SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() WHERE Id =@Id                    
  END                
                
 IF(@IsShowList=1)                
 BEGIN                
	EXEC GetCaptureCallList @IsDeleted = '0', @SortExpression = 'Id', @SortType = 'DESC', @FromIndex = '1', @PageSize = '50'                
 END                
END 
