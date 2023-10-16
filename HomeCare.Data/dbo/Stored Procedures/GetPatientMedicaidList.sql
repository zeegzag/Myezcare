CREATE PROCEDURE [dbo].[GetPatientMedicaidList]              
@StartDate DATE =NULL,                                  
@EndDate  DATE=NULL,               
@SortExpression NVARCHAR(100),                                  
@SortType NVARCHAR(10),                                
@FromIndex INT,                                
@PageSize INT                    
                     
AS                                
BEGIN                                
    DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()                             
;WITH CTEGetPatientMedicaidList AS                                
 (                                 
  SELECT *,COUNT(T1.ReferralID) OVER() AS Count FROM                                 
  (                                
   SELECT ROW_NUMBER() OVER (ORDER BY                                 
                            
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Patient' THEN t.LastName END END ASC,                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Patient' THEN t.LastName END END DESC,                           
                
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN  CONVERT(varchar, t.CreatedDate, 101) END END ASC,                                                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN  CONVERT(varchar, t.CreatedDate, 101) END END DESC,                         
                
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Dose' THEN t.Dose END END ASC,                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Dose' THEN t.Dose END END DESC,    
     
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Unit' THEN t.Unit END END ASC,                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Unit' THEN t.Unit END END DESC,    
    
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Frequency' THEN t.Frequency END END ASC,                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Frequency' THEN t.Frequency END END DESC,     
     
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Route' THEN t.Route END END ASC,                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Route' THEN t.Route END END DESC                
                        
   ) AS ROW,                                
   t.*  FROM     (                      
                
                
SELECT DISTINCT R.ReferralID, R.FirstName, R.LastName,dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) AS Patient,
RM.Dose, RM.Unit, RM.Frequency, RM.Route, ISNULL(RM.Quantity, 'NA') AS Quantity, convert(varchar,R.CreatedDate,101) as CreatedDate            
FROM ReferralMedication RM                         
INNER JOIN Referrals R ON R.ReferralID=RM.ReferralID AND R.IsDeleted=0                 
--LEFT JOIN ReferralOnHoldDetails ROH ON ROH.ReferralID=R.ReferralID              
WHERE RM.IsDeleted=0      
            
) AS T                
                
)  AS T1 )                    
                    
SELECT * FROM CTEGetPatientMedicaidList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                                 
END  