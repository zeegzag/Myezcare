CREATE PROCEDURE [dbo].[HC_SetAddVehiclePage]                                
@VehicleID BIGINT = 0                     
                               
AS                                
BEGIN                  
                   
  SELECT *  FROM Vehicles WHERE VehicleID = @VehicleID;                   
                   
  SELECT tc.ContactID as TransportID, Concat(tc.FirstName, ' ', tc.LastName) AS TransportName, tc.ContactType       
  FROM TransportContacts tc (nolock)              
 LEFT JOIN TransportMaster tm (nolock)  ON tm.TransportMasterID = ContactType               
  WHERE tc.IsDeleted=0 AND tm.Title='Transport Service'            
          
 SELECT TransportMasterID AS VehicleTypeID, Title AS VehicleTypes FROM TransportMaster  TM  (nolock)       
 INNER JOIN TransportMasterType TMT  (nolock) ON TMT.TransportMasterTypeID = TM.ItemType                    
 WHERE TMT.Name='Transport Type' AND TM.IsDeleted=0           
      
 Select EmployeeID, EmployeeName=Live_Beta.dbo.GetGeneralNameFormat(FirstName,LastName)       
 From Live_Beta.dbo.Employees (nolock)       
 Where IsDeleted=0 ORDER BY LastName ASC                        
                     
END 