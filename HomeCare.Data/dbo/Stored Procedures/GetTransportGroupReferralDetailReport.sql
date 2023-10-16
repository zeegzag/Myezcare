  
  
--TransportGroup            
CREATE PROCEDURE [rpt].[GetTransportGroupReferralDetailReport]                               
(                  
  @FacilityID bigint = 0 ,  
  @TransportGroupID bigint = 0 ,  
  @StartDate datetime = null,  
  @EndDate datetime = null  
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
 tg.TripDirection   ,    
 tg.Name as GroupName,    
 dm2.Title   AS TripDirectionDetail,    
 tg.StartDate,    
 tg.EndDate,    
 tg.RouteDesc  ,  
 VehicleName=CONCAT(VIN_Number,'-',Model,'-',BrandName),  
 [Facilities].[Address] AS FacilityAddress  
  FROM [FLEET].TransportGroupAssignPatient TGAP (NOLOCK)             
  INNER JOIN [FLEET].TransportGroup tg (NOLOCK)        
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
        
  LEFT JOIN DDmaster dm2  (NOLOCK)                                         
      ON dm2.DDMasterID = tg.TripDirection    
  LEFT JOIN [DEVAdmin].[DBO].Vehicles Vehicles   (nolock)      
 ON Vehicles.VehicleID = tg.VehicleID  
  LEFT JOIN [Facilities] (NOLOCK)    
 ON [Facilities].FacilityID = tg.FacilityID  
  WHERE             
  (TGAP.TransportGroupID IN  (@TransportGroupID) or IsNull(@TransportGroupID,0)=0 )  
  and IsNull(TGAP.IsDeleted,0) = 0                                            
  and (tg.StartDate >=  @StartDate or @StartDate is null)           
  and (tg.StartDate <=  @EndDate or @EndDate is null)          
  and [Facilities].FacilityID = @FacilityID  
                                      
END 
GO

