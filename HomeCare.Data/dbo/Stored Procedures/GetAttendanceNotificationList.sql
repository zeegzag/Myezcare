-- EXEC GetAttendanceNotificationList @fromdate='2017-02-01',@todate='2017-02-28'  
-- EXEC GetAttendanceNotificationList @FromDate = '2017/05/01', @ToDate = '2017/05/31', @ReferralStatusIDs = '1', @PayorIDs = '1,2,4', @ScheduleStatusID = '2'  
CREATE PROCEDURE [dbo].[GetAttendanceNotificationList]   
@FromDate date= null,   
@ToDate date= null,  
@ReferralStatusIDs varchar(max)='1',  
@PayorIDs varchar(150)='1,2,4',  
@ScheduleStatusID bigint=2,  
@ReferralID bigint =null,  
@FacilityID bigint=0,   
@CreatedBy bigint=0,   
@ReferralMonthlySummariesIDs varchar(max)=null,  
@IsFromMonthlyService bit = 1  
AS            
BEGIN   
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
   
        --Which Client Attend Schedule this is getting here       
        SELECT ReferralID,FirstName,LastName, dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat) AS ClientName,AHCCCSID,Dob, RecordRequestEmail, CaseManager,  
  ReferralMonthlySummariesID,Medication,Breakfast,BreakfastTxt,Lunch,LunchTxt,Dinner,DinnerTxt,MoodforThroughoutWeekend,  
  MoodforThroughoutWeekendTxt,OutingDetails,PCIInformation,TreatmentPlan,MedicationsDispensed,MedicationsDispensedTxt,Nextvisit,  
  CurrentServicePlan,BelongingsandMedicationsReturned,CoordinationofCareatPickup,CoordinationofCareatPickupTxt,CoordinationofCareatDropOff,  
  CoordinationofCareatDropOffTxt,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate, CompletedBy,Signature,CoordinationofCareatPickupOption,  
  CoordinationofCareatDropOffOption,MonthlySummaryStartDate,MonthlySummaryEndDate   
  FROM  
    (  
         select  R.ReferralID, R.FirstName,R.LastName,R.MiddleName, R.AHCCCSID,R.CISNumber,  
   ISNULL(R.MonthlySummaryEmail,CM.Email)as RecordRequestEmail,R.Dob,dbo.GetGenericNameFormat(CM.FirstName,'', CM.LastName,@NameFormat)  as CaseManager,   
   RMS.ReferralMonthlySummariesID,RMS.Breakfast,RMS.BreakfastTxt,RMS.Lunch,RMS.LunchTxt,RMS.Dinner,RMS.DinnerTxt,RMS.MoodforThroughoutWeekend,  
   RMS.MoodforThroughoutWeekendTxt,RMS.OutingDetails,RMS.PCIInformation,RMS.TreatmentPlan,RMS.MedicationsDispensedTxt,RMS.Nextvisit,  
   RMS.CoordinationofCareatPickup,RMS.CoordinationofCareatPickupTxt,RMS.CoordinationofCareatDropOff,RMS.CoordinationofCareatPickupOption,  
   RMS.CoordinationofCareatDropOffOption,  
   CONVERT(VARCHAR(10), RMS.MonthlySummaryStartDate, 101) as  MonthlySummaryStartDate,   
   CONVERT(VARCHAR(10), RMS.MonthlySummaryEndDate, 101) as  MonthlySummaryEndDate,   
   --CONVERT(VARCHAR(10),CONVERT(datetime,RMS.MonthlySummaryStartDate ,1),111) as  MonthlySummaryStartDate,   
   --CONVERT(VARCHAR(10),CONVERT(datetime,RMS.MonthlySummaryEndDate ,1),111) as MonthlySummaryEndDate,  
      
   RMS.CoordinationofCareatDropOffTxt,RMS.CreatedBy,RMS.UpdatedBy,RMS.CreatedDate,RMS.UpdatedDate,  
   case  when RMS.Medication=1 then 'Yes' ELSE 'No' END as Medication,  
   case  when RMS.MedicationsDispensed=1 then 'Yes' ELSE 'No' END as MedicationsDispensed,  
   case  when RMS.CurrentServicePlan=1 then 'Yes' ELSE 'No' END as CurrentServicePlan,  
   case  when RMS.BelongingsandMedicationsReturned=1 then 'Yes' ELSE 'No' END as BelongingsandMedicationsReturned,  
   EMP.FirstName +' '+ EMP.LastName as CompletedBy, ES.SignaturePath as Signature  
  
   --DENSE_RANK() Over(Partition by R.Referralid order by RMS.MonthlySummaryDate DESC) DenseRank,COUNT(R.ReferralID) as Counts  
   FROM Referrals R   
   Inner Join ScheduleMasters SM on R.ReferralID=SM.ReferralID  AND SM.IsDeleted =0    
   Inner Join CaseManagers CM on CM.CaseManagerID=R.CaseManagerID  
   Inner Join ReferralPayorMappings RPM on RPM.ReferralID = R.ReferralID and RPM.IsActive=1 and RPM.IsDeleted=0    
   Inner Join ReferralMonthlySummaries RMS on R.ReferralID = RMS.ReferralID  
   Inner Join Employees EMP on  EMP.EmployeeID=RMS.UpdatedBy  
   LEFT JOIN EmployeeSignatures ES on ES.EmployeeSignatureID=EMP.EmployeeSignatureID  
         WHERE  R.IsDeleted=0 AND  
 --R.ReferralID=2892 AND  
  (  
   (@IsFromMonthlyService=1 ) --If From MonthlyService then following conditions need to true  
   and(     
    (SM.IsDeleted IS NOT NULL AND SM.IsDeleted = 0) AND RMS.IsDeleted=0  
    AND ((@FromDate is null OR SM.StartDate >= @FromDate) AND (@ToDate is null OR SM.StartDate<= @ToDate))  
    AND ((@FromDate is null OR RMS.MonthlySummaryStartDate >= @FromDate) AND (@ToDate is null OR RMS.MonthlySummaryStartDate<= @ToDate))  
    AND (SM.ScheduleStatusID = @ScheduleStatusID)  
    AND (R.ReferralStatusID in (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ReferralStatusIDs)))  
    AND (  (@PayorIDs is null) or(len(@PayorIDs)=0 ) or  
     (RPM.PayorID  in (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@PayorIDs )))  
     )      
   )  
  )  
  OR(  
   (@IsFromMonthlyService=0 ) --If From Print Report or Group Sanpshot then following conditions need to be true   
   and(   
     (  
      (@ReferralID is null) or (@ReferralID = 0) or  
      (R.ReferralID = @ReferralID)    
     )   
     AND RMS.IsDeleted=0  
     and(  
      (@ReferralMonthlySummariesIDs is null) or (len(@ReferralMonthlySummariesIDs) = 0) or  
      (RMS.ReferralMonthlySummariesID in (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ReferralMonthlySummariesIDs)))  
     )  
     AND ((@FromDate is null OR RMS.MonthlySummaryStartDate >= @FromDate) AND (@ToDate is null OR RMS.MonthlySummaryStartDate<= @ToDate))  
     AND (@FacilityID=0 OR RMS.FacilityID=@FacilityID)  
     AND (@CreatedBy=0 OR RMS.CreatedBy=@CreatedBy)  
    )  
  )  
  
  
 GROUP BY R.ReferralID,R.FirstName,R.LastName,R.MiddleName, R.AHCCCSID,R.CISNumber,R.MonthlySummaryEmail,CM.Email,R.Dob,CM.FirstName ,CM.LastName,  
      RMS.ReferralMonthlySummariesID,RMS.Medication,RMS.Breakfast,RMS.BreakfastTxt,RMS.Lunch,RMS.LunchTxt,RMS.Dinner,RMS.DinnerTxt,RMS.MoodforThroughoutWeekend,  
    RMS.MoodforThroughoutWeekendTxt,RMS.OutingDetails,RMS.PCIInformation,RMS.TreatmentPlan,RMS.MedicationsDispensed,RMS.MedicationsDispensedTxt,RMS.Nextvisit,  
    RMS.CurrentServicePlan,RMS.BelongingsandMedicationsReturned,RMS.CoordinationofCareatPickup,RMS.CoordinationofCareatPickupTxt,RMS.CoordinationofCareatDropOff,  
    RMS.CoordinationofCareatDropOffTxt,RMS.CreatedBy,RMS.UpdatedBy,RMS.CreatedDate,RMS.UpdatedDate,EMP.FirstName,EMP.LastName,ES.SignaturePath,  
    RMS.CoordinationofCareatDropOff,RMS.CoordinationofCareatPickupOption,  
    RMS.CoordinationofCareatDropOffOption,RMS.MonthlySummaryStartDate,RMS.MonthlySummaryEndDate  
)  AS t  ORDER BY LastName --where t.DenseRank = 1  
  
  
  
  -- Which Client Not Attend Schedule this is getting here   
     -- SELECT R.ReferralID, R.FirstName,R.LastName,R.AHCCCSID,R.CISNumber,ISNULL(R.MonthlySummaryEmail,CM.Email) AS RecordRequestEmail, R.Dob, CM.FirstName + ' ' + CM.LastName as CaseManager    
  
  SELECT * FROM (  
     SELECT  R.ReferralID, R.FirstName,R.LastName,dbo.GetGenericNameFormat(R.FirstName,R.MiddleName, R.LastName,@NameFormat) AS ClientName, R.AHCCCSID,R.CISNumber,ISNULL(R.MonthlySummaryEmail,CM.Email) AS RecordRequestEmail, R.Dob, dbo.GetGenericNameFormat(CM.FirstName,'', CM.LastName,@NameFormat) as CaseManager    
     FROM Referrals R  
  LEFT JOIN ScheduleMasters SM on R.ReferralID=SM.ReferralID   
  INNER JOIN CaseManagers CM on CM.CaseManagerID=R.CaseManagerID  
  INNER JOIN ReferralPayorMappings RPM on RPM.ReferralID = R.ReferralID and RPM.IsActive=1 and RPM.IsDeleted=0    
  WHERE R.IsDeleted=0 AND  
    --R.ReferralID=1992 AND    
   R.ReferralStatusID in (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ReferralStatusIDs)) AND    
   RPM.PayorID  in (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@PayorIDs ))  
         GROUP BY R.ReferralID,R.FirstName,R.LastName,R.MiddleName, R.AHCCCSID,R.CISNumber,R.MonthlySummaryEmail,CM.Email,R.Dob,CM.FirstName ,CM.LastName  
     ) AS T   
    
  WHERE ReferralID NOT IN (  
    SELECT DISTINCT R.ReferralID  
    FROM Referrals R  
    INNER JOIN ScheduleMasters SM on R.ReferralID=SM.ReferralID   
    INNER JOIN ReferralPayorMappings RPM on RPM.ReferralID = R.ReferralID and RPM.IsActive=1 and RPM.IsDeleted=0    
    WHERE R.IsDeleted=0 AND  
     --R.ReferralID=1992 AND    
     ((SM.StartDate >= @FromDate AND SM.StartDate <= @ToDate ) AND   
     ((SM.ScheduleStatusID IS NULL OR SM.ScheduleStatusID IN (@ScheduleStatusID) ) AND (SM.IsDeleted IS NULL OR  SM.IsDeleted = 0) ) )   AND   
     R.ReferralStatusID in (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ReferralStatusIDs)) AND    
     RPM.PayorID  in (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@PayorIDs ))  
         )  
     ORDER BY LastName  
  
  
  
  --SELECT R.ReferralID, R.FirstName,R.LastName,R.AHCCCSID,R.CISNumber,'jyadav@kairasoftware.com' AS RecordRequestEmail, R.Dob, CM.FirstName + ' ' + CM.LastName as CaseManager    
  --   FROM Referrals R  
  --LEFT JOIN ScheduleMasters SM on R.ReferralID=SM.ReferralID   
  --INNER JOIN CaseManagers CM on CM.CaseManagerID=R.CaseManagerID  
  --INNER JOIN ReferralPayorMappings RPM on RPM.ReferralID = R.ReferralID and RPM.IsActive=1 and RPM.IsDeleted=0    
  --WHERE  
  --  --R.ReferralID=2892    
  -- ( (SM.StartDate IS NULL) OR  (SM.StartDate < @FromDate OR SM.StartDate> @ToDate ) OR   
  -- ( (SM.StartDate >= @FromDate OR SM.StartDate <= @ToDate ) AND  (SM.ScheduleStatusID IS NULL OR SM.ScheduleStatusID NOT IN (@ScheduleStatusID) AND (SM.IsDeleted IS NULL OR SM.IsDeleted = 0) ) )  ) AND   
  -- R.ReferralStatusID in (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ReferralStatusIDs)) AND    
  -- RPM.PayorID  in (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@PayorIDs ))  
  --       GROUP BY R.ReferralID,R.FirstName,R.LastName,R.AHCCCSID,R.CISNumber,R.MonthlySummaryEmail,CM.Email,R.Dob,CM.FirstName ,CM.LastName  
  
 END   