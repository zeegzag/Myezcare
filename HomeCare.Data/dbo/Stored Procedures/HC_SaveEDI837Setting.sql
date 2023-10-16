--   exec [HC_SaveEDI837Setting] 1,'ISA01_AuthorizationInformationQualifier','00'  
CREATE PROCEDURE [dbo].[HC_SaveEDI837Setting]                  
@PayorID bigint ,    
@Key varchar(MAX),  
@Val varchar(MAX)     
AS                                                
BEGIN      
     
BEGIN TRANSACTION trans                                                                    
  
BEGIN TRY    
 DECLARE @query  AS NVARCHAR(MAX);   
 set @query = 'UPDATE PayorEdi837Settings SET '+@Key+'='''+@Val+''' WHERE PayorID='+CONVERT(VARCHAR(100),@PayorID)    
   
 --SELECT  @query    
 exec(@query)    
  
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