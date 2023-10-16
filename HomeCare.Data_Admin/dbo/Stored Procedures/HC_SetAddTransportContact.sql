CREATE PROCEDURE [dbo].[HC_SetAddTransportContact]                            
@ContactID BIGINT = 0                 
                           
AS                            
BEGIN              
 SELECT 0;                
 SELECT *  FROM TransportContacts WHERE ContactID = @ContactID;    
   
 SELECT *  FROM States;      
   
 SELECT TransportMasterID AS ContactTypeID, Title AS ContactTypes FROM TransportMaster  TM                 
 INNER JOIN TransportMasterType TMT ON TMT.TransportMasterTypeID = TM.ItemType              
 WHERE TMT.Name='Transport Service' AND TM.IsDeleted=0            
     
   SELECT OrganizationID, CompanyName AS OrganizationName FROM Organizations where IsDeleted=0  
                 
END 