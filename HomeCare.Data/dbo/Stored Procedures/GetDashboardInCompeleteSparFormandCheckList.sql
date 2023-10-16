CREATE procedure [dbo].[GetDashboardInCompeleteSparFormandCheckList]        
@AssigneeID bigint,              
@SortExpression NVARCHAR(100),            
@SortType NVARCHAR(10),                    
@FromIndex INT,                    
@PageSize INT,      
@ReferralStatusIds varchar(100)      
AS                    
BEGIN                    
   DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()                  
;WITH CTEReferralInternalSparForm AS                    
 (                     
  SELECT *,COUNT(T1.ReferralID) OVER() AS Count FROM                     
  (                    
   SELECT ROW_NUMBER() OVER (ORDER BY              
         
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralID' THEN CAST(t.ReferralID AS bigint) END END ASC,              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralID' THEN CAST(t.ReferralID AS bigint) END END DESC,            
         
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
         
             
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PayorName' THEN CONVERT(varchar(100),t.PayorName) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PayorName' THEN CONVERT(varchar(100),t.PayorName) END END DESC,        
         
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AgencyName' THEN CONVERT(varchar(50),t.AgencyName) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AgencyName' THEN CONVERT(varchar(50),t.AgencyName) END END DESC,        
         
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AgencyLocationName' THEN CONVERT(varchar(100),t.AgencyLocationName) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AgencyLocationName' THEN CONVERT(varchar(100),t.AgencyLocationName) END END DESC,        
         
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CaseManager' THEN CONVERT(varchar(50),t.CaseManager) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CaseManager' THEN CONVERT(varchar(50),t.CaseManager) END END DESC,        
         
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'IsSparFormCompleted' THEN  CAST(t.IsSparFormCompleted AS bit) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'IsSparFormCompleted' THEN  CAST(t.IsSparFormCompleted as bit) END END DESC,          
                
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'IsCheckListCompleted' THEN  CAST(t.IsCheckListCompleted AS bit) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'IsCheckListCompleted' THEN  CAST(t.IsCheckListCompleted as bit) END END DESC            
         
         
  ) AS ROW,                    
   t.* FROM                    
   (                    
      SELECT REF.ReferralID, REF.ClientID,REF.CISNumber,REF.AHCCCSID,REF.FirstName,REF.LastName,FullName = dbo.GetGenericNameFormat(REF.FirstName,REF.MiddleName, REF.LastName,@NameFormat),Gender,dbo.GetAge(REF.Dob) as Age ,              
      IsSparFormCompleted=ISNULL(RSF.IsSparFormCompleted,0),IsCheckListCompleted=ISNULL(RCL.IsCheckListCompleted,0),            
      A.NickName AS AgencyName,Al.LocationName as AgencyLocationName,            
      CaseManager = dbo.GetGenericNameFormat(C.FirstName,'', C.LastName,@NameFormat),
	  P.PayorName                  
 FROM Referrals REF                   
      LEFT JOIN ReferralSparForms RSF on RSF.ReferralID=REF.ReferralID                  
      LEFT JOIN ReferralCheckLists RCL on RCL.ReferralID=REF.ReferralID                  
      LEFT JOIN ReferralPayorMappings RPM on Rpm.ReferralID=REF.ReferralID                
      LEFT JOIN Payors P on P.PayorID=RPM.PayorID                
      LEFT JOIN Agencies A on A.AgencyID=REF.AgencyID              
      LEFT JOIN AgencyLocations AL ON  AL.AgencyLocationID=REF.AgencyLocationID              
      LEFT JOIN CaseManagers C on c.CaseManagerID= ref.CaseManagerID                
    where             
   ((RCL.IsCheckListCompleted IS NULL OR RCL.IsCheckListCompleted=0) OR (RSF.IsSparFormCompleted IS NULL OR RSF.IsSparFormCompleted=0))            
    AND REF.IsDeleted=0 AND (RPM.IsActive is NULL OR RPM.IsActive=1 )        
    AND (@AssigneeID=0 OR REF.Assignee=@AssigneeID)        
    AND (REF.ReferralStatusID in (select CAST(Val AS BIGINT) from GetCSVTable(@ReferralStatusIds)))  
    )      
    AS T)    
    AS T1)                         
 SELECT * FROM CTEReferralInternalSparForm  WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                     
END  