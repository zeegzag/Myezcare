
-- EXEC GetFormList @MarketID = '0', @FormCategoryID = '0', @FormName = '', @FormNumber = '', @IsDeleted = '0', @SortExpression = 'EBFormID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'      
CREATE PROCEDURE [dbo].[GetSavedFormList]      
@UDT_EBFromMappingTable UDT_EBFromMappingTable ReadOnly,  
@OrganizationID BIGINT=0,    
@MarketID BIGINT=null,                        
@FormCategoryID BIGINT=NULL,                        
@FormName VARCHAR(20)=NULL,        
@FormNumber VARCHAR(20)=NULL,        
@IsDeleted BIGINT = -1,                       
@SORTEXPRESSION NVARCHAR(100),                
@SORTTYPE NVARCHAR(10),              
@FROMINDEX INT,              
@PAGESIZE INT               
AS                        
BEGIN                          
;WITH CTEFormList AS                    
 (                         
  SELECT *,COUNT(EBFormID) OVER() AS COUNT FROM                    
  (                        
   SELECT ROW_NUMBER() OVER (ORDER BY                    
                    
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Name' THEN EF.Name END END ASC,                    
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Name' THEN EF.Name END END DESC,                    
                
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FormLongName' THEN EF.FormLongName END END ASC,                    
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FormLongName' THEN EF.FormLongName END END DESC,             
            
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Version' THEN EF.Version END END ASC,                    
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Version' THEN EF.Version END END DESC,             
            
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FormCategory' THEN EC.Name END END ASC,                    
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FormCategory' THEN EC.Name END END DESC,      
  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedBy' THEN UEFM.CreatedBy END END ASC,                    
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedBy' THEN UEFM.CreatedBy END END DESC,      
  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'UpdatedBy' THEN UEFM.UpdatedBy END END ASC,                    
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'UpdatedBy' THEN UEFM.UpdatedBy END END DESC,      
  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN  CONVERT(DATETIME, UEFM.CreatedDate, 103) END END ASC,                                          
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN  CONVERT(DATETIME, UEFM.CreatedDate , 103) END END DESC,   
  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'UpdatedDate' THEN  CONVERT(DATETIME, UEFM.UpdatedDate, 103) END END ASC,                                          
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'UpdatedDate' THEN  CONVERT(DATETIME, UEFM.UpdatedDate , 103) END END DESC,   
  
  
       
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FormPrice' THEN CONVERT(DECIMAL(18,2),EF.FormPrice) END END ASC,                    
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FormPrice' THEN CONVERT(DECIMAL(18,2),EF.FormPrice) END END DESC      
           
            
 ) AS ROW,                        
       
 UEFM.EbriggsFormMppingID,UEFM.EBriggsFormID,UEFM.ReferralID,UEFM.PatientName,UEFM.EmployeeID,UEFM.EmployeeName,SavedFormCreatedBy = UEFM.CreatedBy,  
 SavedFormCreatedDate=UEFM.CreatedDate,SavedFormUpdatedBy =UEFM.UpdatedBy,SavedFormUpdatedDate=UEFM.UpdatedDate,EF.*, FormCategory=EC.Name,
 TEMP_FormLongName=EF.FormLongName+'_'+  CONVERT(VARCHAR(100),uefm.EbriggsFormMppingID)  ---replace(convert(varchar(8),uefm.CreatedDate,108),':','')    
 FROM   @UDT_EBFromMappingTable UEFM  
 INNER JOIN EBForms EF  ON EF.EBFormID=UEFM.OriginalEBFormID AND EF.FormId=UEFM.FormId --AND OFM.OrganizationID=@OrganizationID    
 LEFT JOIN EBCategories EC ON EC.EBCategoryID= EF.EBCategoryID      
 WHERE EF.IsActive=1 AND  ((@IsDeleted = -1) OR EF.IsDeleted=@IsDeleted)             
   AND ((@FormName IS NULL OR LEN(@FormName)=0) OR EF.FormLongName LIKE '%' + @FormName+ '%')                        
   AND ((@FormNumber  IS NULL OR LEN(@FormNumber )=0) OR (EF.Name LIKE '%' + @FormNumber + '%'))                            
   AND ((@FormCategoryID IS NULL OR LEN(@FormCategoryID)=0 OR @FormCategoryID=0) OR (EF.EBCategoryID=@FormCategoryID))        
   AND ((@MarketID IS NULL OR LEN(@MarketID)=0 OR @MarketID=0) OR (@MarketID  IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(EF.EbMarketIDs)) ))        
  ) AS P1              
 )                        
 SELECT * FROM CTEFormList WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)                       
                      
END