--SELECT * FROM dbo.CommonNotes  
--updatedBy      UpdatedDate      Description     --vikas          06-june-2019               change from EmployeeID to EmployeesID in where condition      --vikas    17-july-2019              alter proc for getting comma seprated value from Employee id   -- exec GetCommonNotes '4296',''     --EXEC GetCommonNotes @ReferralID = 4012, @EmployeeID = '621', @EmployeesID = 622            
CREATE PROCEDURE [dbo].[GetCommonNotes]     
	@LoggedInUser nvarchar(200),          
	@ReferralID bigint=null,          
	@EmployeeID Nvarchar(50)=null         
	AS          
	BEGIN          
		IF @ReferralID = 0           
			SET @ReferralID = NULL          
		IF @EmployeeID = 0           
			SET @EmployeeID = NULL                   
		IF (@ReferralID IS NULL)  
		BEGIN   
			Select c.*,EmployeesID,dbo.GetGeneralNameFormat(e.FirstName,e.LastName) AS CreatedByName          
			FROM CommonNotes c          
			INNER JOIN Employees e ON e.EmployeeID=C.CreatedBy          
			WHERE (@LoggedInUser IS NOT NULL AND ','+c.EmployeesID+',' LIKE '%,'+@LoggedInUser+',%')  AND c.IsDeleted=0           
			ORDER BY c.CreatedDate DESC            
		END 
		ELSE  
		BEGIN  
			Select c.*,EmployeesID,dbo.GetGeneralNameFormat(e.FirstName,e.LastName) AS CreatedByName          
			FROM CommonNotes c          
			INNER JOIN Employees e ON e.EmployeeID=C.CreatedBy          
			WHERE ((@ReferralID IS NOT NULL AND c.ReferralID=@ReferralID) and (@LoggedInUser IS NOT NULL 
			--AND ','+c.EmployeesID+',' LIKE '%,'+@LoggedInUser+',%'))  
			AND c.IsDeleted=0))          
			ORDER BY c.CreatedDate DESC  
        END          
	END 