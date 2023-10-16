CREATE PROCEDURE [dbo].[ValidateChangeServiceCode]
@NewServiceStartDate Date=null,        
@PayorID bigint = 0,        
@PosID bigint = 0,        
@NewServiceCodeID bigint = 0
AS
BEGIN
SELECT (S.ServiceCode + CASE WHEN M.ModifierCode IS NOT NULL THEN ' - ' + M.ModifierCode ELSE '' END +' : '+ServiceName) AS ServiceCode,
CONVERT(VARCHAR(10),CONVERT(datetime,PSM.POSStartDate,1),101) AS POSStartDate,
CONVERT(VARCHAR(10),CONVERT(datetime,PSM.POSEndDate,1),101) AS POSEndDate,
P.ShortName AS Payor,PSM.Rate,PSM.PosID,PSM.PayorServiceCodeMappingID

FROM PayorServiceCodeMapping PSM
INNER JOIN ServiceCodes S ON S.ServiceCodeID=PSM.ServiceCodeID
INNER JOIN Payors P ON P.PayorID=PSM.PayorID
LEFT JOIN Modifiers M ON M.ModifierID=S.ModifierID 
WHERE PSM.PayorID=@PayorID AND PSM.PosID=@PosID AND PSM.ServiceCodeID=@NewServiceCodeID  AND S.IsDeleted=0
AND ((@NewServiceStartDate IS NULL) OR (@NewServiceStartDate BETWEEN POSStartDate  and POSEndDate))
END
