
-- [GetOrganizationList] '','','','',null,null,-1,'OrganizationID','ASC',1,100      
-- UPDATE Organizations SET IsDeleted=0  
  
-- DELETE FROM Organizations WHERE OrganizationID > 5  
  
CREATE PROCEDURE [dbo].[GetOrganizationList]             
@OrganizationTypeID BIGINT=null,      
@OrganizationStatusID BIGINT=null,      
@DisplayName NVARCHAR(200)=null,      
@CompanyName NVARCHAR(200)=null,      
@DomainName NVARCHAR(200)=null,      
@StartDate DATE=null,      
@EndDate DATE=null,      
@IsDeleted BIGINT = -1,     
@SortExpression NVARCHAR(100),                              
@SortType NVARCHAR(10),                            
@FromIndex INT,                            
@PageSize INT         
             
AS                          
BEGIN                          
 ;WITH CTEOrganizationList AS                                
 (                                 
 SELECT *,COUNT(T1.OrganizationID) OVER() AS Count FROM                                 
 (                                
  SELECT ROW_NUMBER() OVER (ORDER BY                       
  CASE WHEN @SortType = 'ASC' THEN                                
   CASE                                       
    WHEN @SortExpression = 'OrganizationType' THEN OT.OrganizationTypeName
	WHEN @SortExpression = 'OrganizationStatus' THEN OS.OrganizationStatusName                  
    WHEN @SortExpression = 'DisplayName' THEN DisplayName                    
    WHEN @SortExpression = 'CompanyName' THEN CompanyName                    
    WHEN @SortExpression = 'DomainName' THEN DomainName                    
   END                                 
  END ASC,                                
  CASE WHEN @SortType = 'DESC' THEN                                
   CASE                                       
    WHEN @SortExpression = 'OrganizationType' THEN OT.OrganizationTypeName                  
	WHEN @SortExpression = 'OrganizationStatus' THEN OS.OrganizationStatusName
    WHEN @SortExpression = 'DisplayName' THEN DisplayName                    
    WHEN @SortExpression = 'CompanyName' THEN CompanyName                    
    WHEN @SortExpression = 'DomainName' THEN DomainName               
   END                                
  END DESC,       
              
  CASE WHEN @SortType = 'ASC' THEN                                
   CASE                                       
    WHEN @SortExpression = 'StartDate' THEN StartDate              
    WHEN @SortExpression = 'EndDate' THEN EndDate              
   END                                 
  END ASC,                                
  CASE WHEN @SortType = 'DESC' THEN                                
   CASE                                       
    WHEN @SortExpression = 'StartDate' THEN StartDate              
    WHEN @SortExpression = 'EndDate' THEN EndDate              
   END                                
  END DESC    
  ) AS Row,    
  OG.*, OT.OrganizationTypeName, OS.OrganizationStatusName, ISNULL(OE.OrganizationEsignID, 0) AS OrganizationEsignID
  FROM  Organizations OG  
  LEFT JOIN OrganizationTypes OT ON  OT.OrganizationTypeID=OG.OrganizationTypeID  
  LEFT JOIN OrganizationStatuses OS ON  OS.OrganizationStatusID=OG.OrganizationStatusID  
  LEFT JOIN OrganizationEsigns OE ON OG.OrganizationID = OE.OrganizationID AND OE.IsInProcess = 1
  
  
  WHERE    
  ((CAST(@IsDeleted AS BIGINT)=-1) OR OG.IsDeleted=@IsDeleted)      
  AND       
  ((@OrganizationTypeID IS NULL OR @OrganizationTypeID=0) OR OG.OrganizationTypeID=@OrganizationTypeID)
  AND 
  ((@OrganizationStatusID IS NULL OR @OrganizationStatusID=0) OR OG.OrganizationStatusID=@OrganizationStatusID)                      
  AND       
  ((@DisplayName IS NULL OR LEN(@DisplayName)=0) OR OG.DisplayName LIKE '%' + @DisplayName + '%')                      
  AND       
  ((@CompanyName IS NULL OR LEN(@CompanyName)=0) OR OG.CompanyName LIKE '%' + @CompanyName + '%')                      
  AND       
  ((@DomainName IS NULL OR LEN(@DomainName)=0) OR OG.DomainName LIKE '%' + @DomainName + '%')                      
  AND       
  ((@StartDate is null OR CONVERT(DATE,OG.StartDate) >= @StartDate) and (@EndDate is null OR CONVERT(DATE,OG.EndDate) <= @EndDate))       
  )As T1    
)         
  
                         
 SELECT * FROM CTEOrganizationList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                              
END