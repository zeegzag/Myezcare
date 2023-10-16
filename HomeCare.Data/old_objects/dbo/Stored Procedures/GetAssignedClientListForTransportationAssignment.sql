--select * from TransportationGroups order by 1 desc    
    
  --EXEC GetAssignedClientListForTransportationAssignment @Date = '2016-12-16'    
CREATE PROCEDURE [dbo].[GetAssignedClientListForTransportationAssignment]        
 @Date date        
AS        
BEGIN        
 select TG.*, (SELECT         
     STUFF((SELECT ',' + convert(varchar(max),s.StaffID)        
     FROM TransportationGroupStaffs s         
     WHERE s.TransportationGroupID = TG.TransportationGroupID             
     FOR XML PATH('')),1,1,'')) AS StaffIDs,        
        
     (SELECT         
     STUFF((SELECT '/ ' + convert(varchar(max),e.LastName+', '+e.FirstName)        
     FROM TransportationGroupStaffs s         
     inner join Employees e on e.EmployeeID=s.StaffID        
     WHERE s.TransportationGroupID = TG.TransportationGroupID        
     FOR XML PATH('')),1,1,'')) AS StaffNames,        
        
     (SELECT  STUFF((SELECT ',' + convert(varchar(max),TGFS.TransportationFilterID)        
     FROM TransportationGroupFilterMapping TGFS         
     WHERE TGFS.TransportationGroupClientID = TGC.TransportationGroupClientID        
     FOR XML PATH('')),1,1,'')) AS TransportationFilterIDs,        
        
     (SELECT STUFF((SELECT ',' + convert(varchar(max),TF.TransportationFilterName) from TransportationGroupFilterMapping TGFS        
     inner join TransportationFilters TF on TF.TransportationFilterID=TGFS.TransportationFilterID        
     where TGFS.TransportationGroupClientID=TGC.TransportationGroupClientID          
     FOR XML PATH('')),1,1,'')) AS TransportationFilterNames, TGTL.Location GroupLocation, TGF.FacilityName GroupFacilityName,TGF.FacilityColorScheme,  
   F.FacilityName , PickUPL.Location ,        
   TGC.TransportationGroupClientID ,         
   R.LastName+', '+R.FirstName Name,R.Gender,dbo.GetAge(R.Dob) Age, ss.ScheduleStatusID,ss.ScheduleStatusName  ,       
   C.LastName+', '+C.FirstName as ParentName, C.Phone1 Phone,R.IsDeleted AS IsReferralDeleted        
 from TransportationGroups TG        
  inner join TransportLocations TGTL on TG.LocationID = TGTL.TransportLocationID  
   inner join Facilities TGF on TG.FacilityID = TGF.FacilityID  
  left join TransportationGroupClients TGC on TG.TransportationGroupID = TGC.TransportationGroupID        
  left join ScheduleMasters SM on SM.ScheduleID = TGC.ScheduleID        
  left join ScheduleStatuses ss on ss.ScheduleStatusID=SM.ScheduleStatusID        
  left join Referrals R on SM.ReferralID=R.ReferralID        
  left join Facilities F on SM.FacilityID = F.FacilityID         
  left join TransportLocations PickUPL on PickUPL.TransportLocationID = sm.PickUpLocation     
  left join ContactMappings CM on CM.ReferralID=R.ReferralID and ( CM.ContactTypeID =1)    --CM.IsDCSLegalGuardian=1 OR    
  left join Contacts C on C.ContactID = CM.ContactID         
 where TG.IsDeleted=0 and         
 TG.TransportationDate=@Date         
END 
