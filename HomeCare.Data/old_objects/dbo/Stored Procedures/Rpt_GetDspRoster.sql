--EXEC Rpt_GetDspRoster @AgencyID = '0', @PayorID = '0', @ReferralStartDate = '2017/10/12', @IsDeleted = '0', @MISSING = 'Missing', @EXPIRED = 'Expired', @ReferralStatusIDs = '1,2,3,4,5,6,7,8,9,10,11,,12,13,14'  
CREATE PROCEDURE [dbo].[Rpt_GetDspRoster]                        
 @AgencyID bigint=0,  
 @PayorID bigint=0,                                                   
 @ReferralStartDate date=null,                        
 @ReferralEndDate date=null,                        
 @ServiceStartDate date=null,                        
 @ServiceEndDate date=null,                        
 @IsDeleted bigint =-1,                        
 @MISSING VARCHAR(15),                        
 @EXPIRED VARCHAR(15),  
 @ReferralStatusIDs varchar(max)=null  
AS                                                                
BEGIN                                   
  DECLARE @CurrentDate Date=CAST(GETDATE() AS DATE);   
                                             
  SELECT       
   CASE         
     WHEN  R.RespiteService=1 AND R.LifeSkillsService=1  AND R.CounselingService=1 THEN 'Respite- Facility Based, Life Skill, Counseling'            
     WHEN R.RespiteService=1 AND R.LifeSkillsService=1 THEN  'Respite- Facility Based,  Life Skill'            
     WHEN R.LifeSkillsService=1 AND R.CounselingService=1 THEN  'Life Skill,  Counseling'            
     WHEN R.RespiteService=1 AND R.CounselingService=1 THEN  'Respite- Facility Based,  Counseling'         
     WHEN R.LifeSkillsService=1  THEN  'Life Skill'         
     WHEN R.RespiteService=1 THEN  'Respite- Facility Based'         
     WHEN R.CounselingService=1 THEN  'Counseling'            
   END  as ProgrameName,   
   CONVERT(VARCHAR(10),CONVERT(datetime,R.ReferralDate,1),111) as ReferralDate ,   
    'Yes'  ReferralAccepted,  CONVERT(VARCHAR(10),  
 CONVERT(datetime,R.FirstDOS,1),111) as FirstDOS,   
 --CONVERT(datetime,N.ServiceDate,1),111) as FirstDOS,   
   R.LastName,R.FirstName, CONVERT(VARCHAR(10),CONVERT(datetime,R.Dob,1),111) as Dob,R.CISNumber,   
   A.NickName as Agency,  CM.LastName+' '+CM.FirstName as CaseManager,R.HealthPlan,
   --CASE WHEN R.ClosureDate is null THEN CONVERT(varchar(10),'N/A') ELSE CONVERT(VARCHAR(10),CONVERT(datetime,R.ClosureDate,1),111) END as ClosureDate,  
   R.ClosureDate,  
   R.ClosureReason,R.ClosureReason AS AdditionalNoteForClosureDenial,                     
   dbo.GetDateDifference(N.ServiceDate,R.ClosureDate)as LengthofStay, RPM.PayorID,  
   DATEDIFF(day,R.ReferralDate,N.ServiceDate) AS ReferralResponse , ReferralStatus=RS.Status   
   from Referrals R                        
   left join ReferralStatuses RS on RS.ReferralStatusID=R.ReferralStatusID                 
   left join Agencies A on A.AgencyID=R.AgencyID                        
   left join ReferralPayorMappings RPM on RPM.ReferralID=R.ReferralID AND RPM.IsActive=1  
   left join CaseManagers  CM on CM.CaseManagerID=R.CaseManagerID                  
   left join Notes N ON N.NoteID=(select top 1 NoteID  from Notes                         
  
   where ReferralID=R.ReferralID and IsDeleted=0 order by NoteID ASC)                             
   WHERE  
      --RS.ReferralStatusID IN (1,10) AND                                                    
   ((CAST(@IsDeleted AS BIGINT)=-1) OR R.IsDeleted=@IsDeleted)                                                              
   AND (( CAST(@AgencyID AS BIGINT)=0) OR R.AgencyID = CAST(@AgencyID AS BIGINT))                        
   AND (( CAST(@PayorID AS BIGINT)=0) OR RPM.PayorID = CAST(@PayorID AS BIGINT))                        
   AND ((@ReferralStartDate is null OR R.ReferralDate>= @ReferralStartDate) and (@ReferralEndDate is null OR R.ReferralDate<= @ReferralEndDate))                        
   AND ((@ServiceStartDate is null OR N.ServiceDate >= @ServiceStartDate) and (@ServiceEndDate is null OR N.ServiceDate <= @ServiceEndDate))                        
   AND (@ReferralStatusIDs is null OR R.ReferralStatusID in (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ReferralStatusIDs)) )  
        
  ORDER BY A.NickName ASC  
                                               
  SELECT       
   CASE         
     WHEN  R.RespiteService=1 AND R.LifeSkillsService=1  AND R.CounselingService=1 THEN 'Respite- Facility Based, Life Skill, Counseling'            
     WHEN R.RespiteService=1 AND R.LifeSkillsService=1 THEN  'Respite- Facility Based,  Life Skill'            
     WHEN R.LifeSkillsService=1 AND R.CounselingService=1 THEN  'Life Skill,  Counseling'            
     WHEN R.RespiteService=1 AND R.CounselingService=1 THEN  'Respite- Facility Based,  Counseling'         
     WHEN R.LifeSkillsService=1  THEN  'Life Skill'         
     WHEN R.RespiteService=1 THEN  'Respite- Facility Based'         
     WHEN R.CounselingService=1 THEN  'Counseling'            
   END  as ProgrameName,   
   CONVERT(VARCHAR(10),CONVERT(datetime,R.ReferralDate,1),111) as ReferralDate ,   
    'Yes'  ReferralAccepted, CONVERT(VARCHAR(10),  
 CONVERT(datetime,R.FirstDOS,1),111) as FirstDOS,   
 --CONVERT(datetime,N.ServiceDate,1),111) as FirstDOS,   
   R.LastName,R.FirstName, CONVERT(VARCHAR(10),CONVERT(datetime,R.Dob,1),111) as Dob,R.CISNumber,   
   A.NickName as Agency,  CM.LastName+' '+CM.FirstName as CaseManager,R.HealthPlan,
   --CASE WHEN R.ClosureDate is null THEN CONVERT(varchar(10),'N/A') ELSE CONVERT(VARCHAR(10),CONVERT(datetime,R.ClosureDate,1),111) END as ClosureDate,  
   R.ClosureDate,  
   R.ClosureReason,R.ClosureReason AS AdditionalNoteForClosureDenial,                     
   dbo.GetDateDifference(N.ServiceDate,R.ClosureDate)as LengthofStay, RPM.PayorID,  
   DATEDIFF(day,R.ReferralDate,N.ServiceDate) AS ReferralResponse, ReferralStatus=RS.Status    
    
   from Referrals R                        
   left join ReferralStatuses RS on RS.ReferralStatusID=R.ReferralStatusID                      
   left join Agencies A on A.AgencyID=R.AgencyID                        
   left join ReferralPayorMappings RPM on RPM.ReferralID=R.ReferralID  AND RPM.IsActive=1  
   left join CaseManagers  CM on CM.CaseManagerID=R.CaseManagerID                  
   left join Notes N ON N.NoteID=(select top 1 NoteID from Notes where ReferralID=R.ReferralID and IsDeleted=0 order by NoteID DESC)                             
   WHERE  
     RS.ReferralStatusID IN (4) AND                                          
   ((CAST(@IsDeleted AS BIGINT)=-1) OR R.IsDeleted=@IsDeleted)                                                              
   AND (( CAST(@AgencyID AS BIGINT)=0) OR R.AgencyID = CAST(@AgencyID AS BIGINT))                        
   AND (( CAST(@PayorID AS BIGINT)=0) OR RPM.PayorID = CAST(@PayorID AS BIGINT))                        
   AND ((@ReferralStartDate is null OR R.ReferralDate>= @ReferralStartDate) and (@ReferralEndDate is null OR R.ReferralDate<= @ReferralEndDate))                        
   AND ((@ServiceStartDate is null OR N.ServiceDate >= @ServiceStartDate) and (@ServiceEndDate is null OR N.ServiceDate <= @ServiceEndDate))  
   --AND (( CAST(@ReferralStatusID AS BIGINT)=0) OR r.ReferralStatusID = CAST(@ReferralStatusID AS BIGINT))  
                        
  ORDER BY A.NickName ASC    
  
END
