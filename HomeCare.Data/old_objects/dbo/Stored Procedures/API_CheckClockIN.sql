--exec [API_CheckClockIN] 221124,'2019-12-11 09:07:00 AM',30,18  
-- 2019-12-11T09:07:02-08:00  
-- EXEC API_CheckClockIN @ScheduleID = N'221603', @ClockInTime = N'2019-12-12 12:00:17 AM', @ClockInBeforeTime = N'30', @RoleID = N'18'  
/*  
Created by sagar 8 dec 2019 := this is check the Early_ClockIn_Allowed Permissions and retunr true false  
*/  
--Updated By Kundan on 12-Dec-2019 for (@BeforeClockIn = -999), rpm.IsDeleted=0  
 CREATE PROCEDURE [dbo].[API_CheckClockIN]        
	@ScheduleID BIGINT,      
	@ClockInTime DateTime,      
	@ClockInBeforeTime INT,  
	@RoleID bigint     
	AS                                
	BEGIN      
	 DECLARE @Diff  BIGINT;      
	 DECLARE @EarlyClockInAllowed nvarchar(100);  
	 DECLARE @BeforeClockIn INT;
	 SELECT @EarlyClockInAllowed=PermissionCode from RolePermissionMapping RPM        
	INNER JOIN Permissions P on P.PermissionID=RPM.PermissionID        
	WHERE RPM.IsDeleted=0 AND RPM.RoleID=@RoleID AND CompanyHasAccess=1 -- and PermissionName='No Early Clockin'  


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
	 FROM dbo.RolePermissionMapping rpm INNER JOIN dbo.Permissions p ON rpm.PermissionID = p.PermissionID  
	 WHERE p.Parentid=(SELECT permissionid FROM permissions WHERE dbo.permissions.PermissionCode='Mobile_Early_Clock_in') and rpm.RoleID=@Roleid and rpm.IsDeleted=0
  


  -- By Kundan to check is clock in greater then end time of schedule
  IF(@ClockInTime >  (SELECT EndDate FROM ScheduleMasters WHERE ScheduleID=@ScheduleID AND IsDeleted=0))
	BEGIN
		SELECT -1 RETURN;
	END

	Select @Diff=datediff(mi, @ClockInTime, (SELECT StartDate FROM ScheduleMasters WHERE ScheduleID=@ScheduleID AND IsDeleted=0))         
	 IF((@Diff <= @BeforeClockIn OR @Diff>=-15) OR (@BeforeClockIn = -300)) 
	  Select @BeforeClockIn
	 ELSE      
	  Select 0
 END 