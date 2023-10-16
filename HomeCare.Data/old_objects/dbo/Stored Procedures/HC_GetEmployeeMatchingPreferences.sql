-- EXEC HC_GetEmployeeMatchingPreferences 2,3769
CREATE PROCEDURE [dbo].[HC_GetEmployeeMatchingPreferences]
@EmployeeID BIGINT,
@ReferralID BIGINT
AS
BEGIN


SELECT Value=E.EmployeeID, Name= E.FirstName+' '+E.LastName FROM Employees E WHERE E.EmployeeID=@EmployeeID
SELECT Value=R.ReferralID, Name=R.LastName+', '+ R.FirstName FROM Referrals R WHERE R.ReferralID=@ReferralID


SELECT * FROM (
SELECT R.ReferralID,P.PreferenceName, ClientName=R.LastName+', '+ R.FirstName,
P.PreferenceID,E.EmployeeID,EmployeeName= E.FirstName+' '+E.LastName, 
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
