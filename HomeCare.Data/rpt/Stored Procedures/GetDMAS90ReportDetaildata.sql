  
/*    
 =============================================    
 Author:  Kalpesh Patel    
 Create date: 02/07/2020    
 Description: DMAS90    
 EXEc [rpt].[GetDMAS90ReportDetaildata] @ReferralID=30043,@CareType='41',    
    @EmployeeID= 0,@date= '05/25/2020 10:00:00 AM',    
    @ScheduleID ='83521, 83523'    
 =============================================    
 */    
CREATE PROCEDURE [rpt].[GetDMAS90ReportDetaildata]    
 -- Add the parameters for the stored procedure here    
 --DECLARE    
 @ReferralID int = '30043',    
 @CareType int = '41',     
 @EmployeeID INT = 0,    
 @date datetime = '01/31/2021 10:00:00 AM',    
 @ScheduleID AS VARCHAR(MAX)='83522,83521'    
-- SELECT @ReferralID=59,@CareType=139,@EmployeeID=0,@date='2021-01-24 00:00:00',@ScheduleID=N'27795, 27796, 27797, 27798, 27799'    
 AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
--SET DATEFIRST 7 ;    
--IF OBJECT_ID('tempdb..#VisitDetail') IS NOT NULL DROP TABLE #VisitDetail    
--IF OBJECT_ID('tempdb..#PrePivot') IS NOT NULL DROP TABLE #PrePivot    
    
 -- declare @ReferralID int = '30043', @CareType int = '41',     
 --@dt datetime = '05/25/2020 12:00:00 AM',     
 ----@EndDate datetime = '05/30/2020 12:00:00 AM' ,    
 --@EmployeeID AS INT,@Dates datetime='05/30/2020 12:00:00 AM',    
 --@ScheduleID AS VARCHAR(MAX)='83522,83521'    
  Declare @VisitDetail table    
  (ScheduleID bigint,     
 EmployeeVisitID bigint,     
 VisitTaskDetail nvarchar(max),    
 ClosInTime VARCHAR(MAX),    
 ClockOutTime VARCHAR(MAX),    
 NumberOfHours VARCHAR(MAX),    
 VisitTimeInMinutes int,  
 ScheduleDate date,    
 IsDone VARCHAR(MAX) ,  
 RankOrder int  
)    
    
    
insert INTO @VisitDetail(ScheduleID ,  EmployeeVisitID ,  VisitTaskDetail , ClosInTime , ClockOutTime , NumberOfHours, VisitTimeInMinutes , ScheduleDate , IsDone,RankOrder )    
 SELECT DISTINCT       
  sm.ScheduleID,EmployeeVisits.EmployeeVisitID,    
  vt.VisitTaskDetail,    
  FORMAT(CONVERT(datetime,EmployeeVisits.ClockInTime),'hh:mm tt') AS ClosInTime,    
  FORMAT(CONVERT(datetime,EmployeeVisits.ClockOutTime),'hh:mm tt') AS ClockOutTime,    
  FORMAT(CONVERT(datetime,(EmployeeVisits.ClockOutTime-EmployeeVisits.ClockInTime)),'HH:mm') + ' Hrs' as NumberOfHours,   
  DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, CONVERT(TIME, ClockOutTime -ClockInTime)), 0), CONVERT(TIME, ClockOutTime -ClockInTime)) VisitTimeInMinutes,  
  TRY_CAST(sm.StartDate AS DATE)as ScheduleDate,    
  CASE WHEN (evn.ServiceTime > 0 AND evn.servicetime IS NOT null) THEN 'YES' ELSE Null END AS IsDone,   
   ROW_NUMBER() OVER(PARTITION BY EmployeeVisits.EmployeeVisitID ORDER BY EmployeeVisits.EmployeeVisitID  ) RankOrder  
 FROM            VisitTasks AS vt      
     inner JOIN ReferralTaskMappings AS rtm ON vt.VisitTaskID = rtm.VisitTaskID AND rtm.ReferralID=@ReferralID       
     LEFT outer JOIN VisitTaskCategories ON vt.VisitTaskCategoryID = VisitTaskCategories.VisitTaskCategoryID      
     Inner JOIN dbo.Referrals r ON rtm.ReferralID = r.ReferralID --AND r.ReferralID=@ReferralID      
     inner JOIN ScheduleMasters AS sm ON  sm.referralID = r.ReferralID      
     inner JOIN EmployeeVisits ON  sm.ScheduleID = EmployeeVisits.ScheduleID      
     left  JOIN EmployeeVisitNotes AS evn ON evn.employeeVisitID=EmployeeVisits.EmployeeVisitID AND evn.ReferralTaskMappingID = rtm.ReferralTaskMappingID      
     LEFT OUTER JOIN  employees emp ON emp.EmployeeID = sm.EmployeeID      
     inner join DDMaster dm on dm.DDMasterID=vt.CareType      
     Inner join ContactMappings cm on cm.ReferralID=sm.ReferralID      
     inner join Contacts c on c.ContactID=cm.ContactID      
  LEFT JOIN EmployeeSignatures es ON es.EmployeeSignatureID=emp.EmployeeSignatureID     
 WHERE              
 (r.ReferralID = @ReferralID)     
 and (@EmployeeID = 0 or sm.EmployeeID=@EmployeeID)     
 --and sm.EmployeeID=@EmployeeID      
 AND vt.VisitTaskType='Task'  AND dbo.EmployeeVisits.IsPCACompleted=1 AND EmployeeVisits.IsDeleted=0     
 AND (@CareType = 0 or vt.CareType=@CareType)      
 --AND (cast(sm.StartDate as date) >= @dt AND cast(sm.StartDate as date) <=DATEADD(DAY, 8 - DATEPART(WEEKDAY, @dt), CAST(@dt AS DATE)))     
 AND CAST(SM.STARTDATE AS DATE) BETWEEN CAST(@date AS DATE) AND CAST(DATEADD(DD,6,@DATE) AS DATE)    
 --and cast(sm.StartDate as date) between DATEADD(dd, 0 - (@@DATEFIRST + 6 + DATEPART(dw, @date)) % 7, @date) AND     
 --  DATEADD(dd, 6 - (@@DATEFIRST + 6 + DATEPART(dw, @date)) % 7, @date)    
 AND EXISTS (select 1 from dbo.SplitString(@ScheduleID, ',') WHERE sm.ScheduleID=TRY_CAST(Item AS INT))    
 --AND sm.ScheduleID IN(83521, 83523)    
    
 ;WITH cte_numbers(n, WEEKDATE)     
 AS (    
  SELECT     
   0, CAST(@date AS DATE)    
  UNION ALL    
  SELECT n + 1,DATEADD(DAY, 1,WEEKDATE)    
    
  FROM  cte_numbers WHERE n < 6    
 )    
     
 --Select * FROM  cte_numbers Ca     
 --LEFT JOIN #visitDetail V ON V.ScheduleDate=Ca.WEEKDATE    
 --Order by V.ScheduleDate,V.ClosInTime,V.VisitTaskDetail    
    
    
 SELECT distinct    
  ScheduleID,V.EmployeeVisitID,ISNULL(V.VisitTaskDetail,'0') AS VisitTaskDetail,    
  V.ClosInTime,V.ClockOutTime,V.NumberOfHours, CASE V.RankOrder WHEN 1 THEN V.VisitTimeInMinutes ELSE 0 END VisitTimeInMinutes,  
  FORMAT(CA.WEEKDATE,'MM/dd/yyyy') AS ScheduleDate,DATENAME(WEEKDAY,CA.WEEKDATE) AS ScheduleDay,V.IsDone    
 FROM  cte_numbers Ca     
 LEFT JOIN @VisitDetail V ON V.ScheduleDate=Ca.WEEKDATE    
 Order by ScheduleDate,ClosInTime,VisitTaskDetail    
  
  
  
END
