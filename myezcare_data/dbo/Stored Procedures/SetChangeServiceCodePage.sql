CREATE PROCEDURE [dbo].[SetChangeServiceCodePage]      
@LoggedInUserID bigint      
AS      
BEGIN      
      
 SELECT F.FacilityID Value,F.FacilityName Name from Facilities F where (F.IsDeleted=0 AND ParentFacilityID=0) ORDER BY FacilityName ASC
 
 SELECT PayorID as Value,ShortName As Name from Payors where IsDeleted=0 order by ShortName ASC
 
 SELECT EmployeeID Value,LastName +', '+FirstName Name FROM Employees where IsActive=1 and IsDeleted=0 Order By LastName ASC     
 
 SELECT ServiceCodeID AS Value, S.ServiceCode + CASE WHEN M.ModifierCode IS NOT NULL THEN ' - ' + M.ModifierCode ELSE '' END +' : '+ServiceName AS Name FROM ServiceCodes S
 LEFT JOIN Modifiers M ON M.ModifierID=S.ModifierID  WHERE S.IsDeleted=0  Order By S.ServiceCode ASC     
 
 select * from PlaceOfServices Where IsDeleted=0
END

