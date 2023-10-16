CREATE PROCEDURE [dbo].[API_GetFacilityRAL]    
 @EmployeeID BIGINT                  
AS                  
BEGIN                                              
SELECT fa.FacilityID,fa.FacilityName,fm.EmployeeID
FROM  RAL_FacilityMapping fm             
 LEFT JOIN Facilities  fa ON fm.FacilityID = fa.FacilityID                  
 WHERE fm.EmployeeID=@EmployeeID                  
END