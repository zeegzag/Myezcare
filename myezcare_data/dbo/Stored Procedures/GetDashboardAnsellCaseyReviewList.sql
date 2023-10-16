-- EXEC GetDashboardAnsellCaseyReviewList @SortExpression = 'ClientName', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @AssigneeID = '10080'
CREATE PROCEDURE[dbo].[GetDashboardAnsellCaseyReviewList]               
@AssigneeID bigint,        
@SortExpression NVARCHAR(100),                  
@SortType NVARCHAR(10),                
@FromIndex INT,                
@PageSize INT    
    
AS                
BEGIN                
                
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
      
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedByName' THEN CAST(t.CreatedByName AS varchar) END END ASC,                                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedByName' THEN CAST(t.CreatedByName AS varchar) END END DESC,      

	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AssignedByName' THEN CAST(t.AssignedByName AS varchar) END END ASC,                                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AssignedByName' THEN CAST(t.AssignedByName AS varchar) END END DESC,      
      
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN  CONVERT(date, t.CreatedDate, 105) END END ASC,                                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN  CONVERT(date, t.CreatedDate, 105) END END DESC    ,

	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AssessmentDate' THEN  CONVERT(date, t.AssessmentDate, 105) END END ASC,                                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AssessmentDate' THEN  CONVERT(date, t.AssessmentDate, 105) END END DESC,   
	
	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'OverAllAverage' THEN CAST(t.OverAllAverage AS float) END END ASC,                                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'OverAllAverage' THEN CAST(t.OverAllAverage AS float) END END DESC  
    
   ) AS ROW,                
   t.* FROM                
   (        SELECT REF.ReferralID, REF.ClientID,REF.CISNumber,REF.AHCCCSID,REF.FirstName,REF.LastName,Gender,dbo.GetAge(REF.Dob) as Age ,            
RIM.ReferralAssessmentID,RIM.CreatedDate, RIM.AssessmentDate, 
  
 ( 
 (RIM.Permanency+RIM.DailyLiving+RIM.SelfCare+RelationshipsAndCommunication+RIM.HousingAndMoneyManagement+RIM.WorkAndStudyLife+
  RIM.CareerAndEducationPlanning+RIM.LookingForward)/8) AS OverAllAverage,

  EMP1.LastName + ', ' + EMP1.FirstName AS CreatedByName,
  EMP2.LastName + ', ' + EMP2.FirstName AS AssignedByName,      
 A.NickName AS AgencyName,Al.LocationName as AgencyLocationName,C.LastName+', '+C.FirstName as CaseManager,P.PayorName                
 FROM Referrals REF   
               
      INNER JOIN ReferralAssessmentReview RIM  ON REF.ReferralID=RIM.ReferralID                
      LEFT JOIN ReferralPayorMappings RPM on Rpm.ReferralID=REF.ReferralID              
      LEFT JOIN Payors P on P.PayorID=RPM.PayorID
      LEFT JOIN Agencies A on A.AgencyID=REF.AgencyID            
      LEFT JOIN AgencyLocations AL ON  AL.AgencyLocationID=REF.AgencyLocationID            
      LEFT JOIN CaseManagers C on c.CaseManagerID= REF.CaseManagerID              
      --LEFT JOIN Employees EMP ON EMP.EmployeeID=RIM.Assignee        
      LEFT JOIN Employees EMP1 on EMP1.EmployeeID=RIM.CreatedBy
	  LEFT JOIN Employees EMP2 on EMP2.EmployeeID=RIM.AssignedBy        
    where         
    REF.IsDeleted=0 --AND  RIM.IsDeleted=0 
	 AND (RPM.IsActive is NULL OR RPM.IsActive=1 )          
        AND RIM.Assignee = @AssigneeID AND RIM.MarkAsComplete=0
  )   
  AS T)  AS T1 )    
    
SELECT * FROM CTEDashboardAnsellCaseyReview WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                 
END 
