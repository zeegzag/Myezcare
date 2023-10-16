CREATE PROCEDURE [dbo].[HC_ClockInOut]                                                      
@EmployeeID BIGINT = 0                                             
                                                       
AS                                                        
BEGIN     
    
DECLARE @currentDateTime DATETIME = (SELECT CAST((SWITCHOFFSET(GETUTCDATE(), (SELECT current_utc_offset FROM sys.time_zone_info WHERE name=(    
SELECT TimeZone FROM OrganizationSettings)))) AS DATETIME))    
                   
 SELECT FacilityID,FacilityName  FROM [dbo].[Facilities] (nolock) WHERE IsDeleted=0;                               
                      
 SELECT cast(dm.[Value] as bigint) as AttendanceDetailId  ,dm.Title AS AttendanceDetailName                         
 FROM [dbo].ddmaster dm   (nolock)                       
 INNER JOIN [dbo].lu_DDMasterTypes lu (nolock) on lu.DDMasterTypeID = dm.ItemType                          
 WHERE lu.Name='Attendance Detail'   and dm.IsDeleted=0                        
                      
 SELECT cast(dm.[Value] as bigint) as AttendanceBreakDetailId  ,dm.Title AS AttendanceBreakDetailName                         
 FROM [dbo].ddmaster dm   (nolock)                       
 INNER JOIN [dbo].lu_DDMasterTypes lu (nolock) on lu.DDMasterTypeID = dm.ItemType                          
 WHERE lu.Name='Attendance Break Detail'   and dm.IsDeleted=0          
                 
 SELECT * FROM EmployeeAttendanceMaster (NOLOCK) WHERE EmployeeID = @EmployeeID        
 and cast(CreatedDate as date) = cast(@currentDateTime as date)        
        
 SELECT * FROM EmployeeAttendanceDetail (NOLOCK)         
 WHERE         
 AttendanceMasterId IN (SELECT Id FROM EmployeeAttendanceMaster (NOLOCK) WHERE EmployeeID = @EmployeeID and cast(CreatedDate as date) = cast(@currentDateTime as date))        
 order by Id asc       
    
SELECT TimeZone FROM OrganizationSettings    
SELECT @currentDateTime AS CurrentTimeZoneDate    
    
END 