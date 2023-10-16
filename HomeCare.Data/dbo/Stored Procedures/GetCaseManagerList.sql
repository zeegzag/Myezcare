-- EXEC [GetCaseManagerList] @SortExpression='CaseManagerID',@SortType='ASC',@FromIndex='1',@PageSize='10000'  
CREATE PROCEDURE [dbo].[GetCaseManagerList]    
 @Name VARCHAR(100) = NULL,    
 @Email VARCHAR(50) = NULL,    
 @Phone VARCHAR(20) = NULL,
 @CaseWorkerID VARCHAR(MAX) = NULL,
 @AgencyID BIGINT = 0,    
 @AgencyLocationID BIGINT = 0,    
 @IsDeleted BIGINT = -1,    
 @SortExpression NVARCHAR(100),      
 @SortType NVARCHAR(10),    
 @FromIndex INT,    
 @PageSize INT    
AS    
BEGIN    
 ;WITH CTECaseManagerList AS    
 (     
  SELECT *,COUNT(T1.CaseManagerID) OVER() AS Count FROM     
  (    
   SELECT ROW_NUMBER() OVER (ORDER BY     
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CaseManagerID' THEN CaseManagerID END END ASC,    
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CaseManagerID' THEN CaseManagerID END END DESC,    
  
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralCount' THEN ReferralCount END END ASC,    
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralCount' THEN ReferralCount END END DESC,    
  
     
    CASE WHEN @SortType = 'ASC' THEN    
      CASE           
      WHEN @SortExpression = 'Name' THEN Name    
      WHEN @SortExpression = 'Email' THEN Email    
      WHEN @SortExpression = 'Phone' THEN Phone 
	  WHEN @SortExpression = 'CaseWorkerID' THEN CaseWorkerID
      WHEN @SortExpression = 'AgencyName' THEN AgencyName    
      --WHEN @SortExpression = 'AgencyLocationName' THEN AL.LocationName    
      END     
    END ASC,    
    CASE WHEN @SortType = 'DESC' THEN    
      CASE           
      WHEN @SortExpression = 'Name' THEN Name    
      WHEN @SortExpression = 'Email' THEN Email    
      WHEN @SortExpression = 'Phone' THEN Phone
	  WHEN @SortExpression = 'CaseWorkerID' THEN CaseWorkerID
      WHEN @SortExpression = 'AgencyName' THEN AgencyName    
      --WHEN @SortExpression = 'AgencyLocationName' THEN AL.LocationName    
      END    
    END DESC    
  ) AS Row,    
     
   * FROM (  
   SELECT CM.CaseManagerID, CM.LastName + ', ' +CM.FirstName AS Name, CM.Email,CM.Phone, Cm.CaseWorkerID,   
   A.NickName AS AgencyName,CM.IsDeleted, ReferralCount=COUNT(R.ReferralID) --  AL.LocationName AS AgencyLocationName,  
   FROM  CaseManagers CM ----update employee table to caseManager 'Balwinder'----Date'--29-03-2020'  
   INNER JOIN Agencies A ON A.AgencyID = CM.AgencyID    
   LEFT JOIN Referrals R ON R.CaseManagerID=CM.CaseManagerID AND R.IsDeleted=0    
   --LEFT JOIN AgencyLocations AL ON AL.AgencyLocationID = CM.AgencyLocationID   
     
      
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR CM.IsDeleted=@IsDeleted)    
   AND ((@Email IS NULL OR LEN(@Email)=0) OR CM.Email LIKE '%' + @Email + '%')    
   AND   
    ((@Name IS NULL OR LEN(CM.LastName)=0)     
   OR (    
       (CM.FirstName LIKE '%'+@Name+'%' )OR      
    (CM.LastName  LIKE '%'+@Name+'%') OR      
    (CM.FirstName +' '+CM.LastName like '%'+@Name+'%') OR      
    (CM.LastName +' '+CM.FirstName like '%'+@Name+'%') OR      
    (CM.FirstName +', '+CM.LastName like '%'+@Name+'%') OR      
    (CM.LastName +', '+CM.FirstName like '%'+@Name+'%')))   
       
   --((@Name IS NULL OR LEN(@Name)=0) OR (CM.FirstName+' '+CM.LastName LIKE '%' + @Name + '%'))           
   AND ((@Phone IS NULL OR LEN(@Phone)=0) OR CM.Phone LIKE '%' + @Phone + '%') 
   AND ((@CaseWorkerID IS NULL OR LEN(@CaseWorkerID)=0) OR CM.CaseWorkerID LIKE '%' + @CaseWorkerID + '%') 
   AND (( CAST(@AgencyID AS BIGINT)=0) OR CM.AgencyID = CAST(@AgencyID AS BIGINT))            
   AND (( CAST(@AgencyLocationID AS BIGINT)=0) OR CM.AgencyLocationID = CAST(@AgencyLocationID AS BIGINT))    
   GROUP BY CM.CaseManagerID, CM.LastName , CM.FirstName, CM.Email,CM.Phone, A.NickName , CM.IsDeleted ,CM.CaseWorkerID 
  
  ) AS T2 ) AS T1   
      
 )    
     
 SELECT * FROM CTECaseManagerList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)     
END  