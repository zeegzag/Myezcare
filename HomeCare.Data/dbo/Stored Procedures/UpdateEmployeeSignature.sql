CREATE PROCEDURE [dbo].[UpdateEmployeeSignature]          
 @EmployeeSignatureID BIGINT,        
 @EmployeeID BIGINT,        
 @SignaturePath NVARCHAR(500),    
 @ImageKey NVARCHAR(20)    
    
AS                            
BEGIN  
 IF @ImageKey='ProfilePic'  
  UPDATE Employees SET ProfileImagePath=@SignaturePath,UpdatedBy=@EmployeeID,UpdatedDate=GETDATE() WHERE EmployeeID=@EmployeeID  
  
IF @ImageKey='Siganture'  
BEGIN  
 IF(@EmployeeSignatureID = 0)      
  BEGIN    
    
  --IF @ImageKey='Siganture'    
   INSERT INTO EmployeeSignatures(SignaturePath,EmployeeID) VALUES (@SignaturePath,@EmployeeID)    
  --IF @ImageKey='ProfilePic'    
  --INSERT INTO EmployeeSignatures(ProfileImagePath,EmployeeID) VALUES (@SignaturePath,@EmployeeID)    
        
   UPDATE Employees SET EmployeeSignatureID=@@IDENTITY WHERE EmployeeID=@EmployeeID        
  END        
 ELSE        
  BEGIN    
 --IF @ImageKey='Siganture'    
  UPDATE EmployeeSignatures SET SignaturePath=@SignaturePath, EmployeeID=@EmployeeID WHERE EmployeeSignatureID=@EmployeeSignatureID    
 --IF @ImageKey='ProfilePic'  
  --UPDATE EmployeeSignatures SET ProfileImagePath=@SignaturePath, EmployeeID=@EmployeeID WHERE EmployeeSignatureID=@EmployeeSignatureID  
  END        
END  
END