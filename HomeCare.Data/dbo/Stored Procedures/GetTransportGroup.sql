--TransportGroup      
-- [GetTransportGroup] @FacilityID=1,@StartDate='2022-03-01'    
CREATE PROCEDURE [dbo].[GetTransportGroup]                      
(                      
  @FacilityID bigint = 0      ,           
  @StartDate datetime = null,    
  @EndDate datetime = null    
)                       
AS                                    
BEGIN                        
      SELECT      
     TransportGroupID      
   , [Name]      
   , FacilityID      
   , TripDirection      
   , StartDate      
   , EndDate      
   , VehicleID      
   , RouteDesc      
   , IsDeleted      
   , CreatedDate      
   , CreatedBy      
   , UpdatedDate      
   , UpdatedBy      
   FROM      
  [fleet].TransportGroup (NOLOCK)       
   WHERE      
  FacilityID = @FacilityID  and    
  (TransportGroup.StartDate >= @StartDate or @StartDate is null) and    
  (TransportGroup.StartDate <= @EndDate or @EndDate is null)    
               
DECLARE @TransportGroupIDs NVARCHAR(MAX) =                                    
(                    
 SELECT STRING_AGG(TransportGroupID, ', ')                              
 FROM       
 [fleet].TransportGroup   (NOLOCK)                                      
 WHERE                                        
      FacilityID = @FacilityID     and IsNull(IsDeleted ,0)=0              
)         
      
                                    
  SELECT                                    
  DISTINCT        
    r.LastName + ', ' + r.FirstName AS Name,                     
    c.Address              ,      
 TGAP.ReferralID,      
    TGAP.Note,      
 TGAP.IsDeleted,      
 TGAP.IsBillable      
  FROM [fleet].TransportGroupAssignPatient TGAP (NOLOCK)       
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
  TGAP.TransportGroupID IN  (select val from GetCSVTable(@TransportGroupIDs))                    
  and IsNull(TGAP.IsDeleted,0) = 0                                      
          
                                
END 