----EXEC API_SetEmployeeVisitTime @ScheduleID = N'2', @ReferralID = N'1', @EmployeeId = N'2', @Type = N'2', @IsByPass = N'False', @IsCaseManagement = N'False', @IsApprovalRequired = N'False', @ActionTaken = NULL, @EarlyClockOutComment = NULL, @ByPassReason = NULL, @Lat = N'0.0', @Long = N'0.0', @BeforeClockIn = N'-15', @ClockInTime = N'2019-06-20 11:10:12 PM', @ClockOutTime = N'2019-06-20 11:10:12 PM', @Distance = N'50', @SystemID = N'75.83.81.14'


----Exec API_SetEmployeeVisitTime @ScheduleID=762, @EmployeeID=135, @Type=2, @Lat=-3.29431266523898, @Long=55.9271035250276,@ClockInTime=null,@ClockOutTime=null,@SystemID=null                            
CREATE PROCEDURE [dbo].[API_SetEmployeeVisitTime]                  
 @ScheduleID BIGINT,                        
 @ReferralID BIGINT,                        
 @EmployeeID BIGINT,                        
 @Type INT,                        
 @IsByPass BIT,          
 @IsCaseManagement BIT,  
 @IsApprovalRequired BIT,          
 @ActionTaken NVARCHAR(1000),          
 @EarlyClockOutComment NVARCHAR(MAX),                      
 @ByPassReason NVARCHAR(MAX),                      
 @Lat DECIMAL(10,7),                        
 @Long DECIMAL(10,7),                    
 @BeforeClockIn INT,                    
 @ClockInTime DateTime,                        
 @ClockOutTime DateTime,                        
 @Distance int,                        
 @SystemID VARCHAR(100)                        
AS                                                
BEGIN                                              
DECLARE @g geography;                    
DECLARE @h geography;                    
DECLARE @ContactLatitude float;                                    
DECLARE @ContactLongitude float;                    
DECLARE @EmployeeVisitID BIGINT;      
DECLARE @CheckIsApprovalRequired BIT;      
DECLARE @CheckActionTaken INT;      
DECLARE @CaretypeId bigint    
 SELECT @CARETYPEID=CARETYPEID FROM SCHEDULEMASTERS WHERE SCHEDULEID=@SCHEDULEID    
                   
--Required Task Added or Not Check                  
 If (Exists(                  
 Select RM.ReferralTaskMappingID from ReferralTaskMappings RM                  
 INNER JOIN VisitTasks V ON V.VisitTaskID=RM.VisitTaskID                  
 Where ReferralID=@ReferralID AND V.VisitTaskType='Task' AND RM.IsRequired=1 AND RM.IsDeleted=0 AND (@CaretypeId IS NULL OR  V.CARETYPE=@CARETYPEID )
 AND RM.ReferralTaskMappingID NOT IN                  
 (                  
 Select evn.ReferralTaskMappingID from EmployeeVisits ev                  
 INNER JOIN EmployeeVisitNotes evn ON evn.EmployeeVisitID=ev.EmployeeVisitID                  
 WHERE ev.ScheduleID=@ScheduleID                  
 )) AND @Type=2)                  
BEGIN                  
 SELECT -4 RETURN;                  
END                  
                  
 --Early ClockIn Check                   
 IF((SELECT DATEDIFF(MINUTE,(SELECT StartDate FROM ScheduleMasters WHERE ScheduleID=@ScheduleID),@ClockInTime)) < @BeforeClockIn)                    
 BEGIN                    
 SELECT -3 RETURN;                    
 END                    
                    
 SELECT @ContactLatitude = c.Latitude, @ContactLongitude=c.Longitude FROM ScheduleMasters sm                                              
 INNER JOIN ContactMappings cm ON cm.ReferralID=sm.ReferralID                                              
 INNER JOIN Contacts c ON c.ContactID=cm.ContactID WHERE sm.ScheduleID=@ScheduleID AND cm.ContactTypeID=1                                  
                   
 --Missing Patient Coordinates                                    
 IF((@ContactLatitude IS NULL OR @ContactLongitude IS NULL) AND @IsByPass=0)                        
 BEGIN                            
 SELECT -2 RETURN;                            
