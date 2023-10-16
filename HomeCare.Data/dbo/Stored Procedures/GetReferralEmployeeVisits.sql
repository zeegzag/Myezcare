                   
ALTER PROCEDURE [dbo].GetReferralEmployeeVisits                    
(                    
 @EmployeeID bigint=0,                    
 @SlotDate datetime= null   ,        
 @TransportationType int=2 --1-transportation assignment, 2-- transportation group        
)                    
AS                    
BEGIN  
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
--declare @EmployeeID bigint=40057               
--declare @SlotDate datetime= cast(GETDATE() as date)             
            
 IF @SlotDate IS NULL                     
 BEGIN                    
  set @SlotDate = cast(GETDATE() as date)                    
 END                    
-- declare @SlotDate datetime= cast(GETDATE() as date)                    
IF @TransportationType = 1        
BEGIN        
 SELECT DISTINCT                    
  r.ReferralID,                                   
  dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) AS Name,                    
  c.Address,                                    
     c.City,                                    
     c.ZipCode,                                    
     c.State,                    
  TAP.Note,                    
  TAP.TransportAssignPatientID,                    
  EVTL.Id  as EmployeeVisitsTransportLogDetailId  ,                
  EVT.Id AS EmployeeVisitsTransportLogId,                
  EVTL.ClockInTime ,            
  EVTL.ClockOutTime ,            
  IsNull(c.Latitude ,c.OldLatitude) as Latitude,                
  IsNull(c.Longitude ,c.OldLongitude) as Longitude,                 
 c.Phone1 ,                
 c.Phone2 ,                
 c.OtherPhone,                
 c.Email                 
                 
  --E.*                    
 --c.*,                    
 --cmp.*                    
 ----,R.*                     
 from                     
 --TransportAssignPatient (nolock)                    
                     
                     
 Employees E (NOLOCK)                     
 inner join [Kundan_Admin].DBO.Vehicles V (NOLOCK)                     
 on V.EmployeeID = E.EmployeeID                    
 and E.EmployeeID = @EmployeeID                    
 inner join Transport T (nolock)                    
 on T.VehicleID = V.VehicleID                    
 inner join TransportAssignPatient TAP (nolock)                    
 on TAP.TransportID = T.TransportID and IsNull(TAP.IsDeleted ,0)=0                    
 inner join ReferralTimeSlotDates RTSD (nolock)                    
 on RTSD.ReferralID = TAP.ReferralID                    
 and RTSD.ReferralTSDate = @SlotDate                    
 inner join Referrals R (nolock)                    
 on R.ReferralID = TAP.ReferralID                    
                   
   -------------                
   LEFT JOIN ContactMappings cmp (NOLOCK)                                   
           ON cmp.ReferralID = R.ReferralID                                    
           AND cmp.ContactTypeID = 1                                    
         LEFT JOIN Contacts c (NOLOCK)                    
           ON c.ContactID = cmp.ContactID                    
     and IsNull(c.IsDeleted,0) = 0                
   -------------                  
     LEFT JOIN EmployeeVisitsTransportLogDetail EVTL (NOLOCK)                    
     ON EVTL.ReferralID = R.ReferralID           
  and cast(EVTL.ClockInTime as date) = @SlotDate          
   -------------                
   LEFT JOIN EmployeeVisitsTransportLog EVT (NOLOCK)                 
   ON EVT.TransportAssignPatientID = TAP.TransportAssignPatientID               
   and EVT.VisitDate = @SlotDate              
 --where E.EmployeeID = @EmployeeID                    
END        
ELSE        
BEGIN        
        
         
 SELECT DISTINCT                    
  r.ReferralID,                                   
  dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) AS Name,                    
  c.Address,                                    
     c.City,                                    
     c.ZipCode,                                    
  c.State,                    
  TGP.Note,                    
  TGP.TransportGroupID,                    
  EVTL.Id  as EmployeeVisitsTransportLogDetailId  ,                
  EVT.Id AS EmployeeVisitsTransportLogId,                
  EVTL.ClockInTime ,            
  EVTL.ClockOutTime ,            
  IsNull(c.Latitude ,c.OldLatitude) as Latitude,                
  IsNull(c.Longitude ,c.OldLongitude) as Longitude,                 
 c.Phone1 ,                
 c.Phone2 ,                
 c.OtherPhone,                
 c.Email                 
                 
  --E.*                    
 --c.*,                    
 --cmp.*                    
 ----,R.*                     
 from                     
 --TransportAssignPatient (nolock)                    
                     
                     
 Employees E (NOLOCK)                     
 inner join [Kundan_Admin].DBO.Vehicles V (NOLOCK)                     
 on V.EmployeeID = E.EmployeeID                    
 and E.EmployeeID = @EmployeeID                    
 inner join [fleet].TransportGroup TG (nolock)                    
 on TG.VehicleID = V.VehicleID                    
 inner join [fleet].TransportGroupAssignPatient TGP (nolock)                    
 on TGP.TransportGroupID = TG.TransportGroupID and IsNull(TGP.IsDeleted ,0)=0                    
 inner join ReferralTimeSlotDates RTSD (nolock)                    
 on RTSD.ReferralID = TGP.ReferralID                    
 and RTSD.ReferralTSDate = @SlotDate                    
 inner join Referrals R (nolock)                    
 on R.ReferralID = TGP.ReferralID                    
                   
   -------------                
   LEFT JOIN ContactMappings cmp (NOLOCK)                                   
           ON cmp.ReferralID = R.ReferralID                                    
           AND cmp.ContactTypeID = 1                                    
         LEFT JOIN Contacts c (NOLOCK)                    
           ON c.ContactID = cmp.ContactID                    
     and IsNull(c.IsDeleted,0) = 0                
   -------------                  
     LEFT JOIN EmployeeVisitsTransportLogDetail EVTL (NOLOCK)                    
     ON EVTL.ReferralID = R.ReferralID           
  and cast(EVTL.ClockInTime as date) = @SlotDate          
   -------------                
   LEFT JOIN EmployeeVisitsTransportLog EVT (NOLOCK)                 
   ON EVT.TransportGroupID = TG.TransportGroupID               
   and EVT.VisitDate = @SlotDate              
 --where E.EmployeeID = @EmployeeID                    
END        
END 