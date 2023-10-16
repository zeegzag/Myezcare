  
--    EXEC rpt.BulkTimesheet_Conclusion '60247,50251','DEVHomecare'  
CREATE PROCEDURE [rpt].[BulkTimesheet_Conclusion]   
@EmployeeVisitID varchar(MAX)='0',             
@dbname varchar(50) = NULL    
AS                                            
BEGIN            
       
 DECLARE @DateFormat VARCHAR(MAX)= dbo.GetOrgDateTimeFormat()   
   
  SELECT  V.VisitTaskDetail,  
  RM.ReferralTaskMappingID,EVN.Description as Answer, EVN.AlertComment   ,ev.EmployeeVisitID ,evn.EmployeeVisitNoteID  
 FROM EmployeeVisits EV                
 INNER JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID                
 inner JOIN ReferralTaskMappings RM on RM.ReferralTaskMappingID=EVN.ReferralTaskMappingID                
 INNER JOIN VisitTasks V on V.VisitTaskID=RM.VisitTaskID                
                   
 WHERE --(evn.EmployeeVisitID = @EmployeeVisitID)  
 (@EmployeeVisitID = '0' OR  TRY_CAst(evn.EmployeeVisitID AS varchar(MAX)) in (select Item from dbo.SplitString(@EmployeeVisitID, ',')))   
 AND v.VisitTaskType='Conclusion'       
 AND EVN.Description IS NOT NULL  and evn.IsDeleted=0  
                 
                  
END 