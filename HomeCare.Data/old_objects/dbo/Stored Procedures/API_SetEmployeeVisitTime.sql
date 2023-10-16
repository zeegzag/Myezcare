----EXEC API_SetEmployeeVisitTime @ScheduleID = N'2', @ReferralID = N'1', @EmployeeId = N'2', @Type = N'2', @IsByPass = N'False', @IsCaseManagement = N'False', @IsApprovalRequired = N'False', @ActionTaken = NULL, @EarlyClockOutComment = NULL, @ByPassReason = NULL, @Lat = N'0.0', @Long = N'0.0', @BeforeClockIn = N'-15', @ClockInTime = N'2019-06-20 11:10:12 PM', @ClockOutTime = N'2019-06-20 11:10:12 PM', @Distance = N'50', @SystemID = N'75.83.81.14'                        
/*                  
Change by Sagar [8 dec 2019] : To modify the Early Clock In Permissions based on RoleID dontiation and update two columns '@IsEarlyClockIn','@EarlyClockInComment' and '@RoleID'                      
Change by Sagar,28 Dec 2019(Line number 94 to 102  ): for Early clock in not allowed in case no Early clockin permission        
Changed by Kundan, 10 Oct 2020: added @IsVirtualVisit flag to check visit type      
*/  
  
CREATE PROCEDURE [dbo].[API_SetEmployeeVisitTime]  
  @ScheduleID bigint,  
  @ReferralID bigint,  
  @EmployeeID bigint,  
  @Type int,  
  @IsByPass bit,  
  @IsCaseManagement bit,  
  @IsApprovalRequired bit,  
  @ActionTaken nvarchar(1000),  
  @EarlyClockOutComment nvarchar(max),  
  @ByPassReason nvarchar(max),  
  @Lat decimal(10, 7),  
  @Long decimal(10, 7),  
  @BeforeClockIn nvarchar(100),  
  @ClockInTime datetime,  
  @ClockOutTime datetime,  
  @Distance int,  
  @SystemID varchar(100),  
  @IsEarlyClockIn bit = NULL,  
  @EarlyClockInComment nvarchar(max) = NULL  
