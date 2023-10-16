
--  exec [rpt].[BulkTimesheet_Task] '60247,50251','DEVHomecare'

CREATE PROCEDURE [rpt].[BulkTimesheet_Task] 
@EmployeeVisitID varchar(MAX)='0',           
@dbname varchar(50) = NULL  
AS                                          
BEGIN          
     
  DECLARE @DateFormat VARCHAR(MAX);  
  SELECT @DateFormat=[Admin_Myezcare_Live].dbo.fn_getDateFormat(@dbname)

 SELECT EV.EmployeeVisitID, V.VisitTaskDetail,RM.ReferralTaskMappingID,V.MinimumTimeRequired,RM.IsRequired,                  
 VG.VisitTaskCategoryName as CategoryName,VG.VisitTaskCategoryID as CategoryId,                   
 VG1.VisitTaskCategoryName as SubCategoryName, VG1.VisitTaskCategoryID as SubCategoryId,EVN.ServiceTime, EVN.IsSimpleTask         
 FROM EmployeeVisits EV            
 INNER JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID          
 INNER JOIN ReferralTaskMappings RM on RM.ReferralTaskMappingID=EVN.ReferralTaskMappingID            
 INNER JOIN VisitTasks V on V.VisitTaskID=RM.VisitTaskID            
 LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=V.VisitTaskCategoryID                  
 LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=V.VisitTaskSubCategoryID                  
 WHERE  (@EmployeeVisitID = '0' OR  TRY_CAst(ev.EmployeeVisitID AS varchar(MAX)) in (select Item from dbo.SplitString(@EmployeeVisitID, ','))) 
 AND v.VisitTaskType='Task'            
             
                
END   