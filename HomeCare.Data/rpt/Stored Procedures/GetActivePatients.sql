CREATE PROCEDURE [rpt].[GetActivePatients]    
	@Name NVARCHAR(MAX) = NULL,
	@ReferralStatusIDs NVARCHAR(MAX) = NULL,
	@CareTypeIDs NVARCHAR(MAX) = NULL,
	@Zones NVARCHAR(MAX) = NULL
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    

SELECT
    R.ReferralID,
    R.FirstName,
    R.[LastName],
	P.PatientName,
	C.Email,
	ISNULL(C.Phone1,C.Phone2) As PhoneNumber,    
    LS.*,
	LA.*
FROM Referrals R
LEFT JOIN ContactMappings CMP with(nolock) ON CMP.ReferralID = R.ReferralID and CMP.ContactTypeID = 1                      
LEFT JOIN Contacts C with(nolock)  ON C.ContactID = CMP.ContactID  
CROSS APPLY (
    SELECT [dbo].[GetGeneralNameFormat](R.FirstName,R.LastName)  PatientName
) P
CROSS APPLY (
    SELECT TOP 1
        RTSM.StartDate,
        RTSM.EndDate,
        EDA.*,
		CT.*
    FROM ReferralTimeSlotMaster RTSM
	CROSS APPLY (
		SELECT 
			STRING_AGG(DD.CareTypeId, ',') CareTypeIDs,
			STRING_AGG(DD.CareType, ',') CareTypes
		FROM (
			SELECT DISTINCT
				RTS.CareTypeId CareTypeID,
				C.Title CareType
			FROM ReferralTimeSlotDetails RTS 
			INNER JOIN DDMaster C ON C.ItemType = 1 AND C.DDMasterID = RTS.CareTypeId
			WHERE 
				RTSM.ReferralTimeSlotMasterID = RTS.ReferralTimeSlotMasterID 
				AND RTS.IsDeleted = 0
		) DD
    ) CT
    CROSS APPLY (
        SELECT
            CASE WHEN RTSM.IsEndDateAvailable = 1 THEN RTSM.EndDate
            ELSE DATEADD(DAY, -1, DATEADD(YEAR, 1, RTSM.StartDate))
            END FinalEndDate
    ) ED
    CROSS APPLY (
        SELECT
            ED.FinalEndDate,
            CASE WHEN ED.FinalEndDate <= CONVERT(DATE, GETDATE()) THEN 1 ELSE 0  END IsActive
    ) EDA
    WHERE RTSM.ReferralID = R.ReferralID
        AND RTSM.IsDeleted = 0
        AND RTSM.StartDate <= CONVERT(DATE, GETDATE())
		AND (ISNULL(@CareTypeIDs, '') = '' OR EXISTS (
			SELECT 1 
			FROM dbo.SplitString(CT.CareTypeIDs, ',') CL
			INNER JOIN dbo.SplitString(@CareTypeIDs, ',') PL ON CL.Item = PL.Item
			)
		) 
    ORDER BY RTSM.StartDate DESC
) LS
OUTER APPLY (
	SELECT TOP 1 A.LastActiveDate LastVisitDate FROM dbo.ScheduleMasters SM 
	LEFT JOIN dbo.EmployeeVisits EV ON EV.IsDeleted = 0 AND EV.ScheduleID = SM.ScheduleID
	OUTER APPLY ( SELECT CONVERT(DATE, EV.ClockInTime) LastActiveDate ) A
	WHERE SM.IsDeleted = 0 AND SM.ReferralID = R.ReferralID
	ORDER BY EV.EmployeeVisitID DESC
) LA
 WHERE     
	R.IsDeleted = 0    
	AND (ISNULL(@ReferralStatusIDs, '') = '' OR R.ReferralStatusID IN (SELECT Item FROM dbo.SplitString(@ReferralStatusIDs, ',')))
	AND (ISNULL(@Zones, '') = '' OR c.City IN (SELECT Item FROM dbo.SplitString(@Zones, ',')))  
	AND (ISNULL(@Name, '') = '' OR P.PatientName LIKE '%' + @Name + '%')
    
END