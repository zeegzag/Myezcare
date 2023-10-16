
--Exec API_GetPCATask @ScheduleID=132638,@ReferralID=24234, @VisitTaskType='Task'                           
CREATE PROCEDURE [dbo].[API_GetPCATask]                      
 @ScheduleID BIGINT,                      
 @ReferralID BIGINT,                                  
 @VisitTaskType NVARCHAR(10)                                  
AS                                              
BEGIN                          
 DECLARE @TotalServiceTime INT;                  
 DECLARE @TotalTime INT;                  
 DECLARE @EmployeeVisitID BIGINT = (SELECT EmployeeVisitID FROM EmployeeVisits WHERE ScheduleID=@ScheduleID)                  
                  
 SELECT @TotalServiceTime=(SUM(ServiceTime)) FROM EmployeeVisitNotes WHERE EmployeeVisitID=@EmployeeVisitID AND IsDeleted=0                  
 SELECT @TotalTime=DATEDIFF(MINUTE, StartDate, EndDate) FROM ScheduleMasters WHERE ScheduleID=@ScheduleID                  
                   
 --TaskList                                             
 EXEC API_GetTaskList @ScheduleID,@ReferralID,@VisitTaskType                  
                  
 --ServiceCodeList                  
 SELECT DISTINCT PSC.ServiceCodeID,ServiceName,Description,IsBillable,        
 --CareTypeTitle= CASE WHEN DM.Title IS NOT NULL THEN DM.Title ELSE '' END,        
 ServiceCode = SC.ServiceCode +                        
    CASE WHEN (SC.ModifierID IS NULL OR SC.ModifierID ='') THEN '' ELSE ' -'+                              
    STUFF(                                  
    (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                                  
    FROM Modifiers M  where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))                                  
    FOR XML PATH ('')), 1, 1, '')                              
    END                                
 FROM PayorServiceCodeMapping PSC                  
 INNER JOIN ServiceCodes SC ON SC.ServiceCodeID=PSC.ServiceCodeID                  
 --LEFT JOIN DDMaster DM on DM.DDMasterID = SC.CareType                            
 LEFT JOIN Modifiers M ON M.ModifierID=SC.ModifierID AND M.IsDeleted=0                  
 WHERE PayorID = (SELECT PayorID FROM ScheduleMasters WHERE ScheduleID=@ScheduleID)        
        
 --Care Type List        
 SELECT DISTINCT CareTypeID=DM.DDMasterID,CareType=DM.Title        
 FROM PayorServiceCodeMapping PSC                  
 INNER JOIN DDMaster DM on DM.DDMasterID = PSC.CareType AND PSC.IsDeleted=0        
 WHERE PayorID = (SELECT PayorID FROM ScheduleMasters WHERE ScheduleID=@ScheduleID)        
                  
 --OtherTask                 
 SELECT OtherActivity=EVN.Description,OtherActivityTime=ServiceTime,EmployeeVisitID,EVN.ServiceCodeID,            
 ServiceName,SC.Description,IsBillable,CareTypeID=DM.DDMasterID,CareType=DM.Title,      
 ServiceCode = SC.ServiceCode +                        
    CASE WHEN (SC.ModifierID IS NULL OR SC.ModifierID ='') THEN '' ELSE ' -'+                              
    STUFF(                                  
    (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                                  
    FROM Modifiers M  where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))                                  
    FOR XML PATH ('')), 1, 1, '')                              
    END                                
 FROM EmployeeVisitNotes EVN              
 LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID=EVN.ServiceCodeID      
 LEFT JOIN DDMaster DM on DM.DDMasterID = EVN.CareTypeID      
 LEFT JOIN Modifiers M ON M.ModifierID=SC.ModifierID AND M.IsDeleted=0                  
 WHERE EVN.EmployeeVisitID=@EmployeeVisitID AND EVN.ServiceTime>0 AND EVN.ReferralTaskMappingID IS NULL                  
                 
 --RemainingTime  
 DECLARE @RemainingTime INT =(@TotalTime-@TotalServiceTime)  
 SELECT ISNULL(@RemainingTime,0) AS RemainingTime  
         
END       
    
