CREATE PROCEDURE [dbo].[GetCareTypeScheduleList]    
 @ReferralID BIGINT = 0,    
 @StartDate DATE = NULL,    
 @EndDate DATE = NULL,    
 @SortExpression NVARCHAR(100),    
 @SortType NVARCHAR(10),    
 @FromIndex INT,    
 @PageSize INT    
AS                            
BEGIN    
 ;WITH CTECareTypeList AS                            
 (                             
  SELECT *,COUNT(t1.CareTypeTimeSlotID) OVER() AS Count FROM                             
  (                            
   SELECT ROW_NUMBER() OVER (ORDER BY                             
                        
                                            
                         
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Name' THEN TBL1.Name END END ASC,    
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Name' THEN TBL1.Name END END DESC,    
    
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CareTypeName' THEN TBL1.CareTypeName END END ASC,    
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CareTypeName' THEN TBL1.CareTypeName END END DESC,    
             
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Count' THEN TBL1.NumOfTime END END ASC,    
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Count' THEN TBL1.NumOfTime END END DESC,    
                            
    CASE WHEN @SortType = 'ASC' THEN                            
      CASE                             
      WHEN @SortExpression = 'StartDate' THEN TBL1.StartDate                            
      END                             
    END ASC,                            
    CASE WHEN @SortType = 'DESC' THEN                            
      CASE                             
      WHEN @SortExpression = 'EndDate' THEN TBL1.EndDate                            
      END                            
    END DESC          
  ) AS Row,  * FROM (                  
                    
   SELECT DISTINCT CareTypeTimeSlotID,SM.ReferralID,SM.CareTypeID,CareTypeName=DD.Title,Name=dbo.GetGeneralNameFormat(R.FirstName,R.LastName),NumOfTime=SM.Count,SM.Frequency,    
   SM.StartDate,SM.EndDate, CaseManagerName=dbo.GetGeneralNameFormat(CM.FirstName,CM.LastName)  
   FROM  CareTypeTimeSlots SM    
   INNER JOIN Referrals R on R.ReferralID=SM.ReferralID    
   LEFT JOIN CaseManagers CM ON CM.CaseManagerID=R.CaseManagerID
   INNER JOIN DDMaster DD ON DD.DDMasterID=SM.CareTypeID    
   WHERE ((@ReferralID =0 OR LEN(@ReferralID)=0) OR SM.ReferralID=@ReferralID)                    
   AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR SM.StartDate LIKE '%' + CONVERT(VARCHAR(20),@StartDate) + '%')                            
   AND ((@EndDate IS NULL OR LEN(@EndDate)=0) OR SM.EndDate LIKE '%' + CONVERT(VARCHAR(20),@EndDate) + '%')                    
                    
  )   AS TBL1           
                     
  ) AS t1  )                            
                       
 SELECT * FROM CTECareTypeList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                             
   select * from ReferralCareGivers                        
END
