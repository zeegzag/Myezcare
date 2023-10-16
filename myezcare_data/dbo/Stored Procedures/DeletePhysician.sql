

CREATE PROCEDURE [dbo].[DeletePhysician]    
 @PhysicianName NVARCHAR(100) = NULL,        
 @Email NVARCHAR(50) = NULL,          
 @Address NVARCHAR(100) = NULL,  
 @NPINumber NVARCHAR(20) = null,      
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
   UPDATE Physicians SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() WHERE PhysicianID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))                     
  END                
                
 IF(@IsShowList=1)                
 BEGIN                
  EXEC GetPhysicianList @PhysicianName,@Email,@Address,@NPINumber,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize                
 END                
END 

