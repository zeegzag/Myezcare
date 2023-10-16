-- EXEC HC_GetAuthorizationLoadModel @ReferralID=4003    
CREATE PROCEDURE [dbo].[HC_GetAuthorizationLoadModel]    
@ReferralID BIGINT         
AS                     
BEGIN                    
     
 DECLARE @InfinateDate DATE='2099/12/31';    
    
 SELECT Value=P.PayorID, Name=P.PayorName +' | '+CONVERT(VARCHAR(15),FORMAT(rpm.PayorEffectiveDate,Admin_Myezcare_Live.dbo.fn_getDateFormat(db_name())))+' - '+CONVERT(VARCHAR(15),FORMAT(rpm.PayorEffectiveEndDate,Admin_Myezcare_Live.dbo.fn_getDateFormat(DB_NAME()))) FROM ReferralPayorMappings RPM    
 INNER JOIN Payors P ON P.PayorID=RPM.PayorID AND P.IsDeleted=0    
 WHERE RPM.ReferralID=@ReferralID 
 --AND GETDATE() BETWEEN RPM.PayorEffectiveDate AND ISNULL(RPM.PayorEffectiveEndDate,@InfinateDate) 
 AND RPM.IsDeleted=0  
 --AND RPM.Precedence=1  
 END
GO

