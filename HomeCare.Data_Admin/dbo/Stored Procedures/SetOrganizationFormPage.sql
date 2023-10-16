CREATE PROCEDURE [dbo].[SetOrganizationFormPage]               
@OrganizationID BIGINT               
AS                  
BEGIN                  
                  
SELECT * FROM EbMarkets                  
SELECT * FROM EbCategories                  
              
DECLARE @LatestUpdateDate DATETIME;              
SELECT @LatestUpdateDate=LastFormSyncDate FROM Organizations  WHERE OrganizationID=@OrganizationID              
              
SELECT EBF.*, FormCategory=EBC.Name,OrganizationFormID=0,              
IsNewForm=CASE WHEN  @LatestUpdateDate IS NULL OR EBF.CreatedDate > @LatestUpdateDate THEN 1 ELSE 0 END               
FROM EBForms EBF              
left JOIN EBCategories EBC  ON EBC.EBCategoryID=EBF.EBCategoryID              
WHERE EBF.IsActive=1 AND EBF.IsDeleted=0       
     AND  ',' + EBF.OrganizationIDs + ',' LIKE '%,' + CONVERT(NVARCHAR(MAX), @OrganizationID) + ',%'          
              
--SELECT EBF.*,FormCategory=EBC.Name, OFRM.OrganizationFormID FROM OrganizationForms OFRM              
--INNER JOIN EBForms EBF ON EBF.EBFormID=OFRM.EBFormID              
--INNER JOIN EBCategories EBC  ON EBC.EBCategoryID=EBF.EBCategoryID              
--WHERE EBF.IsActive=1 AND EBF.IsDeleted=0   AND OrganizationID=@OrganizationID     
;WITH OrgForms AS(
    SELECT 
        MAX(OrganizationFormID) OrganizationFormID,
        EBFormID 
    FROM OrganizationForms 
    WHERE OrganizationID = @OrganizationID 
    GROUP BY EBFormID
)          
SELECT EBF.EBFormID,EBF.FromUniqueID,EBF.Id,EBF.FormId,EBF.Name,FormLongName=ISNULL(OFRM.OrganizationFriendlyFormName,EBF.FormLongName),OriginalFormName=EBF.FormLongName,        
FriendlyFormName=OFRM.OrganizationFriendlyFormName,EBF.NameForUrl,EBF.Version,EBF.IsActive,EBF.HasHtml,EBF.NewHtmlURI,EBF.HasPDF,EBF.NewPdfURI,EBF.EBCategoryID,          
EBF.EbMarketIDs,EBF.FormPrice,EBF.CreatedDate,EBF.UpdatedDate,EBF.UpdatedBy,EBF.IsDeleted,          
EBF.IsInternalForm,EBF.InternalFormPath, EBF.IsOrbeonForm, FormCategory=EBC.Name,OFRM.OrganizationFormID,              
IsNewForm=CASE WHEN  @LatestUpdateDate IS NULL OR EBF.CreatedDate > @LatestUpdateDate THEN 1 ELSE 0 END      
FROM OrganizationForms OFRM     
INNER JOIN OrgForms O ON O.OrganizationFormID = OFRM.OrganizationFormID            
INNER JOIN EBForms EBF ON EBF.EBFormID=OFRM.EBFormID              
INNER JOIN EBCategories EBC  ON EBC.EBCategoryID=EBF.EBCategoryID              
WHERE EBF.IsActive=1 AND EBF.IsDeleted=0   AND OrganizationID=@OrganizationID          
      AND  ',' + EBF.OrganizationIDs + ',' LIKE '%,' + CONVERT(NVARCHAR(MAX), @OrganizationID) + ',%'       
                  
END