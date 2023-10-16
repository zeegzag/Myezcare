-- EXEC Rpt_GetLSTeamMemberCaseload 0,NULL,NULL,'',1  
CREATE PROCEDURE  [dbo].[Rpt_GetLSTeamMemberCaseload]                                         
 @RegionID bigint=0,                                
 @StartDate date=null,                                        
 @EndDate date=null,                                        
 @ClientName varchar(100) = NULL,                                         
 @ServiceID int = -1    
AS                                              
BEGIN                                     
 SELECT  R.LastName, R.FirstName,dbo.GetAge(r.Dob) Age,CONVERT(VARCHAR(10),CONVERT(datetime,r.Dob ,1),111)as Birthdate,      
 CM.FirstName+' '+ CM.LastName as Casemanager, CONVERT(VARCHAR(10),CONVERT(datetime,R.ZSPLifeSkillsExpirationDate ,1),111) AS ServicePlanDue,    
 CONVERT(VARCHAR(10),CONVERT(datetime,R.ACAssessmentExpirationDate ,1),111) AS ACServicePlanDue,    
 (select CONVERT(VARCHAR(10),CONVERT(datetime,MAX(OutcomeMeasurementDate) ,1),111) from ReferralOutcomeMeasurements where ReferralID=R.ReferralID) as OMLastCompleted, PCNT.FirstName+' '+PCNT.LastName as Parent   
                                      
   from Referrals R                                              
   left join CaseManagers CM on CM.CaseManagerID=R.CaseManagerID                                              
   left join Regions RG on R.RegionID=RG.RegionID    
   left join ContactMappings PCT on PCT.ReferralID=r.ReferralID AND PCT.ContactTypeID=1             
   left join Contacts PCNT on PCNT.ContactID=PCT.ContactID                                
   WHERE                                         
   R.IsDeleted=0  AND (( CAST(@RegionID AS BIGINT)=0) OR R.RegionID= CAST(@RegionID AS BIGINT))                                        
   AND ((@StartDate is null OR R.ReferralDate>= @StartDate) and (@EndDate is null OR R.ReferralDate<= @EndDate))                                  
   AND   
   ((@ClientName IS NULL OR LEN(r.LastName)=0)       
    OR    
  ( (r.FirstName LIKE '%'+@ClientName+'%' )OR      
    (r.LastName  LIKE '%'+@ClientName+'%') OR      
    (r.FirstName +' '+r.LastName like '%'+@ClientName+'%') OR      
    (r.LastName +' '+r.FirstName like '%'+@ClientName+'%') OR      
    (r.FirstName +', '+r.LastName like '%'+@ClientName+'%') OR      
    (r.LastName +', '+r.FirstName like '%'+@ClientName+'%'))    
  )     
   AND (( CAST(@ServiceID AS bigint)=-1)    
   OR   (CAST(@ServiceID AS bigint) = 0 and r.RespiteService = 1)    
   OR   (CAST(@ServiceID AS bigint) = 1 and r.LifeSkillsService = 1)    
   OR   (CAST(@ServiceID AS bigint) = 2 and r.CounselingService = 1))    
   ORDER BY R.LastName ASC    
          
END 
