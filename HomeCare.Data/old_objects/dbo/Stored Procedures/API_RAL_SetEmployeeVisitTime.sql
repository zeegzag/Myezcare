  
----EXEC API_SetEmployeeVisitTime @ScheduleID = N'2', @ReferralID = N'1', @EmployeeId = N'2', @Type = N'2', @IsByPass = N'False', @IsCaseManagement = N'False', @IsApprovalRequired = N'False', @ActionTaken = NULL, @EarlyClockOutComment = NULL, @ByPassReason = NULL, @Lat = N'0.0', @Long = N'0.0', @BeforeClockIn = N'-15', @ClockInTime = N'2019-06-20 11:10:12 PM', @ClockOutTime = N'2019-06-20 11:10:12 PM', @Distance = N'50', @SystemID = N'75.83.81.14'                    
          
  
CREATE PROCEDURE [dbo].[API_RAL_SetEmployeeVisitTime]                                      
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
 @BeforeClockIn nvarchar(100),                                        
 @ClockInTime DateTime,                                            
 @ClockOutTime DateTime,                                            
 @Distance int,                                            
 @SystemID VARCHAR(100),               
 @IsEarlyClockIn bit=null,              
 @EarlyClockInComment NVARCHAR(MAX)=null                                            
AS                                                                    
BEGIN                                                                  
DECLARE @g geography;                                        
DECLARE @h geography;                                        
DECLARE @ContactLatitude float;                                                        
DECLARE @ContactLongitude float;                                        
DECLARE @EmployeeVisitID BIGINT;                          
DECLARE @CheckIsApprovalRequired BIT;                          
DECLARE @CheckActionTaken INT;                          
DECLARE @CaretypeId bigint;                
DECLARE @MClockoutTime bigint;             
DECLARE @EarlyClockInAllowed nvarchar(100);               
DECLARE @AutoApprovedEarlyClockIn nvarchar(100);            
DECLARE @ManualApprovedEarlyClockIn nvarchar(100);             
 SELECT @CARETYPEID=CARETYPEID FROM SCHEDULEMASTERS WHERE SCHEDULEID=@SCHEDULEID                        
                    
  SELECT @BeforeClockIn= CASE permissionname when '15 min' THEN -15                  
      when '30 min' THEN -30                  
      when '45 min' THEN -45                  
      when '60 min' THEN -60                  
      when '90 min' THEN -90                  
      when '120 min' THEN -120        
   when 'No Time Limit' THEN -300      
   when 'No Early Clockin' THEN 0                    
      ELSE -30                  
      end                    
   FROM dbo.RolePermissionMapping rpm INNER JOIN dbo.Permissions p ON rpm.PermissionID = p.PermissionID INNER JOIN dbo.Employees e ON e.RoleID = rpm.RoleID WHERE e.EmployeeID=@Employeeid AND p.Parentid=(SELECT permissionid FROM permissions WHERE dbo.permissions.PermissionCode='Mobile_Early_Clock_in')  and rpm.isdeleted=0                 
        
 --Get Early Auto Approved Early ClockIn               
select  @AutoApprovedEarlyClockIn=PermissionCode              
FROM dbo.RolePermissionMapping rpm INNER JOIN dbo.Permissions p ON rpm.PermissionID = p.PermissionID INNER JOIN dbo.Employees e ON e.RoleID = rpm.RoleID WHERE e.EmployeeID=1 AND p.permissionid=(SELECT permissionid FROM permissions WHERE dbo.permissions.PermissionCode='Auto_Approved_Early_ClockIn') and rpm.isdeleted=0                
        
select  @ManualApprovedEarlyClockIn=PermissionCode              
FROM dbo.RolePermissionMapping rpm INNER JOIN dbo.Permissions p ON rpm.PermissionID = p.PermissionID INNER JOIN dbo.Employees e ON e.RoleID = rpm.RoleID WHERE e.EmployeeID=1 AND p.permissionid=(SELECT permissionid FROM permissions WHERE dbo.permissions.PermissionCode='Manual_Approved_Early_ClockIn')  and rpm.isdeleted=0               
    
