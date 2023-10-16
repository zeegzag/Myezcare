--EXEC API_IVR_ClockIn @EmployeeID = N'171', @ReferralIds = N'13997,13998', @StartDateBefore = N'2018-04-03 05:57:26 PM', @StartDateAfter = N'2018-04-03 06:57:26 PM', @ClockInTime = N'2018-04-03 06:27:26 PM'    
CREATE PROCEDURE [dbo].[API_IVR_ClockIn]        
 @EmployeeID bigint,        
 @ReferralIds nvarchar(max),
 @ApprovalRequiredIVRBypassPermission VARCHAR(100),
 @BypassAction INT,
 @StartDateBefore datetime,      
 @StartDateAfter datetime,      
 @ClockInTime datetime        
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
     
 DECLARE @tmpScheduleMasters TABLE (ScheduleID bigint)      
      
 INSERT INTO @tmpScheduleMasters (ScheduleID)      
 SELECT sm.ScheduleID      
 FROM dbo.ScheduleMasters sm       
 WHERE sm.EmployeeID=@EmployeeID AND sm.StartDate BETWEEN @StartDateBefore AND @StartDateAfter AND sm.ReferralID IN (SELECT * FROM @tmpTable tt) AND sm.IsDeleted=0  
     
    IF EXISTS(SELECT 1 FROM @tmpScheduleMasters tsm)      
  BEGIN        
   IF NOT EXISTS (SELECT 1 FROM dbo.EmployeeVisits ev WHERE ev.ScheduleID IN (SELECT tsm.ScheduleID FROM @tmpScheduleMasters tsm))        
    BEGIN        
     INSERT INTO dbo.EmployeeVisits (ScheduleID,ClockInTime,IVRClockIn,IsDeleted,CreatedDate,CreatedBy,SurveyCompleted,IsApprovalRequired,ActionTaken)      
     SELECT tsm.ScheduleID,@ClockInTime,1,0,@ClockInTime,@EmployeeID,0,@IsApprovalRequired,@BypassAction FROM @tmpScheduleMasters tsm      
    END        
   SELECT 1 AS TransactionResultId;        
  END        
 ELSE        
  BEGIN        
   SELECT 0 AS TransactionResultId;        
  END        
END
