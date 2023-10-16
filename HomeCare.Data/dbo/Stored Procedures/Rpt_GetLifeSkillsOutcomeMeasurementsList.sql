CREATE PROCEDURE  [dbo].[Rpt_GetLifeSkillsOutcomeMeasurementsList] 
 @RegionID bigint=0,                              
 @StartDate date=null,                                      
 @EndDate date=null,                                      
 @ClientName varchar(100)= NULL,                                       
 @ServiceID int = -1   
AS                                            
BEGIN                                   
 SELECT  R.LastName+', '+R.FirstName as ClientName,WorkCommunity,ScheduledAppointment,AskForHelp,CommunicateEffectively,PositiveBehavior,QualityFriendship,
RespectOther,StickUp,LifeSkillGoals,StayOutOfTrouble,SuccessfulInSchool,SuccessfulInLiving,AdulthoodNeeds                  
  from Referrals R
   left join Regions RG on R.RegionID=RG.RegionID
   left join ReferralOutcomeMeasurements ROM on ROM.ReferralID=R.ReferralID
   WHERE
   (( CAST(@RegionID AS BIGINT)=0) OR R.RegionID= CAST(@RegionID AS BIGINT))                                      
   AND ((@StartDate is null OR ROM.OutcomeMeasurementDate>= @StartDate) and (@EndDate is null OR ROM.OutcomeMeasurementDate<= @EndDate))                                
   AND ((@ClientName IS NULL OR LEN(@ClientName)=0) 
			OR R.FirstName LIKE '%' + @ClientName + '%' 
			OR R.LastName LIKE '%' + @ClientName + '%' 
			OR R.FirstName+' '+r.LastName LIKE '%' + @ClientName + '%' 
			OR R.LastName+' '+R.FirstName LIKE '%' + @ClientName + '%'
			OR R.LastName+ ', '+R.FirstName LIKE '%' + @ClientName + '%'
			OR R.FirstName+ ', '+R.LastName LIKE '%' + @ClientName + '%'			
       )
	   AND (( CAST(@ServiceID AS bigint)=-1)  
   OR   (CAST(@ServiceID AS bigint) = 0 and r.RespiteService = 1)  
   OR   (CAST(@ServiceID AS bigint) = 1 and r.LifeSkillsService = 1)  
   OR   (CAST(@ServiceID AS bigint) = 2 and r.CounselingService = 1))  
  
   ORDER BY R.LastName ASC  
END