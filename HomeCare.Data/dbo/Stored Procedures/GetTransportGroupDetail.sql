--TransportGroup          
CREATE PROCEDURE [dbo].[GetTransportGroupDetail]                             
(                          
  @TransportGroupID bigint = 0                                     
)                           
AS                                        
BEGIN                            
  SELECT                                        
 DISTINCT            
    r.LastName + ', ' + r.FirstName AS Name,                         
    c.Address              ,          
 TGAP.ReferralID,          
    TGAP.Note,          
 TGAP.IsDeleted,          
 TGAP.IsBillable    ,      
 tg.TripDirection      
  FROM [fleet].TransportGroupAssignPatient TGAP (NOLOCK)           
  INNER JOIN [fleet].TransportGroup tg (NOLOCK)      
  ON tg.TransportGroupID = TGAP.TransportGroupID      
  INNER JOIN Referrals r  (NOLOCK)           
  ON r.ReferralID = TGAP.ReferralID          
  LEFT JOIN ContactMappings cmp (NOLOCK)                                        
          ON cmp.ReferralID = r.ReferralID                                        
          AND cmp.ContactTypeID = 1                                        
        LEFT JOIN Contacts c (NOLOCK)                                        
          ON c.ContactID = cmp.ContactID                                        
        LEFT JOIN referralGroup rg  (NOLOCK)                                       
          ON rg.ReferralID = r.ReferralID                           
    LEFT JOIN ReferralDocuments rd  (NOLOCK)                         
    ON rd.UserID = r.ReferralID AND rd.FileName ='Client-FaceSheet' AND rd.ComplianceID=-5                          
  LEFT JOIN DDmaster dm  (NOLOCK)                                       
          ON dm.DDMasterID IN (select val from GetCSVTable(R.CareTypeIds))            
  WHERE           
  TGAP.TransportGroupID IN  (@TransportGroupID)                        
  and IsNull(TGAP.IsDeleted,0) = 0                                          
              
                                    
END 