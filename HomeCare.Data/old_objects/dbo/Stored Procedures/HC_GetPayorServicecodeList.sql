-- EXEC HC_GetPayorServicecodeList 39,''
CREATE PROCEDURE [dbo].[HC_GetPayorServicecodeList]
 @ReferralBillingAuthorizationID BIGINT,                        
 @SearchText VARCHAR(MAX),                        
 @PageSize int=10                    
AS                          
BEGIN                          
 SELECT TOP (@PageSize)          
 SC.ServiceCodeID,SC.ServiceName,SC.Description,SC.IsBillable, PSM.DailyUnitLimit , PSM.MaxUnit, PSM.UnitType, PSM.PerUnitQuantity,
 ServiceCode = SC.ServiceCode +              
    CASE WHEN (SC.ModifierID IS NULL OR SC.ModifierID='' ) THEN '' ELSE ' -'+                    
    STUFF(                        
    (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                        
    FROM Modifiers M  where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID)) AND M.IsDeleted=0                       
    FOR XML PATH ('')), 1, 1, '')                    
    END                      
 FROM  ReferralBillingAuthorizations RBA
 INNER JOIN  PayorServiceCodeMapping PSM ON PSM.PayorID=RBA.PayorID AND RBA.ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID

 AND ( PSM.POSStartDate BETWEEN RBA.StartDate AND RBA.EndDate OR PSM.POSEndDate  BETWEEN RBA.StartDate AND RBA.EndDate )


 INNER JOIN ServiceCodes SC ON SC.ServiceCodeID=PSM.ServiceCodeID AND PSM.IsDeleted=0 --AND PSM.PayorID=@PayorID
 LEFT JOIN Modifiers M ON M.ModifierID=SC.ModifierID AND M.IsDeleted=0                    
 WHERE                         
 (                        
  ServiceCode LIKE '%'+@SearchText+'%' OR                        
  ServiceName LIKE '%'+@SearchText+'%' --OR          
 )                      
 AND SC.IsDeleted=0                    
END