END                            
                            
 SELECT @g=dbo.GetGeoFromLatLng(@ContactLatitude,@ContactLongitude)                    
 SELECT @h = dbo.GetGeoFromLatLng(@Lat,@Long)                                    
       
 SELECT TOP 1 @EmployeeVisitID=EmployeeVisitID,@CheckActionTaken=ActionTaken,@CheckIsApprovalRequired=IsApprovalRequired FROM EmployeeVisits WHERE ScheduleID=@ScheduleID      
                                        
 IF((SELECT @g.STDistance(@h))<@Distance OR @IsByPass=1)                                   
 BEGIN                                        
 IF(@EmployeeVisitID IS NOT NULL)                             
  BEGIN                                        
   UPDATE EmployeeVisits SET                                
   ClockInTime= CASE WHEN @Type=1 THEN @ClockInTime ELSE ClockInTime END,       
   ClockOutTime= CASE WHEN @Type=2 THEN @ClockOutTime ELSE ClockOutTime END,                        
   IsByPassClockOut=@IsByPass,Latitude=@Lat,Longitude=@Long,      
   IsApprovalRequired=CASE WHEN @IsByPass=1 THEN @IsApprovalRequired ELSE @CheckIsApprovalRequired END,      
   ActionTaken=CASE WHEN @IsByPass=1 THEN @ActionTaken ELSE @CheckActionTaken END,      
   EarlyClockOutComment=@EarlyClockOutComment,                      
   ByPassReasonClockOut=@ByPassReason                      
   WHERE ScheduleID=@ScheduleID  
     
   IF(@IsCaseManagement=1)  
   BEGIN  
   IF(@Type=1)
   UPDATE ScheduleMasters SET StartDate=@ClockInTime WHERE ScheduleID=@ScheduleID  
   IF(@Type=2)
   UPDATE ScheduleMasters SET EndDate=@ClockOutTime WHERE ScheduleID=@ScheduleID
 --UPDATE ScheduleMasters SET StartDate=CASE WHEN @Type=1 THEN @ClockInTime ELSE NULL END,                                        
 --EndDate=CASE WHEN @Type=2 THEN @ClockOutTime ELSE NULL END  
 --WHERE ScheduleID=@ScheduleID  
   END  
    
   DECLARE @Count INT;    
   SET @Count=(SELECT COUNT(ReferralTaskMappingID) FROM ReferralTaskMappings RTM        
 INNER JOIN VisitTasks V ON V.VisitTaskID=RTM.VisitTaskID AND V.VisitTaskType='Conclusion'        
  WHERE RTM.ReferralID=@ReferralID AND RTM.IsDeleted=0)    
    
 IF @Count=0    
  UPDATE EmployeeVisits SET SurveyCompleted=1 WHERE EmployeeVisitID=@EmployeeVisitID    
    
    
    
  END                                        
  ELSE                                        
  BEGIN                                        
   INSERT INTO EmployeeVisits (ScheduleID,ClockInTime,ClockOutTime,SystemID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,IsByPassClockIn,ByPassReasonClockIn,Latitude,Longitude,IsApprovalRequired, ActionTaken) VALUES           
   (@ScheduleID,                                        
   CASE WHEN @Type=1 THEN @ClockInTime ELSE NULL END,                                        
   CASE WHEN @Type=2 THEN @ClockOutTime ELSE NULL END,                        
   @SystemID,GETUTCDATE(),@EmployeeID,GETUTCDATE(),@EmployeeID,@IsByPass,@ByPassReason,@Lat,@Long,@IsApprovalRequired,@ActionTaken)  
     
   IF(@IsCaseManagement=1)  
   BEGIN  
 UPDATE ScheduleMasters SET StartDate=CASE WHEN @Type=1 THEN @ClockInTime ELSE NULL END,                                        
 EndDate=CASE WHEN @Type=2 THEN @ClockOutTime ELSE NULL END  
 WHERE ScheduleID=@ScheduleID  
   END  
  
             
  END                                        
 SELECT 1;                                            
 END                                            
 ELSE                                            
 BEGIN                                            
 Select -1;                                            
 END                              
END
