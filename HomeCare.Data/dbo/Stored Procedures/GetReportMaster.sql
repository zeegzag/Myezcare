--CreatedBy                     UpdatedDate                   Description            
--Vishwas                      29/feb/2020               For get reportmaster list           
-- exec [GetCareType] '4296',''            
CREATE PROCEDURE [dbo].[GetReportMaster]           
 @loggedInUser BIGINT =0               
AS                 
BEGIN                
          
--select * from ReportMaster where IsDeleted=0
select rm.ReportID,ReportName,SqlString,rm.ReportDescription,rm.DataSet,rm.RDL_FileName,rm.IsDeleted,rm.IsActive,rm.Category,rm.IsDisplay --rm.ReportID,rm.ReportName,rpm.ReportPermissionMappingID,rpm.RoleID,r.RoleName 
from ReportPermissionMapping rpm 
inner join Admin_Myezcare_Live.dbo.ReportMaster rm on rm.ReportID=rpm.ReportID
inner join Roles r on r.RoleID=rpm.RoleID
inner join employees e on e.RoleID=rpm.RoleID
where rpm.IsDeleted=0 and rm.IsDeleted=0 and rm.IsActive=1 and IsDisplay=1  and e.EmployeeID=@loggedInUser
                
END 