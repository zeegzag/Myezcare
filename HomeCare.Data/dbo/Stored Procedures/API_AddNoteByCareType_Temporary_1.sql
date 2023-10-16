-- EXEC [API_AddNoteByCareType_Temporary] '10906', '47', null, 0                     
-- SELECT  * FROM  Notes_Temporary                    
CREATE PROCEDURE [dbo].[API_AddNoteByCareType_Temporary]                        
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



  DECLARE @IsSupervisingProviderRequired BIT;                      
  SELECT TOP 1 @IsSupervisingProviderRequired = IsSupervisingProviderRequired FROM PayorEdi837Settings WHERE PayorID=@PayorID
  
                        
  BEGIN TRANSACTION trans                        
    BEGIN TRY                        
                      
                        
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
        IsBillable bit,                          EmployeeID bigint,                        
        EmployeeFirstName nvarchar(200),                        
        EmployeeLastName nvarchar(200),                        
        EmployeeUniqueID varchar(200),                        
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
        ReferralBillingAuthorizationID int,              
  RenderingProvider_TaxonomyCode NVARCHAR(100)              
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
        EmployeeUniqueID,                        
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
  RenderingProvider_TaxonomyCode ,              
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
            --E.EmployeeUniqueID,                        
			EmployeeUniqueID= CASE WHEN @IsSupervisingProviderRequired=1 THEN E.HHA_NPI_ID ELSE EmployeeUniqueID END,                      
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
            RBA.ReferralBillingAuthorizationID,              
   RenderingProvider_TaxonomyCode = DT.Value            
            
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
            INNER JOIN ReferralBillingAuthorizations AS RBA   ON RBA.ReferralID = RPM.ReferralID  AND RBA.ReferralBillingAuthorizationID = SM.ReferralBillingAuthorizationID       
   AND RBA.Type='CMS1500' AND RBA.IsDeleted=0 AND (CONVERT(date,EV.ClockInTime) between RBA.StartDate and RBA.EndDate) and RBA.PayorID=RPM.PayorID                       
            
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
            
     LEFT JOIN DDMaster DT  ON DT.DDMasterID = RBA.TaxonomyID            
          WHERE                        
            EV.EmployeeVisitID = @EMPLOYEEVISITID                        
            AND ev.isDeleted = 0                        
            AND ev.isPCACompleted = 1 --AND (N.IsDeleted=0 OR N.IsDeleted IS NULL)                              
                         
        ) AS T                        
                          
                    
                    
                      
                       
                    
                      
   declare             
  @BillingProvider_NM103_NameLastOrOrganizationName varchar(max)            
  select @BillingProvider_NM103_NameLastOrOrganizationName=BillingProvider_NM103_NameLastOrOrganizationName from OrganizationSettings     
                     
                     
  --Insert into Note Table - Only New Notes                          
      INSERT INTO Notes_Temporary                        
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
        ReferralBillingAuthorizationID,              
  RenderingProvider_TaxonomyCode,        
  SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName,        
  SupervisingProvidername2420DLoop_NM104_NameFirst,        
  SupervisingProvidername2420DLoop_REF02_ReferenceId        
        
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
              
    NULL,                        
          @BillingProviderName,          
          @BillingProviderFirstName,                        
          @BillingProviderAddress,                        
          @BillingProviderCity,                        
          @BillingProviderState,                        
          @BillingProviderZipcode,                        
          @BillingProviderNPI,           
          
          
          
          
          
          
                    
          --EmployeeID,                        
          --@BillingProvider_NM103_NameLastOrOrganizationName,                        
--EmployeeFirstName,                        
          --@BillingProviderAddress,                        
          --@BillingProviderCity,                        
          --@BillingProviderState,                        
          --@BillingProviderZipcode,                        
          --CASE                        
          --  WHEN HHA_NPI_ID IS NULL OR                        
          --    LEN(HHA_NPI_ID) = 0 THEN @BillingProviderNPI                        
          --  ELSE HHA_NPI_ID                        
          --END AS 'HHA_NPI_ID',            
              
          
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
          ReferralBillingAuthorizationID,              
       RenderingProvider_TaxonomyCode      ,        
            
    EmployeelastName,        
    EmployeeFirstName,        
    EmployeeUniqueID        
         
        
        FROM @Temp T1                        
        WHERE                        
          T1.RankOrder = 1                        
          AND T1.AddNewNote = 1 -- T1.NoteID IS NULL          
                        
                 
                     
 -- EXEC [API_AddNoteByCareType_Temporary] '11540', 0, null, 0                     
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
                  
          
  N.RenderingProviderName = @BillingProviderName,                    
        N.RenderingProviderFirstName = @BillingProviderFirstName,                    
        N.RenderingProviderAddress = @BillingProviderAddress,                    
        N.RenderingProviderCity = @BillingProviderCity,                    
        N.RenderingProviderState = @BillingProviderState,                    
        N.RenderingProviderZipcode = @BillingProviderZipcode,                    
        N.RenderingProviderNPI = @BillingProviderNPI,          
          
          
          
          
  --N.RenderingProviderName = @BillingProvider_NM103_NameLastOrOrganizationName,            
  --      N.RenderingProviderFirstName = T.EmployeeFirstName,                        
  --      N.RenderingProviderAddress = T.Address,                        
  --      N.RenderingProviderCity = T.City,                        
  --      N.RenderingProviderState = T.StateCode,                        
  --      N.RenderingProviderZipcode = T.ZipCode,                        
  --      N.RenderingProviderNPI =                        
  --                                CASE                        
  --                                  WHEN T.HHA_NPI_ID IS NULL OR                        
  --                                    LEN(T.HHA_NPI_ID) = 0 THEN @BillingProviderNPI                        
  --                         ELSE T.HHA_NPI_ID                        
  --                                END,                        
                                                                           
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
        N.ReferralBillingAuthorizationID = T.ReferralBillingAuthorizationID,              
  N.RenderingProvider_TaxonomyCode = T.RenderingProvider_TaxonomyCode,       
        
  N.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName  = T.EmployeelastName,        
  N.SupervisingProvidername2420DLoop_NM104_NameFirst      = T.EmployeeFirstName,        
  N.SupervisingProvidername2420DLoop_REF02_ReferenceId     = T.EmployeeUniqueID        
                
      FROM Notes_Temporary N                        
      INNER JOIN @Temp T                        
        ON T.NoteID = N.NoteID                        
                    
   -- EXEC [API_AddNoteByCareType_Temporary] '11540', 0, null, 0                     
                
                        
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                        
                        
                        
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
        1 AS TransactionResultId,ERROR_MESSAGE() AS ErrorMessage;                                
    END                        
    IF @@TRANCOUNT > 0                        
    BEGIN                        
   SELECT 1 AS TransactionResultId,ERROR_MESSAGE() AS ErrorMessage;                     
      ROLLBACK TRANSACTION trans                        
    END                        
  END CATCH                        
END