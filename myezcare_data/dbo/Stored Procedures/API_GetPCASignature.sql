--Exec API_GetPCASignature @EmployeeID=170,@EmployeeVisitID=20088      
CREATE PROCEDURE [dbo].[API_GetPCASignature]    
 @EmployeeID BIGINT,      
 @EmployeeVisitID BIGINT        
AS                                        
BEGIN        
      
 --SELECT es.SignaturePath,es.EmployeeSignatureID,ev.PatientSignature FROM EmployeeSignatures es      
 --LEFT JOIN Employees e ON es.EmployeeSignatureId=e.EmployeeSignatureId      
 --LEFT JOIN dbo.EmployeeVisits ev ON ev.EmployeeSignatureID=es.EmployeeSignatureID AND ev.EmployeeVisitID=@EmployeeVisitID      
 --Where e.EmployeeId=@EmployeeID      
      
 --SELECT es.SignaturePath,es.EmployeeSignatureID,ev.PatientSignature,e.MobileNumber,ev.IVRClockOut,ev.ClockOutTime FROM Employees e        
 --LEFT JOIN EmployeeSignatures es ON es.EmployeeSignatureID=e.EmployeeSignatureID      
 --LEFT JOIN dbo.EmployeeVisits ev ON ev.EmployeeSignatureID=es.EmployeeSignatureID AND ev.EmployeeVisitID=@EmployeeVisitID      
 --WHERE e.EmployeeID=@EmployeeID  
  
 SELECT es.SignaturePath,es.EmployeeSignatureID,ev.IVRClockOut,ev.ClockOutTime,ev.PatientSignature,e.MobileNumber From EmployeeVisits ev  
 INNER JOIN Employees e ON e.EmployeeID=@EmployeeID  
 LEFT JOIN EmployeeSignatures es ON es.EmployeeSignatureID=e.EmployeeSignatureID  
 WHERE EmployeeVisitID=@EmployeeVisitID  
  
END
