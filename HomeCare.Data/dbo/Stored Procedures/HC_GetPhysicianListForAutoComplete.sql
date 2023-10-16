
--UpdaredBy:Akhilesh
--UpdaredDate:22 march 2020
--Description:For get PhysicianTypeID

-- exec [HC_GetPhysicianListForAutoComplete] 'S'                  
CREATE PROCEDURE [dbo].[HC_GetPhysicianListForAutoComplete]                        
 @SearchText VARCHAR(MAX),                      
 @PageSize int=10                 
AS                        
BEGIN                        
 select TOP (@PageSize)                
 P.PhysicianID,P.FirstName,P.MiddleName,P.LastName,P.Email,P.Phone,Mobile,P.Address,P.City,P.StateCode,P.ZipCode,P.NPINumber,DM.Title AS PhysicianTypeID     
 from Physicians p    
 INNER JOIN DDMaster DM ON DM.DDMasterID =P.PhysicianTypeID            
 WHERE                         
   (                        
    P.FirstName LIKE '%'+@SearchText+'%' OR                        
    P.MiddleName LIKE '%'+@SearchText+'%'  OR      
    P.LastName LIKE '%'+@SearchText+'%'                     
   ) and P.IsDeleted = 0              
END      
--SELECT * FROM Physicians