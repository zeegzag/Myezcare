-- EXEC [GetDashboardInternalMissingandExpireDocumentList] @SortExpression = 'AgencyName', @SortType = 'ASC', @FromIndex = '1', @PageSize = '100'          
CREATE procedure [dbo].[GetDashboardInternalMissingandExpireDocumentList]               
@AssigneeID bigint,              
@SortExpression NVARCHAR(100),                      
@SortType NVARCHAR(10),                    
@FromIndex INT,                    
@PageSize INT   ,      
@ReferralStatusIds varchar(100)                 
AS                    
BEGIN                    
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()                   
;WITH CTEDashboardInternalMissingandExpireDocumentList AS                    
 (                     
  SELECT *,COUNT(T1.ReferralID) OVER() AS Count FROM                     
  (                    
   SELECT ROW_NUMBER() OVER (ORDER BY                     
        --Referrals        
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
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CaseManager' THEN CONVERT(varchar(50),t.CaseManager) END END DESC         
            
   
    
    ) AS ROW,t.* FROM           
   (                   
   SELECT REF.ReferralID,REF.ClientID,REF.CISNumber,REF.AHCCCSID,REF.FirstName,REF.FirstName as TF,REF.LastName,FullName = dbo.GetGenericNameFormat(REF.FirstName,REF.MiddleName, REF.LastName,@NameFormat),Gender,dbo.GetAge(REF.Dob) as Age ,                
  A.NickName AS AgencyName,Al.LocationName as AgencyLocationName,              
  CaseManager = dbo.GetGenericNameFormat(C.FirstName,'', C.LastName,@NameFormat),P.PayorName            
   FROM Referrals REF                        
      LEFT JOIN ReferralPayorMappings RPM on Rpm.ReferralID=REF.ReferralID                  
      LEFT JOIN Payors P on P.PayorID=RPM.PayorID                  
      LEFT JOIN Agencies A on A.AgencyID=REF.AgencyID                
   LEFT JOIN AgencyLocations AL ON  AL.AgencyLocationID=REF.AgencyLocationID                              
      LEFT JOIN CaseManagers C on c.CaseManagerID= ref.CaseManagerID                  
  where               
  REF.IsDeleted=0 AND  (RPM.IsActive is NULL OR RPM.IsActive=1 ) AND (@AssigneeID=0 OR REF.Assignee=@AssigneeID)         
  AND              
  (    
 CareConsent=0 OR SelfAdministrationofMedication=0 OR HealthInformationDisclosure=0 OR AdmissionRequirements=0 OR AdmissionOrientation=0   
 OR ZarephathCrisisPlan='N' OR  (PHI=0 OR (PHI=1 AND PHIExpirationDate<getdate()))  
 OR (  
    (RespiteService=0 AND  LifeSkillsService=0 AND CounselingService=0 AND ConnectingFamiliesService=0)  
  OR (RespiteService=1 AND (ZSPRespite=0 OR (ZSPRespite=1 AND ZSPRespiteExpirationDate<getdate())) )  
  OR (LifeSkillsService=1 AND (ZSPLifeSkills=0 OR (ZSPLifeSkills=1 AND ZSPLifeSkillsExpirationDate<getdate())) )  
  OR (CounselingService=1 AND (ZSPCounselling=0 OR (ZSPCounselling=1 AND ZSPCounsellingExpirationDate<getdate())) ) )  
  OR (ConnectingFamiliesService=1 AND (ZSPConnectingFamilies=0 OR (ZSPConnectingFamilies=1 AND ZSPConnectingFamiliesExpirationDate < getdate())))   
  )       
  AND REF.ReferralStatusID in (select CAST(Val AS BIGINT) from GetCSVTable(@ReferralStatusIds))        
            
  ) AS T) AS T1)                    
                     
 SELECT * FROM CTEDashboardInternalMissingandExpireDocumentList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                     
END  