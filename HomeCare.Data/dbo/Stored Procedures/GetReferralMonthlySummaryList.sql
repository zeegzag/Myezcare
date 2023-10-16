--exec GetReferralMonthlySummaryList @ReferralID=2892
CREATE PROCEDURE [dbo].[GetReferralMonthlySummaryList]  
@ReferralID bigint,
@ClientName varchar(100),
@CreatedBy bigint=0,
@StartDate Date =null,
@EndDate Date=null,
@FacilityID bigint=0, 
@RegionID bigint=0,    
@IsDeleted BIGINT = -1,        
@SORTEXPRESSION NVARCHAR(100),             
@SORTTYPE NVARCHAR(10),            
@FROMINDEX INT,                            
@PAGESIZE INT

AS  

                           
BEGIN                              
;WITH CTEReferralMonthlySummaries AS                        
 (                             
  SELECT *,COUNT(T1.ReferralMonthlySummariesID) OVER() AS Count FROM                        
  (                            
   SELECT ROW_NUMBER() OVER (ORDER BY                        
                        
		 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralMonthlySummariesID' THEN ReferralMonthlySummariesID END END ASC,                        
		 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralMonthlySummariesID' THEN ReferralMonthlySummariesID END END DESC,

		 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FacilityName' THEN FacilityName END END ASC,                        
		 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FacilityName' THEN FacilityName END END DESC,

		 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ServiceDates' THEN CONVERT(datetime, ROM.MonthlySummaryStartDate, 103) END END ASC,                        
		 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ServiceDates' THEN CONVERT(datetime, ROM.MonthlySummaryStartDate, 103) END END DESC,

		 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN CONVERT(datetime, ROM.CreatedDate, 103) END END ASC,                        
		 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN CONVERT(datetime, ROM.CreatedDate, 103) END END DESC,

		 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedBy' THEN Emp.FirstName END END ASC,                        
		 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedBy' THEN Emp.FirstName END END DESC,

		 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'TreatmentPlan' THEN TreatmentPlan END END ASC,                        
		 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'TreatmentPlan' THEN TreatmentPlan END END DESC,

        CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClientName' THEN R.LastName END END ASC,                        
		 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClientName' THEN R.LastName END END DESC,

		 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Region' THEN RG.RegionName END END ASC,                        
		 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Region' THEN RG.RegionName END END DESC
		     		 
          
    ) AS ROW,     
		  ROM.ReferralMonthlySummariesID, ROM.CreatedDate,Emp.FirstName + ' ' + Emp.LastName as CreatedName,ROM.TreatmentPlan,
		  ROM.MonthlySummaryStartDate,ROM.MonthlySummaryEndDate, F.FacilityName, ROM.ReferralID, R.LastName+', '+R.FirstName AS ClientName,RG.RegionName,ROM.IsDeleted
			 FROM ReferralMonthlySummaries ROM
			 INNER JOIN Referrals R ON R.ReferralID=ROM.ReferralID
		     INNER JOIN Employees Emp on Emp.EmployeeID=Rom.CreatedBy
			 LEFT JOIN Facilities F on F.FacilityID=Rom.FacilityID
			 LEFT JOIN Regions RG on RG.RegionID=F.RegionID
 			where  	 ((CAST(@IsDeleted AS BIGINT)=-1) OR ROM.IsDeleted=@IsDeleted)
			AND (@ReferralID=0 OR ROM.ReferralID=@ReferralID)
			AND ( (@StartDate IS NULL) OR ( ROM.MonthlySummaryStartDate >=@StartDate)) AND ((@EndDate IS NULL) OR ( ROM.MonthlySummaryEndDate <=@EndDate))
			AND (@FacilityID=0 OR ROM.FacilityID=@FacilityID)
			AND (@RegionID=0 OR RG.RegionID=@RegionID)
			AND (@CreatedBy=0 OR ROM.CreatedBy=@CreatedBy)
			AND ((@ClientName IS NULL)      	   
			OR          
			   (
					(r.FirstName LIKE '%'+@ClientName+'%' )OR            
					(r.LastName  LIKE '%'+@ClientName+'%') OR            
					(r.FirstName +' '+r.LastName like '%'+@ClientName+'%') OR            
					(r.LastName +' '+r.FirstName like '%'+@ClientName+'%') OR            
					(r.FirstName +', '+r.LastName like '%'+@ClientName+'%') OR            
					(r.LastName +', '+r.FirstName like '%'+@ClientName+'%'))          
			   )  
			
	         
   ) AS T1        
 )        
 SELECT * FROM CTEReferralMonthlySummaries WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)END
 
 --EXEC GetReferralMonthlySummaryList @ReferralID = '2892',@SortExpression = '', @SortType = '', @FromIndex = '1', @PageSize = '10'