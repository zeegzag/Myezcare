CREATE PROCEDURE HC_GetPayorFromERA835Response  
@PayorIdentificationNumber NVARCHAR(MAX),  
@PayorSubmissionName NVARCHAR(MAX)  
AS  
BEGIN  
  
SELECT * FROM Payors WHERE EraPayorID= @PayorIdentificationNumber OR  LOWER(PayorSubmissionName)  =  LOWER(@PayorSubmissionName)--  OR  NPINumber=@PayorNPINumber  
  
  
END