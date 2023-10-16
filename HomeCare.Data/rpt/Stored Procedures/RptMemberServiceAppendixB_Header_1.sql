--  [rpt].[RptMemberServiceAppendixB_Header]  '84,65'
-- =============================================    
-- Author:  Ashar A    
-- Create date: 23 Oct 2021    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [rpt].[RptMemberServiceAppendixB_Header]     
 @ReferralID nvarchar(50)  = null    
AS    
    
 SELECT FirstName + ' ' + LastName AS PatientName, MONTH(GETDATE()) AS CurrentMonth,    
 YEAR(GETDATE()) AS CurrentYear, FLOOR(DATEDIFF(DAY, Dob, getdate()) / 365.25) AS Age     
 FROM Referrals WHERE ReferralID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ReferralID))  
--  FROM Referrals WHERE ReferralID = @ReferralID 