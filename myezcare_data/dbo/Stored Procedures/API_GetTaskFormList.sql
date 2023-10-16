CREATE PROCEDURE [dbo].[API_GetTaskFormList]      
@ReferralTaskMappingID BIGINT        
AS        
BEGIN      
DECLARE @VisitTaskID BIGINT      
SELECT @VisitTaskID=VisitTaskID FROM ReferralTaskMappings WHERE ReferralTaskMappingID=@ReferralTaskMappingID      


DECLARE @EBForms TABLE(
EBFormID nvarchar(MAX),
FromUniqueID nvarchar(MAX),
Id nvarchar(MAX),
FormId nvarchar(MAX),
Name nvarchar(MAX),
FormLongName nvarchar(MAX),
NameForUrl nvarchar(MAX),
Version	nvarchar(MAX),
IsActive bit,
HasHtml	bit,
NewHtmlURI nvarchar(MAX),
HasPDF bit,
NewPdfURI nvarchar(MAX),
EBCategoryID nvarchar(MAX),
EbMarketIDs	nvarchar(MAX),
FormPrice decimal(10, 2),
CreatedDate	datetime,
UpdatedDate	datetime,
UpdatedBy bigint,
IsDeleted bit,
IsInternalForm bit,
InternalFormPath nvarchar(MAX)
)

INSERT INTO @EBForms
EXEC GetAdminDatabseTableData EBForms

DECLARE @OrganizationForms TABLE(
OrganizationFormID bigint,
EBFormID nvarchar(MAX),
OrganizationID bigint,
CreatedDate	datetime,
UpdatedDate	datetime,
CreatedBy bigint,
UpdatedBy bigint,
OrganizationFriendlyFormName nvarchar(MAX)
)

INSERT INTO @OrganizationForms
EXEC GetAdminDatabseTableData OrganizationForms



SELECT DISTINCT TFM.TaskFormMappingID,VisitTaskID,TFM.EBFormID,IsRequired,FormNumber=EBF.Name,    
 FormName=ISNULL(OFRM.OrganizationFriendlyFormName,EBF.FormLongName),      
 EBF.NameForUrl,EBF.Version,EBF.InternalFormPath,EBF.IsInternalForm,EBF.EBFormID,EBF.FormId,  
 IsFilled=CASE WHEN (EBFM.EbriggsFormMppingID>0 OR EBFM.EbriggsFormMppingID IS NOT NULL) THEN 1 ELSE 0 END  
 FROM TaskFormMappings TFM          
 INNER JOIN @OrganizationForms OFRM ON OFRM.EBFormID=TFM.EBFormID          
 INNER JOIN @EBForms EBF ON EBF.EBFormID=OFRM.EBFormID  
 LEFT JOIN EbriggsFormMppings EBFM ON EBFM.TaskFormMappingID=TFM.TaskFormMappingID AND EBFM.ReferralTaskMappingID=@ReferralTaskMappingID AND EmployeeVisitNoteID IS NULL  
 WHERE TFM.VisitTaskID=@VisitTaskID AND TFM.IsDeleted=0

 --SELECT DISTINCT TFM.TaskFormMappingID,VisitTaskID,TFM.EBFormID,IsRequired,FormNumber=EBF.Name,    
 --FormName=ISNULL(OFRM.OrganizationFriendlyFormName,EBF.FormLongName),      
 --EBF.NameForUrl,EBF.Version,EBF.InternalFormPath,EBF.IsInternalForm,EBF.EBFormID,EBF.FormId,  
 --IsFilled=CASE WHEN (EBFM.EbriggsFormMppingID>0 OR EBFM.EbriggsFormMppingID IS NOT NULL) THEN 1 ELSE 0 END  
 --FROM TaskFormMappings TFM          
 --INNER JOIN MyezcareOrganization.dbo.OrganizationForms OFRM ON OFRM.EBFormID=TFM.EBFormID          
 --INNER JOIN MyezcareOrganization.dbo.EBForms EBF ON EBF.EBFormID=OFRM.EBFormID  
 --LEFT JOIN EbriggsFormMppings EBFM ON EBFM.TaskFormMappingID=TFM.TaskFormMappingID AND EBFM.ReferralTaskMappingID=@ReferralTaskMappingID AND EmployeeVisitNoteID IS NULL  
 --WHERE TFM.VisitTaskID=@VisitTaskID AND TFM.IsDeleted=0
END
