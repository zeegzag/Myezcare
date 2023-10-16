
CREATE PROCEDURE [adc].[HC_DayCare_SetScheduleAttendenceModel]  
AS            
BEGIN            
     
 SELECT FacilityID,FacilityName FROM Facilities F WHERE IsDeleted=0  ORDER BY  F.FacilityName ASC          
    
END