AS  
BEGIN  
  DECLARE @g geography;  
  DECLARE @h geography;  
  DECLARE @ContactLatitude float;  
  DECLARE @ContactLongitude float;  
  DECLARE @EmployeeVisitID bigint;  
  DECLARE @CheckIsApprovalRequired bit;  
  DECLARE @CheckActionTaken int;  
  DECLARE @CaretypeId bigint;  
  DECLARE @MClockoutTime bigint;  
  DECLARE @EarlyClockInAllowed nvarchar(100);  
  DECLARE @AutoApprovedEarlyClockIn nvarchar(100);  
  DECLARE @ManualApprovedEarlyClockIn nvarchar(100);  
  DECLARE @IsVirtualVisit bit=null;  
  SELECT  
    @CARETYPEID = CARETYPEID,  
    @IsVirtualVisit = IsVirtualVisit  
  FROM SCHEDULEMASTERS  
  WHERE  
    SCHEDULEID = @SCHEDULEID  
  
  SELECT  
    @BeforeClockIn =  
                    CASE permissionname  
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
  FROM dbo.RolePermissionMapping rpm  
  INNER JOIN dbo.Permissions p  
    ON rpm.PermissionID = p.PermissionID  
  INNER JOIN dbo.Employees e  
    ON e.RoleID = rpm.RoleID  
  WHERE  
    e.EmployeeID = @Employeeid  
    AND p.Parentid =  
    (  
      SELECT  
        permissionid  
      FROM permissions  
      WHERE  
        dbo.permissions.PermissionCode = 'Mobile_Early_Clock_in'  
    )  
    AND rpm.isdeleted = 0  
  
  --Get Early Auto Approved Early ClockIn                   
  SELECT  
    @AutoApprovedEarlyClockIn = PermissionCode  
  FROM dbo.RolePermissionMapping rpm  
  INNER JOIN dbo.Permissions p  
    ON rpm.PermissionID = p.PermissionID  
  INNER JOIN dbo.Employees e  
    ON e.RoleID = rpm.RoleID  
  WHERE  
    e.EmployeeID = @Employeeid  
    AND p.permissionid =  
    (  
      SELECT  
        permissionid  
      FROM permissions  
      WHERE  
        dbo.permissions.PermissionCode = 'Auto_Approved_Early_ClockIn'  
    )  
    AND rpm.isdeleted = 0  
  
  SELECT  
    @ManualApprovedEarlyClockIn = PermissionCode  
  FROM dbo.RolePermissionMapping rpm  
  INNER JOIN dbo.Permissions p  
    ON rpm.PermissionID = p.PermissionID  
  INNER JOIN dbo.Employees e  
    ON e.RoleID = rpm.RoleID  
  WHERE  
    e.EmployeeID = @Employeeid  
    AND p.permissionid =  
    (  
      SELECT  
        permissionid  
      FROM permissions  
      WHERE  
        dbo.permissions.PermissionCode = 'Manual_Approved_Early_ClockIn'  
    )  
    AND rpm.isdeleted = 0  
  
  -- #start Changed By Fenil - Reason: As per discussion with Pallav need to allow user to late clockout we will validate on timesheet  
  --IF (@Type = 2)  
  --BEGIN  
  --  --exceeded the Late Clock-out time Limit 30 min                    
  --  SELECT  
  --    @MClockoutTime = DATEDIFF(MINUTE,  
  --    (  
  --      SELECT  
  --        CONVERT(datetime, EndDate)  
  --      FROM ScheduleMasters  
  --      WHERE  
  --        ScheduleID = @ScheduleID  
  --        AND ReferralID = @ReferralID  
  --    )  
  --    , CONVERT(datetime, GETDATE()))  
  --  IF (@MClockoutTime > 30)  
  --  BEGIN  
  --    SELECT  
  --      -5  
  --    RETURN;  
  --  END  
  --END  
  -- #end Changed By Fenil - Reason: As per discussion with Pallav need to allow user to late clockout we will validate on timesheet  
  
  --Required Task Added or Not Check                                          
  IF (EXISTS  
    (  
      SELECT  
        RM.ReferralTaskMappingID  
      FROM ReferralTaskMappings RM  
      INNER JOIN VisitTasks V  
        ON V.VisitTaskID = RM.VisitTaskID  
      WHERE  
        ReferralID = @ReferralID  
        AND V.VisitTaskType = 'Task'  
        AND RM.IsRequired = 1  
        AND RM.IsDeleted = 0  
        AND (@CaretypeId IS NULL  
          OR V.CARETYPE = @CARETYPEID)  
        AND RM.ReferralTaskMappingID NOT IN  
        (  
          SELECT  
            evn.ReferralTaskMappingID  
          FROM EmployeeVisits ev  
          INNER JOIN EmployeeVisitNotes evn  
            ON evn.EmployeeVisitID = ev.EmployeeVisitID  
          WHERE  
            ev.ScheduleID = @ScheduleID  
        )  
    )  
    AND @Type = 2)  
  BEGIN  
    SELECT  
      -4  
    RETURN;  
  END  
  
  --Early ClockIn Check             
  IF (@BeforeClockIn != -300  
    AND @BeforeClockIn != 0)  
  BEGIN  
    IF (  
      (  
        SELECT  
          DATEDIFF(MINUTE,  
          (  
            SELECT  
              StartDate  
            FROM ScheduleMasters  
            WHERE  
              ScheduleID = @ScheduleID  
          )  
          , @ClockInTime)  
      )  
      < @BeforeClockIn)  
    BEGIN  
      SELECT  
        -3  
      RETURN;  
    END  
  END  
  -- Start Change by Sagar,28 Dec 2019: for Early clock in not allowed in case no Early clockin permission         
  IF (@BeforeClockIn = 0)  
  BEGIN  
    IF (  
      (  
        SELECT  
          DATEDIFF(MINUTE,  
          (  
            SELECT  
              StartDate  
            FROM ScheduleMasters  
            WHERE  
              ScheduleID = @ScheduleID  
          )  
          , @ClockInTime)  
      )  
      < -15)  
    BEGIN  
      SELECT  
        -3  
      RETURN;  
    END  
  END  
  --End      
  
  SELECT  
    @ContactLatitude = c.Latitude,  
    @ContactLongitude = c.Longitude  
  FROM ScheduleMasters sm  
  INNER JOIN ContactMappings cm  
    ON cm.ReferralID = sm.ReferralID  
  INNER JOIN Contacts c  
    ON c.ContactID = cm.ContactID  
  WHERE  
    sm.ScheduleID = @ScheduleID  
    AND cm.ContactTypeID = 1  
  
  --Missing Patient Coordinates                                                            
  IF ((@ContactLatitude IS NULL  
    OR @ContactLongitude IS NULL)  
    AND @IsByPass = 0)  
  BEGIN  
    SELECT  
      -2  
    RETURN;  
  END  
  
  --Missing Visit Coordinates                                                            
  IF ((ISNULL(@Lat, 0) = 0  
    OR ISNULL(@Long, 0) = 0)  
    AND @IsByPass = 0)  
  BEGIN  
    SELECT  
      -2  
    RETURN;  
  END  
  
  SELECT  
    @g = dbo.GetGeoFromLatLng(@ContactLatitude, @ContactLongitude)  
  SELECT  
    @h = dbo.GetGeoFromLatLng(@Lat, @Long)  
  
  SELECT TOP 1  
    @EmployeeVisitID = EmployeeVisitID,  
    @CheckActionTaken = ActionTaken,  
    @CheckIsApprovalRequired = IsApprovalRequired  
  FROM EmployeeVisits  
  WHERE  
    ScheduleID = @ScheduleID  
    AND IsDeleted = 0

  IF (  
    (  
      SELECT  
        @g.STDistance(@h)  
    )  
    < @Distance  
    OR @IsByPass = 1  
    OR @IsVirtualVisit = 1)  
  BEGIN  
    IF (@EmployeeVisitID IS NOT NULL)  
    BEGIN  
      UPDATE EmployeeVisits  
      SET  
        ClockInTime =  
                     CASE  
                       WHEN @Type = 1 THEN @ClockInTime  
                       ELSE ClockInTime  
                     END,  
        ClockOutTime =  
                      CASE  
                        WHEN @Type = 2 THEN @ClockOutTime  
                        ELSE ClockOutTime  
                      END,  
        IsByPassClockOut = @IsByPass,  
        Latitude = @Lat,  
        Longitude = @Long,  
        IsApprovalRequired =  
                            CASE  
                              WHEN @IsByPass = 1 THEN @IsApprovalRequired  
                              WHEN @AutoApprovedEarlyClockIn IS NOT NULL THEN 1  
                              ELSE @CheckIsApprovalRequired  
                            END,  
        ActionTaken =  
                     CASE  
                       WHEN @IsByPass = 1 THEN @ActionTaken  
                       ELSE @CheckActionTaken  
                     END,  
        EarlyClockOutComment = @EarlyClockOutComment,  
        ByPassReasonClockOut = @ByPassReason,  
        IsEarlyClockIn = @IsEarlyClockIn,  
        EarlyClockInComment = @EarlyClockInComment  
      WHERE  
        ScheduleID = @ScheduleID  
  
      --IF(@IsCaseManagement=1)                          
      --BEGIN                          
      --IF(@Type=1)                        
      --UPDATE ScheduleMasters SET StartDate=@ClockInTime WHERE ScheduleID=@ScheduleID                          
      --IF(@Type=2)                        
      --UPDATE ScheduleMasters SET EndDate=@ClockOutTime WHERE ScheduleID=@ScheduleID            
  
  
      --UPDATE ScheduleMasters SET StartDate=CASE WHEN @Type=1 THEN @ClockInTime ELSE NULL END,                                                                
      --EndDate=CASE WHEN @Type=2 THEN @ClockOutTime ELSE NULL END                          
      --WHERE ScheduleID=@ScheduleID                          
      --END                          
  
      DECLARE @Count int;  
      SET @Count =  
      (  
        SELECT  
          COUNT(ReferralTaskMappingID)  
        FROM ReferralTaskMappings RTM  
        INNER JOIN VisitTasks V  
          ON V.VisitTaskID = RTM.VisitTaskID  
          AND V.VisitTaskType = 'Conclusion'  
        WHERE  
          RTM.ReferralID = @ReferralID  
          AND RTM.IsDeleted = 0  
      )  
  
      IF @Count = 0  
        UPDATE EmployeeVisits  
        SET  
          SurveyCompleted = 1  
        WHERE  
          EmployeeVisitID = @EmployeeVisitID  
		  AND ClockOutTime IS NOT NULL
    END  
    ELSE  
    BEGIN  
      INSERT INTO EmployeeVisits  
      (  
        ScheduleID,  
        ClockInTime,  
        ClockOutTime,  
        SystemID,  
        CreatedDate,  
        CreatedBy,  
        UpdatedDate,  
        UpdatedBy,  
        IsByPassClockIn,  
        ByPassReasonClockIn,  
        Latitude,  
        Longitude,  
        IsApprovalRequired,  
        ActionTaken  
      )  
      VALUES  
      (  
        @ScheduleID,  
        CASE  
          WHEN @Type = 1 THEN @ClockInTime  
          ELSE NULL  
        END,  
        CASE  
          WHEN @Type = 2 THEN @ClockOutTime  
          ELSE NULL  
        END,  
        @SystemID,  
        GETUTCDATE(),  
        @EmployeeID,  
        GETUTCDATE(),  
        @EmployeeID,  
        @IsByPass,  
        @ByPassReason,  
        @Lat,  
        @Long,  
        @IsApprovalRequired,  
        @ActionTaken  
      )  
  
      IF (@IsCaseManagement = 1)  
      BEGIN  
        UPDATE ScheduleMasters  
        SET  
          StartDate =  
                     CASE  
                       WHEN @Type = 1 THEN @ClockInTime  
                       ELSE NULL  
                     END,  
          EndDate =  
                   CASE  
                     WHEN @Type = 2 THEN @ClockOutTime  
                     ELSE NULL  
                   END  
        WHERE  
          ScheduleID = @ScheduleID  
      END  
      -- Changed By Pallav - Reason: isPCACompeleted happening in the middle of the process. Removed the update for isPCACompleted and added the condition isSigned for the PCACompleted                         
  
      ----------------------------------------Pallav Change Start Here-----------------------      
      IF (@ManualApprovedEarlyClockIn IS NOT NULL  
        AND @AutoApprovedEarlyClockIn IS NULL)  
      BEGIN  
        UPDATE EmployeeVisits  
        SET  
          IsApprovalRequired = 1  
        WHERE  
          ScheduleID = @ScheduleID  
      END  
      IF (@ManualApprovedEarlyClockIn IS NULL  
        AND @AutoApprovedEarlyClockIn IS NOT NULL)  
      BEGIN  
        UPDATE EmployeeVisits  
        SET  
          IsApprovalRequired = 0  
        WHERE  
          ScheduleID = @ScheduleID  
      END  
  
      IF  
        (  
          SELECT  
            IsSigned  
          FROM EmployeeVisits  
          WHERE  
            scheduleid = @scheduleid  
        )  
        = 1  
      BEGIN  
        UPDATE EmployeeVisits  
        SET  
          IsPCACompleted = 1  
        WHERE  
          ScheduleID = @ScheduleID  
      END  
      ELSE  
      BEGIN  
        UPDATE EmployeeVisits  
        SET  
          IsPCACompleted = 0  
        WHERE  
          ScheduleID = @ScheduleID  
      END  
    ----------------------------------------Pallav Change End Here-----------------------      
  
    END  
    EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule',  
                                        @ScheduleID,  
                                        '222',  
                                        '25'  
    SELECT  
      1;  
  END  
  ELSE  
  BEGIN  
    SELECT  
      -1;  
  END  
END