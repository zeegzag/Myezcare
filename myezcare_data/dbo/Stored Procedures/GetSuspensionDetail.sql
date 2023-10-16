 -- EXEC GetSuspensionDetail 117,90
CREATE PROCEDURE [dbo].[GetSuspensionDetail]  
 @ReferralID bigint,
 @dayCount int 
AS  
BEGIN  
 SET NOCOUNT ON;  
	DECLARE @count int 
 	select * from ReferralSuspentions where ReferralID=@ReferralID	

	 --  (select *,(select count(*) from ReferralBehaviorContracts RBCIN where  RBC.ReferralBehaviorContractID != RBCIN.ReferralBehaviorContractID 
		--and RBCIN.ReferralID=RBC.ReferralID and RBCIN.WarningDate between DATEADD(D,-90,RBC.WarningDate) and DATEADD(D,90,RBC.WarningDate) )  from ReferralBehaviorContracts RBC where RBC.ReferralID=@ReferralID
		--)
	
	(select  @count=count(*) from ReferralBehaviorContracts RBC where RBC.ReferralID=@ReferralID AND RBC.IsActive=1 AND RBC.IsDeleted=0
		and (select count(*) from ReferralBehaviorContracts RBCIN where  RBC.ReferralBehaviorContractID != RBCIN.ReferralBehaviorContractID AND RBC.IsActive=1 AND RBC.IsDeleted=0 AND RBCIN.IsActive=1 AND RBCIN.IsDeleted=0
		and RBCIN.ReferralID=RBC.ReferralID and RBCIN.WarningDate between DATEADD(D,-90,RBC.WarningDate) and DATEADD(D,89,RBC.WarningDate) ) > 1)

	select case when @count >= 3 then 1 else 0 end
END




