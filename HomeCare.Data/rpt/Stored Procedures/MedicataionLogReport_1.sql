--  EXEC [rpt].[MedicataionLogReport] @ReferralID=30051, @StartDate='2021-06-15'  
CREATE PROCEDURE [rpt].[MedicataionLogReport]   


@ReferralID BIGINT=0,  
@StartDate DATE =null  
  as 
BEGIN  

--@SearchDate VARCHAR(MAX)  
--SET @SearchDate =CAST((SELECT CONCAT(RIGHT('0' + RTRIM(YEAR(@StartDate)), 4), '-',RIGHT('0' + RTRIM(MONTH(@StartDate)), 2))) as varchar)

SELECT DISTINCT R.ReferralID, R.FirstName, R.LastName, CONCAT(R.FirstName,' ',R.LastName) AS FullName, M.MedicationName,  
CAST((SELECT CONCAT(RIGHT('0' + RTRIM(MONTH(@StartDate)), 2), '/',RIGHT('0' + RTRIM(YEAR(@StartDate)), 4))) as varchar) AS AdmissionDate,  
CONVERT(varchar, RM.StartDate, 8) AS Time, convert(varchar,RM.StartDate,101) as CreatedDate,
RM.Unit, CONCAT(RM.Dose, ' ',RM.Unit) AS DoseWithUnit  
FROM ReferralMedication RM                       
INNER JOIN Referrals R ON R.ReferralID = RM.ReferralID AND R.IsDeleted=0 AND  RM.IsDeleted=0   
INNER JOIN Medication M ON M.MedicationId = RM.MedicationId AND RM.IsDeleted=0    
WHERE R.ReferralID = @ReferralID AND RM.IsActive=1 --AND  
--CAST((SELECT CONCAT(RIGHT('0' + RTRIM(YEAR(RM.CreatedDate)), 4), '-',RIGHT('0' + RTRIM(MONTH(RM.CreatedDate)), 2))) as varchar) = @SearchDate  
 and rm.EndDate >=@StartDate
END