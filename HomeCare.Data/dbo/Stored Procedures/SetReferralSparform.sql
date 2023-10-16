CREATE PROCEDURE [dbo].[SetReferralSparform]  
@ReferralID bigint  
AS  
BEGIN  
 SELECT ref.ReferralID, rsf.ReferralSparFormID, rsf.ReviewDate, rsf.AdmissionDate, rsf.AssessmentDate, rsf.AssessmentCompletedAndSignedByBHP, rsf.IdentifyDTSDTOBehavior,  
 rsf.DemographicDate, rsf.IsROI, rsf.IsSNCD, rsf.DTSDTOUpdateText, rsf.AdditionInformation, rsf.ServicePlanCompleted, rsf.ServicePlanSignedDatedByBHP,  
 rsf.ServicePlanIdentified, rsf.ServicePlanHasFrequency, rsf.ServicePlanAdditionalInfo, rsf.IsSparFormCompleted,rsf.SparFormCompletedBy,rsf.SparFormCompletedDate, rsf.BHPReviewSignature, rsf.Date,  
 rsf.CreatedDate, rsf.CreatedBy, rsf.UpdatedDate, rsf.UpdatedBy, rsf.SystemID,rsf.IsSparFormOffline,ref.CASIIScore    
  FROM Referrals ref   
 LEFT JOIN ReferralSparForms rsf on ref.ReferralID=rsf.ReferralID  
 where ref.ReferralID=@ReferralID;  
  
 SELECT ref.ReferralID, ref.LastName + ', ' + ref.FirstName AS ReferralName, ref.Dob AS DateOfBirth, a.NickName AS ReferringAgency,ref.ReferralDate  
 FROM Referrals ref   
  LEFT JOIN Agencies a ON a.AgencyID=ref.AgencyID  
  LEFT JOIN CaseManagers cm ON cm.CaseManagerID=ref.CaseManagerID  
 WHERE ref.ReferralID=@ReferralID;  
END