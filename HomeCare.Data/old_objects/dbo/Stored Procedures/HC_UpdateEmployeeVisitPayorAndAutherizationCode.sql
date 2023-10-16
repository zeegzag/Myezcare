-- =============================================  
-- Author:  Kundan Kumar Rai  
-- Create date: 06-06-2020  
-- Description: Update employee visits payors and autherization code  
-- =============================================  
CREATE PROCEDURE [dbo].[HC_UpdateEmployeeVisitPayorAndAutherizationCode]  
 @EmployeeVisitID BIGINT,  
 @PayorID BIGINT = 0,  
 @ReferralBillingAuthorizationID BIGINT = 0,  
 @UpdatedBy BIGINT  
AS  
BEGIN  
 DECLARE @ScheduleID BIGINT;  
 SELECT @ScheduleID = ScheduleID FROM EmployeeVisits WHERE EmployeeVisitID = @EmployeeVisitID  
   
 IF(@ScheduleID IS NOT NULL)  
 BEGIN  
  UPDATE ScheduleMasters SET  
   PayorID = @PayorID, --CASE WHEN @PayorID = 0 THEN PayorID ELSE @PayorID END,  
   ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID, --CASE WHEN @ReferralBillingAuthorizationID = 0 THEN ReferralBillingAuthorizationID ELSE @ReferralBillingAuthorizationID END,  
   UpdatedDate = GETUTCDATE(),  
   UpdatedBy = @UpdatedBy  
  WHERE ScheduleID = @ScheduleID;  
  
  UPDATE Notes SET PayorID = @PayorID,  
       UpdatedDate = GETUTCDATE(),  
       UpdatedBy = @UpdatedBy  
  WHERE EmployeeVisitID = @EmployeeVisitID;  

  EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @ScheduleID,'222','25'
 END  
  
 SELECT rba.AuthorizationCode, p.PayorName FROM ReferralBillingAuthorizations rba INNER JOIN Payors p ON p.PayorID=rba.PayorID  
 WHERE rba.ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID  AND rba.IsDeleted=0 AND p.IsDeleted=0;  
  
END  