  
CREATE PROCEDURE [dbo].[GetEmpRefSchOptions_ClientOnHoldData_HC]                 
  @ReferralID BIGINT = 0,            
  @ScheduleID BIGINT = 0,            
  @StartDate DATETIME,            
  @EndDate DATETIME,            
  @EmployeeName VARCHAR(MAX) = '',            
  @MileRadius BIGINT = NULL,                                                
  @StrSkillList VARCHAR(MAX) = NULL,            
  @StrPreferenceList VARCHAR(MAX) = NULL,            
  @PreferenceType_Prefernce VARCHAR(100) = 'Preference',            
  @PreferenceType_Skill VARCHAR(100) = 'Skill',            
  @SameDateWithTimeSlot BIT,            
  @SortExpression NVARCHAR(100),            
  @SortType NVARCHAR(10),            
  @FromIndex INT,            
  @PageSize INT,            
  @SortIndexArray VARCHAR(MAX),            
  @DDType_CareType INT,            
  @CareTypeID VARCHAR(MAX) = NULL   
    
  AS    
BEGIN      
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
     SELECT RH.*,              
   CreatedBy =dbo.GetGenericNameFormat(EC.FirstName,EC.MiddleName, EC.LastName,@NameFormat),            
    UpdatedBy =dbo.GetGenericNameFormat(EU.FirstName,EU.MiddleName, EU.LastName,@NameFormat),              
   CurrentActiveGroup = CASE               
     WHEN GETDATE() BETWEEN RH.StartDate              
      AND RH.EndDate              
    THEN 1              
     ELSE 0              
     END,              
   OldActiveGroup = CASE               
     WHEN GETDATE() > RH.EndDate              
    THEN 1              
     ELSE 0              
     END              
    FROM ReferralOnHoldDetails RH  WITH (NOLOCK)            
    INNER JOIN Employees EC WITH (NOLOCK)             
   ON EC.EmployeeID = RH.CreatedBy              
    INNER JOIN Employees EU WITH (NOLOCK)             
   ON EU.EmployeeID = RH.UpdatedBy              
    WHERE RH.IsDeleted = 0              
   AND RH.ReferralID = @ReferralID               
     ORDER BY StartDate DESC              
    
END  