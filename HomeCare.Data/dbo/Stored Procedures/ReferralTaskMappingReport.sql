CREATE PROCEDURE [dbo].[ReferralTaskMappingReport]      
 @ReferralId BIGINT      
AS      
BEGIN      
--LOGO & ADDRESS      
    DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()   
      
      
select       
SiteLogo,      
CONCAT(      
 Submitter_NM103_NameLastOrOrganizationName, + ', ',+      
 BillingProvider_N301_Address,+ ', ',+      
 BillingProvider_N401_City,+ ', ',+      
 BillingProvider_N402_State,+ ', ',+      
 BillingProvider_N403_Zipcode)      
  AS OrgInfo      
from [dbo].[OrganizationSettings];      
      
--referal name      
select       
dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat) as ReferralName       
from referrals where ReferralID = @ReferralID;      
      
Create Table #AboutAndAddressInfo (      
    AboutInfo varchar(max),      
 AddressInfo varchar(max)      
);      
      
Insert Into #AboutAndAddressInfo      
(AboutInfo,AddressInfo) values ((select CONCAT(Address, ', ', city, ', ', State, ', ', ZipCode) as AddressInfo from Contacts where contactid = (select ContactID from ContactMappings where ReferralID = @ReferralID and ContactMappingID=1)),       
(select CONCAT( 'BIRTHDAY: ', Dob, ' AGE: ', CAST(DATEDIFF(YY,Dob,GETDATE()) AS VARCHAR), ' HEIGHT: ', Height, 'WEIGHT: ', Weight) AS AboutInfo from referrals where ReferralID = @ReferralID))      
      
select * from #AboutAndAddressInfo      
--referal address      
  drop table #AboutAndAddressInfo       
      
--ABOUT --  select DATEDIFF(YY,Dob,GETDATE()) from referrals      
      
      
--MEDICATIONS       
select  Dose, Route, Frequency, PatientInstructions from ReferralMedication  where ReferralID = @ReferralId;      
      
--SERVICE PLAN      
      
Create Table #ServicePlan (      
    VisitTaskID int      
);      
      
Insert Into #ServicePlan      
Select VisitTaskID from ReferralTaskMappings Where ReferralID = @ReferralId;      
      
select distinct vtc.VisitTaskCategoryName, vt.VisitTaskDetail, vtc.VisitTaskCategoryID from       
ReferralTaskMappings rtm       
join VisitTasks vt on rtm.VisitTaskID = vt.VisitTaskID      
join VisitTaskCategories vtc on vtc.VisitTaskCategoryID = vt.VisitTaskCategoryID      
      
where rtm.VisitTaskID in (select VisitTaskID from #ServicePlan);      
      
DROP table #ServicePlan;      
      
      
--TASK CODES      
select distinct Day1 ='Monday', vt.VisitTaskDetail as VisitTaskDetail1 from ReferralTaskMappings rtm       
join VisitTasks vt on rtm.VisitTaskID = vt.VisitTaskID      
where Days like '%1%' and rtm.ReferralID =  @ReferralID      
      
select distinct Day2 ='Tuesday', vt.VisitTaskDetail as VisitTaskDetail2 from ReferralTaskMappings rtm       
join VisitTasks vt on rtm.VisitTaskID = vt.VisitTaskID      
where Days like '%2%' and rtm.ReferralID =  @ReferralID      
      
select distinct Day3 ='Wednesday', vt.VisitTaskDetail VisitTaskDetail3 from ReferralTaskMappings rtm       
join VisitTasks vt on rtm.VisitTaskID = vt.VisitTaskID      
where Days like '%3%' and rtm.ReferralID =  @ReferralID      
      
select distinct Day4 ='Thursday', vt.VisitTaskDetail VisitTaskDetail4 from ReferralTaskMappings rtm       
join VisitTasks vt on rtm.VisitTaskID = vt.VisitTaskID      
where Days like '%4%' and rtm.ReferralID =  @ReferralID      
      
select distinct Day5 ='Friday', vt.VisitTaskDetail VisitTaskDetail5 from ReferralTaskMappings rtm       
join VisitTasks vt on rtm.VisitTaskID = vt.VisitTaskID      
where Days like '%5%' and rtm.ReferralID =  @ReferralID      
      
select distinct Day6 ='Saturday', vt.VisitTaskDetail VisitTaskDetail6 from ReferralTaskMappings rtm       
join VisitTasks vt on rtm.VisitTaskID = vt.VisitTaskID      
where Days like '%6%' and rtm.ReferralID =  @ReferralID      
      
select distinct Day7 ='Sunday', vt.VisitTaskDetail VisitTaskDetail7 from ReferralTaskMappings rtm       
join VisitTasks vt on rtm.VisitTaskID = vt.VisitTaskID      
where Days like '%7%' and rtm.ReferralID =  @ReferralID      
      
--GOAL      
select Goal from referraltaskmappingsgoal where ReferralID = @ReferralID AND IsActive = 1  
      
END      
      
--    EXEC ReferralTaskMappingReport 10163      
--select * from Referrals where ReferralID=10163      
  
  