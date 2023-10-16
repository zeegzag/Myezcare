
CREATE PROCEDURE [dbo].[DeleteDepartment]
	@DepartmentID BIGINT = 0,
	@EmployeeID BIGINT = 0,

	@Location VARCHAR(200) = NULL,
	@Address VARCHAR(500) = NULL,
	@SortExpression NVARCHAR(100),  
	@SortType NVARCHAR(10),
	@IsDeleted BIGINT =-1,
	@FromIndex INT,
	@PageSize INT,
	@ListOfIdsInCSV VARCHAR(300),
	@IsShowList bit,
	@loggedInID BIGINT	
AS
BEGIN    

	IF(LEN(@ListOfIdsInCSV)>0)
	BEGIN
			
		--IF EXISTS (SELECT * FROM Employees WHERE DepartmentID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV)))
		--BEGIN 
		--	SELECT NULL;
		--	RETURN NULL;
		--END
		--ELSE
		--BEGIN
			UPDATE Departments SET ISDELETED=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE()
			WHERE DepartmentID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))		  	
		--END
				
	END

	IF(@IsShowList=1)
	BEGIN
		EXEC GETDEPARTMENTLIST @DepartmentID, @EmployeeID, @Location, @Address,@IsDeleted, @SortExpression, @SortType, @FromIndex, @PageSize
	END
END