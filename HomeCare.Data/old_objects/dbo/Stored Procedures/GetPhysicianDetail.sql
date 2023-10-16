
--CreatedBy: Akhilesh
--CreatedDate: 20 march 20
--Description: For get PhysicianDetail

-- exec [GetPhysicianDetail] 0    
    
CREATE PROCEDURE [dbo].[GetPhysicianDetail]          
@PhysicianID BIGINT=0      
AS          
BEGIN          
          
SELECT * FROM Physicians Where PhysicianID=@PhysicianID      
          
SELECT * FROM States ORDER BY StateName ASC   
  
select DDMasterID AS PhysicianTypeID,Title AS PhysicianTypeName from DDMaster where ItemType=26         
      
END    
GO
