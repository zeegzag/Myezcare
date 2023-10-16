CREATE PROCEDURE [dbo].[GetComplianceList]              
 @DocumentName NVARCHAR(MAX) = NULL,              
 @UserType INT = -1,                  
 @DocumentationType INT = -1,    
 @IsTimeBased INT = -1,    
 @Type NVARCHAR(50)=NULL,    
 @IsDeleted BIGINT = -1,              
 @SortExpression NVARCHAR(100),                              
 @SortType NVARCHAR(10),                            
 @FromIndex INT,                            
 @PageSize INT                  
AS                          
BEGIN

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


 ;WITH CTEComplianceList AS                                
 (                                 
  SELECT *,COUNT(ComplianceID) OVER() AS Count FROM                                 
  (                                
   SELECT ROW_NUMBER() OVER (ORDER BY              
    CASE WHEN @SortType = 'ASC' THEN               
     CASE               
      WHEN @SortExpression = 'DocumentName' THEN DocumentName          
   WHEN @SortExpression = 'Type' THEN Type    
     END              
    END ASC,              
    CASE WHEN @SortType = 'DESC' THEN              
     CASE              
      WHEN @SortExpression = 'DocumentName' THEN DocumentName          
   WHEN @SortExpression = 'Type' THEN Type    
     END              
    END DESC,             
 CASE WHEN @SortType = 'ASC' THEN               
     CASE               
      WHEN @SortExpression = 'UserType' THEN UserType              
      WHEN @SortExpression = 'DocumentationType' THEN DocumentationType              
      WHEN @SortExpression = 'IsTimeBased' THEN IsTimeBased              
     END              
    END ASC,              
    CASE WHEN @SortType = 'DESC' THEN              
     CASE              
      WHEN @SortExpression = 'UserType' THEN UserType              
      WHEN @SortExpression = 'DocumentationType' THEN DocumentationType              
      WHEN @SortExpression = 'IsTimeBased' THEN IsTimeBased                
     END              
    END DESC             
   ) AS Row,* FROM    
   (SELECT DISTINCT C.ComplianceID,C.UserType,DocumentationType,DocumentName,IsTimeBased,C.IsDeleted,C.Type,C.ParentID,C.Value,    
   EBFRM.EBFormID,FormName=ISNULL(ORGFRM.OrganizationFriendlyFormName,EBFRM.FormLongName),EBFRM.NameForUrl,EBFRM.Version,  
   DocumentCount=(COUNT(EBFM.EbriggsFormMppingID) OVER(PARTITION BY EBFM.SubSectionID)+COUNT(RD.ComplianceID) OVER(PARTITION BY RD.ComplianceID))
   ,(Select SUBSTRING((SELECT ',' + CONVERT(varchar(100),RoleID) FROM dbo.SectionPermissions SP where SP.ComplianceID=C.ComplianceID  FOR XML PATH('')), 2 , 9999)) As SelectedRoles -- Nikunj(252)        
   FROM  Compliances C    
   LEFT JOIN @EBForms EBFRM ON EBFRM.EBFormID=C.EBFormID      
   LEFT JOIN @OrganizationForms ORGFRM ON ORGFRM.EBFormID=EBFRM.EBFormID  
   LEFT JOIN EbriggsFormMppings EBFM ON EBFM.SubSectionID=C.ComplianceID AND EBFM.IsDeleted=0  
 LEFT JOIN ReferralDocuments RD ON RD.ComplianceID=C.ComplianceID      
   WHERE             
   ((CAST(@IsDeleted AS BIGINT)=-1) OR C.IsDeleted=@IsDeleted)                                
   AND ((CAST(@UserType AS INT)=-1) OR C.UserType=@UserType)                                
   AND ((CAST(@DocumentationType AS INT)=-1) OR DocumentationType=@DocumentationType)    
   AND ((CAST(@IsTimeBased AS INT)=-1) OR IsTimeBased=@IsTimeBased)    
   AND ((@Type IS NULL OR LEN(@Type)=0) OR (Type=@Type))    
   AND ((@DocumentName IS NULL OR LEN(@DocumentName)=0) OR (DocumentName like '%'+@DocumentName+'%'))    
   ) AS T    
   ) AS T1    
  )                      
 SELECT * FROM CTEComplianceList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                              
END
