CREATE PROCEDURE [dbo].[HC_SetScheduleAssignmentModel01]            
@Preference_Skill VARCHAR(50),    
@Preference_Preference VARCHAR(50)    
AS            
BEGIN            
     
 SELECT * FROM Preferences WHERE IsDeleted=0 AND KeyType=@Preference_Preference    
 SELECT * FROM Preferences WHERE IsDeleted=0 AND KeyType=@Preference_Skill    
 select FrequencyCodeID,Code from FrequencyCodes where UsedInHomeCare=1 order by Code ASC                 
 SELECT * from ScheduleStatuses order by ScheduleStatusName ASC                
 SELECT EmployeeID,FirstName,LastName,MobileNumber FROM Employees E WHERE IsDeleted=0 AND IsActive=1 ORDER BY  E.FirstName ASC  
   select dds.DDMasterID as ServiceTypeID,dds.Title as ServiceTypeName from DDMaster dds      
  inner join lu_DDMasterTypes as luddm on dds.ItemType=luddm.DDMasterTypeID where dds.IsDeleted=0 and luddm.Name='Service Type'           
END