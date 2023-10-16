-- EXEC Rpt_GetRespiteUsage @StartDate = '2016-10-1', @EndDate = '2017-9-30', @IsDeleted = '-1'

-- exec Rpt_GetRespiteUsage            
CREATE PROCEDURE  [dbo].[Rpt_GetRespiteUsage]                  
 @ReferralStatusID bigint=0,            
 @AgencyID bigint=0,            
 @RegionID bigint=0,            
 @IsDeleted int=-1,              
 @StartDate date=null,            
 @EndDate Date =null            
AS                          
BEGIN            
            
SELECT  Agency, PayorName,ImportHours= ISNULL(ImportHours,0), AHCCCSID, CISNumber, LastName +', '+FirstName as Name          
, Oct= ISNULL(OCT,0), Nov= ISNULL(NOV,0), Dec= ISNULL(Dec,0) , Jan= ISNULL(JAN,0),Feb= ISNULL(Feb,0),Mar= ISNULL(MAR,0),            
Apr= ISNULL(Apr,0),May= ISNULL(MAY,0),            
Jun= ISNULL(JUN,0),Jul= ISNULL(JUL,0),Aug= ISNULL(Aug,0),            
Sep= ISNULL(SEP,0), StartDate=@StartDate, EndDate=@EndDate      
 FROM (            
 SELECT Agency,PayorName=ShortName,ImportHours, AHCCCSID,CISNumber ,FirstName,LastName,month,sum(Calulatedtime) as Calulatedtime FROM (            
 
	SELECT  Agency,ShortName,ImportHours, AHCCCSID, CISNumber ,FirstName,LastName,LEFT(DATENAME(MONTH,ISNULL(ServiceDate,@StartDate)),3)as [month],                        
	 Calulatedtime= (CASE WHEN ServiceCode='S5151' THEN 24  * COUNT(ServiceCode)
					WHEN ServiceCode!='S5151' THEN CAST( (15 * SUM(CalculatedUnit)) / 60  as decimal(16,2) ) 
					ELSE 0 END)
		FROM (
					 SELECT DISTINCT A.NickName As Agency,P.ShortName,R.ImportHours, R.AHCCCSID,R.CISNumber ,R.FirstName,R.LastName, N.ServiceDate,N.CalculatedUnit,
					 N.NoteID, N.ServiceCode
					 FROM Referrals R
					 --LEFT JOIN Referrals R on N.ReferralID=R.ReferralID                  
					 LEFT JOIN Notes N on N.ReferralID=R.ReferralID  AND N.CheckRespiteHours=1  AND N.IsBillable=1 AND N.IsDeleted=0
					 AND ((@StartDate is null OR N.ServiceDate >= @StartDate) and (@EndDate is null OR N.ServiceDate<= @EndDate))      
					 LEFT JOIN ReferralPayorMappings RPM on RPM.ReferralID=R.ReferralID AND  RPM.IsActive=1             
					 LEFT JOIN Payors P on P.PayorID=RPM.PayorID                  
					 LEFT JOIN ServiceCodes SC on SC.ServiceCodeID=N.ServiceCodeID AND SC.CheckRespiteHours=1                    
					 --LEFT JOIN ReferralRespiteUsageLimit RRU on RRU.ReferralID=r.ReferralID  AND RRU.IsActive=1                   
					 LEFT JOIN Agencies A on A.AgencyID=R.AgencyID              
					 LEFT JOIN Regions RG on RG.RegionID=R.RegionID            
					 LEFT JOIN ReferralStatuses  RS on RS.ReferralStatusID=R.ReferralStatusID            
					 where             
					 ((CAST(@IsDeleted AS BIGINT)=-1) OR R.IsDeleted=@IsDeleted)  -- AND R.ReferralID IN (3357,  638)                                         
					   AND (( CAST(@RegionID AS BIGINT)=0) OR R.RegionID = CAST(@RegionID AS BIGINT))                  
					   AND (( CAST(@ReferralStatusID AS BIGINT)=0) OR R.ReferralStatusID = CAST(@ReferralStatusID AS BIGINT))                  
					   AND (( CAST(@AgencyID AS BIGINT)=0) OR R.AgencyID = CAST(@AgencyID AS BIGINT))                  
	   ) AS T1
      group by Agency,ShortName,ImportHours,AHCCCSID,CISNumber,FirstName,LastName,ServiceDate,ServiceCode            
) AS T            
 GROUP BY Agency,ShortName,CISNumber,ImportHours, AHCCCSID,FirstName,LastName,month            
   )              
 as s              
PIVOT                  
(                  
   SUM(Calulatedtime)                  
   FOR [month] IN (JAN,FEB,MAR,APR,                   
    MAY,JUN,JUL,AUG,SEP,OCT,NOV,DEC)                  
)          
AS pvt ORDER BY LastName ASC  
            
              
END 

-- EXEC Rpt_GetRespiteUsage @StartDate = '2016-10-1', @EndDate = '2017-9-30', @IsDeleted = '-1'


--SELECT ReferralID FROM Referrals WHERE AHCCCSID='A66254125'
--SELECT ReferralID FROM Referrals WHERE AHCCCSID='A45793455'
