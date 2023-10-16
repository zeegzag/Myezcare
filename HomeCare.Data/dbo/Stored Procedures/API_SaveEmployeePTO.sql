CREATE PROCEDURE [dbo].[API_SaveEmployeePTO]  
 @EmployeeDayOffID BIGINT,    
 @EmployeeID BIGINT,    
 @StartTime DATETIME = NULL,          
 @EndTime DATETIME = NULL,       
 @DayOffStatus NVARCHAR(50),      
 @DayOffTypeID BIGINT=NULL,    
 @EmployeeComment NVARCHAR(1000),  
 @SystemID VARCHAR(100)    
AS      
BEGIN          
    
 BEGIN TRANSACTION trans                                                                                  
  BEGIN TRY                                                                
                                                       
   IF (@EmployeeDayOffID=0)  
    BEGIN  
     INSERT INTO EmployeeDayOffs(EmployeeID,StartTime,EndTime,DayOffStatus,EmployeeComment,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,DayOffTypeID)  
    VALUES(@EmployeeID,@StartTime,@EndTime,@DayOffStatus,@EmployeeComment,GETDATE(),@EmployeeID,GETDATE(),@EmployeeID,@SystemID,@DayOffTypeID)  
    END        
   ELSE        
    BEGIN        
     UPDATE EmployeeDayOffs SET StartTime=@StartTime,EndTime=@EndTime,DayOffTypeID=@DayOffTypeID,EmployeeComment=@EmployeeComment,UpdatedBy=@EmployeeID,  
    UpdatedDate=GETDATE(),SystemID=@SystemID  WHERE EmployeeDayOffID=@EmployeeDayOffID
    END  
                                                 
 SELECT 1 AS TransactionResultId;                                                                
                                                                
 IF @@TRANCOUNT > 0                                                                                  
 BEGIN                                                                                   
  COMMIT TRANSACTION trans                                                                          
 END                                                                                  
END TRY                                                                   
BEGIN CATCH                                                    
 SELECT -1 AS TransactionResultId,ERROR_MESSAGE() AS ErrorMessage;                                                                                  
       
 IF @@TRANCOUNT > 0                                                                                  
 BEGIN                                                                                   
  ROLLBACK TRANSACTION trans                                                                                   
 END                                                                    
END CATCH             
END