CREATE PROCEDURE [dbo].[API_AddNoteByCareType]
  --@EmployeeVisitNoteID BIGINT=0,                                                      
  --@ReferralTaskMappingID BIGINT=0,                                                                        
  @EmployeeVisitID bigint = 0,
  @EmployeeID bigint = 0,
  --@Description NVARCHAR(1000)=null,                                                
  --@ServiceTime BIGINT=0,                                                              
  @SystemID varchar(100) = NULL,
  @ResultRequired bit = 1
AS
BEGIN
  DECLARE @MedicaidType varchar(50);
  SET @MedicaidType = 'Medicaid'
  DECLARE @PayorID bigint;
  SET @PayorID =
  (
    SELECT
      SM.PayorID
    FROM EmployeeVisits EV
    INNER JOIN ScheduleMasters SM
      ON SM.ScheduleID = EV.ScheduleID
    WHERE
      EV.EmployeeVisitID = @EmployeeVisitID
  )
  IF (@PayorID IS NULL
    OR LEN(@PayorID) = 0
    OR @PayorID = 0)
  BEGIN
    IF (@ResultRequired = 1)
    BEGIN
      SELECT
        1 AS TransactionResultId;
    END
    RETURN;
  END

  DECLARE @BillingProviderName varchar(max);
  DECLARE @BillingProviderFirstName varchar(max) = '';
  DECLARE @BillingProviderNPI varchar(max);
  DECLARE @BillingProviderAddress varchar(max);
  DECLARE @BillingProviderCity varchar(max);
  DECLARE @BillingProviderState varchar(max);
  DECLARE @BillingProviderZipcode varchar(max);
  DECLARE @BillingProviderEIN varchar(max);

  SELECT TOP 1
    @BillingProviderName = BillingProvider_NM103_NameLastOrOrganizationName,
    @BillingProviderNPI = BillingProvider_NM109_IdCode,
    @BillingProviderAddress = BillingProvider_N301_Address,
    @BillingProviderCity = BillingProvider_N401_City,
    @BillingProviderState = BillingProvider_N402_State,
    @BillingProviderZipcode = BillingProvider_N403_Zipcode,
    @BillingProviderEIN = BillingProvider_REF02_ReferenceIdentification
  FROM OrganizationSettings

  BEGIN TRANSACTION trans
    BEGIN TRY
      --Declare @ScheduleID int   
      -- -- Kunda Rai: 16-06-2020  
      -- -- Updating ReferralBillingAuthorizationID into ScheduleMasters is does not exists.  
      -- IF((SELECT SM.ReferralBillingAuthorizationID FROM EmployeeVisits EV INNER JOIN ScheduleMasters SM ON SM.ScheduleID = EV.ScheduleID WHERE EV.EmployeeVisitID = @EmployeeVisitID) IS NULL)  
      -- BEGIN  
      -- select @ScheduleID= sm.scheduleID from ScheduleMasters sm inner join employeevisits ev on sm.ScheduleID=ev.ScheduleID where ev.EmployeeVisitID=@EmployeeVisitID  

      --  UPDATE SM SET SM.ReferralBillingAuthorizationID = RBA.ReferralBillingAuthorizationID  
      --  --select ev.employeevisitid,rbmax.AuthorizationCode, sm.ReferralBillingAuthorizationID,rba.ReferralBillingAuthorizationID,rba.CareType,sm.CareTypeId,sm.StartDate  
      --  FROM EmployeeVisits EV INNER JOIN ScheduleMasters SM ON SM.ScheduleID = EV.ScheduleID  
      --          INNER JOIN ReferralBillingAuthorizations RBA ON RBA.CareType = SM.CareTypeId and rba.ReferralID=sm.ReferralID and (sm.StartDate between rba.StartDate and rba.enddate) and (sm.EndDate between rba.startdate and rba.EndDate)  
      --          Inner join PayorServiceCodeMapping psm on psm.ServiceCodeID=RBA.ServiceCodeID and sm.CareTypeId=psm.CareType and psm.CareType=rba.CareType and psm.PayorID=sm.PayorID  
      --          inner join   
      --          (select rba1.ReferralBillingAuthorizationID,rba1.AuthorizationCode,max(enddate) MaxEndDate from ReferralBillingAuthorizations rba1 group by rba1.ReferralBillingAuthorizationID,rba1.AuthorizationCode) as rbmax   
      --             on rbmax.ReferralBillingAuthorizationID=rba.ReferralBillingAuthorizationID and rba.EndDate=rbmax.MaxEndDate   
      --  WHERE RBA.IsDeleted = 0 AND SM.IsDeleted = 0 AND EV.IsDeleted = 0 and sm.ScheduleID=@ScheduleID  


      --END  

      DECLARE @Temp TABLE (
        RankOrder int,
        ReferralID bigint,
        AHCCCSID nvarchar(100),
        CISNumber nvarchar(100),
        ServiceDate date,
        ClockInTime datetime,
        ClockOutTime datetime,
        ServiceCodeID bigint,
        ServiceCode nvarchar(200),
        ServiceName nvarchar(200),
        Description nvarchar(500),
        MaxUnit int,
        DailyUnitLimit int,
        UnitType int,
        PerUnitQuantity float,
        ServiceCodeType int,
        ServiceCodeStartDate date,
        ServiceCodeEndDate date,
        IsBillable bit,
        EmployeeID bigint,
        EmployeeFirstName nvarchar(200),
        EmployeeLastName nvarchar(200),
        HHA_NPI_ID nvarchar(100),
        Address nvarchar(200),
        City nvarchar(100),
        StateCode nvarchar(100),
        ZipCode nvarchar(100),
        PayorID bigint,
        PayorName nvarchar(100),
        PayorShortName nvarchar(100),
        PayorAddress nvarchar(200),
        PayorIdentificationNumber nvarchar(100),
        PayorCity nvarchar(100),
        PayorState nvarchar(100),
        PayorZipCode nvarchar(100),
        EmployeeVisitNoteID bigint,
        NoteID bigint,
        POSStartDate date,
        POSEndDate date,
        Rate float,
        SumOfServiceTime bigint,
        ModifierID varchar(500),

        --PayorServiceCodeMappingID BIGINT,                        
        AddNewNote bit,
        RoundUpUnit int,
        SumOfUnit float,
        ReferralBillingAuthorizationID int
      )

      --Insert into Temp Table - Loads All Visit Task And Related Notes                                                                
      INSERT INTO @Temp
      (
        RankOrder,
        ReferralID,
        AHCCCSID,
        CISNumber,
        ServiceDate,
        ClockInTime,
        ClockOutTime,
        ServiceCodeID,
        ServiceCode,
        ServiceName,
        Description,
        MaxUnit,
        DailyUnitLimit,
        UnitType,
        PerUnitQuantity,
        ServiceCodeType,
        ServiceCodeStartDate,
        ServiceCodeEndDate,
        IsBillable,
        EmployeeID,
        EmployeeFirstName,
        EmployeeLastName,
        HHA_NPI_ID,
        Address,
        City,
        StateCode,
        ZipCode,
        PayorID,
        PayorName,
        PayorShortName,
        PayorAddress,
        PayorIdentificationNumber,
        PayorCity,
        PayorState,
        PayorZipCode,
        EmployeeVisitNoteID,
        NoteID,
        POSStartDate,
        POSEndDate,
        Rate,
        SumOfServiceTime,
        ModifierID,
        AddNewNote,
        RoundUpUnit,
        ReferralBillingAuthorizationID,
        SumOfUnit
      )
        SELECT
          *,
          SumOfUnit =
                       CASE
                         WHEN UnitType = 1 THEN CONVERT(int, (SumOfServiceTime / PerUnitQuantity + (CASE
                             WHEN ((SumOfServiceTime % PerUnitQuantity) >= RoundUpUnit) THEN 1
                             ELSE 0
                           END)))
                         ELSE 1
                       END
        FROM
        (
          SELECT TOP 1
            RankOrder = ROW_NUMBER() OVER (PARTITION BY EV.EmployeeVisitID, R.ReferralID, SC.ServiceCodeID ORDER BY SC.ServiceCodeID),
            R.ReferralID,
            RPM.BeneficiaryNumber AS 'AHCCCSID', --R.AHCCCSID,        
            R.CISNumber,
            ServiceDate = CONVERT(date, EV.ClockInTime),
            EV.ClockInTime,
            EV.ClockOutTime,
            SC.ServiceCodeID,
            SC.ServiceCode,
            SC.ServiceName,
            SC.Description,
            RBA.MaxUnit,
            RBA.DailyUnitLimit,
            RBA.UnitType,
            RBA.PerUnitQuantity,
            ISNULL(SC.ServiceCodeType, 0) AS ServiceCodeType,
            SC.ServiceCodeStartDate,
            SC.ServiceCodeEndDate,
            SC.IsBillable,
            E.EmployeeID,
            EmployeeFirstName = E.FirstName,
            EmployeeLastName = E.LastName,
            E.HHA_NPI_ID,
            E.Address,
            E.City,
            E.StateCode,
            E.ZipCode,
            SM.PayorID,
            P.PayorName,
            PayorShortName = P.ShortName,
            PayorAddress = P.Address,
            PayorIdentificationNumber = P.PayorIdentificationNumber,
            PayorCity = P.City,
            PayorState = P.StateCode,
            PayorZipCode = P.ZipCode,
            NULL EmployeeVisitNoteID,
            N.NoteID,
            --RBA.POSStartDate,RBA.POSEndDate,         
            RBA.StartDate,
            RBA.EndDate,
            RBA.Rate,
            SumOfServiceTime = DATEDIFF(mi, ev.clockinTime, ev.ClockOutTime),
            SC.ModifierID,
            --RBA.PayorServiceCodeMappingID,                        
            AddNewNote =
                          CASE
                            WHEN ISNULL(SUM(N.NoteID) OVER (PARTITION BY R.ReferralID, SC.ServiceCodeID), 0) > 0 THEN 0
                            ELSE 1
                          END,
            RBA.RoundUpUnit,
            RBA.ReferralBillingAuthorizationID
          FROM Referrals AS R
          INNER JOIN ScheduleMasters AS SM
            ON R.ReferralID = SM.ReferralID
            INNER JOIN
            --ReferralTaskMappings AS RMT ON RMT.ReferralID = R.ReferralID INNER JOIN     
            EmployeeVisits AS EV
              ON SM.ScheduleID = EV.ScheduleID
            INNER JOIN Employees AS E
              ON SM.EMPLOYEEID = E.EMPLOYEEID
            INNER JOIN
            --    VisitTasks AS VT ON RMT.VisitTaskID = VT.VisitTaskID INNER JOIN    
            --      ServiceCodes AS SC ON VT.ServiceCodeID = SC.ServiceCodeID left JOIN  -- Changed by Pallav as the notes were not getting added due to missing Auth Numbers   
            ReferralPayorMappings AS RPM
              ON SM.ReferralID = RPM.ReferralID
            INNER JOIN ReferralBillingAuthorizations AS RBA
              ON RBA.ReferralID = RPM.ReferralID
              AND RBA.ReferralBillingAuthorizationID = SM.ReferralBillingAuthorizationID
            INNER JOIN ServiceCodes SC
              ON SC.servicecodeid = rba.servicecodeid
            INNER JOIN Payors AS P
              ON RPM.PayorID = P.PayorID
            LEFT JOIN
            --PayorServiceCodeMapping AS PSC ON SC.ServiceCodeID = PSC.ServiceCodeID AND PSC.PAYORID=P.PAYORID and PSC.POSEndDate>=getdate() and sm.caretypeID=psc.caretype inner join   
            --ReferralBillingAuthorizationServiceCodes AS RB ON RB.ServiceCodeID = PSC.ServiceCodeID and RBA.ReferralBillingAuthorizationID = RB.ReferralBillingAuthorizationID AND PSC.PayorID = RBA.PayorID left  JOIN      
            Notes N
              ON N.ServiceCodeID = SC.ServiceCodeID
              AND N.ReferralID = R.ReferralID
              AND N.EmployeeVisitID = EV.EmployeeVisitID
              AND (N.IsDeleted = 0
              OR N.IsDeleted IS NULL)
            LEFT JOIN DDMaster DM
              ON DM.DDMasterID = RPM.BeneficiaryTypeID
              AND DM.Title = @MedicaidType
          WHERE
            EV.EmployeeVisitID = @EMPLOYEEVISITID
            AND ev.isDeleted = 0
            AND ev.isPCACompleted = 1 --AND (N.IsDeleted=0 OR N.IsDeleted IS NULL)      
        ) AS T
      --Insert into Note Table - Only New Notes                              
      INSERT INTO Notes
      (
        ReferralID,
        AHCCCSID,
        CISNumber,
        ServiceDate,
        StartTime,
        EndTime,
        ServiceCodeID,
        ServiceCode,
        ServiceName,
        Description,
        MaxUnit,
        DailyUnitLimit,
        UnitType,
        PerUnitQuantity,
        ServiceCodeType,
        ServiceCodeStartDate,
        ServiceCodeEndDate,
        IsBillable,
        RenderingProviderID,
        RenderingProviderName,
        RenderingProviderFirstName,
        RenderingProviderAddress,
        RenderingProviderCity,
        RenderingProviderState,
        RenderingProviderZipcode,
        RenderingProviderNPI,
        PayorID,
        PayorName,
        PayorShortName,
        PayorAddress,
        PayorIdentificationNumber,
        PayorCity,
        PayorZipcode,
        POSStartDate,
        POSEndDate,
        Rate,
        CalculatedUnit,
        CalculatedAmount,
        CalculatedServiceTime,
        ModifierID,
        MarkAsComplete,
        --PayorServiceCodeMappingID,  
        BillingProviderID,
        BillingProviderName,
        BillingProviderEIN,
        BillingProviderNPI,
        BillingProviderAddress,
        BillingProviderCity,
        BillingProviderState,
        BillingProviderZipcode,
        EmployeeVisitID,
        --EmployeeVisitNoteIDs                               
        CreatedDate,
        CreatedBy,
        UpdatedDate,
        UpdatedBy,
        SystemID,
        ReferralBillingAuthorizationID
      )
        SELECT
          ReferralID,
          AHCCCSID,
          CISNumber,
          ServiceDate,
          ClockInTime,
          ClockOutTime,
          ServiceCodeID,
          ServiceCode,
          ServiceName,
          Description,
          MaxUnit,
          DailyUnitLimit,
          UnitType,
          PerUnitQuantity,
          ServiceCodeType,
          ServiceCodeStartDate,
          ServiceCodeEndDate,
          IsBillable,
          -- EmployeeID,    ,       EmployeeFirstName,       Address,City,StateCode,ZipCode,    
          EmployeeID,
          EmployeeLastName,
          EmployeeFirstName,
          @BillingProviderAddress,
          @BillingProviderCity,
          @BillingProviderState,
          @BillingProviderZipcode,
          CASE
            WHEN HHA_NPI_ID IS NULL OR
              LEN(HHA_NPI_ID) = 0 THEN @BillingProviderNPI
            ELSE HHA_NPI_ID
          END AS 'HHA_NPI_ID',
          PayorID,
          PayorName,
          PayorShortName,
          PayorAddress,
          PayorIdentificationNumber,
          PayorCity,
          PayorZipcode,
          POSStartDate,
          POSEndDate,
          Rate,
          SumOfUnit,
          T1.SumOfUnit * Rate,
          SumOfServiceTime,
          ModifierID,
          1,
          --PayorServiceCodeMappingID,  
          -1,
          @BillingProviderName,
          @BillingProviderEIN,
          @BillingProviderNPI,
          @BillingProviderAddress,
          @BillingProviderCity,
          @BillingProviderState,
          @BillingProviderZipcode,
          @EmployeeVisitID,
          --STUFF(                                                                
          --       (SELECT ', ' + convert(varchar(10), T2.EmployeeVisitNoteID, 120)                                                                
          --        FROM @Temp T2                                                                
          --    where T1.ServiceCodeID = T2.ServiceCodeID                                                                
          --        FOR XML PATH (''))                                                                
          --        , 1, 1, ''),                                                                
          GETDATE(),
          @EmployeeID,
          GETDATE(),
          @EmployeeID,
          @SystemID,
          ReferralBillingAuthorizationID
        FROM @Temp T1
        WHERE
          T1.RankOrder = 1
          AND T1.AddNewNote = 1 -- T1.NoteID IS NULL                                                                

      --Update Notes Table                                                                
      UPDATE N
      SET
        N.ServiceCodeID = T.ServiceCodeID,
        N.ServiceCode = T.ServiceCode,
        N.ServiceName = T.ServiceName,
        N.Description = T.Description,
        N.MaxUnit = T.MaxUnit,
        N.DailyUnitLimit = T.DailyUnitLimit,
        N.UnitType = T.UnitType,
        N.PerUnitQuantity = T.PerUnitQuantity,
        --N.ServiceCodeType=T.ServiceCodeType,                                                                
        N.ServiceCodeStartDate = T.ServiceCodeStartDate,
        N.ServiceCodeEndDate = T.ServiceCodeEndDate,
        N.IsBillable = T.IsBillable,
        N.Rate = T.Rate,
        N.CalculatedUnit = T.SumOfUnit,
        N.CalculatedAmount = T.SumOfUnit * T.Rate,
        N.CalculatedServiceTime = T.SumOfServiceTime,
        N.ModifierID = T.ModifierID,
        --N.PayorServiceCodeMappingID=T.PayorServiceCodeMappingID,                                                 
        N.RenderingProviderName = T.EmployeeLastName,
        N.RenderingProviderFirstName = T.EmployeeFirstName,
        N.RenderingProviderAddress = T.Address,
        N.RenderingProviderCity = T.City,
        N.RenderingProviderState = T.StateCode,
        N.RenderingProviderZipcode = T.ZipCode,
        N.RenderingProviderNPI =
                                  CASE
                                    WHEN T.HHA_NPI_ID IS NULL OR
                                      LEN(T.HHA_NPI_ID) = 0 THEN @BillingProviderNPI
                                    ELSE T.HHA_NPI_ID
                                  END,
        --N.RenderingProviderNPI=T.HHA_NPI_ID,                                            
        N.BillingProviderName = @BillingProviderName,
        N.BillingProviderNPI = @BillingProviderNPI,
        N.BillingProviderEIN = @BillingProviderEIN,
        N.BillingProviderAddress = @BillingProviderAddress,
        N.BillingProviderCity = @BillingProviderCity,
        N.BillingProviderState = @BillingProviderState,
        N.BillingProviderZipcode = @BillingProviderZipcode,
        N.EmployeeVisitID = @EmployeeVisitID,
        N.UpdatedDate = GETDATE(),
        N.UpdatedBy = @EmployeeID,
        N.ReferralBillingAuthorizationID = T.ReferralBillingAuthorizationID
      FROM Notes N
      INNER JOIN @Temp T
        ON T.NoteID = N.NoteID

      ----Update NoteID in EmployeeVisitNotes Table                                                                
      --UPDATE EVN                                                                
      --SET EVN.NoteID=N.NoteID,EVN.ServiceCodeID=V.ServiceCodeID,UpdatedDate=GETDATE(),UpdatedBy=@LoggedInID                                                                
      --FROM Notes N                                                                
      --INNER JOIN ScheduleMasters SM ON SM.ReferralID=N.ReferralID AND SM.EmployeeID=N.RenderingProviderID AND                                                                 
      --(N.ServiceDate BETWEEN CONVERT(DATE,SM.StartDate) AND CONVERT(DATE,SM.EndDate)) AND SM.IsDeleted=0                                                                
      --INNER JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID                                                                
      --INNER JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID AND EVN.IsDeleted=0                                                                
      --INNER JOIN ReferralTaskMappings RTM ON RTM.ReferralTaskMappingID=EVN.ReferralTaskMappingID                                                                
      --INNER JOIN VisitTasks V ON V.VisitTaskID=RTM.VisitTaskID AND V.VisitTaskType='Task' AND V.ServiceCodeID=N.ServiceCodeID                                                    



      ----Update NoteID in EmployeeVisitNotes Table (NEW)                                                  
      UPDATE EVN
      SET
        EVN.NoteID = N.NoteID,
        EVN.ServiceCodeID = V.ServiceCodeID,
        UpdatedDate = GETDATE(),
        UpdatedBy = @EmployeeID
      FROM EmployeeVisitNotes EVN
      LEFT JOIN ReferralTaskMappings RTM
        ON RTM.ReferralTaskMappingID = EVN.ReferralTaskMappingID
        LEFT JOIN VisitTasks V
          ON V.VisitTaskID = RTM.VisitTaskID
          AND V.VisitTaskType = 'Task'
        INNER JOIN EmployeeVisits EV
          ON EV.EmployeeVisitID = EVN.EmployeeVisitID
        INNER JOIN ScheduleMasters SM
          ON SM.ScheduleID = EV.ScheduleID
        LEFT JOIN Notes N
          ON N.ReferralID = SM.ReferralID
          AND N.RenderingProviderID = SM.EmployeeID
          AND N.EmployeeVisitID = EV.EmployeeVisitID
          AND N.ServiceDate = CONVERT(date, EV.ClockInTime)
          AND N.PayorID = SM.PayorID
          AND (N.ServiceCodeID = V.ServiceCodeID
          OR (N.ServiceCodeID = EVN.ServiceCodeID
          AND EVN.ReferralTaskMappingID IS NULL))
      WHERE
        EVN.EmployeeVisitID = @EmployeeVisitID
        AND EVN.IsDeleted = 0


      --DELETE Notes                                                

      DECLARE @DeletedTABLE TABLE (
        NoteID bigint,
        EmployeeVisitNoteID bigint
      )
      INSERT INTO @DeletedTABLE
        SELECT DISTINCT
          N.NoteID,
          EVN.EmployeeVisitNoteID
        FROM EmployeeVisitNotes EVN
        INNER JOIN Notes N
          ON EVN.NoteID = N.NoteID
          AND EVN.IsDeleted = 1
        WHERE
          EVN.EmployeeVisitID = @EmployeeVisitID


      UPDATE N
      SET
        N.IsDeleted = 1
      FROM @DeletedTABLE DN
      INNER JOIN Notes N
        ON N.NoteID = DN.NoteID
        LEFT JOIN EmployeeVisitNotes EVN
          ON DN.EmployeeVisitNoteID != EVN.EmployeeVisitNoteID
          AND DN.NoteID = EVN.NoteID
          AND EVN.IsDeleted = 0
      WHERE
        EVN.NoteID IS NULL

      UPDATE N
      SET
        N.IsDeleted = 0
      FROM EmployeeVisitNotes EVN
      INNER JOIN Notes N
        ON EVN.NoteID = N.NoteID
        AND EVN.IsDeleted = 0
      WHERE
        EVN.EmployeeVisitID = @EmployeeVisitID

      --UPDATE N SET N.IsDeleted=CASE WHEN (EVN.NoteID IS Null) THEN 1 ELSE 0 END                                                    
      --FROM Notes N                                                  
      --LEFT JOIN EmployeeVisitNotes EVN ON EVN.NoteID=N.NoteID AND EVN.ServiceCodeID=N.ServiceCodeID                                                  

      ----------------------------------------NoteDXCodeMappings-----------------------------------------                                                                

      DECLARE @NoteDxTemp TABLE (
        ReferralDXCodeMappingID bigint,
        ReferralID bigint,
        NoteID bigint,
        DXCodeID bigint,
        DXCodeName varchar(50),
        DxCodeType varchar(50),
        Precedence int,
        StartDate date,
        EndDate date,
        Description varchar(500),
        DXCodeWithoutDot varchar(50),
        DxCodeShortName varchar(50)
      )

      INSERT INTO @NoteDxTemp
        --Select                                                                
        --RDM.ReferralDXCodeMappingID,T.ReferralID,EVN.NoteID,DC.DXCodeID,DC.DXCodeName,DC.DxCodeType,RDM.Precedence,RDM.StartDate,RDM.EndDate,DC.Description,                                                                
        --DC.DXCodeWithoutDot,DCT.DxCodeShortName                                                                
        -- FROM ReferralDXCodeMappings RDM                                                                
        -- INNER JOIN @Temp T ON T.ReferralID=RDM.ReferralID AND T.RankOrder=1 --AND T.NoteID IS NULL                                                                
        -- INNER JOIN EmployeeVisitNotes EVN ON T.EmployeeVisitNoteID=EVN.EmployeeVisitNoteID                                                                
        -- INNER JOIN DXCodes DC ON DC.DXCodeID=RDM.DXCodeID                                
        -- INNER JOIN DxCodeTypes DCT ON DCT.DxCodeTypeID=DC.DxCodeType                                            
        -- WHERE EVN.NoteID Is Not Null                                            

        SELECT
          RDM.ReferralDXCodeMappingID,
          T.ReferralID,
          T.Noteid,
          DC.DXCodeID,
          DC.DXCodeName,
          DC.DxCodeType,
          RDM.Precedence,
          RDM.StartDate,
          RDM.EndDate,
          DC.Description,
          DC.DXCodeWithoutDot,
          DCT.DxCodeShortName
        FROM ReferralDXCodeMappings RDM
        LEFT JOIN Notes T
          ON T.ReferralID = RDM.ReferralID --AND T.RankOrder=1 --AND T.NoteID IS NULL                                                                
          INNER JOIN schedulemasters SM
            ON SM.ReferralID = T.ReferralID --and SM.employeeid=T.employeeid  
          INNER JOIN EmployeeVisits EV
            ON EV.SCheduleID = SM.ScheduleID
          INNER JOIN DXCodes DC
            ON DC.DXCodeID = RDM.DXCodeID
          INNER JOIN DxCodeTypes DCT
            ON DCT.DxCodeTypeID = DC.DxCodeType
        WHERE
          EV.EmployeeVisitID = @EmployeeVisitID

      DELETE NDM
        FROM NoteDXCodeMappings NDM
        INNER JOIN @NoteDxTemp ndt
          ON ndt.NoteID = ndm.NoteID
          AND ndt.ReferralID = ndm.ReferralID
          AND ndt.DXCodeID != ndm.DXCodeID

      INSERT INTO NoteDXCodeMappings
        SELECT DISTINCT
          NDT.ReferralDXCodeMappingID,
          NDT.ReferralID,
          NDT.NoteID,
          NDT.DXCodeID,
          NDT.DXCodeName,
          NDT.DxCodeType,
          NDT.Precedence,
          NDT.StartDate,
          NDT.EndDate,
          NDT.Description,
          NDT.DXCodeWithoutDot,
          NDT.DxCodeShortName
        FROM @NoteDxTemp NDT
        LEFT JOIN NoteDXCodeMappings NDM
          ON NDM.ReferralDXCodeMappingID = NDT.ReferralDXCodeMappingID
          LEFT JOIN NoteDXCodeMappings NDM1
            ON NDM1.NoteID = NDT.NoteID
        WHERE
          NDM.ReferralDXCodeMappingID IS NULL
          OR NDM1.NoteID IS NULL

      --DELETE NoteDXCodeMappings                                                                
      --DELETE NDM FROM NoteDXCodeMappings NDM                                             
      --INNER JOIN @NoteDxTemp NDT ON NDT.ReferralDXCodeMappingID=NDM.ReferralDXCodeMappingID                                                                
      --INNER JOIN @NoteDxTemp NDT1 ON NDT1.NoteID=NDM.NoteID                                                                
      --WHERE NDT.ReferralDXCodeMappingID IS NULL OR NDT1.NoteID IS NULL                                            

      --Update NoteDXCodeMappings Table                                                                
      UPDATE NDM
      SET
        NDM.ReferralDXCodeMappingID = temp.ReferralDXCodeMappingID,
        NDM.NoteID = temp.NoteID,
        NDM.DXCodeID = temp.DXCodeID,
        NDM.DXCodeName = temp.DXCodeName,
        NDM.DxCodeType = temp.DxCodeType,
        NDM.Precedence = temp.Precedence,
        NDM.StartDate = temp.StartDate,
        NDM.EndDate = temp.EndDate,
        NDM.Description = temp.Description,
        NDM.DXCodeWithoutDot = temp.DXCodeWithoutDot,
        NDM.DxCodeShortName = temp.DxCodeShortName
      FROM NoteDXCodeMappings NDM
      INNER JOIN @NoteDxTemp temp
        ON NDM.ReferralDXCodeMappingID = temp.ReferralDXCodeMappingID
        AND NDM.NoteID = temp.NoteID


      ----------------------------------------SignatureLogs-----------------------------------------                                                                
      -- INSERT into SignatureLogs Table                                                                
      INSERT INTO SignatureLogs
      (
        NoteID,
        Signature,
        EmployeeSignatureID,
        SignatureBy,
        Name,
        Date,
        MacAddress,
        SystemID
      )
        SELECT DISTINCT
          N.NoteID,
          ES.SignaturePath,
          ES.EmployeeSignatureID,
          ES.EmployeeID,
          dbo.GetGeneralNameFormat(E.FirstName, E.LastName),
          GETDATE(),
          NULL,
          @SystemID
        FROM EmployeeVisits EV
        INNER JOIN ScheduleMasters SM
          ON SM.ScheduleID = EV.ScheduleID
          INNER JOIN Employees E
            ON E.EmployeeID = SM.EmployeeID
          INNER JOIN EmployeeSignatures ES
            ON ES.EmployeeSignatureID = E.EmployeeSignatureID
          INNER JOIN EmployeeVisitNotes EVN
            ON EVN.EmployeeVisitID = EV.EmployeeVisitID
          INNER JOIN Notes N
            ON N.NoteID = EVN.NoteID
          LEFT JOIN SignatureLogs SL
            ON SL.NoteID = N.NoteID
        WHERE
          EV.EmployeeVisitID = @EmployeeVisitID
          AND SL.NoteID IS NULL


      -- DELETE SignatureLogs                       
      --DELETE FROM SignatureLogs                                                                
      --WHERE NoteID NOT IN (SELECT NoteID FROM EmployeeVisitNotes WHERE EmployeeVisitID=@EmployeeVisitID AND NoteID is not null)                                                    

      DELETE FROM SignatureLogs
      WHERE NoteID IN
        (
          SELECT
            NoteID
          FROM Notes
          WHERE
            IsDeleted = 1
        )



      IF (@ResultRequired = 1)
      BEGIN
        SELECT
          1 AS TransactionResultId;
      END

      IF @@TRANCOUNT > 0
      BEGIN
      COMMIT TRANSACTION trans
    END
  END TRY

  BEGIN CATCH
    --Changed Select -1 to  1  due to deadlock error message that was coming up on the mobile application. API_AddNote Should not be triggered from the mobile application. -Pallav Date : 08/20/2019                                            
    IF (@ResultRequired = 1)
    BEGIN
      SELECT
        1 AS TransactionResultId;--,ERROR_MESSAGE() AS ErrorMessage;        
    END
    IF @@TRANCOUNT > 0
    BEGIN
      ROLLBACK TRANSACTION trans
    END
  END CATCH
END