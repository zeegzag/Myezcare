-- HC_GetEmployeesForScheduling @ReferralID=1951,@EmployeeName=''
CREATE PROCEDURE [dbo].[HC_GetEmployeesForScheduling]
@ReferralID BIGINT,
@EmployeeName VARCHAR(MAX),
@PreferenceType_Prefernce VARCHAR(100)='Preference',
@PreferenceType_Skill VARCHAR(100)='Skill'
AS
BEGIN



DECLARE @TotalPrefeCount BIGINT=0;
DECLARE @TotalSkillCount BIGINT=0;

SELECT  
@TotalPrefeCount=SUM(CASE WHEN PR.KeyType=@PreferenceType_Prefernce THEN 1 ELSE 0 END) OVER (PARTITION BY RP.ReferralID),
@TotalSkillCount=SUM(CASE WHEN PR.KeyType=@PreferenceType_Skill THEN 1 ELSE 0 END) OVER (PARTITION BY RP.ReferralID)
FROM ReferralPreferences  RP
INNER JOIN Preferences PR ON PR.PreferenceID=RP.PreferenceID
WHERE ReferralID=@ReferralID

IF(@TotalPrefeCount=0) SET @TotalPrefeCount=1;
IF(@TotalSkillCount=0) SET @TotalSkillCount=1;

PRINT @TotalPrefeCount
PRINT @TotalSkillCount



DECLARE @TempEmployee Table(
   ReferralID BIGINT,
   RefPreferenceID BIGINT,
   EmpPreferenceID  BIGINT,
   EmployeeID BIGINT,
   FirstName VARCHAR(100),
   LastName VARCHAR(100),
   IsDeleted BIT,
   OrderRank INT,
   PreferencesMatchPercent INT,
   SkillsMatchPercent INT,
   EmpLatLong GeoGraphy,
   KeyType VARCHAR(MAX)
) 


INSERT INTO @TempEmployee
SELECT ReferralID,RefPreferenceID,EmpPreferenceID,EmployeeID,FirstName,LastName,IsDeleted,OrderRank,PreferencesMatchCount,SkillsMatchCount,EmpLatLong,KeyType FROM (

SELECT R.ReferralID,PE.KeyType,
RefPreferenceID=RP.PreferenceID,EmpPreferenceID=EP.PreferenceID,E.EmployeeID,E.FirstName,E.LastName, E.IsDeleted, EmpLatLong=[dbo].GetGeoFromLatLng(E.Latitude,E.Longitude),
PreferencesMatchCount=SUM(CASE WHEN PE.KeyType=@PreferenceType_Prefernce AND RP.PreferenceID IS NOT NULL THEN 1 ELSE 0 END) OVER( PARTITION BY EP.EmployeeID) ,--* 100/@TotalPrefeCount ,
SkillsMatchCount=SUM(CASE WHEN PE.KeyType=@PreferenceType_Skill THEN 1 ELSE 0 END) OVER( PARTITION BY R.ReferralID,EP.EmployeeID) ,--* 100/@TotalSkillCount ,
--SkillsMatchCount=COUNT(R.ReferralID) OVER( PARTITION BY R.ReferralID,EP.EmployeeID,PE.KeyType)  ,

OrderRank= DENSE_RANK() OVER(PARTITION BY  EP.EmployeeID ORDER BY R.ReferralID DESC ,EP.EmployeePreferenceID DESC)  
FROM Employees E 
LEFT JOIN EmployeePreferences EP ON E.EmployeeID=EP.EmployeeID
LEFT JOIN ReferralPreferences RP ON RP.PreferenceID=EP.PreferenceID AND RP.ReferralID=@ReferralID
LEFT JOIN Preferences PE ON PE.PreferenceID=EP.PreferenceID
--LEFT JOIN Preferences PR ON PR.PreferenceID=RP.PreferenceID
LEFT JOIN (SELECT ReferralID=@ReferralID) R ON  R.ReferralID=RP.ReferralID

WHERE 1=1 AND E.EmployeeID=18
AND
( @EmployeeName IS NULL 
   OR (
   	   (E.FirstName LIKE '%'+@EmployeeName+'%' )OR  
	   (E.LastName  LIKE '%'+@EmployeeName+'%') OR  
	   (E.FirstName +' '+E.LastName like '%'+@EmployeeName+'%') OR  
	   (E.LastName +' '+E.FirstName like '%'+@EmployeeName+'%') OR  
	   (E.FirstName +', '+E.LastName like '%'+@EmployeeName+'%') OR  
	   (E.LastName +', '+E.FirstName like '%'+@EmployeeName+'%')))

--ORDER BY ReferralID DESC,MatchCount DESC,FirstName ASC
) AS Temp WHERE 1=1 --AND Temp.OrderRank=1 




SELECT * FROM @TempEmployee



SELECT * FROM (
SELECT  E.*, Distance=EmpLatLong.STDistance([dbo].GetGeoFromLatLng(C.Latitude,C.Longitude)) * 0.000621371,--  RefLatLong=[dbo].GetGeoFromLatLng(C.Latitude,C.Longitude),
SUM(DATEDIFF(mi, SM.StartDate,SM.EndDate) / 60.0) OVER ( PARTITION BY E.EmployeeID ORDER BY E.EmployeeID ASC) AS EmployeeUsedHours,
DENSE_RANK() OVER ( PARTITION BY E.EmployeeID ORDER BY SM.ScheduleID ASC) AS EmployeeRank,
C.ContactID
FROM @TempEmployee E
LEFT JOIN ScheduleMasters SM ON SM.EmployeeID = E.EmployeeID AND SM.IsDeleted=0
LEFT JOIN ContactMappings CM ON CM.ReferralID = E.ReferralID AND CM.ContactTypeID=1
LEFT JOIN Contacts C ON C.ContactID= CM.ContactID 

) AS TEMP WHERE 1=1 AND EmployeeRank=1
ORDER BY ReferralID DESC,Distance ASC--,SkillsMatchCount ASC, PreferencesMatchCount DESC, FirstName ASC

END
