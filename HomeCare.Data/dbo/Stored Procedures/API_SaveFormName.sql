CREATE PROCEDURE [dbo].[API_SaveFormName]              
@EbriggsFormMppingID BIGINT,    
@FormName NVARCHAR(500)    
AS              
BEGIN    
     
 DECLARE @OriginalEBFormID NVARCHAR(MAX)    
 SELECT @OriginalEBFormID=OriginalEBFormID FROM EbriggsFormMppings WHERE EbriggsFormMppingID=@EbriggsFormMppingID    
     
 IF(@FormName IS NULL OR @FormName='')      
 BEGIN  
  
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
  
SELECT @FormName=ISNULL(OrganizationFriendlyFormName,FormLongName)+'_'+ replace(convert(NVARCHAR, getdate(), 101),'/', '')      
  FROM @OrganizationForms ORGFRM      
  INNER JOIN @EBForms EF ON EF.EBFormID=ORGFRM.EBFormID      
  WHERE EF.EBFormID=@OriginalEBFormID  
  
  --SELECT @FormName=ISNULL(OrganizationFriendlyFormName,FormLongName)+'_'+ replace(convert(NVARCHAR, getdate(), 101),'/', '')      
  --FROM MyezcareOrganization.dbo.OrganizationForms ORGFRM      
  --INNER JOIN MyezcareOrganization.dbo.EBForms EF ON EF.EBFormID=ORGFRM.EBFormID      
  --WHERE EF.EBFormID=@OriginalEBFormID      
 END    
     
 UPDATE EbriggsFormMppings SET FormName=@FormName WHERE EbriggsFormMppingID=@EbriggsFormMppingID    
END