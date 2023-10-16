--  EXEC [GetReferralDXCodeMappingsList] 12
CREATE PROCEDURE [dbo].[GetReferralDXCodeMappingsList]                     
 @ReferralID bigint = 0                              

AS                  
 BEGIN          
SELECT DCM.ReferralDXCodeMappingID, DCM.ReferralID, DCM.DXCodeID, DCM.Precedence, DX.DXCodeName, DX.DXCodeWithoutDot, DX.DxCodeType, DX.Description, DXT.DxCodeShortName FROM ReferralDXCodeMappings DCM
INNER JOIN DXCodes DX ON DX.DXCodeID = DCM.DXCodeID 
INNER JOIN DxCodeTypes DXT ON DXT.DxCodeTypeID = DX.DxCodeType    

 WHERE DCM.IsDeleted=0 AND DCM.ReferralID=@ReferralID ORDER BY Precedence ASC         
      
END