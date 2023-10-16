CREATE PROCEDURE [dbo].[FillGroupMonthlySummaryModel]      
@LoggedInUserID bigint      
AS      
BEGIN      	
	--Facility List For Dropdown
	select F.FacilityID Value,F.FacilityName Name from Facilities F where (F.IsDeleted=0)  order by FacilityName ASC;        --AND ParentFacilityID=0)
	select F.FacilityID Value,F.FacilityName Name from Facilities F  order by FacilityName ASC;        --AND ParentFacilityID=0)
	--Payor List For Dropdown
	select PayorID as Value,ShortName As Name from Payors where IsDeleted=0 order by ShortName ASC

	Select SignatureBy=E.LastName+' ' +E.FirstName, SignaturePath=ES.SignaturePath from Employees E
	INNER JOIN EmployeeSignatures ES ON E.EmployeeSignatureID=ES.EmployeeSignatureID WHERE E.EmployeeID=@LoggedInUserID
END
