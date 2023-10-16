CREATE PROCEDURE HC_PayorEnrollment_Update    
@PayorID BIGINT,    
@EnrollType NVARCHAR(100),    
@EnrollStatus NVARCHAR(10),    
@EnrollLogMessage NVARCHAR(MAX),  
  
@EnrollType_ERA NVARCHAR(MAX) = 'era',  
@EnrollType_CMS1500 NVARCHAR(MAX) = '1500'  
    
AS    
BEGIN    
    
IF(@EnrollType = @EnrollType_ERA)    
UPDATE Payors SET ERAEnroll_Status = @EnrollStatus, ERAEnroll_Log = @EnrollLogMessage WHERE PayorID =@PayorID    
    
IF(@EnrollType = @EnrollType_CMS1500)    
UPDATE Payors SET CMS1500Enroll_Status = @EnrollStatus, CMS1500Enroll_Log = @EnrollLogMessage WHERE PayorID =@PayorID    
    
    
END