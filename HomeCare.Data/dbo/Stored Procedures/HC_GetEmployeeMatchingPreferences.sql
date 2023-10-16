-- EXEC HC_GetEmployeeMatchingPreferences 2,3769  
CREATE PROCEDURE [dbo].[HC_GetEmployeeMatchingPreferences]  
@EmployeeID BIGINT,  
@ReferralID BIGINT  
AS  
BEGIN  
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
  
SELECT Value=E.EmployeeID, Name= dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) FROM Employees E WHERE E.EmployeeID=@EmployeeID  
SELECT Value=R.ReferralID, Name=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) FROM Referrals R WHERE R.ReferralID=@ReferralID  
  
  
SELECT * FROM (  
SELECT R.ReferralID,P.PreferenceName, ClientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),  
P.PreferenceID,E.EmployeeID,EmployeeName= dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),   
IsMatched = CASE WHEN E.EmployeeID IS NOT NULL THEN 1 ELSE 0 END,  
MatchedPercent= SUM(CASE WHEN E.EmployeeID IS NOT NULL THEN 1 ELSE 0 END) OVER(PARTITION BY R.ReferralID) * 100 / COUNT(R.ReferralID) OVER(PARTITION BY R.ReferralID)  
  
FROM Referrals R  
INNER JOIN ReferralPreferences RP ON R.ReferralID=RP.ReferralID  
LEFT JOIN EmployeePreferences EP ON EP.PreferenceID=RP.PreferenceID AND EP.EmployeeID=@EmployeeID  
LEFT JOIN Employees E ON  E.EmployeeID=EP.EmployeeID --AND E.EmployeeID=2  
LEFT JOIN Preferences P ON P.PreferenceID=RP.PreferenceID  
WHERE R.ReferralID=@ReferralID  
) AS Temp    
ORDER BY EmployeeID DESC, PreferenceName ASC  
  
END  