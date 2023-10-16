

CREATE PROCEDURE [dbo].[GetPhysicianDetail]      
@PhysicianID BIGINT  
AS      
BEGIN      
      
SELECT * FROM Physicians Where PhysicianID=@PhysicianID  
      
SELECT * FROM States ORDER BY StateName ASC      
  
END

