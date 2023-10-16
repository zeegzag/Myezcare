--EXEC GetDashboardInternalMessgaeList @SortExpression = 'ClientName', @SortType = 'ASC', @FromIndex = '1', @PageSize = '10', @InternalMessageAssigneeID = '1', @AssigneeID = '0'    
-- exec GetDashboardInternalMessgaeList 1,0,'ClientName','ASC',1,10      
      
CREATE procedure [dbo].[GetDashboardInternalMessgaeList]                     
@InternalMessageAssigneeID bigint,              
@AssigneeID bigint,              
@SortExpression NVARCHAR(100),                        
@SortType NVARCHAR(10),                      
@FromIndex INT,                      
@PageSize INT          
          
AS                      
BEGIN           

 DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()  
 
;WITH CTEReferralInternalMessage AS                      
 (                       
  SELECT *,COUNT(T1.ReferralID) OVER() AS Count FROM                       
  (                      
   SELECT ROW_NUMBER() OVER (ORDER BY                       
                  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralInternalMessageID' THEN t.ReferralID END END ASC,                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralInternalMessageID' THEN t.ReferralID END END DESC,                 
               
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
            
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Note' THEN CAST(t.Note AS varchar) END END ASC,                                            
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Note' THEN CAST(t.Note AS varchar) END END DESC,            
            
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Status' THEN CAST(t.IsResolved AS bit) END END ASC,                                            
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Status' THEN CAST(t.IsResolved AS bit) END END DESC,            
            
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedByName' THEN CAST(t.CreatedByName AS varchar) END END ASC,                                            
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedByName' THEN CAST(t.CreatedByName AS varchar) END END DESC,            
            
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN  CONVERT(date, t.CreatedDate, 105) END END ASC,                                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN  CONVERT(date, t.CreatedDate, 105) END END DESC      
   ) AS ROW,                      
   t.* FROM                      
   (        SELECT DISTINCT REF.ReferralID, REF.ClientID,REF.CISNumber,REF.AHCCCSID,REF.FirstName,REF.LastName, FullName = dbo.GetGenericNameFormat(REF.FirstName,REF.MiddleName, REF.LastName,@NameFormat),
   Gender,dbo.GetAge(REF.Dob) as Age ,RIM.ReferralInternalMessageID,RIM.Note,RIM.CreatedDate,RIM.IsResolved, 
   CreatedByName = dbo.GetGenericNameFormat(EMP1.FirstName,EMP1.MiddleName, EMP1.LastName,@NameFormat),A.NickName AS AgencyName,Al.LocationName as AgencyLocationName, 
   CaseManager = dbo.GetGenericNameFormat(C.FirstName,'', C.LastName,@NameFormat)                      
 FROM Referrals REF                       
      LEFT JOIN ReferralInternalMessages RIM  ON REF.referralid=RIM.referralid                      
      --LEFT JOIN ReferralPayorMappings RPM on Rpm.ReferralID=REF.ReferralID    
      --LEFT JOIN Payors P on P.PayorID=RPM.PayorID      
      LEFT JOIN Agencies A on A.AgencyID=REF.AgencyID                  
      LEFT JOIN AgencyLocations AL ON  AL.AgencyLocationID=REF.AgencyLocationID                  
      LEFT JOIN CaseManagers C on c.CaseManagerID= REF.CaseManagerID                    
      LEFT JOIN Employees EMP ON EMP.EmployeeID=RIM.Assignee              
      LEFT JOIN employees EMP1 on EMP1.EmployeeID=RIM.CreatedBy              
    where               
    REF.IsDeleted=0 AND  RIM.IsDeleted=0  
  --AND (RPM.IsActive is NULL OR RPM.IsActive=1)    
        AND RIM.Assignee = @InternalMessageAssigneeID AND RIM.IsResolved=0      
        --AND (@AssigneeID=0 OR REF.Assignee=@AssigneeID)         
  )         
  AS T)  AS T1 )          
          
SELECT * FROM CTEReferralInternalMessage WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                       
END  