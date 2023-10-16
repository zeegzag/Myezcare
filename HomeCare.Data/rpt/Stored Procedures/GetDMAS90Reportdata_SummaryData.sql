/*
 =============================================
 Author:		Kalpesh Patel
 Create date: 02/07/2020
 Description:	DMAS90
 EXEc [rpt].[GetDMAS90Reportdata_SummaryData] @ReferralID=30043,@CareType='41',
				@EmployeeID= 0,@date= '05/25/2020 10:00:00 AM',
				@ScheduleID ='83521, 83523'
 =============================================
 */
CREATE PROCEDURE [rpt].[GetDMAS90Reportdata_SummaryData]
	-- Add the parameters for the stored procedure here
	@ReferralID int = '30043',
	@CareType int = '41', 
	@EmployeeID INT = 0,
	@date DATE = '05/25/2020 10:00:00 AM',
	@ScheduleID AS VARCHAR(MAX)='83522,83521' 	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	; WITH CTE AS (
	SELECT 
		ROW_NUMBER() Over (Partition by vt.visittaskdetail Order by emv.CreatedDate DESC) AS RowNum,
		vt.visittaskdetail, vt.visittasktype, evn.alertcomment, 
		CASE WHEN emv.surveycompleted > 0 THEN emv.surveycomment ELSE '' END AS Notes,
		--CASE WHEN emv.surveycompleted = 1 THEN 'Yes' ELSE 'NO' END AS Survey, 
		evn.Description as Survey,
		emv.CreatedDate AS SurveyDate  
		FROM schedulemasters AS sm
		INNER JOin referraltaskmappings AS rtm ON rtm.referralid = sm.referralid
		INNER JOIN visittasks vt ON vt.visittaskid = rtm.visittaskid and vt.visittasktype = 'conclusion' 
		LEFT JOIN employeevisits AS emv ON sm.scheduleid = emv.scheduleid
		LEFT JOIN employeevisitnotes AS evn 
			ON evn.employeevisitid = emv.employeevisitid
			AND evn.ReferralTaskMappingID=rtm.ReferralTaskMappingID
			--AND evn.alertcomment IS NOT NULL
	WHERE 1=1
	AND rtm.Referralid=@ReferralID
	AND (@CareType = 0 or sm.CareTypeId=@CareType)
	AND emv.ispcacompleted = 1 
	AND emv.isdeleted = 0
	--and cast(sm.StartDate as date) between DATEADD(dd, 0 - (@@DATEFIRST + 5 + DATEPART(dw, '05/25/2020')) % 7, '05/25/2020') AND 
	--	DATEADD(dd, 6 - (@@DATEFIRST + 5 + DATEPART(dw, '05/25/2020')) % 7, '05/25/2020')
	AND EXISTS (select 1 from dbo.SplitString(@ScheduleID, ',') WHERE sm.ScheduleID=TRY_CAST(Item AS INT))
	)
	SELECT 
		visittaskdetail,visittasktype,alertcomment,Notes,Survey,SurveyDate
	FROM CTE where rownum=1;

	/*--Old Code
	;WITH Note AS(
		SELECT TOP 1
		evn.alertcomment,
		CASE WHEN employeevisits.surveycompleted > 0 THEN employeevisits.surveycomment ELSE '' END AS Notes,
		CASE WHEN employeevisits.surveycompleted = 1 THEN 'Yes' ELSE 'NO' END AS Survey,
		sm.referralid,employeevisits.CreatedDate as SurveyDate
		FROM schedulemasters AS sm
		INNER JOIN employeevisits ON sm.scheduleid = employeevisits.scheduleid
		INNER JOIN employeevisitnotes AS evn ON evn.employeevisitid = employeevisits.employeevisitid
		WHERE 
		sm.referralid = @ReferralID 
		and (@EmployeeID = 0 or sm.EmployeeID=@EmployeeID)
		AND employeevisits.ispcacompleted = 1 
		AND employeevisits.isdeleted = 0
		and cast(sm.StartDate as date) between DATEADD(dd, 0 - (@@DATEFIRST + 5 + DATEPART(dw, @date)) % 7, @date) AND 
			DATEADD(dd, 6 - (@@DATEFIRST + 5 + DATEPART(dw, @date)) % 7, @date)
		AND EXISTS (select 1 from dbo.SplitString(@ScheduleID, ',') WHERE sm.ScheduleID=TRY_CAST(Item AS INT))
		GROUP BY evn.alertcomment,employeevisits.surveycompleted,employeevisits.surveycomment,
			sm.startdate,sm.referralid,employeevisits.CreatedDate
		ORDER BY sm.startdate DESC
	) 
	SELECT 
	  vt.visittaskdetail, vt.visittasktype, nt.alertcomment, nt.Notes, nt.Survey, nt.SurveyDate  
	 FROM Note nt
	 INNER JOIN referraltaskmappings AS rtm ON rtm.referralid = nt.referralid
	 INNER JOIN visittasks vt ON vt.visittaskid = rtm.visittaskid
	 INNER JOIN ScheduleMasters AS sm ON  sm.referralID = nt.referralid
	 WHERE vt.visittasktype = 'conclusion' 
	 AND (@CareType = 0 or sm.CareTypeId=@CareType)
	 GROUP BY vt.visittaskdetail, vt.visittasktype,nt.alertcomment,
	 nt.Notes,nt.Survey,nt.SurveyDate
	 */
END