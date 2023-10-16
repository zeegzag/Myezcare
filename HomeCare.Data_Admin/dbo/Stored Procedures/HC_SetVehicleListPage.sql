﻿-- EXEC [HC_SetVehicleListPage]              
CREATE PROCEDURE [dbo].[HC_SetVehicleListPage]                  
                     
AS                    
BEGIN                    
               
 SELECT TransportMasterID AS ContactTypeID, Title AS ContactTypes FROM TransportMaster  TM                 
 INNER JOIN TransportMasterType TMT ON TMT.TransportMasterTypeID = TM.ItemType              
 WHERE TMT.Name='Transport Service' AND TM.IsDeleted=0      
     
     
 SELECT 0;           
 SELECT 0;                   
 SELECT 0;                                  
END 