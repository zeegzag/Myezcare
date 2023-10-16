CREATE PROCEDURE [dbo].[SaveOrganizationSelectedForms]                    
@UDT_OrganizationFormsTable   UDT_OrganizationFormsTable  READONLY,                      
@OrganizationID BIGINT,                      
@LoggedInUserId BIGINT                
AS                          
BEGIN                      
                     
                    
 -- MARKET VALUES ADD/UPDATE/DELETE  START                    
                    
 UPDATE OFRM SET                    
 OFRM.EBFormID=UM.EBFormID, OFRM.UpdatedDate=GETUTCDATE(), OFRM.UpdatedBy=@LoggedInUserId                    
 FROM @UDT_OrganizationFormsTable UM                    
 LEFT JOIN OrganizationForms OFRM ON OFRM.OrganizationFormID=UM.OrganizationFormID AND OFRM.OrganizationID=@OrganizationID                   
 WHERE OFRM.OrganizationFormID IS NOT NULL                   
                     
 INSERT INTO OrganizationForms(EBFormID,OrganizationID,CreatedDate,UpdatedDate,CreatedBy,UpdatedBy)                    
 SELECT UM.EBFormID,@OrganizationID,GETUTCDATE(), GETUTCDATE(),@LoggedInUserId,@LoggedInUserId                 
 FROM @UDT_OrganizationFormsTable UM                    
 LEFT JOIN OrganizationForms OFRM ON OFRM.OrganizationFormID=UM.OrganizationFormID  AND OFRM.OrganizationID=@OrganizationID                       
 WHERE OFRM.OrganizationFormID IS  NULL                 
                 
                   
 DELETE OFRM                  
 FROM OrganizationForms OFRM                    
 LEFT JOIN @UDT_OrganizationFormsTable UM ON (UM.OrganizationFormID=OFRM.OrganizationFormID OR UM.EBFormID = OFRM.EBFormID )  
 WHERE UM.OrganizationFormID IS NULL    AND OFRM.OrganizationID=@OrganizationID                        
          
            
 --DELETE Organization Tags            
 DELETE OFT FROM OrganizationFormTags OFT            
 LEFT JOIN OrganizationForms ORGFRM ON ORGFRM.OrganizationFormID=OFT.OrganizationFormID        
 WHERE ORGFRM.OrganizationFormID IS NULL AND ORGFRM.OrganizationID=@OrganizationID            
              
 UPDATE Organizations SET LastFormSyncDate=GETUTCDATE() WHERE OrganizationID=@OrganizationID                   
              
 SELECT 1 AS TransactionResultId                     
END