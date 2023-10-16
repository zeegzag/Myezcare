        
--  EXEC GetPatientBirthday '2021-04-07', '2021-07-07', 'Birthday', 'ASC', 1, 20         
          
CREATE PROCEDURE [dbo].[GetPatientBirthday]               
@StartDate DATE = NULL,                                
@EndDate  DATE = NULL,           
@SortExpression NVARCHAR(100),                                  
@SortType NVARCHAR(10),                                
@FromIndex INT,                                
@PageSize INT                    
                    
AS                                
BEGIN                                
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()                        
;WITH List AS                                
 (                                 
  SELECT *,COUNT(T1.ReferralID) OVER() AS Count FROM                                 
  (                                
   SELECT ROW_NUMBER() OVER (ORDER BY                                 
                            
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Patient' THEN t.LastName END END ASC,                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Patient' THEN t.LastName END END DESC,      
       
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Birthday' THEN t.DateOfBirth END END ASC,                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Birthday' THEN t.DateOfBirth END END DESC                
                        
   ) AS ROW,                                
   t.*  FROM     (                      
            
SELECT DISTINCT        
 R.ReferralID, R.FirstName, R.LastName,  PatientName = dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),          
          
 FORMAT (R.Dob, 'dd, MMMM') as DateOfBirth,      
 ISNULL(max(C.Phone1), 'NA') AS [Phone]          
  FROM (          
SELECT CAST(CONCAT(RIGHT('0' + RTRIM(MONTH(DOB)), 2),'-',RIGHT('0' + RTRIM(DAY(DOB)), 2)) as varchar) as cdob, * FROM Referrals where Dob is not null           
) R           
 INNER JOIN (select ReferralID,max(ContactID)ContactID from ContactMappings group by ReferralID) CM on cm.ReferralID = R.ReferralID          
 INNER JOIN (select ContactID,max(Phone1) Phone1 from Contacts group by ContactID ) C on C.ContactID = CM.ContactID         
WHERE          
 R.cdob >= CAST((SELECT CONCAT(RIGHT('0' + RTRIM(MONTH(@StartDate)), 2), '-',RIGHT('0' + RTRIM(DAY(@StartDate)), 2))) as varchar)           
 AND R.cdob  <=          
  CAST((SELECT CONCAT(RIGHT('0' + RTRIM(MONTH(@EndDate)), 2),'-',RIGHT('0' + RTRIM(DAY(@EndDate)), 2))) as varchar)          
  AND R.IsDeleted = 0          
    GROUP BY R.ReferralID, R.FirstName,R.MiddleName, R.LastName, R.Dob, C.Phone1        
  ) t          
          
)  AS T1 )                    
                    
SELECT * FROM List WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                                 
END 