-- EXEC GetSchEmpRefSkills @ReferralID = '1951', @EmployeeID = '106', @PreferenceType = 'Preference'  
CREATE PROCEDURE [dbo].[HC_PrivateDuty_GetSchEmpRefSkills]
@ReferralID BIGINT,  
@EmployeeID BIGINT,  
@PreferenceType VARCHAR(100)  
AS  
BEGIN  
  
  
SELECT * FROM (  
SELECT R.ReferralID,P.PreferenceName,  
P.PreferenceID  ,E.EmployeeID,  
IsMatched = CASE WHEN E.EmployeeID IS NOT NULL THEN 1 ELSE 0 END,  
MatchedPercent= SUM(CASE WHEN E.EmployeeID IS NOT NULL THEN 1 ELSE 0 END) OVER(PARTITION BY R.ReferralID) * 100 / COUNT(R.ReferralID) OVER(PARTITION BY R.ReferralID)  
  
FROM Referrals R  
INNER JOIN ReferralPreferences RP ON R.ReferralID=RP.ReferralID  
INNER JOIN Preferences P ON P.PreferenceID=RP.PreferenceID AND P.KeyType =@PreferenceType  
LEFT JOIN EmployeePreferences EP ON EP.PreferenceID=RP.PreferenceID AND EP.EmployeeID=@EmployeeID  
LEFT JOIN Employees E ON  E.EmployeeID=EP.EmployeeID --AND E.EmployeeID=2  
  
WHERE R.ReferralID=@ReferralID  
) AS Temp    
ORDER BY PreferenceName ASC  
  
  
END  
  
--SELECT * FROM ReferralPreferences  RP  
--INNER JOIN Preferences P ON P.PreferenceID= RP.PreferenceID  
--WHERE ReferralID=1951