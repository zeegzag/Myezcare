--EXEC API_CheckClockIN @ScheduleID = N'83167', @ClockInTime = N'2021-02-02 02:02:48 PM', @ClockInBeforeTime = NULL, @RoleID = N'32'  
    
--exec [API_CheckClockIN] 221124,'2019-12-11 09:07:00 AM',30,18       
-- 2019-12-11T09:07:02-08:00       
-- EXEC API_CheckClockIN @ScheduleID = N'221603', @ClockInTime = N'2019-12-12 12:00:17 AM', @ClockInBeforeTime = N'30', @RoleID = N'18'       
/*       
    
Created by sagar 8 dec 2019 := this is check the Early_ClockIn_Allowed Permissions and retunr true false      
    
*/     
--Updated By Kundan on 12-Dec-2019 for (@BeforeClockIn = -999), rpm.IsDeleted=0       
CREATE PROCEDURE [dbo].[API_CheckClockIN] @ScheduleID        BIGINT,     
                                          @ClockInTime       DATETIME,     
                                          @ClockInBeforeTime INT,     
                                          @RoleID            BIGINT     
AS     
  BEGIN     
      DECLARE @Diff BIGINT;     
      DECLARE @EarlyClockInAllowed NVARCHAR(100);     
      DECLARE @BeforeClockIn INT;     
    
      SELECT @EarlyClockInAllowed = permissioncode     
      FROM   rolepermissionmapping RPM     
             INNER JOIN permissions P     
                     ON P.permissionid = RPM.permissionid     
      WHERE  RPM.isdeleted = 0     
             AND RPM.roleid = @RoleID     
             AND companyhasaccess = 1     
      -- and PermissionName='No Early Clockin'         
      SELECT @BeforeClockIn = CASE permissionname     
                                WHEN '15 min' THEN -15     
                                WHEN '30 min' THEN -30     
                                WHEN '45 min' THEN -45     
                                WHEN '60 min' THEN -60     
                                WHEN '90 min' THEN -90     
                                WHEN '120 min' THEN -120     
                                WHEN 'No Time Limit' THEN -300     
                                WHEN 'No Early Clockin' THEN 0     
                                ELSE -30     
                              END     
      FROM   dbo.rolepermissionmapping rpm     
             INNER JOIN dbo.permissions p     
                     ON rpm.permissionid = p.permissionid     
      WHERE  p.parentid = (SELECT permissionid     
                           FROM   permissions     
                           WHERE     
             dbo.permissions.permissioncode = 'Mobile_Early_Clock_in')     
             AND rpm.roleid = @Roleid     
             AND rpm.isdeleted = 0     
    
      IF @clockInTime < (SELECT startdate     
                         FROM   schedulemasters     
                         WHERE  scheduleid = @ScheduleID     
                                AND isdeleted = 0)     
        BEGIN     
            SELECT @Diff = Datediff(mi, (SELECT startdate     
                                         FROM   schedulemasters     
                                         WHERE  scheduleid = @ScheduleID     
                                                AND isdeleted = 0), @ClockInTime     
                           )     
    
            IF( ( @Diff >= @BeforeClockIn     
                   OR @Diff >=- 15 )     
                 OR ( @BeforeClockIn = -300 ) )     
              SELECT @BeforeClockIn     
            ELSE     
              SELECT 0     
        END     
      ELSE     
        BEGIN     
             SELECT ISNULL(@BeforeClockIn, -15)   
        END     
  END 