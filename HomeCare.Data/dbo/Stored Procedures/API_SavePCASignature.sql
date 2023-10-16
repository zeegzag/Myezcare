CREATE PROCEDURE [dbo].[API_SavePCASignature]        
 @EmployeeVisitID BIGINT,                    
 @ScheduleID BIGINT,      
 @EmployeeSignatureID BIGINT,      
 @PatientSignature NVARCHAR(MAX),  
 @PCACompletedLat DECIMAL(10,7),  
 @PCACompletedLong DECIMAL(10,7),  
 @PCACompletedIPAddress VARCHAR(100)  
AS                              
BEGIN    
  Update EmployeeVisits      
  SET EmployeeSignatureID=@EmployeeSignatureID,PatientSignature=@PatientSignature,IsPCACompleted=1,PCACompletedLat=@PCACompletedLat,PCACompletedLong=@PCACompletedLong,  
  PCACompletedIPAddress=@PCACompletedIPAddress,UpdatedDate=GETUTCDATE()  
  WHERE EmployeeVisitID=@EmployeeVisitID AND ScheduleID=@ScheduleID        
END