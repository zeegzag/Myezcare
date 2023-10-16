CREATE PROCEDURE [dbo].[GetOrganizationFormListForMapping]               
@OrganizationID BIGINT               
AS                  
BEGIN                  
                  
;WITH OrgForms AS(
    SELECT 
        MAX(OrganizationFormID) OrganizationFormID,
        EBFormID 
    FROM OrganizationForms 
    WHERE OrganizationID = @OrganizationID 
    GROUP BY EBFormID
) 
SELECT EBF.EBFormID,EBF.FromUniqueID,EBF.Id,EBF.FormId,EBF.Name,FormLongName=ISNULL(OFRM.OrganizationFriendlyFormName,EBF.FormLongName),          
EBF.NameForUrl,EBF.Version,EBF.IsActive,EBF.HasHtml,EBF.NewHtmlURI,EBF.HasPDF,EBF.NewPdfURI,EBF.EBCategoryID,EBF.IsInternalForm,EBF.InternalFormPath,      
EBF.EbMarketIDs,EBF.FormPrice,EBF.CreatedDate,EBF.UpdatedDate,EBF.UpdatedBy,EBF.IsDeleted,FormCategory=EBC.Name,OFRM.OrganizationFormID,  EBF.IsOrbeonForm,    
Tags = STUFF((SELECT ', ' + FT.FormTagName      
           FROM OrganizationFormTags OFT      
     INNER JOIN FormTags FT ON FT.FormTagID=OFT.FormTagID      
           WHERE OFT.OrganizationFormID = OFRM.OrganizationFormID      
          FOR XML PATH('')), 1, 2, '')          
FROM OrganizationForms OFRM   
INNER JOIN OrgForms O ON O.OrganizationFormID = OFRM.OrganizationFormID           
INNER JOIN EBForms EBF ON EBF.EBFormID=OFRM.EBFormID              
INNER JOIN EBCategories EBC  ON EBC.EBCategoryID=EBF.EBCategoryID              
WHERE EBF.IsActive=1 AND EBF.IsDeleted=0        
              
                  
END