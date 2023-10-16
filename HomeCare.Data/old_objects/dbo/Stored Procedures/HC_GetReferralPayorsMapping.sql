-- =============================================
-- Author:		Kundan Kumar Rai
-- Create date: 5 June 2020
-- Description:	Get refferal payors
-- =============================================
CREATE PROCEDURE [dbo].[HC_GetReferralPayorsMapping]
	 @ReferralID BIGINT = 0,                                             
	 @StartDate DATETIME
AS
BEGIN
	 SELECT P.PayorName,P.ShortName, P.PayorID, RPM.Precedence FROM ReferralPayorMappings RPM                          
		 INNER JOIN Payors P ON P.PayorID=RPM.PayorID                          
		 WHERE RPM.ReferralID=@ReferralID AND  RPM.Precedence IS NOT NULL AND RPM.IsDeleted=0                      
		 AND  CONVERT(DATE,@StartDate) BETWEEN RPM.PayorEffectiveDate AND RPM.PayorEffectiveEndDate                          
		 ORDER BY RPM.Precedence ASC  
END
