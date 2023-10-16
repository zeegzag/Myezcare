-- =============================================      
-- Author:  Ashar      
-- Create date: 22 Oct 2021      
-- Description:   
-- =============================================      
CREATE PROCEDURE [rpt].[RptMemberServiceAppendixA_Header]       
 @ReferralID bigint  = 0      
AS      
 select (FirstName + ' ' + LastName) AS MemberName, MONTH(GETDATE()) AS CurrentMonth,     
 YEAR(GETDATE()) AS CurrentYear,CONVERT(date,GETDATE()) AS CurrentDates,    rpm.BeneficiaryNumber  
 from Referrals R       
 left join ReferralPayorMappings rpm on rpm.ReferralID=r.ReferralID and getdate() between rpm.PayorEffectiveDate and rpm.PayorEffectiveEndDate and rpm.IsDeleted=0 and rpm.IsActive=1   
 left join Payors P on p.PayorID=rpm.PayorID  
 left join DDMaster DD on dd.DDMasterID=rpm.BeneficiaryTypeID and dd.Title='Medicaid'  
 left join lu_DDMasterTypes lud on dd.ItemType=lud.DDMasterTypeID and lud.Name='Beneficiary Type'  
 WHERE R.ReferralID =  @ReferralID