IF(@Type=2)     
BEGIN         
  --exceeded the Late Clock-out time Limit 30 min                
 select @MClockoutTime=DATEDIFF(MINUTE,(select convert(datetime,EndDate) from ScheduleMasters where ScheduleID=@ScheduleID and ReferralID=@ReferralID),convert(datetime,getdate()))                
  --if(@MClockoutTime>30)                
  --BEGIN                
  --SELECT -5 RETURN;                   
  --END                  
END                                           
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
if(@BeforeClockIn !=-300 and @BeforeClockIn != 0)        
BEGIN               
 IF((SELECT DATEDIFF(MINUTE,(SELECT StartDate FROM ScheduleMasters WHERE ScheduleID=@ScheduleID),@ClockInTime)) < @BeforeClockIn)                                       
 BEGIN                                       
 SELECT -3 RETURN;                                       
 END                                       
END  
-- Start Change by Sagar,28 Dec 2019: for Early clock in not allowed in case no Early clockin permission     
if(@BeforeClockIn = 0)  
BEGIN  
   IF((SELECT DATEDIFF(MINUTE,(SELECT StartDate FROM ScheduleMasters WHERE ScheduleID=@ScheduleID),@ClockInTime)) < -15)  
   BEGIN                                       
   SELECT -3 RETURN;                                       
   END  
END        
--End  
                                        
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
   IsApprovalRequired=CASE WHEN @IsByPass=1 THEN @IsApprovalRequired WHEN @AutoApprovedEarlyClockIn is not null THEN 1  ELSE @CheckIsApprovalRequired END,                          
   ActionTaken=CASE WHEN @IsByPass=1 THEN @ActionTaken ELSE @CheckActionTaken END,                          
   EarlyClockOutComment=@EarlyClockOutComment,            
   ByPassReasonClockOut=@ByPassReason,              
   IsEarlyClockIn=@IsEarlyClockIn,              
   EarlyClockInComment=@EarlyClockInComment                                         
   WHERE ScheduleID=@ScheduleID                      
                         
   --IF(@IsCaseManagement=1)                      
   --BEGIN                      
   IF(@Type=1)                    
   UPDATE ScheduleMasters SET StartDate=@ClockInTime WHERE ScheduleID=@ScheduleID                      
   IF(@Type=2)                    
   UPDATE ScheduleMasters SET EndDate=@ClockOutTime WHERE ScheduleID=@ScheduleID        
           
                 
 --UPDATE ScheduleMasters SET StartDate=CASE WHEN @Type=1 THEN @ClockInTime ELSE NULL END,                                                            
 --EndDate=CASE WHEN @Type=2 THEN @ClockOutTime ELSE NULL END                      
 --WHERE ScheduleID=@ScheduleID                      
   --END                      
                        
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
-- Changed By Pallav - Reason: isPCACompeleted happening in the middle of the process. Removed the update for isPCACompleted and added the condition isSigned for the PCACompleted                       
  
----------------------------------------Pallav Change Start Here-----------------------  
IF(@ManualApprovedEarlyClockIn is not null and @AutoApprovedEarlyClockIn is null)        
BEGIN    
   UPDATE EmployeeVisits SET IsApprovalRequired=1 WHERE ScheduleID=@ScheduleID           
END    
IF(@ManualApprovedEarlyClockIn is  null and @AutoApprovedEarlyClockIn is  not null)        
BEGIN    
   UPDATE EmployeeVisits SET IsApprovalRequired=0 WHERE ScheduleID=@ScheduleID                      
END    
  
if(select IsSigned from EmployeeVisits where scheduleid=@scheduleid)=1  
begin   
   UPDATE EmployeeVisits SET IsPCACompleted=1 WHERE ScheduleID=@ScheduleID                      
end  
else  
Begin  
   UPDATE EmployeeVisits SET IsPCACompleted=0 WHERE ScheduleID=@ScheduleID                      
end  
----------------------------------------Pallav Change End Here-----------------------  
                                 
  END    
  EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @ScheduleID,'222','25'                                                    
 SELECT 1;                                                                
 END                                                                
 ELSE                             
 BEGIN                                                                
 Select -1;                                                             
 END                                                  
END