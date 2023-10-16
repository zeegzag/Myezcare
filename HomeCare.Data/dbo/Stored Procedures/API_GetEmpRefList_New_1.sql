  
-- =============================================    
-- Author:  Ashar A    
-- Create date: 26 Oct 2021    
-- Description: SP to get list of patient for a employee with caretype and group id parameters    
-- =============================================    
CREATE PROCEDURE [dbo].[API_GetEmpRefList_New]    
 @EmployeeIDs NVARCHAR(500) = NULL    
 ,@ReferralIDs NVARCHAR(500) = NULL    
 ,@StartDate DATE =null    
 ,@EndDate DATE = null    
 ,@PayorIDs NVARCHAR(500) = NULL     
 ,@SortExpression NVARCHAR(100)    
 ,@SortType NVARCHAR(10)    
 ,@FromIndex INT    
 ,@PageSize INT    
 ,@CareTypeIDs NVARCHAR(500) = NULL    
 ,@GroupIds nvarchar(max) = NULL    
AS    
  
If (@StartDate IS NULL) begin set @startDate=getdate() end  
If (@EndDate IS NULL) begin set @EndDate=getdate() end  
  
Begin  
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
;WITH CTEScheduleMasterList    
 AS (    
  SELECT *    
   ,COUNT(t1.ScheduleID) OVER () AS Count    
  FROM (    
   SELECT ROW_NUMBER() OVER (    
     ORDER BY CASE     
       WHEN @SortType = 'ASC'    
        THEN CASE     
          WHEN @SortExpression = 'StartDate'    
           THEN CONVERT(DATETIME, sm.StartDate, 103)    
          END    
       END ASC    
      ,CASE     
       WHEN @SortType = 'DESC'    
        THEN CASE     
          WHEN @SortExpression = 'StartDate'    
           THEN CONVERT(DATETIME, sm.StartDate, 103)    
          END    
       END DESC    
      ,CASE     
       WHEN @SortType = 'ASC'    
        THEN CASE     
          WHEN @SortExpression = 'EndDate'    
           THEN CONVERT(DATETIME, sm.EndDate, 103)    
          END    
       END ASC    
      ,CASE     
       WHEN @SortType = 'DESC'    
        THEN CASE     
          WHEN @SortExpression = 'EndDate'    
           THEN CONVERT(DATETIME, sm.EndDate, 103)    
          END    
       END DESC    
      ,CASE     
       WHEN @SortType = 'ASC'    
        THEN CASE     
          WHEN @SortExpression = 'UpdatedDate'    
           THEN CONVERT(DATETIME, sm.UpdatedDate, 103)    
          END    
       END ASC    
      ,CASE     
       WHEN @SortType = 'DESC'    
        THEN CASE     
          WHEN @SortExpression = 'UpdatedDate'    
           THEN CONVERT(DATETIME, sm.UpdatedDate, 103)    
          END    
       END DESC    
      ,CASE     
       WHEN @SortType = 'ASC'    
        THEN CASE     
          WHEN @SortExpression = 'CreatedDate'    
           THEN CONVERT(DATETIME, sm.CreatedDate, 103)    
          END    
       END ASC    
      ,CASE     
       WHEN @SortType = 'DESC'    
        THEN CASE     
          WHEN @SortExpression = 'CreatedDate'    
           THEN CONVERT(DATETIME, sm.CreatedDate, 103)    
          END    
       END DESC    
      ,CASE     
       WHEN @SortType = 'ASC'    
        THEN CASE     
          WHEN @SortExpression = 'Status'    
           THEN ss.ScheduleStatusName    
          END    
       END ASC    
      ,CASE     
       WHEN @SortType = 'DESC'    
        THEN CASE     
          WHEN @SortExpression = 'Status'    
           THEN ss.ScheduleStatusName    
          END    
       END DESC    
      ,CASE     
       WHEN @SortType = 'ASC'    
        THEN CASE     
          WHEN @SortExpression = 'Name'    
           THEN r.LastName    
          END    
       END ASC    
      ,CASE     
       WHEN @SortType = 'DESC'    
        THEN CASE     
          WHEN @SortExpression = 'Name'    
           THEN r.LastName    
          END    
       END DESC    
      ,CASE     
       WHEN @SortType = 'ASC'    
        THEN CASE     
          WHEN @SortExpression = 'ParentName'    
           THEN c.LastName + ' ' + c.FirstName    
          END    
       END ASC    
      ,CASE     
       WHEN @SortType = 'DESC'    
        THEN CASE     
          WHEN @SortExpression = 'ParentName'    
           THEN c.LastName + ' ' + c.FirstName    
          END    
       END DESC    
      ,CASE     
       WHEN @SortType = 'ASC'    
        THEN CASE     
          WHEN @SortExpression = 'Facility'    
           THEN f.FacilityName    
          END    
       END ASC    
      ,CASE     
 WHEN @SortType = 'DESC'    
        THEN CASE     
          WHEN @SortExpression = 'Facility'    
           THEN f.FacilityName    
          END    
       END DESC    
      ,CASE     
       WHEN @SortType = 'ASC'    
        THEN CASE     
          WHEN @SortExpression = 'EmployeeName'    
           THEN E.LastName    
          END    
       END ASC    
      ,CASE     
       WHEN @SortType = 'DESC'    
        THEN CASE     
          WHEN @SortExpression = 'EmployeeName'    
           THEN E.LastName    
          END    
       END DESC    
      ,CASE     
       WHEN @SortType = 'ASC'    
        THEN CASE     
          WHEN @SortExpression = 'Placement'    
           THEN r.PlacementRequirement    
          END    
       END ASC    
      ,CASE     
       WHEN @SortType = 'DESC'    
        THEN CASE     
          WHEN @SortExpression = 'Placement'    
           THEN r.PlacementRequirement    
          END    
       END DESC    
      ,CASE     
       WHEN @SortType = 'ASC'    
        THEN CASE     
          WHEN @SortExpression = 'Comment'    
           THEN sm.Comments    
          END    
       END ASC    
      ,CASE     
       WHEN @SortType = 'DESC'    
        THEN CASE     
          WHEN @SortExpression = 'Comment'    
           THEN sm.Comments    
          END    
       END DESC    
      ,CASE     
       WHEN @SortType = 'ASC'    
        THEN CASE     
          WHEN @SortExpression = 'Age'    
           THEN R.Dob    
          END    
       END ASC    
      ,CASE     
       WHEN @SortType = 'DESC'    
        THEN CASE     
          WHEN @SortExpression = 'Age'    
           THEN R.Dob    
          END    
       END DESC    
      ,CASE     
       WHEN @SortType = 'ASC'    
        THEN CASE     
          WHEN @SortExpression = 'Address'    
           THEN c.Address    
          END    
       END ASC    
      ,CASE     
       WHEN @SortType = 'DESC'    
        THEN CASE     
          WHEN @SortExpression = 'Address'    
           THEN c.Address    
          END    
       END DESC    
     ) AS Row    
    ,sm.ScheduleID    
    ,sm.ReferralID    
    ,sm.FacilityID    
    ,f.FacilityName    
    ,sm.EmployeeID    
    ,EmployeeName = dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat)    
    ,EmpEmail = E.Email    
    ,EmpMobile = E.MobileNumber    
    ,PatAddress = c.Address + ', ' + c.City + ', ' + c.STATE + ' - ' + c.ZipCode    
    ,EmpAddress = E.Address + ', ' + E.City + ', ' + E.StateCode + ' - ' + E.ZipCode    
    ,sm.StartDate    
    ,sm.EndDate    
    ,sm.ScheduleStatusID    
    ,ss.ScheduleStatusName    
    ,r.PlacementRequirement    
    ,sm.Comments    
    ,sm.PickUpLocation    
    ,sm.DropOffLocation    
    ,dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) AS PatientName    
    ,dbo.GetGenericNameFormat(c.FirstName,'', c.LastName,@NameFormat) AS ParentName    
    ,c.Phone1    
    ,c.Phone2    
    ,c.Email    
    ,c.Address    
    ,c.City    
    ,c.STATE    
    ,c.ZipCode    
    ,r.RegionID    
    ,r.PermissionForEmail    
    ,r.PermissionForSMS    
    ,r.PermissionForVoiceMail    
    ,sm.WhoCancelled    
    ,sm.WhenCancelled    
    ,sm.CancelReason    
    ,r.BehavioralIssue    
    ,sm.UpdatedDate    
    ,sm.IsReschedule    
    ,R.PermissionForMail    
    ,dbo.GetAge(R.Dob) Age    
    ,SM.EmailSent    
    ,SM.SMSSent    
    ,SM.NoticeSent    
    ,r.PCMVoiceMail    
    ,r.PCMMail    
    ,R.PCMSMS    
    ,r.PCMEmail    
    ,p.PayorID    
    ,p.PayorName    
    ,d.DDMasterID CareTypeID    
    ,d.Title CareType    
    ,sm.ReferralBillingAuthorizationID    
    ,rba.AuthorizationCode    
   FROM ScheduleMasters sm    
   INNER JOIN ScheduleStatuses ss ON ss.ScheduleStatusID = sm.ScheduleStatusID    
   INNER JOIN Referrals r ON r.ReferralID = sm.ReferralID    
   INNER JOIN ContactMappings CM ON CM.ReferralID = sm.ReferralID    
    AND CM.ContactTypeID = 1    
   INNER JOIN Contacts c ON c.ContactID = CM.ContactID    
   LEFT JOIN Facilities f ON f.FacilityID = sm.FacilityID    
   INNER JOIN Employees E ON E.EmployeeID = sm.EmployeeID    
   LEFT JOIN Payors p on p.PayorID=sm.PayorID                     
   LEFT JOIN DDMaster d on d.DDMasterID=sm.CareTypeId         
   LEFT JOIN ReferralBillingAuthorizations rba ON rba.ReferralBillingAuthorizationID = sm.ReferralBillingAuthorizationID AND rba.ReferralID = sm.ReferralID     
   LEFT JOIN EmployeeVisits EV ON EV.ScheduleID = sm.ScheduleID AND EV.IsDeleted = 0    
   WHERE (sm.IsDeleted = 0)    
       AND EV.EmployeeVisitID IS NULL    
    AND (    
     LEN(@EmployeeIDs) = 0    
     OR SM.EmployeeID IN (    
      SELECT EL.[val]    
      FROM dbo.GetCSVTable(@EmployeeIDs) EL    
      )    
     )    
    AND (    
     @ReferralIDs IS NULL    
     OR SM.ReferralID IN (    
      SELECT RL.[val]    
      FROM dbo.GetCSVTable(@ReferralIDs) RL    
      )    
     )    
    AND (    
     @PayorIDs IS NULL    
     OR SM.PayorID IN (    
      SELECT PL.[val]    
      FROM dbo.GetCSVTable(@PayorIDs) PL    
      )    
     )    
    AND (    
     @CareTypeIDs IS NULL    
     OR SM.CareTypeID IN (    
      SELECT CL.[val]    
      FROM dbo.GetCSVTable(@CareTypeIDs) CL    
      )    
     )    
    AND (    
     @GroupIds IS NULL    
     OR sm.ReferralID IN (SELECT DISTINCT ReferralID FROM ReferralGroup WHERE Val IN (SELECT GL.[val]    
      FROM dbo.GetCSVTable(@GroupIds) GL))    
     )    
    AND (    
     (    
      @StartDate IS NULL    
      OR (CONVERT(DATE, SM.StartDate) >= @StartDate)    
      )    
     AND (    
      @EndDate IS NULL    
      OR (CONVERT(DATE, SM.EndDate) <= @EndDate)    
      )    
     )    
   ) AS t1    
  )    
 SELECT *    
 FROM CTEScheduleMasterList    
 WHERE ROW BETWEEN ((@PageSize * (@FromIndex - 1)) + 1)    
   AND (@PageSize * @FromIndex)  
  
End