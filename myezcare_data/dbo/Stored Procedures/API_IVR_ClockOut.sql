--EXEC API_IVR_ClockOut @EmployeeID = N'170', @ReferralIds = N'13997,13998,13999', @StartDateBefore = N'2018-04-04 03:45:30 PM', @StartDateAfter = N'2018-04-04 04:45:30 PM', @ClockOutTime = N'2018-04-04 04:15:30 PM',            
CREATE PROCEDURE [dbo].[API_IVR_ClockOut]              
 @EmployeeID bigint,              
 @ReferralIds nvarchar(max),
 @ApprovalRequiredIVRBypassPermission VARCHAR(100),
 @BypassAction INT,
 @StartDateBefore datetime,              
 @StartDateAfter datetime,              
 @ClockOutTime datetime              
AS                
BEGIN              
              
 -- SET NOCOUNT ON added to prevent extra result sets from                
 -- interfering with SELECT statements.                
 SET NOCOUNT ON;              
 
 DECLARE @IsApprovalRequired BIT = 0
 DECLARE @RoleID BIGINT
 DECLARE @tmpTable TABLE (ReferralId bigint)

 SELECT @RoleID=RoleID FROM Employees WHERE EmployeeID=@EmployeeID

 --Code for set as approval required visit
 IF EXISTS(SELECT P.PermissionID FROM RolePermissionMapping RPM
 INNER JOIN Permissions P ON P.PermissionID=RPM.PermissionID  
 WHERE RPM.RoleID=@RoleID AND P.PermissionCode=@ApprovalRequiredIVRBypassPermission AND RPM.IsDeleted=0)
	SET @IsApprovalRequired=1
 ELSE
	SET @BypassAction=NULL
              
 INSERT INTO @tmpTable (ReferralId)              
 SELECT c.RESULT FROM dbo.CSVtoTableWithIdentity(@ReferralIds,',') c              
              
 DECLARE @tmpScheduleMasters TABLE (ScheduleID bigint,ClockOut datetime)    
 DECLARE @tmpSchedules TABLE (ScheduleID bigint)    
    
 --Find today's visit which clockin is done but cloclout is remain    
 INSERT INTO @tmpSchedules(ScheduleID)    
 SELECT ScheduleID FROM ScheduleMasters    
 WHERE EmployeeID=@EmployeeID AND ReferralID IN (SELECT * FROM @tmpTable tt) AND IsDeleted=0 AND CONVERT(DATE,EndDate)=CONVERT(DATE,@ClockOutTime)    
    
 --Find all the schedules of logged in employee before and after 30 minutes of EndDate(Time)        
 INSERT INTO @tmpScheduleMasters (ScheduleID)              
 SELECT sm.ScheduleID            
 FROM dbo.ScheduleMasters sm               
 WHERE sm.EmployeeID=@EmployeeID AND sm.EndDate BETWEEN @StartDateBefore AND @StartDateAfter AND sm.ReferralID IN (SELECT * FROM @tmpTable tt) AND sm.IsDeleted=0      
              
 IF EXISTS(SELECT 1 FROM @tmpScheduleMasters tsm)                
  BEGIN                
   IF NOT EXISTS (SELECT 1 FROM dbo.EmployeeVisits ev WHERE ev.ScheduleID IN (SELECT tsm.ScheduleID FROM @tmpScheduleMasters tsm))                
    BEGIN                
     INSERT INTO dbo.EmployeeVisits (ScheduleID,ClockOutTime,IsDeleted,CreatedDate,CreatedBy,SurveyCompleted,IVRClockOut,IsApprovalRequired,ActionTaken)                
     SELECT tsm.ScheduleID,@ClockOutTime,0,@ClockOutTime,@EmployeeID,0,1,@IsApprovalRequired,@BypassAction FROM @tmpScheduleMasters tsm              
    END                
   ELSE                
    BEGIN              
 DECLARE @COT datetime;            
 SELECT @COT=ClockOutTime FROM EmployeeVisits WHERE ScheduleID IN (SELECT tsm.ScheduleID FROM @tmpScheduleMasters tsm)            
 IF (@COT is null)            
     UPDATE ev              
     SET ClockOutTime = @ClockOutTime,IVRClockOut=1,UpdatedDate = @ClockOutTime,UpdatedBy = @EmployeeID,IsApprovalRequired=@IsApprovalRequired,ActionTaken=@BypassAction
     from dbo.EmployeeVisits ev              
     INNER JOIN @tmpScheduleMasters tsm ON ev.ScheduleID=tsm.ScheduleID              
    END                
   SELECT 1 AS TransactionResultId;                
  END                
 ELSE    
  BEGIN    
    
 IF EXISTS( SELECT 1 FROM EmployeeVisits WHERE ScheduleID IN (SELECT ScheduleID FROM @tmpSchedules) AND ClockInTime IS NOT NULL AND ClockOutTime IS NULL)    
  SELECT 2 AS TransactionResultId;    
 ELSE    
  SELECT 0 AS TransactionResultId;    
  END                
END
