/* EXEC GetReferralList                     
@NotifyCaseManagerID = '-1',                     
@ChecklistID = '-1',                     
@IsDeleted = '0',                     
@ClinicalReviewID = '-1',                     
@IsSaveAsDraft = '-1',                     
@Groupdids = '',                     
@ServicetypeId = '',                     
@DDType_PatientSystemStatus = '12',                     
@EmployeeId = '1',                     
@ServerDateTime = '2021/07/13 23:28:24',                     
@RecordAccess = '-1',                     
@SortExpression = 'ClientName',                     
@SortType = 'ASC',                     
@FromIndex = '1',                     
@PageSize = '50'                      
*/                    
-- [dbo].[GetTransportAssignPatient]       @TransportID=10, @ClientName='K'                  
                  
CREATE PROCEDURE [dbo].[GetTransportAssignPatient]                         
(                      
  @TransportID bigint = 0,                                    
  @AssigneeID bigint = 0,                                    
  @ClientName varchar(100) = NULL,                                    
  @PayorID bigint = 0,                                    
  @NotifyCaseManagerID int = -1,                                    
  @ChecklistID int = -1,                                    
  @ClinicalReviewID int = -1,                                    
  @CaseManagerID int = 0,                                    
  @ServiceID int = -1,                                    
  @AgencyID bigint = 0,                                    
  @AgencyLocationID bigint = 0,                                    
  @ReferralStatusID bigint = 0,                                    
  @IsSaveAsDraft int = -1,                                    
  @AHCCCSID varchar(20) = NULL,                                    
  @CISNumber varchar(20) = NULL,                                    
  @IsDeleted bigint = -1,                                    
  @SortExpression varchar(100) = '',                                    
  @SortType varchar(10) = 'ASC',                                    
  @FromIndex int=1,                                    
  @PageSize int=10,                                    
  @ParentName varchar(100) = NULL,                                    
  @ParentPhone varchar(100) = NULL,                                    
  @CaseManagerPhone varchar(100) = NULL,                                    
  @LanguageID bigint = 0,                                    
  @RegionID bigint = 0,                                    
  @DDType_PatientSystemStatus int = 12,                                    
  @EmployeeId int = 0,                                    
  @ServicetypeId int = 0,                                    
  @RecordAccess int = 0,                                    
  @ServerDateTime nvarchar(100) = NULL,                                    
  @Groupdids nvarchar(max) = NULL,                              
  @CareTypeID nvarchar(max) = null                       
                    
)                       
AS                                    
BEGIN                        
                    
---- declare  @TransportID bigint = 10                    
--declare                     
--@TransportID bigint = 10,                                    
--  @AssigneeID bigint = 0,                                    
--  @ClientName varchar(100) = NULL,                                    
--  @PayorID bigint = 0,                                    
--  @NotifyCaseManagerID int = -1,                                    
--  @ChecklistID int = -1,                                    
--  @ClinicalReviewID int = -1,                                    
--  @CaseManagerID int = 0,                                    
--  @ServiceID int = -1,                                    
--  @AgencyID bigint = 0,                                    
--  @AgencyLocationID bigint = 0,                                    
--  @ReferralStatusID bigint = 0,                                    
--  @IsSaveAsDraft int = -1,                                    
--  @AHCCCSID varchar(20) = NULL,                                    
--  @CISNumber varchar(20) = NULL,                                    
--  @IsDeleted bigint = -1,                                    
--  @SortExpression varchar(100),                
--  @SortType varchar(10),                                    
--  @FromIndex int,                                    
--  @PageSize int,                 
--  @ParentName varchar(100) = NULL,                                    
--  @ParentPhone varchar(100) = NULL,                                    
--  @CaseManagerPhone varchar(100) = NULL,                                    
--  @LanguageID bigint = 0,                                    
--  @RegionID bigint = 0,                                    
--  @DDType_PatientSystemStatus int = 12,                                    
--  @EmployeeId int = 0,                                    
--  @ServicetypeId int = 0,                                    
--  @RecordAccess int = 0,                                    
--  @ServerDateTime nvarchar(100) = NULL,                                    
--  @Groupdids nvarchar(max) = NULL,                              
--  @CareTypeID nvarchar(max) = null                       
  --new filter regionid        
      
