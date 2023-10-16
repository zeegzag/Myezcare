﻿-- EXEC GetDashboardAnsellCaseyReviewList  @SortExpression = 'ClientName', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @AssigneeID = '1'  
-- EXEC GetDashboardAssignedNoteReviewList @SortExpression = 'ClientName', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @AssigneeID = '1'  
CREATE PROCEDURE[dbo].[GetDashboardAssignedNoteReviewList]                 
@AssigneeID bigint,          
@SortExpression NVARCHAR(100),                    
@SortType NVARCHAR(10),                  
@FromIndex INT,                  
@PageSize INT      
      
AS                  
BEGIN                  
 DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()               
;WITH CTEDashboardAnsellCaseyReview AS                  
 (                   
  SELECT *,COUNT(T1.ReferralID) OVER() AS Count FROM                   
  (                  
   SELECT ROW_NUMBER() OVER (ORDER BY                   
              
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralID' THEN t.ReferralID END END ASC,            
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralID' THEN t.ReferralID END END DESC,             
           
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN CONVERT(varchar(50),t.AHCCCSID) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN CONVERT(varchar(50),t.AHCCCSID) END END DESC,           
           
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CISNumber' THEN CAST(t.CISNumber AS bigint) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CISNumber' THEN CAST(t.CISNumber AS bigint) END END DESC,                                        
        
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClientName' THEN CONVERT(varchar(50),t.LastName) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClientName' THEN CONVERT(varchar(50),t.LastName) END END DESC,                                       
            
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Gender' THEN CONVERT(char(1),t.Gender) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Gender' THEN CONVERT(char(1),t.Gender) END END DESC,            
                
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Age' THEN CAST(t.Age AS decimal) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Age' THEN CAST(t.Age AS decimal) END END DESC,        
  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'NoteComments' THEN CONVERT(varchar(50),t.NoteComments) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'NoteComments' THEN CONVERT(varchar(50),t.NoteComments) END END DESC,  
        
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedByName' THEN CAST(t.CreatedByName AS varchar) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedByName' THEN CAST(t.CreatedByName AS varchar) END END DESC,        
  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AssignedByName' THEN CAST(t.AssignedByName AS varchar) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AssignedByName' THEN CAST(t.AssignedByName AS varchar) END END DESC,        
        
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN  CONVERT(date, t.CreatedDate, 105) END END ASC,                                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN  CONVERT(date, t.CreatedDate, 105) END END DESC    ,  
  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'NoteAssignedDate' THEN  CONVERT(date, t.NoteAssignedDate, 105) END END ASC,                                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'NoteAssignedDate' THEN  CONVERT(date, t.NoteAssignedDate, 105) END END DESC     
   
      
   ) AS ROW,                  
   t.* FROM                  
   (      
      SELECT REF.ReferralID, REF.ClientID,REF.CISNumber,REF.AHCCCSID,REF.FirstName,REF.LastName,dbo.GetGenericNameFormat(REF.FirstName,REF.MiddleName, REF.LastName,@NameFormat) AS FullName,  Gender,dbo.GetAge(REF.Dob) as Age ,              
  N.NoteID,N.CreatedDate, N.NoteAssignedDate, N.NoteComments,  
 
  dbo.GetGenericNameFormat(EMP1.FirstName,EMP1.MiddleName, EMP1.LastName,@NameFormat) AS CreatedByName,  
  dbo.GetGenericNameFormat(EMP2.FirstName,EMP2.MiddleName, EMP2.LastName,@NameFormat) AS AssignedByName
     FROM NOTES N  
   INNER JOIN Referrals REF ON REF.ReferralID=N.ReferralID  
   INNER JOIN Employees EA ON EA.EmployeeID=N.NoteAssignee AND N.NoteAssignee=@AssigneeID  
   LEFT JOIN Employees EMP1 on EMP1.EmployeeID=N.CreatedBy  
   LEFT JOIN Employees EMP2 on EMP2.EmployeeID=N.NoteAssignedBy          
 --LEFT JOIN ReferralPayorMappings RPM on Rpm.ReferralID=REF.ReferralID                
    --LEFT JOIN Payors P on P.PayorID=RPM.PayorID  
    --LEFT JOIN Agencies A on A.AgencyID=REF.AgencyID              
    --LEFT JOIN AgencyLocations AL ON  AL.AgencyLocationID=REF.AgencyLocationID              
    --LEFT JOIN CaseManagers C on c.CaseManagerID= REF.CaseManagerID                
    --LEFT JOIN Employees EMP ON EMP.EmployeeID=RIM.Assignee          
       
      
 WHERE N.IsDeleted=0 AND N.MarkAsComplete=0 AND          
    REF.IsDeleted=0 --AND  RIM.IsDeleted=0   
  --AND (RPM.IsActive is NULL OR RPM.IsActive=1 )            
  )     
  AS T)  AS T1 )      
      
SELECT * FROM CTEDashboardAnsellCaseyReview WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                   
END   