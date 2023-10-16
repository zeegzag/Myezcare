CREATE PROCEDURE [dbo].[SetEmployeeVisitListPage]              
AS                
BEGIN                                  
 Select EmployeeID, EmployeeName=dbo.GetGeneralNameFormat(FirstName,LastName) From Employees Where IsDeleted=0 ORDER BY LastName ASC                
                
 Select ReferralID, ReferralName=dbo.GetGeneralNameFormat(FirstName,LastName)  From Referrals Where IsDeleted=0 ORDER BY LastName ASC             
             
 select p.PayorID,p.PayorName from  Payors as p Where p.IsDeleted=0 ORDER BY p.PayorName ASC              
            
   select dds.DDMasterID as CareTypeID,dds.Title As CareType  from DDMaster dds          
  inner join lu_DDMasterTypes as luddm on dds.ItemType=luddm.DDMasterTypeID where dds.IsDeleted=0 and luddm.Name='Care Type'                 
        
        
   select dds.DDMasterID as ServiceTypeID,dds.Title As ServiceTypeName  from DDMaster dds          
  inner join lu_DDMasterTypes as luddm on dds.ItemType=luddm.DDMasterTypeID where dds.IsDeleted=0 and luddm.Name='Service Type'   
    
     SELECT r.ReferralID,STRING_AGG(CONCAT('', '', p.PayorID), ',')PayorID ,STRING_AGG(CONCAT('', '', p.PayorName), ',')PayorName from Referrals r   
     inner join [ReferralPayorMappings] as rpm  on rpm.ReferralID=r.ReferralID  
     inner join Payors p on p.PayorID=rpm.PayorID  
     group  by r.ReferralID        
            
END