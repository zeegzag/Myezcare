
/****** Object:  StoredProcedure [dbo].[API_GetProfileDetail]    Script Date: 1/28/2020 1:01:15 AM By Satya for RAL******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[API_GetFacilityRAL]    
 @EmployeeID BIGINT                  
AS                  
BEGIN                                              
SELECT fa.FacilityID,fa.FacilityName,fm.EmployeeID
FROM  RAL_FacilityMapping fm             
 LEFT JOIN Facilities  fa ON fm.FacilityID = fa.FacilityID                  
 WHERE fm.EmployeeID=@EmployeeID                  
END