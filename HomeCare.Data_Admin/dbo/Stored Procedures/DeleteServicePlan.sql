-- [DeleteServicePlan] '',0,0,0,'ServicePlanID','ASC',1,100,'4',1,1
CREATE PROCEDURE [dbo].[DeleteServicePlan]         
@ServicePlanName NVARCHAR(200)=NULL,  
@PerPatientPrice INT=0,
@NumberOfDaysForBilling INT=0,
@IsDeleted BIGINT = 0, 
@SortExpression NVARCHAR(100),                          
@SortType NVARCHAR(10),                        
@FromIndex INT,                        
@PageSize INT,
@ListOfIdsInCSV NVARCHAR(300),
@IsShowList bit,  
@LoggedInID BIGINT
AS                      
BEGIN
	IF(LEN(@ListOfIdsInCSV)>0)  
	BEGIN
		UPDATE ServicePlans
		SET
			IsDeleted = CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END,
			UpdatedDate = GETDATE(),
			UpdatedBy = @LoggedInID
		WHERE
			ServicePlanID IN (SELECT CAST(Val AS BIGINT) FROM GetCsvTable(@ListOfIdsInCSV))  
	END
	
	IF(@IsShowList=1)
	BEGIN
		EXEC GetServicePlanList @ServicePlanName, @PerPatientPrice, @NumberOfDaysForBilling, @IsDeleted, @SortExpression, @SortType, @FromIndex, @PageSize
	END
END