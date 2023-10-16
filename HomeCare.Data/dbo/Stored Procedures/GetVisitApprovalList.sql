CREATE PROCEDURE [dbo].[GetVisitApprovalList]  
  @EmployeeVisitIDs NVARCHAR(MAX),  
  @LoggedInUserID bigint  
AS  
BEGIN  
  DECLARE @HasAllApprovalPermission BIT
  SELECT @HasAllApprovalPermission = [dbo].[IsEmployeeHasPermission](@LoggedInUserID, 'Web_Can_Approve_Visit_All');

  DECLARE @HasGroupApprovalPermission BIT
  SELECT @HasGroupApprovalPermission = [dbo].[IsEmployeeHasPermission](@LoggedInUserID, 'Web_Can_Approve_Visit_Group');

  DECLARE @LoginUserGroupIDs NVARCHAR(MAX)
  IF (@HasGroupApprovalPermission = 1)
  BEGIN
	  SET @LoginUserGroupIDs =              
	  (              
		SELECT              
		  e.GroupIDs              
		FROM dbo.Employees e              
		WHERE              
		  e.EmployeeID = @LoggedInUserID              
	  )
  END

  SELECT
    EV.EmployeeVisitID,
	E.EmployeeID,
	[dbo].[GetGeneralNameFormat](E.FirstName,E.LastName) [Name],
	R.ReferralID,  
	[dbo].[GetGeneralNameFormat](R.FirstName,R.LastName) [PatientName],
	SM.ScheduleID, SM.StartDate, SM.EndDate,
	EV.ClockInTime, EV.ClockOutTime,
	AA.*,

	P.PayorID,P.PayorName,
	D.DDMasterID as CareTypeID,D.Title As CareType,
	SM.ReferralBillingAuthorizationID, RBA.AuthorizationCode

  FROM [dbo].[EmployeeVisits] EV
  INNER JOIN GetCSVTable(@EmployeeVisitIDs) FEV ON CONVERT(BIGINT, FEV.VAL) = EV.EmployeeVisitID
  INNER JOIN [dbo].[ScheduleMasters] SM ON EV.ScheduleID = SM.ScheduleID
  INNER JOIN [dbo].[Employees] E ON E.EmployeeID = SM.EmployeeID
  INNER JOIN [dbo].[Referrals] R ON R.ReferralID = SM.ReferralID
  CROSS APPLY (
	SELECT CASE WHEN (@HasAllApprovalPermission = 1 OR 
						(@LoginUserGroupIDs IS NOT NULL AND EXISTS (              
							SELECT 1              
							FROM GetCSVTable(R.GroupIDs) EG              
							INNER JOIN GetCSVTable(@LoginUserGroupIDs) LG ON EG.val = LG.val)
						)
		   )
		   THEN 1 
		   ELSE 0 END CanApprove
  ) AA
  LEFT JOIN Payors P on P.PayorID = SM.PayorID  
  LEFT JOIN DDMaster D on D.DDMasterID = SM.CareTypeId  
  LEFT JOIN ReferralBillingAuthorizations RBA ON RBA.IsDeleted = 0 
	AND RBA.ReferralBillingAuthorizationID = SM.ReferralBillingAuthorizationID AND RBA.ReferralID = SM.ReferralID  
  WHERE
	EV.IsApproved = 0
  ORDER BY FEV.id
END