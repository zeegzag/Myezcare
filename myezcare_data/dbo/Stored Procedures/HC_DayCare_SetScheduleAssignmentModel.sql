CREATE PROCEDURE [dbo].[HC_DayCare_SetScheduleAssignmentModel]
--@Preference_Skill VARCHAR(50),
--@Preference_Preference VARCHAR(50)
AS        
BEGIN        
 
 SELECT FacilityID,FacilityName FROM Facilities F WHERE IsDeleted=0  ORDER BY  F.FacilityName ASC      

END
