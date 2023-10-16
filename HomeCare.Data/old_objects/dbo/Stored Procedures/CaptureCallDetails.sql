-- exec [CaptureCallDetails] 3     
          
CREATE PROC [dbo].[CaptureCallDetails]                  
@CapturecallID BIGINT=0              
              
AS                        
 BEGIN                        
  select  cc.Id,cc.FirstName,cc.LastName,cc.Contact,cc.Email,cc.Flag,cc.Address,cc.City,cc.StateCode,cc.ZipCode,cc.RoleIds,cc.IsDeleted,cc.CreatedDate,  
  cc.Notes,cc.EmployeesIDs,cc.CallType,cc.RelatedWithPatient,cc.InquiryDate,cc.FileName  
,FilePath,OrbeonID,Status ,E.LastName+', '+E.FirstName as CreatedByName  
 FROM CaptureCall cc   
 inner join employees e on e.EmployeeID=cc.CreatedBy  
 Where Id=@CapturecallID              
  --SELECT * FROM CaptureCall cc Where Id=@CapturecallID            
  --  SELECT cc.Id,cc.FirstName,cc.LastName,cc.Contact,cc.Email,cc.Address,cc.City,cc.StateCode,cc.ZipCode,cc.RoleIds,cc.EmployeesIDs,cc.Notes,cc.CallType,        
  --cc.RelatedWithPatient,cc.InquiryDate,r.RoleName,e.FirstName,e.LastName        
  --FROM CaptureCall CC        
  --inner join Roles r on r.RoleID=CONVERT(bigint,cc.RoleIds)        
  --inner join Employees e on e.EmployeeID=CONVERT(bigint,cc.EmployeesIDs)                
                          
  SELECT * FROM States ORDER BY StateName ASC                 
   declare @ItemType int          
 select @ItemType=DDMasterTypeID from lu_DDMasterTypes where name ='Call Type'          
 select Title as Name,DDMasterID as Value from DDMaster where ItemType=@ItemType          
             
  select LastName+', '+FirstName as ReferralName,ReferralID from Referrals        
  select RoleID,RoleName from Roles      
      
    declare @lu_DDMasterTypes int          
 select @lu_DDMasterTypes=DDMasterTypeID from lu_DDMasterTypes where name ='CRM_Status'          
 select Title as Name,DDMasterID as Value from DDMaster where ItemType=@lu_DDMasterTypes         
       --EXEC GetGeneralMasterList1 @lu_DDMasterTypes  
 END 