  
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
            
DECLARE @EBForms [dbo].[EBFormsData]           
            
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
                
SELECT DISTINCT TaskFormMappingID,VisitTaskID,TFM.EBFormID,TFM.ComplianceID,IsRequired,EBF.Name,FormLongName=ISNULL(OFRM.OrganizationFriendlyFormName,EBF.FormLongName),              
EBF.NameForUrl,EBF.Version,EBF.FormId, EBF.IsInternalForm, EBF.InternalFormPath, EBF.IsOrbeonForm                  
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
  --SELECT            
  --  dm.Title AS Name,            
  --  dm.DDMasterID AS Value            
  --FROM DDMaster dm            
  --INNER JOIN lu_DDMasterTypes lu            
  --  ON lu.DDMasterTypeID = dm.ItemType            
  --WHERE lu.Name = 'Task Response Code'   

  
SELECT N.DisplayName [Name], C.ComplianceID [Value] FROM Compliances C
LEFT JOIN Compliances PC ON PC.ComplianceID = C.ParentID
OUTER APPLY (
	SELECT 
		ISNULL(CONVERT(NVARCHAR(MAX),PC.SortingID) + '.', '') + ISNULL(CONVERT(NVARCHAR(MAX),C.SortingID),'0') SortingID,
		ISNULL(PC.DocumentName + ' > ', '') + C.DocumentName DisplayName  
) N
WHERE C.IsDeleted = 0
AND C.HideIfEmpty = 0 
AND C.[UserType] = 2
ORDER BY N.SortingID, N.DisplayName


  select  dm.Title as Name,dm.DDMasterID as Value from DDMaster dm
inner join lu_DDMasterTypes lu on lu.DDMasterTypeID=dm.ItemType
where lu.Name='Task Option'
                
END 