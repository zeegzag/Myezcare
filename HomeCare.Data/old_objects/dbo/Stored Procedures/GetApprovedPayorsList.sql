CREATE procedure [dbo].[GetApprovedPayorsList]    
@PayorID bigint    
AS  
SELECT FacilityID as BillingProviderID, FacilityName From  Facilities WHERE ParentFacilityID=0 AND IsDeleted=0 ORDER BY FacilityName ASC
