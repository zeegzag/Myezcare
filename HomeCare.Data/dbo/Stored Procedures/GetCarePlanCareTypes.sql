-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetCarePlanCareTypes]
   
   @ReferralID bigint
	
AS
BEGIN
	
	declare @caretyis varchar(500)
	set @caretyis = (select CareTypeIds FROM Referrals where [ReferralID] = @ReferralID)
	select distinct DDMasterID as CareTypeID, Title As CareType from DDMaster WHERE ',' + @caretyis + ',' LIKE '%,' + CAST(DDMasterID as VARCHAR(500)) + ',%';

   
END