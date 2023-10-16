
-- updated by					updatedDate						Description
-- vikas						31-07-2019						Add GetGeneralMasterList sp replace on select query

-- AddVisitTaskPageModel 38,9                
CREATE PROCEDURE [dbo].[AddVisitTaskPageModel]                              
@VisitTaskID BIGINT,                              
@DDType_VisitType INT,          
@DDType_TaskFrequencyCode INT =13          
AS                              
BEGIN                              
                      
SELECT VT.*,                    
ServiceCode = ServiceCode +                      
case                      
when (SC.ModifierID IS NULL OR SC.ModifierID = '')        
then                       
''                      
else                   
' -'+                      
STUFF(                          
(SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                          
FROM Modifiers M                          
where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))                          
FOR XML PATH (''))                          
, 1, 1, '')                      
end                    
--,                    
--DM.Title as CareTypeTitle                 
FROM VisitTasks VT                         
left join ServiceCodes SC on SC.ServiceCodeID=VT.ServiceCodeID                    
LEFT join Modifiers M ON M.ModifierID=SC.ModifierID AND M.IsDeleted=0                     
WHERE VisitTaskID=@VisitTaskID                              
                              
--SELECT Name=Title, Value=DDMasterID FROM DDMaster WHERE ItemType=@DDType_VisitType AND IsDeleted=0
EXEC GetGeneralMasterList @DDType_VisitType 
          
--SELECT Name=Title,Value=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_TaskFrequencyCode          
EXEC GetGeneralMasterList @DDType_TaskFrequencyCode
  
DECLARE @EBForms TABLE(  
EBFormID nvarchar(MAX),  
FromUniqueID nvarchar(MAX),  
Id nvarchar(MAX),  
FormId nvarchar(MAX),  
Name nvarchar(MAX),  
FormLongName nvarchar(MAX),  
NameForUrl nvarchar(MAX),  
Version nvarchar(MAX),  
IsActive bit,  
HasHtml bit,  
NewHtmlURI nvarchar(MAX),  
HasPDF bit,  
NewPdfURI nvarchar(MAX),  
EBCategoryID nvarchar(MAX),  
EbMarketIDs nvarchar(MAX),  
FormPrice decimal(10, 2),  
CreatedDate datetime,  
UpdatedDate datetime,  
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
CreatedDate datetime,  
UpdatedDate datetime,  
CreatedBy bigint,  
UpdatedBy bigint,  
OrganizationFriendlyFormName nvarchar(MAX)  
)  
  
INSERT INTO @OrganizationForms  
EXEC GetAdminDatabseTableData OrganizationForms  
      
SELECT DISTINCT TaskFormMappingID,VisitTaskID,TFM.EBFormID,IsRequired,EBF.Name,FormLongName=ISNULL(OFRM.OrganizationFriendlyFormName,EBF.FormLongName),    
EBF.NameForUrl,EBF.Version      
FROM TaskFormMappings TFM      
INNER JOIN @OrganizationForms OFRM ON OFRM.EBFormID=TFM.EBFormID      
INNER JOIN @EBForms EBF ON EBF.EBFormID=OFRM.EBFormID      
WHERE TFM.VisitTaskID=@VisitTaskID AND TFM.IsDeleted=0  
  
--SELECT DISTINCT TaskFormMappingID,VisitTaskID,TFM.EBFormID,IsRequired,EBF.Name,FormLongName=ISNULL(OFRM.OrganizationFriendlyFormName,EBF.FormLongName),    
--EBF.NameForUrl,EBF.Version      
--FROM TaskFormMappings TFM      
--INNER JOIN MyezcareOrganization.dbo.OrganizationForms OFRM ON OFRM.EBFormID=TFM.EBFormID      
--INNER JOIN MyezcareOrganization.dbo.EBForms EBF ON EBF.EBFormID=OFRM.EBFormID      
--WHERE TFM.VisitTaskID=@VisitTaskID AND TFM.IsDeleted=0  
      
END
