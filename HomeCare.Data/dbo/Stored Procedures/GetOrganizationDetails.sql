CREATE procedure [dbo].[GetOrganizationDetails]        
as        
begin        
select os.OrganizationID,OS.FromEmail,os.ScheduleType  from OrganizationSettings os        
end