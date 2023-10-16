CREATE PROCEDURE [dbo].[GetDashboardResolvedInternalMessgaeList]                   
@InternalMessageAssigneeID bigint,            
@AssigneeID bigint,            
@SortExpression NVARCHAR(100),                      
@SortType NVARCHAR(10),                    
@FromIndex INT,                    
@PageSize INT        
        
AS                    
BEGIN                    
                    
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
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN  CONVERT(date, t.CreatedDate, 105) END END DESC ,  
   
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ResolvedByName' THEN CAST(t.ResolvedByName AS varchar) END END ASC,                                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ResolvedByName' THEN CAST(t.ResolvedByName AS varchar) END END DESC,          
          
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ResolveDate' THEN  CONVERT(date, t.ResolveDate, 105) END END ASC,                                    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ResolveDate' THEN  CONVERT(date, t.ResolveDate, 105) END END DESC   
           
   ) AS ROW,                    
   t.* FROM                    
   (        SELECT REF.ReferralID, REF.ClientID,REF.CISNumber,REF.AHCCCSID,REF.FirstName,REF.LastName,Gender,dbo.GetAge(REF.Dob) as Age ,                
  RIM.ReferralInternalMessageID,RIM.Note,RIM.CreatedDate,RIM.IsResolved,  
  EMP1.LastName + ', ' + EMP1.FirstName AS CreatedByName, EMP.LastName + ', ' + EMP.FirstName AS ResolvedByName, RIM.ResolveDate ,        
  A.NickName AS AgencyName,Al.LocationName as AgencyLocationName,C.LastName+', '+C.FirstName as CaseManager,
  --P.PayorName,
  RIM.MarkAsResolvedRead , RIM.ResolvedComment                  
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
	--AND (RPM.IsActive is NULL OR RPM.IsActive=1 ) 
	AND RIM.MarkAsResolvedRead=0 AND RIM.CreatedBy = @InternalMessageAssigneeID --AND RIM.IsResolved=1     
        AND (@AssigneeID=0 OR REF.CreatedBy=@AssigneeID)       
  ) AS T)  AS T1 )        
        
SELECT * FROM CTEReferralInternalMessage WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                     
END        
--select *  from ReferralInternalMessages
