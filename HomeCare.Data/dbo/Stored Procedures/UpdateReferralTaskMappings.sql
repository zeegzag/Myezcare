CREATE PROCEDURE [dbo].[UpdateReferralTaskMappings]
 @VisitTaskIDs NVARCHAR(MAX),                                             
 @loggedInUserId BIGINT,                            
 @SystemID VARCHAR(100)
AS
BEGIN
INSERT INTO ReferralTaskMappings  
 SELECT  
 V.VisitTaskID, V.IsRequired, 0 IsDeleted, R.ReferralID, GETUTCDATE(), @loggedInUserId, GETUTCDATE(), @loggedInUserId, @SystemID, NULL, NULL,'1,2,3,4,5,6,7' 

 FROM Referrals R  
 INNER JOIN VisitTasks V ON V.VisitTaskID IN (SELECT Val FROM GetCSVTable(@VisitTaskIDs)) AND  (ISNULL(V.CareType, 0) = 0 OR V.CareType IN (SELECT Val FROM GetCSVTable(R.CareTypeIds)))  
 LEFT JOIN ReferralTaskMappings RTM  
 ON RTM.VisitTaskID = V.VisitTaskID AND RTM.ReferralID = R.ReferralID AND RTM.IsDeleted = 0  
 WHERE R.IsDeleted = 0  
 AND V.IsDeleted = 0  
 AND V.IsDefault = 1
 AND RTM.ReferralTaskMappingID IS NULL  
  
 UPDATE RTM  
 SET   
 IsRequired = V.IsRequired,  
 IsDeleted = CASE WHEN (ISNULL(V.CareType, 0) = 0 OR V.CareType IN (SELECT Val FROM GetCSVTable(R.CareTypeIds))) THEN 0 ELSE 1 END  
 FROM Referrals R  
 INNER JOIN VisitTasks V ON V.VisitTaskID IN (SELECT Val FROM GetCSVTable(@VisitTaskIDs))
 INNER JOIN ReferralTaskMappings RTM  
 ON RTM.VisitTaskID = V.VisitTaskID AND RTM.ReferralID = R.ReferralID AND RTM.IsDeleted = 0  
 WHERE R.IsDeleted = 0  
 AND V.IsDefault = 1
 AND  V.IsDeleted = 0

 UPDATE RTM  
 SET   
 IsDeleted = 1  
 FROM Referrals R  
 INNER JOIN VisitTasks V ON V.VisitTaskID IN (SELECT Val FROM GetCSVTable(@VisitTaskIDs))
 INNER JOIN ReferralTaskMappings RTM  
 ON RTM.VisitTaskID = V.VisitTaskID AND RTM.ReferralID = R.ReferralID AND RTM.IsDeleted = 0  
 WHERE R.IsDeleted = 0  
 AND  V.IsDeleted = 1

 END