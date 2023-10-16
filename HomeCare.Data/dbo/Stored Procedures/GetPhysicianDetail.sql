-- exec [GetPhysicianDetail] 0         
          
CREATE PROCEDURE [dbo].[GetPhysicianDetail]                
@PhysicianID BIGINT=0            
AS                
BEGIN                
                
--SELECT * FROM Physicians Where PhysicianID=@PhysicianID     
SELECT p.PhysicianID,p.FirstName,p.MiddleName,p.LastName,p.Email,p.Phone,p.Mobile,p.Address,p.City,p.StateCode,p.ZipCode,p.IsDeleted,p.CreatedDate,p.CreatedBy,p.UpdatedDate,    
p.UpdatedBy,p.SystemID,p.NPINumber,p.PhysicianTypeID,'PhysicianTypeName'=dm.Title    
FROM Physicians P    
left join DDMaster dm on dm.DDMasterID=p.PhysicianTypeID     
Where PhysicianID=@PhysicianID         
                
SELECT * FROM States ORDER BY StateName ASC         
        
select DDMasterID AS PhysicianTypeID,Title AS PhysicianTypeName from DDMaster where ItemType=26               
 select 0;           
END