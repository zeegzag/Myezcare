 
  
CREATE PROCEDURE [dbo].[GetReferralDocumentListNew]                
 @UserType INT,                
 @ReferralID BIGINT=NULL,                
 @EmployeeID BIGINT=NULL,                
 @ComplianceID BIGINT,                    
 @Name NVARCHAR(MAX),                    
 @SearchInDate NVARCHAR(20),                    
 @StartDate DATE = NULL,                    
 @EndDate DATE = NULL,                    
 @KindOfDocument NVARCHAR(20),                    
 @SearchType NVARCHAR(20),                    
 @SortExpression NVARCHAR(100),                                                
 @SortType NVARCHAR(10),                                              
 @FromIndex INT,                                              
 @PageSize INT,       
 @RoleID  BIGINT                                   
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
 IsInternalForm BIT,            
 InternalFormPath NVARCHAR(MAX),            
 FormId NVARCHAR(MAX),            
 EBFormID NVARCHAR(MAX),            
 CreatedBy VARCHAR(200),                    
 CreatedDate DATETIME,                    
 UpdatedBy VARCHAR(200),                    
 UpdatedDate DATE,            
 ComplianceID BIGINT,    
 StoreType VARCHAR(50),    
 GoogleFileId VARCHAR(255),    
IsOrbeonForm bit    
)                    
            
--Insert Data from ReferralDocumnets to Temp table            
INSERT INTO @TempTable(ReferralDocumentID,ComplianceID,Name,FilePath,KindOfDocument,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,ExpirationDate,StoreType,GoogleFileId, IsOrbeonForm)    
SELECT ReferralDocumentID,RD.ComplianceID,Name=FileName,FilePath,KindOfDocument,CreatedBy=Dbo.GetGeneralNameFormat(E.FirstName,E.LastName),RD.CreatedDate,                
UpdatedBy=dbo.GetGeneralNameFormat(EU.FirstName,EU.LastName),RD.UpdatedDate,ExpirationDate, StoreType,GoogleFileId, 0 as IsOrbeonForm    
FROM ReferralDocuments RD                    
LEFT JOIN Employees E ON E.EmployeeID=RD.CreatedBy              
LEFT JOIN Employees EU ON EU.EmployeeID=RD.UpdatedBy            
INNER JOIN Compliances C ON C.ComplianceID=RD.ComplianceID AND C.IsDeleted=0    
INNER JOIN SectionPermissions SP ON SP.ComplianceID= C.ComplianceID AND SP.RoleID=@RoleID              
WHERE UserID=ISNULL(@ReferralID,@EmployeeID) AND RD.UserType=@UserType                
      
      
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
     
      
      
