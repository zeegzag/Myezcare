CREATE PROCEDURE [dbo].[GetReferralList]             
          
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
  @SortExpression varchar(100),                        
  @SortType varchar(10),                        
  @FromIndex int,                        
  @PageSize int,                        
  @ParentName varchar(100) = NULL,                        
  @ParentPhone varchar(100) = NULL,                        
  @CaseManagerPhone varchar(100) = NULL,                        
  @LanguageID bigint = 0,                        
  @RegionID bigint = 0,                        
  @DDType_PatientSystemStatus int = 12,                        
  @EmployeeId int = 0,                        
  @ServicetypeId nvarchar(max) = NULL,                        
  @RecordAccess int = 0,                        
  @ServerDateTime nvarchar(100) = NULL,                        
  @Groupdids nvarchar(max) = NULL,                  
  @CareTypeID nvarchar(max) = null           
            
            
AS                        
BEGIN              
declare @NameFormat VARCHAR(500)=dbo.GetOrgNameFormat();        
        
  DECLARE @LoginUserGroupIDs nvarchar(max) =                        
  (                        
    SELECT                        
      e.GroupIDs                        
    FROM dbo.Employees e                        
    WHERE                        
      e.EmployeeID = @EmployeeId                        
  )                        
  ;                        
  WITH SCList AS                        
  (                        
    SELECT DISTINCT                        
      r.ReferralID                        
    FROM dbo.ScheduleMasters sm                        
    INNER JOIN dbo.Referrals r                        
      ON sm.ReferralID = r.ReferralID                        
      AND r.IsDeleted = 0                        
      AND r.ReferralStatusID = 1                        
    LEFT JOIN EmployeeVisits ev                        
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
    FROM referrals r                        
    INNER JOIN dbo.ReferralCaseloads rc                        
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
        ROW_NUMBER() OVER (ORDER BY                        
                        
        CASE                        
         WHEN @SortType = 'ASC' THEN CASE                        
              WHEN @SortExpression = 'AHCCCSID' THEN CONVERT(varchar(50), t.AHCCCSID)                        
      END                        
        END ASC,                        
        CASE                        
          WHEN @SortType = 'DESC' THEN CASE                        
              WHEN @SortExpression = 'AHCCCSID' THEN CONVERT(varchar(50), t.AHCCCSID)                        
            END                        
        END DESC,                        
                        
        CASE                        
        WHEN @SortType = 'ASC' THEN CASE                        
              WHEN @SortExpression = 'CISNumber' THEN CAST(t.CISNumber AS bigint)                        
   END                        
        END ASC,                        
        CASE                        
          WHEN @SortType = 'DESC' THEN CASE                        
              WHEN @SortExpression = 'CISNumber' THEN CAST(t.CISNumber AS bigint)                        
            END                        
        END DESC,                     
                        
        CASE                        
          WHEN @SortType = 'ASC' THEN CASE                        
              WHEN @SortExpression = 'Gender' THEN CONVERT(char(1), t.Gender)                  
            END                        
        END ASC,                        
        CASE                        
          WHEN @SortType = 'DESC' THEN CASE                        
              WHEN @SortExpression = 'Gender' THEN CONVERT(char(1), t.Gender)                 
            END                        
        END DESC,                        
                        
        CASE                        
          WHEN @SortType = 'ASC' THEN CASE                        
              WHEN @SortExpression = 'Age' THEN t.Dob                        
            END                        
        END ASC,                        
        CASE                        
          WHEN @SortType = 'DESC' THEN CASE                        
              WHEN @SortExpression = 'Age' THEN t.Dob                        
            END                        
        END DESC,                        
                        
        CASE                        
          WHEN @SortType = 'ASC' THEN CASE                        
              WHEN @SortExpression = 'ClientName' THEN t.Name                        
              WHEN @SortExpression = 'NickName' THEN t.ClientNickName                        
              WHEN @SortExpression = 'FaciliatorName' THEN t.FaciliatorName                        
              --WHEN @SortExpression = 'ContractName' THEN t.ContractName                                                  
              WHEN @SortExpression = 'CompanyName' THEN t.CompanyName                        
              WHEN @SortExpression = 'Status' THEN t.Status                        
              WHEN @SortExpression = 'AssigneeName' THEN t.AssigneeName                        
  WHEN @SortExpression = 'Address' THEN t.Address                        
              WHEN @SortExpression = 'CreatedDate' THEN CONVERT(varchar(10), t.CreatedDate, 105)                        
              WHEN @SortExpression = 'ModifiedDate' THEN CONVERT(varchar(10), t.UpdatedDate, 105)                        
            END                        
        END ASC,                        
        CASE                        
          WHEN @SortType = 'DESC' THEN CASE                        
              WHEN @SortExpression = 'ClientName' THEN t.Name                        
              WHEN @SortExpression = 'NickName' THEN t.ClientNickName                        
              WHEN @SortExpression = 'FaciliatorName' THEN t.FaciliatorName                        
              --WHEN @SortExpression = 'ContractName' THEN t.ContractName                                                  
              WHEN @SortExpression = 'CompanyName' THEN t.CompanyName                        
              WHEN @SortExpression = 'Status' THEN t.Status                        
              WHEN @SortExpression = 'AssigneeName' THEN t.AssigneeName                        
              WHEN @SortExpression = 'Address' THEN t.Address                        
 WHEN @SortExpression = 'CreatedDate' THEN CONVERT(varchar(10), t.CreatedDate, 105)--CONVERT(DateTime, r.CreatedDate)                                                        
              WHEN @SortExpression = 'ModifiedDate' THEN CONVERT(varchar(10), t.UpdatedDate, 105) --CONVERT(DateTime, r.UpdatedDate)                                         
            END                        
        END DESC                        
        ) AS Row,                        
        t.*                        
      FROM                        
      (                        
        SELECT DISTINCT                        
          r.ReferralID,                       
          dbo.GetGenericNameFormat(r.FirstName,r.MiddleName,r.LastName,@NameFormat) AS Name,                        
          r.ClientID,                        
          r.AHCCCSID,                        
          r.ReferralStatusID,                        
          r.IsSaveAsDraft,                        
          r.CISNumber,                        
          r.Gender,                        
          r.Dob,                        
          dbo.GetAge(r.Dob) AS Age,                        
          RS.Status,                        
          r.Assignee,                        
          dbo.GetGenericNameFormat(e.FirstName,e.MiddleName,e.LastName,@NameFormat) AS AssigneeName,--rp.PayorID,p.ShortName as ContractName,                                    
          r.PlacementRequirement,                        
          r.CaseManagerID,                        
          dbo.GetGenericNameFormat(cm.FirstName,'',cm.LastName,@NameFormat) AS FaciliatorName,                        
          r.ZSPLifeSkills,                        
          r.ZSPRespite,                        
          r.ZSPCounselling,                        
          r.CreatedDate,                        
          r.CreatedBy,                   
          dbo.GetGenericNameFormat(eself.FirstName,eself.MiddleName,eself.LastName,@NameFormat) AS CreatedName,                        
          a.NickName AS CompanyName,                
          al.LocationName,                        
          rc.IsCheckListCompleted,                        
          rc.ChecklistCompletedBy,                        
          dbo.GetGenericNameFormat(er.FirstName,er.MiddleName,er.LastName,@NameFormat) AS CheckListName,                        
          rc.ChecklistCompletedDate,                        
          rsf.IsSparFormCompleted,                        
          rsf.SparFormCompletedBy,                        
          dbo.GetGenericNameFormat(es.FirstName,es.MiddleName,es.LastName,@NameFormat) AS ClinicalReviewName,                        
          rsf.SparFormCompletedDate,                        
          r.NotifyCaseManager,                        
          r.AgencyID,                        
          r.AgencyLocationID,                        
          r.UpdatedDate,                        
          dbo.GetGenericNameFormat(eselfUP.FirstName,eselfUP.MiddleName,eselfUP.LastName,@NameFormat) AS UpdatedName,                        
          r.IsDeleted,                        
          r.CounselingService,                        
          r.LifeSkillsService,                        
    r.RespiteService,                        
          r.ClientNickName,                        
          dbo.GetCommaSepCategoriesRef(r.ReferralID) AS [GroupNames],                   
    [dbo].[Fn_GetCareTypes](r.CareTypeIds)  AS CareTypeIds,                   
          c.Address,                        
          c.City,                        
          c.ZipCode,                        
          c.State ,                      
  dbo.Fn_GetPayor(r.ReferralID) AS [PayorName],          
  rd.GoogleFileId, rd.ReferralDocumentID          
        FROM Referrals r                        
        LEFT JOIN ReferralStatuses RS                        
          ON rs.ReferralStatusID = r.ReferralStatusID                        
        LEFT JOIN Employees e                        
          ON e.EmployeeID = r.Assignee                        
        LEFT JOIN ReferralPayorMappings rp        
          ON rp.ReferralID = r.ReferralID                        
          AND rp.IsActive = 1                        
          AND rp.IsDeleted = 0                        
          AND rp.Precedence = 1                        
        LEFT JOIN Payors p                        
          ON p.PayorID = rp.PayorID                        
        LEFT JOIN CaseManagers cm                        
          ON cm.CaseManagerID = r.CaseManagerID          
        LEFT JOIN Agencies a                        
          ON a.AgencyID = cm.AgencyID                        
        LEFT JOIN AgencyLocations al                        
          ON al.AgencyLocationID = r.AgencyLocationID                        
        LEFT JOIN ReferralCheckLists rc                        
          ON rc.ReferralID = r.ReferralID                        
        LEFT JOIN ReferralSparForms rsf                        
          ON rsf.ReferralID = r.ReferralID                 
        LEFT JOIN Employees es                        
          ON es.EmployeeID = rsf.SparFormCompletedBy                        
        LEFT JOIN Employees er                        
          ON er.EmployeeID = rc.ChecklistCompletedBy                        
        LEFT JOIN Employees eself                        
          ON eself.EmployeeID = r.CreatedBy                        
        LEFT JOIN Employees eselfUP                        
          ON eselfUP.EmployeeID = r.UpdatedBy                        
        --left join ContactMappings cmp on cmp.ReferralID= r.ReferralID and cmp.ContactTypeID in (1,2)                                                
        LEFT JOIN ContactMappings cmp                        
          ON cmp.ReferralID = r.ReferralID                        
          AND cmp.ContactTypeID = 1                        
        LEFT JOIN Contacts c                        
          ON c.ContactID = cmp.ContactID                        
        LEFT JOIN referralGroup rg                        
          ON rg.ReferralID = r.ReferralID           
    LEFT JOIN ReferralDocuments rd          
    ON rd.UserID = r.ReferralID AND rd.FileName ='Client-FaceSheet' AND rd.ComplianceID=-5          
  LEFT JOIN DDmaster dm                        
          ON dm.DDMasterID IN (select val from GetCSVTable(R.CareTypeIds))                      
        WHERE                        
          ((CAST(@IsDeleted AS bigint) = -1)                        
            OR r.IsDeleted = @IsDeleted)                        
          AND ((@ClientName IS NULL                        
              OR LEN(r.LastName) = 0)                        
     --OR r.FirstName+' '+r.LastName LIKE '%' + @ClientName + '%' OR r.LastName+' '+r.FirstName LIKE '%' + @ClientName + '%')                                                        
          OR                        
          (      
    (r.FirstName LIKE '%' + @ClientName + '%')                        
              OR (r.LastName LIKE '%' + @ClientName + '%')                        
              OR (r.FirstName + ' ' + r.LastName LIKE '%' + @ClientName + '%')                        
              OR (r.LastName + ' ' + r.FirstName LIKE '%' + @ClientName + '%')                        
              OR (r.FirstName + ', ' + r.LastName LIKE '%' + @ClientName + '%')                        
              OR (r.LastName + ', ' + r.FirstName LIKE '%' + @ClientName + '%')      
     OR (dbo.GetGenericNameFormat(r.FirstName,r.MiddleName,r.LastName,@NameFormat) LIKE '%' + @ClientName + '%')      
     )                        
          )                        
          AND ((@AHCCCSID IS NULL                        
              OR LEN(@AHCCCSID) = 0)                        
            OR r.AHCCCSID LIKE '%' + @AHCCCSID + '%')                        
          AND ((@CISNumber IS NULL                        
              OR LEN(@CISNumber) = 0)                        
            OR r.CISNumber LIKE '%' + @CISNumber + '%')                        
          AND ((CAST(@PayorID AS bigint) = 0)                        
            OR rp.PayorID = CAST(@PayorID AS bigint))                    
   AND ((@CareTypeID IS NULL OR LEN(@CareTypeID)=0) Or (r.CareTypeIds like '%'+ @CareTypeID + '%'))             
                  
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
              OR (C.LastName + ', ' + C.FirstName LIKE '%' + @ParentName + '%')      
     OR (dbo.GetGenericNameFormat(C.FirstName,'',C.LastName,@NameFormat) LIKE '%' + @ClientName + '%')      
     ))                        
                        
          AND ((@ParentPhone IS NULL                        
              OR LEN(@ParentPhone) = 0)                        
            OR (c.Phone1 LIKE '%' + @ParentPhone + '%')                        
            OR (c.Phone2 LIKE '%' + @ParentPhone + '%'))                        
          AND ((@CaseManagerPhone IS NULL                        
              OR LEN(@CaseManagerPhone) = 0)                        
            OR cm.Phone LIKE '%' + @CaseManagerPhone + '%')                        
          AND ((CAST(@LanguageID AS bigint) = 0)                        
            OR r.LanguageID = CAST(@LanguageID AS bigint))                        
          AND ((CAST(@RegionID AS bigint) = 0)                        
           OR r.RegionID = CAST(@RegionID AS bigint))                        
          --AND ((CAST(ISNULL(@ServicetypeId, 0) AS bigint) = 0)                        
          --  OR r.ServiceType = CAST(@ServicetypeId AS bigint))                     
    --AND (@ServicetypeId  = '' Or r.ServiceType=CAST(@ServicetypeId AS varchar(MAX)))                    
                  AND (@ServicetypeId  = '' Or r.ServiceType in (select val from GetCSVTable(@ServicetypeId)))  
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
    ) AS t1                        
  )                        
                        
  SELECT                        
  DISTINCT                        
    *                        
  FROM CTEReferralList                        
  WHERE                        
    ROW BETWEEN ((@PageSize * (@FromIndex - 1)) + 1) AND (@PageSize * @FromIndex)                        
                        
END 