DECLARE @TAP_Startdate DATETIME, @TAP_Enddate DATETIME      
 SELECT      
 @TAP_Startdate = TR.Startdate,      
 @TAP_Enddate = TR.EndDate      
 FROM      
 --TransportAssignPatient TAPIDs (NOLOCK)  
 Transport TR (NOLOCK)  
 WHERE TransportID = @TransportID          
      
DECLARE @TransportAssignAvailablePatientIDs NVARCHAR(MAX) =                                    
(                    
 SELECT STRING_AGG(RTSD.ReferralID, ', ')              
 FROM                     
 ReferralTimeSlotDates RTSD (NOLOCK)      
 WHERE               
 (ReferralTSStartTime between @TAP_Startdate and @TAP_Enddate)      
 or      
 (ReferralTSEndTime between @TAP_Startdate and @TAP_Enddate)      
)         
      
DECLARE @TransportAssignPatientIDs NVARCHAR(MAX) =                                    
(                    
 SELECT STRING_AGG(ReferralID, ', ')              
 FROM                     
 TransportAssignPatient TAPIDs (NOLOCK)                   
 WHERE TransportID = @TransportID      and IsNull(IsDeleted ,0)=0         
 and ReferralID in (select val from GetCSVTable(@TransportAssignAvailablePatientIDs))      
)                    
--select @TransportAssignPatientIDs                    
--select * from GetCSVTable(@TransportAssignPatientIDs)                    
                    
                    
                    
  DECLARE @LoginUserGroupIDs nvarchar(max) =                                    
  (                                    
    SELECT                                    
      e.GroupIDs                                    
    FROM dbo.Employees e   (NOLOCK)                                  
    WHERE                                    
      e.EmployeeID = @EmployeeId                                    
  )                                    
  ;                                    
  WITH SCList AS                                    
  (                                    
    SELECT DISTINCT                                    
      r.ReferralID                                    
    FROM dbo.ScheduleMasters sm   (NOLOCK)                                  
    INNER JOIN dbo.Referrals r    (NOLOCK)                                 
      ON sm.ReferralID = r.ReferralID                                    
      AND r.IsDeleted = 0                                    
      AND r.ReferralStatusID = 1                                    
    LEFT JOIN EmployeeVisits ev  (NOLOCK)                                   
      ON ev.ScheduleID = sm.ScheduleID                                    
    WHERE                                    
      sm.EmployeeID = @EmployeeId                                    
      AND CONVERT(date, sm.StartDate) >= GETDATE()                                    
      AND sm.ScheduleStatusID = 2                                    
      AND (ev.IsSigned = 0                                    
        OR ev.IsSigned IS NULL)                                    
    UNION                                    
    SELECT DISTINCT                                    
      r.ReferralID                            
    FROM referrals r  (NOLOCK)                                   
    INNER JOIN dbo.ReferralCaseloads rc    (NOLOCK)                                 
      ON rc.ReferralID = r.ReferralID                                    
      AND r.IsDeleted = 0       
      AND r.ReferralStatusID = 1                                    
                                    
    WHERE                                    
      rc.EmployeeID = @EmployeeId                                    
  ),                                    
  CTEReferralList AS                                    
  (                                    
    SELECT                                    
      *,                                    
      COUNT(t1.ReferralID) OVER () AS Count                                    
    FROM                                    
    (                                    
      SELECT                                    
        ROW_NUMBER() OVER (ORDER BY  t.TransportAssignPatientID DESC,                                   
        CASE                                    
          WHEN @SortType = 'ASC' THEN CASE                                    
              WHEN @SortExpression = 'ClientName' or @SortExpression =  '' THEN t.[Name]          
     WHEN @SortExpression = 'Address' THEN t.Address              
            END                                    
        END ASC,                                    
        CASE                                    
          WHEN @SortType = 'DESC' THEN CASE                                    
              WHEN @SortExpression = 'ClientName' or @SortExpression =  '' THEN t.Name                                        
              WHEN @SortExpression = 'Address' THEN t.Address                  
            END                                    
        END DESC                                    
        ) AS Row,                                    
        t.*                                    
      FROM                                    
      (                     
  SELECT DISTINCT                     
   r.ReferralID,                                   
   r.LastName + ', ' + r.FirstName AS Name,                     
   c.Address                    
   , cast(1 as bit) as isAssigned                    
   , TAPIDs.Startdate as Startdate                  
   , TAPIDs.EndDate                  
   , TAPIDs.Note                  
   , TAPIDs.IsBillable                  
   , TAPIDs.TransportAssignPatientID                  
   , TAPIDs.TransportID                  
    FROM                   
  TransportAssignPatient TAPIDs (NOLOCK)                    
  inner join Referrals r                                                                                 
  on r.ReferralID = TAPIDs.ReferralID                  
  and IsNull(TAPIDs.IsDeleted,0) = 0            
        LEFT JOIN ContactMappings cmp (NOLOCK)                                    
          ON cmp.ReferralID = r.ReferralID                                    
          AND cmp.ContactTypeID = 1                                    
        LEFT JOIN Contacts c (NOLOCK)                                    
          ON c.ContactID = cmp.ContactID                                    
        LEFT JOIN referralGroup rg  (NOLOCK)                                   
          ON rg.ReferralID = r.ReferralID                       
    LEFT JOIN ReferralDocuments rd  (NOLOCK)                     
    ON rd.UserID = r.ReferralID AND rd.FileName ='Client-FaceSheet' AND rd.ComplianceID=-5                      
  LEFT JOIN DDmaster dm  (NOLOCK)                                   
          ON dm.DDMasterID IN (select val from GetCSVTable(R.CareTypeIds))           
        WHERE                                    
  r.ReferralID in (select val from GetCSVTable(@TransportAssignPatientIDs))                    
  and IsNull(TAPIDs.IsDeleted,0) = 0              
  and TAPIDs.TransportID = @TransportID          
  ----1                  
  union                  
  ----2                  
        SELECT DISTINCT                                    
          r.ReferralID,     
    r.LastName + ', ' + r.FirstName AS Name,                             
          c.Address      ,                  
    cast(0 as bit) as isAssigned                    
    , null as Startdate                  
    , null as EndDate                  
    , null as Note                  
    , null as IsBillable                  
    , null as TransportAssignPatientID                  
 , null as TransportID                  
    --, cast(0 as bit) as isAssigned                    
        FROM Referrals r   (NOLOCK)                                  
        LEFT JOIN ReferralStatuses RS    (NOLOCK)                                 
          ON rs.ReferralStatusID = r.ReferralStatusID                                    
        LEFT JOIN Employees e    (NOLOCK)                                 
          ON e.EmployeeID = r.Assignee                                    
        LEFT JOIN ReferralPayorMappings rp   (NOLOCK)                                  
          ON rp.ReferralID = r.ReferralID                                    
 AND rp.IsActive = 1                                    
          AND rp.IsDeleted = 0                                    
          AND rp.Precedence = 1                                    
        LEFT JOIN Payors p  (NOLOCK)                                   
          ON p.PayorID = rp.PayorID                                    
        LEFT JOIN CaseManagers cm  (NOLOCK)                                   
          ON cm.CaseManagerID = r.CaseManagerID                                    
        LEFT JOIN Agencies a  (NOLOCK)                                   
          ON a.AgencyID = cm.AgencyID                                    
        LEFT JOIN AgencyLocations al  (NOLOCK)                                   
          ON al.AgencyLocationID = r.AgencyLocationID                                    
        LEFT JOIN ReferralCheckLists rc  (NOLOCK)                                   
          ON rc.ReferralID = r.ReferralID                                    
        LEFT JOIN ReferralSparForms rsf  (NOLOCK)                                   
          ON rsf.ReferralID = r.ReferralID                                    
        LEFT JOIN Employees es   (NOLOCK)                                  
          ON es.EmployeeID = rsf.SparFormCompletedBy                                    
        LEFT JOIN Employees er                                    
          ON er.EmployeeID = rc.ChecklistCompletedBy                                    
        LEFT JOIN Employees eself (NOLOCK)                                    
          ON eself.EmployeeID = r.CreatedBy                                    
        LEFT JOIN Employees eselfUP (NOLOCK)                                    
          ON eselfUP.EmployeeID = r.UpdatedBy               
        --left join ContactMappings cmp on cmp.ReferralID= r.ReferralID and cmp.ContactTypeID in (1,2)                                                            
        LEFT JOIN ContactMappings cmp (NOLOCK)                                    
          ON cmp.ReferralID = r.ReferralID                                    
          AND cmp.ContactTypeID = 1                                    
        LEFT JOIN Contacts c (NOLOCK)                                    
          ON c.ContactID = cmp.ContactID                                    
        LEFT JOIN referralGroup rg     (NOLOCK)                                
          ON rg.ReferralID = r.ReferralID                       
    LEFT JOIN ReferralDocuments rd   (NOLOCK)                    
    ON rd.UserID = r.ReferralID AND rd.FileName ='Client-FaceSheet' AND rd.ComplianceID=-5                      
  LEFT JOIN DDmaster dm  (NOLOCK)                                   
          ON dm.DDMasterID IN (select val from GetCSVTable(R.CareTypeIds))                                  
        WHERE                         
   r.ReferralID NOT IN (select val from GetCSVTable(@TransportAssignPatientIDs))                     
   AND r.ReferralID IN (select val from GetCSVTable(@TransportAssignAvailablePatientIDs))      
   AND                  
          ((CAST(@IsDeleted AS bigint) = -1)                                    
            OR r.IsDeleted = @IsDeleted)                                    
          AND ((@ClientName IS NULL                                    
              OR LEN(r.LastName) = 0)                                    
     --OR r.FirstName+' '+r.LastName LIKE '%' + @ClientName + '%' OR r.LastName+' '+r.FirstName LIKE '%' + @ClientName + '%')                                                                    
          OR                                    
          ((r.FirstName LIKE '%' + @ClientName + '%')                                    
              OR (r.LastName LIKE '%' + @ClientName + '%')                                    
              OR (r.FirstName + ' ' + r.LastName LIKE '%' + @ClientName + '%')                                    
              OR (r.LastName + ' ' + r.FirstName LIKE '%' + @ClientName + '%')                                    
              OR (r.FirstName + ', ' + r.LastName LIKE '%' + @ClientName + '%')                                    
              OR (r.LastName + ', ' + r.FirstName LIKE '%' + @ClientName + '%'))                                    
          )                                    
          AND ((@AHCCCSID IS NULL                                    
              OR LEN(@AHCCCSID) = 0)                                    
            OR r.AHCCCSID LIKE '%' + @AHCCCSID + '%')                                    
          AND ((@CISNumber IS NULL                                    
              OR LEN(@CISNumber) = 0)                                    
            OR r.CISNumber LIKE '%' + @CISNumber + '%')                                    
          AND ((CAST(@PayorID AS bigint) = 0)                                    
            OR rp.PayorID = CAST(@PayorID AS bigint))                                
   AND ((@CareTypeID  IS NULL OR LEN(@CareTypeID)=0) Or (r.CareTypeIds like '%'+ @CareTypeID + '%'))                         
                              
   --AND (@PayorID  = '' Or rp.PayorID like '%'+ CAST(@PayorID AS varchar(MAX))+ '%')                                  
          AND (@Groupdids IS NULL                                    
            OR LEN(@Groupdids) = 0                             
            OR rg.val IN                                    
      (                                    
     SELECT                                    
       CONVERT(bigint, VAL)                                    
     FROM GetCSVTable(@Groupdids)                                    
      )                                    
          )                                    
                                    
 AND ((CAST(@ReferralStatusID AS bigint) = 0)                                    
            OR r.ReferralStatusID = CAST(@ReferralStatusID AS bigint))                                    
          AND ((CAST(@AssigneeID AS bigint) = 0)                                    
            OR r.Assignee = CAST(@AssigneeID AS bigint))                                    
          AND ((CAST(@ChecklistID AS int) = -1)                                    
            OR (CAST(@ChecklistID AS int) = 0                                    
              AND (rc.IsCheckListCompleted = 0                                    
                OR rc.IsCheckListCompleted IS NULL))                          
            OR rc.IsCheckListCompleted = CAST(@ChecklistID AS int))                                    
          AND ((CAST(@ClinicalReviewID AS int) = -1)                                    
            OR (CAST(@ClinicalReviewID AS int) = 0                                    
              AND (rsf.IsSparFormCompleted = 0                                    
                OR rsf.IsSparFormCompleted IS NULL))                                    
            OR rsf.IsSparFormCompleted = CAST(@ClinicalReviewID AS int))                                    
          AND ((CAST(@IsSaveAsDraft AS int) = -1)                                    
            OR r.IsSaveAsDraft = CAST(@IsSaveAsDraft AS int))                                   
          AND ((CAST(@NotifyCaseManagerID AS int) = -1)                                  
            OR r.NotifyCaseManager = CAST(@NotifyCaseManagerID AS int))                                    
          AND ((CAST(@CaseManagerID AS bigint) = 0)                                    
            OR r.CaseManagerID = CAST(@CaseManagerID AS bigint))                                    
          AND ((CAST(@ServiceID AS bigint) = -1)                                    
            OR (CAST(@ServiceID AS bigint) = 0                                    
              AND r.RespiteService = 1)                                    
            OR (CAST(@ServiceID AS bigint) = 1                         
              AND r.LifeSkillsService = 1)                                    
OR (CAST(@ServiceID AS bigint) = 2                                    
              AND r.CounselingService = 1)                                    
            OR (CAST(@ServiceID AS bigint) = 3                                    
              AND r.ConnectingFamiliesService = 1))                                    
          AND ((CAST(@AgencyID AS bigint) = 0)                                    
            OR r.AgencyID = CAST(@AgencyID AS bigint))                                    
          AND ((CAST(@AgencyLocationID AS bigint) = 0)                                    
            OR r.AgencyLocationID = CAST(@AgencyLocationID AS bigint))                                    
                                    
          AND (                                    
          (@ParentName IS NULL                               
              OR LEN(c.LastName) = 0)                                    
            OR (                                    
          (c.FirstName LIKE '%' + @ParentName + '%')                                    
              OR (c.LastName LIKE '%' + @ParentName + '%')                                    
              OR (C.FirstName + ' ' + C.LastName LIKE '%' + @ParentName + '%')                                    
              OR (C.LastName + ' ' + C.FirstName LIKE '%' + @ParentName + '%')                                    
              OR (C.FirstName + ', ' + C.LastName LIKE '%' + @ParentName + '%')                                    
              OR (C.LastName + ', ' + C.FirstName LIKE '%' + @ParentName + '%')))                                    
                                    
          AND ((@ParentPhone IS NULL                                    
              OR LEN(@ParentPhone) = 0)                                    
            OR (c.Phone1 LIKE '%' + @ParentPhone + '%')                                    
            OR (c.Phone2 LIKE '%' + @ParentPhone + '%'))                                    
          AND ((@CaseManagerPhone IS NULL                                    
              OR LEN(@CaseManagerPhone) = 0)                                    
            OR cm.Phone LIKE '%' + @CaseManagerPhone + '%')                  
          AND ((CAST(@LanguageID AS bigint) = 0)                                    
            OR r.LanguageID = CAST(@LanguageID AS bigint))                                    
          AND ((CAST(@RegionID AS bigint) = 0)                              OR r.RegionID = CAST(@RegionID AS bigint))                                    
          --AND ((CAST(ISNULL(@ServicetypeId, 0) AS bigint) = 0)                                    
          --  OR r.ServiceType = CAST(@ServicetypeId AS bigint))                                 
    AND (@ServicetypeId  = '' Or r.ServiceType=CAST(@ServicetypeId AS varchar(MAX)))                                
                                
          AND (@RecordAccess = -1                                    
            OR (@RecordAccess = 1                                    
              AND EXISTS                                    
     (                                    
     SELECT                                    
      1                                    
     FROM GetCSVTable(r.GroupIDs) eg                                    
     INNER JOIN GetCSVTable(@LoginUserGroupIDs) LG                                    
 ON eg.val = LG.val                                    
     )                                    
    )                                    
                                    
                                    
            OR (@RecordAccess IN (1, 0)                                    
              AND (                                    
    r.Assignee = @EmployeeId                                    
                OR r.CaseManagerID = @EmployeeId                                    
                OR r.ReferralID IN                                 
        (                                    
       SELECT                                    
         ReferralID                                    
       FROM SCList                                    
      )                                    
     )                                    
    )                                    
          )                                    
      ) AS t          
   --order by t.TransportAssignPatientID, t.[Row]        
    ) AS t1                                    
  )                                    
                    
                                    
  SELECT                                    
  DISTINCT                                    
    *                                    
  FROM CTEReferralList                                    
  WHERE                                    
    ROW BETWEEN ((@PageSize * (@FromIndex - 1)) + 1) AND (@PageSize * @FromIndex)                                    
                                
END 