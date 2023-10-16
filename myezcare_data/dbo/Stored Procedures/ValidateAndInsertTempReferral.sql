CREATE PROCEDURE [dbo].[ValidateAndInsertTempReferral]      
 @UserID bigint,      
 @ScheduleDate DATE,      
 @FacilityID BIGINT,      
 @SystemID VARCHAR(50)      
AS      
BEGIN      
 
 
 DECLARE @InfinateDate DATE='2099-12-31'; 

 update TempReferral set ErrorMessage='' WHERE CreatedBy=@UserID;      
      
 Update TempReferral set ErrorMessage='Id is required.' WHERE (Id  IS NULL or LEN(Id)=0) and CreatedBy=@UserID;      

      
 UPDATE T SET T.ErrorMessage='Id not matched.' FROM TempReferral T      
 LEFT JOIN Referrals R ON R.ReferralID=T.Id      
 WHERE T.CreatedBy=@UserID AND R.ReferralID IS NULL;      

 
 UPDATE T SET T.ErrorMessage= 
 CASE WHEN R.DefaultFacilityID IS NULL AND RPM.PayorID IS NULL THEN 'Default Facility & Primary Payor are not set.'
      WHEN R.DefaultFacilityID  IS NULL THEN 'Default Facility is not set.'
      WHEN RPM.PayorID IS NULL THEN 'Primary Payor is not set.'
	  ELSE '' END
 FROM TempReferral T      
 INNER JOIN Referrals R ON R.ReferralID=T.Id    
 LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID=R.ReferralID AND RPM.Precedence=1 AND RPM.IsDeleted=0 AND   IsActive=1
 AND  @ScheduleDate BETWEEN PayorEffectiveDate AND ISNULL(PayorEffectiveEndDate,@InfinateDate)
       


 



      
 UPDATE TempReferral SET ErrorMessage='Done' where ErrorMessage='' and CreatedBy=@UserID      
      
    IF NOT EXISTS (SELECT 1 FROM TempReferral WHERE ErrorMessage<>'Done' and CreatedBy=@UserID)      
    BEGIN      
      
      
  BEGIN TRANSACTION trans                                                                            
  BEGIN TRY           
   INSERT INTO ScheduleMasters(ReferralID,FacilityID,StartDate,EndDate,ScheduleStatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,ReferralTSDateID,PayorID,IsDeleted,IsPatientAttendedSchedule)      
   --SELECT T.Id,@FacilityID,RTD.ReferralTSStartTime,ReferralTSEndTime,2,@UserID,GETDATE(),@UserID,GETDATE(),RTD.ReferralTSDateID,RPM.PayorID,0      
   SELECT T.Id,R.DefaultFacilityID,RTD.ReferralTSStartTime,ReferralTSEndTime,2,@UserID,GETDATE(),@UserID,GETDATE(),RTD.ReferralTSDateID,RPM.PayorID,0,T.IsShow      
   FROM TempReferral T      
   INNER JOIN Referrals R ON R.ReferralID=T.Id      
   LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID=R.ReferralID AND RPM.Precedence=1 AND RPM.IsDeleted=0 AND   IsActive=1
   AND  @ScheduleDate BETWEEN PayorEffectiveDate AND ISNULL(PayorEffectiveEndDate,@InfinateDate)
   LEFT JOIN ReferralTimeSlotDates RTD ON RTD.ReferralID=T.Id AND RTD.ReferralTSDate=@ScheduleDate    
   LEFT JOIN ScheduleMasters SM ON SM.ReferralTSDateID=RTD.ReferralTSDateID AND SM.IsDeleted=0      
   WHERE SM.ReferralTSDateID IS NULL AND RTD.UsedInScheduling=1    
      
         
   UPDATE SM SET SM.FacilityID=R.DefaultFacilityID,SM.StartDate=RTD.ReferralTSStartTime,SM.EndDate=RTD.ReferralTSEndTime,      
   UpdatedBy=@UserID,UpdatedDate=GETDATE(),ReferralTSDateID=RTD.ReferralTSDateID,PayorID=RPM.PayorID,      
   SM.IsPatientAttendedSchedule=CASE WHEN (T.IsShow=1) THEN 1 ELSE 0 END  
   FROM TempReferral T      
   INNER JOIN Referrals R ON R.ReferralID=T.Id      
   LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID=R.ReferralID AND RPM.Precedence=1 AND RPM.IsDeleted=0 AND   IsActive=1
   AND  @ScheduleDate BETWEEN PayorEffectiveDate AND ISNULL(PayorEffectiveEndDate,@InfinateDate)
   LEFT JOIN ReferralTimeSlotDates RTD ON RTD.ReferralID=T.Id AND RTD.ReferralTSDate=@ScheduleDate AND RTD.UsedInScheduling=1    
   LEFT JOIN ScheduleMasters SM ON SM.ReferralTSDateID=RTD.ReferralTSDateID AND SM.IsDeleted=0      
   WHERE SM.ReferralTSDateID IS NOT NULL AND RTD.UsedInScheduling=1  
  
   --DELETE Note  
   UPDATE N SET N.IsDeleted=1 FROM Notes N  
   INNER JOIN ScheduleMasters SM ON SM.ScheduleID=N.ScheduleID  
   WHERE SM.IsPatientAttendedSchedule=0  
      
   DECLARE  @ReferralIDs NVARCHAR(MAX);      
   SELECT @ReferralIDs=STUFF((SELECT ', ' + convert(varchar(MAX), T.Id, 120)        
            FROM TempReferral T      
      INNER JOIN Referrals R ON R.ReferralID=T.Id      
      WHERE T.IsShow=1 FOR XML PATH (''))  , 1, 1, '')      
      
            EXEC HC_DayCare_CreateBillingNotes @ReferralIDs=@ReferralIDs,@ScheduleDate = @ScheduleDate ,@LoggedInID=@UserID,@SystemID =@SystemID      
      
      
      
     --Remove Temp Data--      
   delete from TempReferral where CreatedBy=@UserID      
   --Remove Temp Data--      
   SELECT 1;      
      
   IF @@TRANCOUNT > 0                                                                            
    BEGIN      
     COMMIT TRANSACTION trans      
    END      
  END TRY      
  BEGIN CATCH      
   SELECT 0;      
   IF @@TRANCOUNT > 0      
    BEGIN      
     ROLLBACK TRANSACTION trans      
    END      
  END CATCH      
    END      
    ELSE      
    BEGIN      
    SELECT 0      
    END  


END
