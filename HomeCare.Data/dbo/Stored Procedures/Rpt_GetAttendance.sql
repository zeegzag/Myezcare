CREATE PROCEDURE  [dbo].[Rpt_GetAttendance]                
 @RegionID bigint=0,                            
 @AgencyID bigint=0,                             
 @ReferralStatusID bigint =0,                            
 @StartDate date=null,                                    
 @EndDate date=null,                                    
 @AssigneeID bigint= 0,                            
 @ClientName varchar(100) = NULL,                                     
 @PayorID bigint = 0,                            
 @NotifyCaseManagerID int = -1,                                    
 @ChecklistID INT = -1,                                    
 @ClinicalReviewID INT = -1,                                    
 @CaseManagerID int = 0,                                    
 @ServiceID int = -1,                                          
 @AgencyLocationID bigint =0,                                  
 @IsSaveAsDraft int=-1,                                    
 @AHCCCSID varchar(20)=null,                                    
 @CISNumber varchar(20)=null ,                  
 @IsDeleted int=-1,        
 @ScheduleStatusID bigint=0                     
AS                                          
BEGIN                                 
 SELECT  R.LastName+', '+R.FirstName as ClientName,R.AHCCCSID as AHCCCS_ID, A.NickName,         
 CONVERT(VARCHAR(10),CONVERT(datetime,SC.StartDate,1),111)Schedule_StartDate,                
 CONVERT(VARCHAR(10),CONVERT(datetime,SC.EndDate ,1),111) as Schedule_EndDate,                
  isnull(ss.ScheduleStatusName ,'Not Scheduled')as Status,SC.CancelReason,                
 CONVERT(VARCHAR(10),CONVERT(datetime,R.LastAttendedDate ,1),111) AS Last_Attended_Date                
  from Referrals R                
   left join ScheduleMasters SC on SC.ReferralID=R.ReferralID                
   left join ScheduleStatuses SS on SS.ScheduleStatusID=SC.ScheduleStatusID                                        
   inner join Agencies A on A.AgencyID=R.AgencyID                
   left join AgencyLocations AL on AL.AgencyLocationID=R.AgencyLocationID      
             
   WHERE((CAST(@IsDeleted AS BIGINT)=-1) OR R.IsDeleted=@IsDeleted)                
   AND (( CAST(@AgencyID AS BIGINT)=0) OR R.AgencyID = CAST(@AgencyID AS BIGINT))                
   AND (( CAST(@RegionID AS BIGINT)=0) OR R.RegionID= CAST(@RegionID AS BIGINT))                
   AND (( CAST(@ScheduleStatusID AS BIGINT)=0) OR Sc.ScheduleStatusID= CAST(@ScheduleStatusID AS BIGINT))                
   AND ((@StartDate is null OR SC.StartDate>= @StartDate) and (@EndDate is null OR SC.EndDate<= @EndDate))        
  ORDER BY R.LastName,Sc.StartDate ASC          
END