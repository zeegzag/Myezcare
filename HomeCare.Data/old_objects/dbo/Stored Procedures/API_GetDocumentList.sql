CREATE PROCEDURE [dbo].[API_GetDocumentList]      
 @FromIndex INT,      
 @ToIndex INT,      
 @SortExpression NVARCHAR(100),      
 @SortType NVARCHAR(10),      
 @ComplianceID BIGINT,      
 @UserType INT,      
 @ReferralID BIGINT,      
 @DocumentName NVARCHAR(MAX),      
 @DocumentationType NVARCHAR(20),      
 @SearchInDate NVARCHAR(20),      
 @StartDate DATE = NULL,            
 @EndDate DATE = NULL      
AS                                    
BEGIN            
            
DECLARE @TempTable TABLE (            
 ReferralDocumentID BIGINT,            
 Name NVARCHAR(MAX),            
 FilePath VARCHAR(200),            
 KindOfDocument VARCHAR(50),            
 ExpirationDate DATE,            
 EbriggsFormMppingID BIGINT,            
 EBriggsFormID NVARCHAR(MAX),            
 NameForUrl NVARCHAR(MAX),            
 Version NVARCHAR(MAX),            
 Tags NVARCHAR(MAX),            
 AddedBy VARCHAR(200),            
 ReceivedDate DATE,            
 ComplianceID BIGINT,    
 IsInternalForm BIT           
)            
            
INSERT INTO @TempTable(ReferralDocumentID,ComplianceID,Name,FilePath,KindOfDocument,AddedBy,ReceivedDate,ExpirationDate)            
SELECT ReferralDocumentID,ComplianceID,Name=FileName,FilePath,KindOfDocument,AddedBy=Dbo.GetGeneralNameFormat(E.FirstName,E.LastName),ReceivedDate=RD.CreatedDate,        
ExpirationDate            
FROM ReferralDocuments RD            
LEFT JOIN Employees E ON E.EmployeeID=RD.CreatedBy            
WHERE UserID=@ReferralID AND UserType=@UserType        

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

INSERT INTO @TempTable(EbriggsFormMppingID,ComplianceID,EBriggsFormID,Name,NameForUrl,Version,IsInternalForm,    
--Tags,            
AddedBy,ReceivedDate)            
SELECT EbriggsFormMppingID,SubSectionID,EBriggsFormID,Name=EBFM.FormName,EBF.NameForUrl,EBF.Version,EBF.IsInternalForm,            
--Tags = STUFF((SELECT ', ' + FT.FormTagName              
--           FROM MyezcareOrganization.dbo.OrganizationFormTags OFT              
--     INNER JOIN MyezcareOrganization.dbo.FormTags FT ON FT.FormTagID=OFT.FormTagID              
--           WHERE OFT.OrganizationFormID = ORGFRM.OrganizationFormID              
--          FOR XML PATH('')), 1, 2, ''),            
AddedBy=Dbo.GetGeneralNameFormat(E.FirstName,E.LastName),ReceivedDate=EBFM.CreatedDate            
FROM EbriggsFormMppings EBFM        
LEFT JOIN Employees E ON E.EmployeeID=EBFM.CreatedBy            
INNER JOIN @EBForms EBF ON EBF.EBFormID=EBFM.OriginalEBFormID            
--LEFT JOIN @OrganizationForms ORGFRM ON ORGFRM.EBFormID=EBF.EBFormID            
WHERE ReferralID=@ReferralID AND EBFM.IsDeleted=0 --AND SubSectionID=@ComplianceID            
            
            
 ;WITH CTEComplianceList AS                                          
 (                                           
  SELECT *,COUNT(ReceivedDate) OVER() AS Count FROM                                           
  (                                          
   SELECT ROW_NUMBER() OVER (ORDER BY                        
    CASE WHEN @SortType = 'ASC' THEN                         
     CASE                         
  WHEN @SortExpression = 'Name' THEN Name                    
  WHEN @SortExpression = 'Type' THEN KindOfDocument              
  WHEN @SortExpression = 'AddedBy' THEN AddedBy            
     END                        
    END ASC,                        
    CASE WHEN @SortType = 'DESC' THEN                        
     CASE                        
  WHEN @SortExpression = 'Name' THEN Name                    
  WHEN @SortExpression = 'Type' THEN KindOfDocument              
  WHEN @SortExpression = 'AddedBy' THEN AddedBy            
     END                        
    END DESC,                       
 CASE WHEN @SortType = 'ASC' THEN                         
     CASE                         
      WHEN @SortExpression = 'ReceivedDate' THEN ReceivedDate            
     END                        
    END ASC,                        
    CASE WHEN @SortType = 'DESC' THEN                        
CASE                        
      WHEN @SortExpression = 'ReceivedDate' THEN ReceivedDate            
     END                        
    END DESC                       
  ) AS Row, *            
   FROM  @TempTable               
   WHERE            
   --((@SearchType='Directory' AND ComplianceID=@ComplianceID) OR (@SearchType='Global'))      
   ComplianceID=@ComplianceID      
   AND ((@DocumentName IS NULL OR LEN(@DocumentName)=0) OR (Name like '%'+@DocumentName+'%'))            
   AND ((@DocumentationType IS NULL OR LEN(@DocumentationType)=0) OR (KindOfDocument like '%'+@DocumentationType+'%'))            
   AND ((@SearchInDate='Added' AND ((@StartDate IS NULL AND @EndDate IS NULL) OR             
   (@StartDate IS NULL AND CONVERT(DATE,ReceivedDate)<=@EndDate) OR (@EndDate IS NULL AND CONVERT(DATE,ReceivedDate)>=@StartDate)             
   OR (CONVERT(DATE,ReceivedDate) BETWEEN @StartDate AND @EndDate))) OR            
   (@SearchInDate='Expiry' AND ((@StartDate IS NULL AND @EndDate IS NULL) OR             
   (@StartDate IS NULL AND ExpirationDate<=@EndDate) OR (@EndDate IS NULL AND ExpirationDate>=@StartDate) OR (ExpirationDate BETWEEN @StartDate AND @EndDate))))      
   ) AS T1          
  )                                
 SELECT * FROM CTEComplianceList WHERE ROW BETWEEN @FromIndex AND @ToIndex      
END
