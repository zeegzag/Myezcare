CREATE PROCEDURE [dbo].[HC_SetAddAgencyPage]  
@AgencyID BIGINT    
AS    
BEGIN    
  select RegionID,RegionName from Regions;
   select *  from States;
  select * from  Agencies WHERE  AgencyID= @AgencyID ;  
 END
