
-- =============================================  
-- Author:  <Sameer Malik>  
-- Create date: <2021-Jul-16>  
-- Description: <Universal API interface for Prior Authorization Data>  
-- =============================================  
-- Exec API_UniversalPriorAuthorization 44 ,15  
CREATE PROCEDURE [dbo].[API_UniversalPriorAuthorization]  
 -- Add the parameters for the stored procedure here  
 @ReferralID     bigint,  
 @BillingAuthorizationID  bigint  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 DECLARE @ReferralBillingAuthorizationID INT  
 DECLARE @BillingStartDate  DATETIME  
 DECLARE @BillingEndDate   DATETIME  
 DECLARE @AuthorizationCode  NVARCHAR(50)  
 DECLARE @UnitType    INT  
 DECLARE @PerUnitQuantity  DECIMAL  
 DECLARE @MaxUnit    INT  
 DECLARE @ServiceCode   NVARCHAR(50)  
 DECLARE @CareType    NVARCHAR(50)  
  
 DECLARE @Allocated    DECIMAL  
 DECLARE @Used     DECIMAL  
  
 SELECT   
             @ReferralBillingAuthorizationID=ReferralBillingAuthorizationID,  
    @BillingStartDate = StartDate,   
    @BillingEndDate = EndDate,   
    @AuthorizationCode = AuthorizationCode,   
    @UnitType = RBA.UnitType,   
    @PerUnitQuantity = IsNull(RBA.PerUnitQuantity, 0),   
    @MaxUnit = IsNull(RBA.MaxUnit, 0),  
    @ServiceCode = ServiceCode + ISNULL(':' + M.ModifierCode, ''),  
    @CareType = DDMaster.Title  
  
 FROM  ReferralBillingAuthorizations RBA (NOLOCK)   
    INNER JOIN ServiceCodes ON ServiceCodes.ServiceCodeID = RBA.ServiceCodeID  
	LEFT JOIN Modifiers M ON M.ModifierID = ServiceCodes.ModifierID
    INNER JOIN DDMaster on RBA.CareType = DDMaster.DDMasterID  
 Where  RBA.IsDeleted = 0  
    AND RBA.ReferralID = @ReferralID  
    AND ReferralBillingAuthorizationID = @BillingAuthorizationID  
  
   
  
 ; WITH CTE AS (  
  
  SELECT   
     SM.StartDate,  
     SM.EndDate,  
  
     EV.ClockInTime,  
     EV.ClockOutTime,  
       
     Allocated_Unit =  
       
      Case   
       When @UnitType = 2 Then 1   
       When @UnitType = 1 Then DATEDIFF(MINUTE, SM.StartDate, SM.EndDate) / @PerUnitQuantity   
      END,  
       
     Used_Unit =   
      CASE   
       When @UnitType = 2 Then 1   
       WHEN @UnitType = 1 AND EV.ClockOutTime IS NULL /* AND EV.IsPCACompleted = 0 */ THEN 0   
       ELSE DATEDIFF(MINUTE, EV.ClockInTime, EV.ClockOutTime) / @PerUnitQuantity   
      END  
     
  FROM  EmployeeVisits  EV (NOLOCK)  
  RIGHT OUTER JOIN ScheduleMasters  SM (NOLOCK) ON EV.ScheduleID = SM.ScheduleID   
  --INNER JOIN ReferralBillingAuthorizations RBA ON SM.ReferralBillingAuthorizationID = RBA.ReferralBillingAuthorizationID  
  
  Where  SM.ReferralBillingAuthorizationID = @BillingAuthorizationID  
     AND SM.IsDeleted = 0  
     AND EV.IsDeleted = 0  
     AND SM.StartDate >= @BillingStartDate  
     AND SM.EndDate <= @BillingEndDate  
     AND (@UnitType in (1, 2))  
 )  
 Select   
   @Allocated = IsNull(Sum(Allocated_Unit), 0),  
   @Used = IsNull(Sum(Used_Unit), 0)   
 from CTE  
  
  
 -- Calculate Units for UnitType = 3, Per Day  
 DECLARE @PerDayMax INT = Case When @UnitType = 3 Then DATEDIFF(Day, @BillingStartDate, @BillingEndDate) ELSE 0 END  
 DECLARE @PerDayUsed INT = Case When @UnitType = 3 Then DATEDIFF(Day, @BillingStartDate, GETDATE()) ELSE 0 END  
 DECLARE @PerDayRemaining INT = Case When @UnitType = 3 Then DATEDIFF(Day, GETDATE(), @BillingEndDate) ELSE 0 END  
  
 DECLARE @RemainingCalc INT = Case When @UnitType = 3 THEN @PerDayMax - @PerDayUsed ELSE  @MaxUnit - @Used - @PerDayUsed END  
 DECLARE @UnallocatedCalc INT = Case When @UnitType = 3 THEN @PerDayRemaining ELSE  @MaxUnit - @Allocated END  
  
 Select   
   ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID,  
   StartDate = @BillingStartDate,  
   EndDate = @BillingEndDate,  
   ReferralBillingAuthorizationName = @AuthorizationCode,  
   ServiceCode = @ServiceCode,  
   CareType = @CareType,  
  
   Available = @MaxUnit + @PerDayMax,  
   Allocated = @Allocated + @PerDayMax,  
   Used = @Used + @PerDayUsed,  
   Remaining = Case When @RemainingCalc > 0 OR @MaxUnit > 0 THEN Cast(@RemainingCalc as varchar) ELSE  'n/a' END,  
   Unallocated = Case When @UnallocatedCalc > 0 OR @MaxUnit > 0 THEN Cast(@UnallocatedCalc as varchar) ELSE  'n/a' END  
  
    
END  