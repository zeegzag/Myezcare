  
--EXEC CategoryList @Category = 'AuthorizationCode', @EmployeeVisitIDList = '60248'  
--  exec  CategoryList  'Care Type'       
CREATE PROCEDURE CategoryList      
@Category VARCHAR(MAX) ,    
--@PayorID BIGINT=0,     
--@ReferralID BIGINT=0,    
 @EmployeeVisitIDList NVARCHAR(max)    
AS      
BEGIN      
  IF(@Category='CareType')    
  BEGIN    
 SELECT            
    dm.Title AS Name,            
    dm.DDMasterID AS Value            
  FROM DDMaster dm            
  INNER JOIN lu_DDMasterTypes lu            
    ON lu.DDMasterTypeID = dm.ItemType            
  WHERE lu.Name = 'Care Type' AND dm.IsDeleted=0      
  END    
    
  ELSE  IF(@Category='AuthorizationCode')    
  BEGIN    
                   SELECT *     
INTO #tempTable    
FROM (    
   select EmployeeVisitID,ScheduleID from EmployeeVisits where EmployeeVisitID in (select val from dbo.f_split(@EmployeeVisitIDList, ','))     
) AS x    
    
declare @ReferralID bigint    
declare @ScheduleID bigint    
SELECT @ScheduleID=Scheduleid FROM #tempTable    
select @ReferralID=ReferralId from ScheduleMasters where ScheduleID =@ScheduleID    
    
 --   SELECT  Name = RBA.AuthorizationCode,Value = RBA.ReferralBillingAuthorizationID       
 --FROM ReferralBillingAuthorizations RBA    
 SELECT  Name = RBA.AuthorizationCode,Value = RBA.ReferralBillingAuthorizationID,D.Title AS CareType, RBA.StartDate, RBA.EndDate, SC.ServiceName, SC.ServiceCode         
 FROM ReferralBillingAuthorizations RBA    
  INNER JOIN Referrals R ON R.ReferralID = RBA.ReferralID    
 LEFT JOIN ReferralBillingAuthorizationServiceCodes RBAS ON RBAS.ReferralBillingAuthorizationID = RBA.ReferralBillingAuthorizationID    
 LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID = RBA.ServiceCodeID    
 LEFT JOIN DDMaster D ON D.DDMasterID = RBA.CareType   
 WHERE  RBA.ReferralID = @ReferralID AND RBA.IsDeleted = 0            
       
  END    
    
    ELSE  IF(@Category='Payor')    
  BEGIN           
 select p.PayorName AS Name,p.PayorID AS Value from  Payors as p Where p.IsDeleted=0 ORDER BY p.PayorName ASC            
      
  END    
END      
      
      