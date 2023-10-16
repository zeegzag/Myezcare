-- [GetServicePlanList] '',0,0,0,'ServicePlanID','ASC',1,100  
CREATE PROCEDURE [dbo].[GetServicePlanList]         
@ServicePlanName NVARCHAR(200)=NULL,  
@PerPatientPrice INT=0,
@NumberOfDaysForBilling INT=0,
@IsDeleted BIGINT = 0, 
@SortExpression NVARCHAR(100),                          
@SortType NVARCHAR(10),                        
@FromIndex INT,                        
@PageSize INT     
AS                      
BEGIN
	;WITH CTEServicePlanList AS
	(
	SELECT *,COUNT(T1.ServicePlanID) OVER() AS Count FROM                             
	(                            
		SELECT ROW_NUMBER() OVER 
		(
			ORDER BY                   
			CASE WHEN @SortType = 'ASC' THEN                            
				CASE                                   
					WHEN @SortExpression = 'ServicePlanName' THEN ServicePlanName
				END                             
			END ASC,                            
			CASE WHEN @SortType = 'DESC' THEN                            
				CASE                                   
					WHEN @SortExpression = 'ServicePlanName' THEN ServicePlanName
				END                            
			END DESC,
			CASE WHEN @SortType = 'ASC' THEN                            
				CASE                                   
					WHEN @SortExpression = 'PerPatientPrice' THEN PerPatientPrice
					WHEN @SortExpression = 'NumberOfDaysForBilling' THEN NumberOfDaysForBilling
					WHEN @SortExpression = 'SetupFees' THEN SetupFees
					WHEN @SortExpression = 'Patient' THEN Patient
					WHEN @SortExpression = 'Facility' THEN Facility
					WHEN @SortExpression = 'Task' THEN Task
					WHEN @SortExpression = 'Employee' THEN Employee
					WHEN @SortExpression = 'Billing' THEN Billing
				END                             
			END ASC,                            
			CASE WHEN @SortType = 'DESC' THEN                            
				CASE                                   
					WHEN @SortExpression = 'PerPatientPrice' THEN PerPatientPrice
					WHEN @SortExpression = 'NumberOfDaysForBilling' THEN NumberOfDaysForBilling
					WHEN @SortExpression = 'SetupFees' THEN SetupFees
					WHEN @SortExpression = 'Patient' THEN Patient
					WHEN @SortExpression = 'Facility' THEN Facility
					WHEN @SortExpression = 'Task' THEN Task
					WHEN @SortExpression = 'Employee' THEN Employee
					WHEN @SortExpression = 'Billing' THEN Billing
				END                            
			END DESC
		) AS Row,
		*
		FROM
		(
			SELECT
				SP.*,
				SPR.ModuleName,
				SPR.MaximumAllowedNumber
			FROM
				ServicePlans SP
				INNER JOIN ServicePlanRates SPR ON SP.ServicePlanID = SPR.ServicePlanID				
			WHERE
				((CAST(@IsDeleted AS BIGINT)=-1) OR SP.IsDeleted=@IsDeleted)
				AND ((@ServicePlanName IS NULL OR LEN(@ServicePlanName)=0) OR SP.ServicePlanName LIKE '%' + @ServicePlanName + '%')
				AND (@PerPatientPrice=0 OR SP.PerPatientPrice = @PerPatientPrice)
				AND (@NumberOfDaysForBilling=0 OR SP.NumberOfDaysForBilling = @NumberOfDaysForBilling)
		) AS Temp
		PIVOT
		(
			MIN(MaximumAllowedNumber)
			FOR ModuleName IN ([Patient], [Facility], [Task], [Employee], [Billing])
		) AS Pvt
	) AS T1
	)
	                           
	SELECT * FROM CTEServicePlanList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                          
END