--Insert Data from EbriggsFormMppings to Temp table            
INSERT INTO @TempTable(EbriggsFormMppingID,ComplianceID,EBriggsFormID,Name,NameForUrl,Version,IsInternalForm,InternalFormPath,FormId,EBFormID,            
--Tags,            
CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsOrbeonForm)                    
SELECT DISTINCT EbriggsFormMppingID,SubSectionID,EBriggsFormID,Name=EBFM.FormName,EBF.NameForUrl,EBF.Version,EBF.IsInternalForm,EBF.InternalFormPath,            
EBF.FormId,EBF.EBFormID,            
--Tags = STUFF((SELECT ', ' + FT.FormTagName                      
--           FROM @OrganizationFormTags OFT                      
--     INNER JOIN @FormTags FT ON FT.FormTagID=OFT.FormTagID                      
--           WHERE OFT.OrganizationFormID = ORGFRM.OrganizationFormID                      
--          FOR XML PATH('')), 1, 2, ''),                    
CreatedBy=Dbo.GetGeneralNameFormat(E.FirstName,E.LastName),EBFM.CreatedDate,UpdatedBy=Dbo.GetGeneralNameFormat(EU.FirstName,EU.LastName),    
EBFM.UpdatedDate, IsOrbeonForm    
FROM EbriggsFormMppings EBFM                
LEFT JOIN Employees E ON E.EmployeeID=EBFM.CreatedBy            
LEFT JOIN Employees EU ON EU.EmployeeID=EBFM.UpdatedBy            
INNER JOIN Compliances C ON C.ComplianceID=EBFM.SubSectionID AND C.IsDeleted=0        
INNER JOIN SectionPermissions SP ON SP.ComplianceID= C.ComplianceID AND SP.RoleID=@RoleID          
INNER JOIN @EBForms EBF ON EBF.EBFormID=EBFM.OriginalEBFormID                    
--LEFT JOIN @OrganizationForms ORGFRM ON ORGFRM.EBFormID=EBF.EBFormID                    
WHERE (EBFM.ReferralID=@ReferralID OR EBFM.EmployeeID=@EmployeeID) AND EBFM.IsDeleted=0 --AND SubSectionID=@ComplianceID                    
                    
            
 ;WITH CTEComplianceList AS                                                  
 (                                                   
  SELECT *,COUNT(CreatedDate) OVER() AS Count FROM                                                   
  (                                                  
   SELECT ROW_NUMBER() OVER (ORDER BY                                
    CASE WHEN @SortType = 'ASC' THEN                                 
     CASE                                 
  WHEN @SortExpression = 'Name' THEN Name                            
  WHEN @SortExpression = 'Type' THEN KindOfDocument                      
  WHEN @SortExpression = 'CreatedBy' THEN CreatedBy                    
  WHEN @SortExpression = 'UpdatedBy' THEN UpdatedBy            
     END                                
    END ASC,                                
    CASE WHEN @SortType = 'DESC' THEN                                
     CASE                                
  WHEN @SortExpression = 'Name' THEN Name                            
  WHEN @SortExpression = 'Type' THEN KindOfDocument                      
  WHEN @SortExpression = 'CreatedBy' THEN CreatedBy            
  WHEN @SortExpression = 'UpdatedBy' THEN UpdatedBy                    
     END                            
    END DESC,                               
 CASE WHEN @SortType = 'ASC' THEN                                 
     CASE                                 
      WHEN @SortExpression = 'CreatedDate' THEN CreatedDate            
   WHEN @SortExpression = 'UpdatedDate' THEN UpdatedDate            
     END                                
    END ASC,                                
    CASE WHEN @SortType = 'DESC' THEN                                
    CASE                                
      WHEN @SortExpression = 'CreatedDate' THEN CreatedDate            
   WHEN @SortExpression = 'UpdatedDate' THEN UpdatedDate                    
     END                                
    END DESC                               
   ) AS Row, *                    
   FROM  @TempTable                       
   WHERE                    
   ((@SearchType='Directory' AND ComplianceID=@ComplianceID) OR (@SearchType='Global'))                    
   AND ((@Name IS NULL OR LEN(@Name)=0) OR (Name like '%'+@Name+'%'))                    
   AND ((@KindOfDocument IS NULL OR LEN(@KindOfDocument)=0) OR (KindOfDocument like '%'+@KindOfDocument+'%'))                    
   AND ((@SearchInDate='Added' AND ((@StartDate IS NULL AND @EndDate IS NULL) OR                     
   (@StartDate IS NULL AND CONVERT(DATE,CreatedDate)<=@EndDate) OR (@EndDate IS NULL AND CONVERT(DATE,CreatedDate)>=@StartDate)                     
   OR (CONVERT(DATE,CreatedDate) BETWEEN @StartDate AND @EndDate))) OR                    
   (@SearchInDate='Expiry' AND ((@StartDate IS NULL AND @EndDate IS NULL) OR                     
   (@StartDate IS NULL AND ExpirationDate<=@EndDate) OR (@EndDate IS NULL AND ExpirationDate>=@StartDate) OR (ExpirationDate BETWEEN @StartDate AND @EndDate))))                  
   --((CAST(@IsDeleted AS BIGINT)=-1) OR C.IsDeleted=@IsDeleted)                                                  
   --AND ((CAST(@UserType AS INT)=-1) OR UserType=@UserType)                                                  
   --AND ((CAST(@DocumentationType AS INT)=-1) OR DocumentationType=@DocumentationType)                      
   --AND ((CAST(@IsTimeBased AS INT)=-1) OR IsTimeBased=@IsTimeBased)                      
   --AND ((@Type IS NULL OR LEN(@Type)=0) OR (Type=@Type))                      
   --AND ((@DocumentName IS NULL OR LEN(@DocumentName)=0) OR (DocumentName like '%'+@DocumentName+'%'))                    
   ) AS T1                  
  )                                        
 SELECT * FROM CTEComplianceList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                  
END 