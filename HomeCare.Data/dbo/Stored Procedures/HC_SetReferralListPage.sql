CREATE PROCEDURE [dbo].[HC_SetReferralListPage]           
@DDType_PatientSystemStatus INT = 12,      
@DDType_LanguagePreference INT = 17        
AS          
BEGIN          
 SELECT PayorID,PayorName from Payors order by PayorName ASC          
          
 SELECT * FROM ReferralStatuses  WHERE UsedInHomeCare=1        
 --SELECT d.DDMasterID AS Value,d.Title AS Name        
 --FROM dbo.DDMaster d        
 --WHERE d.ItemType=@DDType_PatientSystemStatus AND IsDeleted=0        
          
 select EmployeeID,LastName+', '+FirstName as Name from Employees order by LastName ASC--where IsDeleted=0          
          
 select EmployeeID,LastName+', '+FirstName as EmployeeName,IsDeleted from Employees where IsDeleted=0 order by LastName ASC          
           
 -- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY          
 SELECT 0;          
          
 -- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY          
 SELECT 0;          
          
 -- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY          
 SELECT 0;          
          
 SELECT CaseManagerID,LastName+', '+FirstName as Name From CaseManagers order by LastName ASC --where IsDeleted=0          
          
 --SELECT  LanguageID,Name from Languages order by Name ASC      
 SELECT LanguageID=DDMasterID ,Name=Title FROM DDMaster WHERE ItemType =@DDType_LanguagePreference AND IsDeleted=0          
         
 SELECT  RegionID,RegionName from  Regions ORDER BY RegionName ASC;          
          
 -- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY          
 SELECT 0;          
          
 select AgencyID,NickName from Agencies order by NickName ASC          
          
 select AgencyLocationID,LocationName from AgencyLocations order by LocationName ASC          
          
 -- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY          
 SELECT 0;          
          
 -- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY          
 SELECT 0;          
          
 -- RETURN 0 FOR THE MODEL DUE TO THE MULTIPLE ENTITY          
 SELECT 0;     
     
      
  select dds.DDMasterID as ServiceTypeID,dds.Title as ServiceTypeName from DDMaster dds      
  inner join lu_DDMasterTypes as luddm on dds.ItemType=luddm.DDMasterTypeID where dds.IsDeleted=0 and luddm.Name='Service Type'          
END