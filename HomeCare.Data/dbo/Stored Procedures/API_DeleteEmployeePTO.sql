CREATE PROCEDURE [dbo].[API_DeleteEmployeePTO]  
 @EmployeeDayOffID BIGINT,    
 @EmployeeID BIGINT,
 @SystemID VARCHAR(100)    
AS      
BEGIN          
    
 BEGIN TRANSACTION trans                                                                                  
  BEGIN TRY                                                                
                                              
     UPDATE EmployeeDayOffs SET IsDeleted=1,UpdatedBy=@EmployeeID,UpdatedDate=GETDATE(),SystemID=@SystemID WHERE EmployeeDayOffID=@EmployeeDayOffID

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