-- EXEC GetRtsMasterList @ReferralID =1951, @SortExpression = 'Name', @SortType = 'DESC', @FromIndex = '1', @PageSize = '100'                      
CREATE PROCEDURE [dbo].[GetRtsMasterList]    
 @ReferralID BIGINT = 0,                      
 @StartDate DATE = NULL,                      
 @EndDate DATE = NULL,                
 @IsDeleted int=-1,                      
 @SortExpression NVARCHAR(100),                        
 @SortType NVARCHAR(10),                      
 @FromIndex INT,                      
 @PageSize INT                       
AS                      
BEGIN

DECLARE @ActiveReferralTimeSlotMasterID BIGINT

SELECT TOP 1 @ActiveReferralTimeSlotMasterID=etsdate.ReferralTimeSlotMasterID FROM ReferralTimeSlotDetails etsd
	INNER JOIN ReferralTimeSlotDates etsdate ON etsdate.ReferralTimeSlotDetailID=etsd.ReferralTimeSlotDetailID
	WHERE etsd.ReferralTimeSlotMasterID IN (SELECT ReferralTimeSlotMasterID FROM ReferralTimeSlotMaster WHERE ReferralID=@ReferralID) AND IsDeleted=0
	AND ReferralTSDate >= CONVERT(DATE,GETDATE())
	ORDER BY ReferralTSDate

 ;WITH CTERtsMasterList AS                      
 (                       
  SELECT *,COUNT(t1.ReferralTimeSlotMasterID) OVER() AS Count FROM                       
  (                      
   SELECT ROW_NUMBER() OVER (ORDER BY                       
                  
               
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralTimeSlotMasterID' THEN TBL1.ReferralTimeSlotMasterID END END ASC,                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralTimeSlotMasterID' THEN TBL1.ReferralTimeSlotMasterID END END DESC,                      
                   
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Name' THEN TBL1.Name END END ASC,                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Name' THEN TBL1.Name END END DESC,       
       
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'TotalRTSDetailCount' THEN TBL1.TotalRTSDetailCount END END ASC,                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'TotalRTSDetailCount' THEN TBL1.TotalRTSDetailCount END END DESC,                       
                      
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
              
   SELECT DISTINCT ets.ReferralTimeSlotMasterID,ets.ReferralID,Name=dbo.GetGeneralNameFormat(e.FirstName,e.LastName),
   CreatedBy=dbo.GetGeneralNameFormat(ec.FirstName,ec.LastName),UpdatedBy=dbo.GetGeneralNameFormat(eu.FirstName,eu.LastName),ets.CreatedDate,ets.UpdatedDate,
   ets.IsDeleted,ets.StartDate,ets.EndDate,ets.IsEndDateAvailable,TotalRTSDetailCount= COUNT(ed.ReferralTimeSlotMasterID) OVER(PARTITION BY ed.ReferralTimeSlotMasterID),
	ActiveStat=CASE WHEN ets.ReferralTimeSlotMasterID=@ActiveReferralTimeSlotMasterID THEN 1 ELSE 0 END,
	ReferralBillingAuthorizationID
   FROM  ReferralTimeSlotMaster ets                     
   INNER JOIN Referrals e on e.ReferralID=ets.ReferralID              
   INNER JOIN Employees ec on ec.EmployeeID=ets.CreatedBy              
   INNER JOIN Employees eu on eu.EmployeeID=ets.UpdatedBy              
   LEFT JOIN ReferralTimeSlotDetails ED ON ets.ReferralTimeSlotMasterID=ED.ReferralTimeSlotMasterID AND ED.IsDeleted=0           
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR ets.IsDeleted=@IsDeleted)                      
   AND ((@ReferralID =0 OR LEN(@ReferralID)=0) OR ets.ReferralID=@ReferralID)              
   AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR ets.StartDate LIKE '%' + CONVERT(VARCHAR(20),@StartDate) + '%')                      
   AND ((@EndDate IS NULL OR LEN(@EndDate)=0) OR ets.EndDate LIKE '%' + CONVERT(VARCHAR(20),@EndDate) + '%')              
              
  )   AS TBL1     
               
            
               
               
                        
  ) AS t1  )                      
                 
 SELECT * FROM CTERtsMasterList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                       
                      
END
