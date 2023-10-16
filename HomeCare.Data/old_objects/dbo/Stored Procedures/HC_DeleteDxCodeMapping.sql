
--UpdatedBy: Akhilesh kamal
--UpdatedDate: 22 Apr 2020
-- Decsription: select DXCodeWithoutDot field
        
CREATE PROCEDURE [dbo].[HC_DeleteDxCodeMapping]          
 @ReferralDXCodeMappingID BIGINT,          
 @ReferralId BIGINT          
AS          
BEGIN          
 DELETE FROM ReferralDXCodeMappings          
 WHERE ReferralDXCodeMappingID=@ReferralDXCodeMappingID;          
          
 SELECT         
 RD.ReferralDXCodeMappingID,D.DXCodeID,D.DXCodeName,D.DXCodeWithoutDot,RD.Precedence,RD.StartDate,RD.EndDate,DT.DxCodeShortName,       
 D.Description,D.EffectiveFrom,D.EffectiveTo,RD.IsDeleted          
 FROM ReferralDXCodeMappings RD          
 INNER JOIN DxCodes D ON D.DXCodeID=RD.DXCodeID          
 Inner Join DxCodeTypes DT on DT.DxCodeTypeID=D.DxCodeType      
 WHERE RD.ReferralID = @ReferralId;       
END       
GO
