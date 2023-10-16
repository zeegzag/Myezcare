CREATE PROCEDURE [dbo].[SaveEmployeeAttendanceMaster]                            
(                          
@Id    int = 0,          
@EmployeeID  bigint=0,          
@WorkMinutes int=0,          
@FacilityID  bigint=0,          
@OrganizationID bigint=0,          
@Note   nvarchar(max)=null         
                      
)                          
AS      
      
DECLARE @currentDateTime DATETIME = (SELECT CAST((SWITCHOFFSET(GETUTCDATE(), (SELECT current_utc_offset FROM sys.time_zone_info WHERE name=(      
SELECT TimeZone FROM OrganizationSettings)))) AS DATETIME))      
      
 IF(@Id=0)                                           
BEGIN                                          
                                          
  INSERT INTO [EmployeeAttendanceMaster]           
  (                          
 EmployeeID          
,WorkMinutes          
,FacilityID          
,OrganizationID          
,Note          
,CreatedDate          
,UpdatedDate           
  )                                           
  VALUES                           
  (                          
   @EmployeeID            
 , @WorkMinutes           
 , @FacilityID            
 , @OrganizationID           
 , @Note             
 , @currentDateTime                
 , @currentDateTime                
  );                                          
                                             
   SELECT 1; RETURN;                                           
END                      
ELSE                                                                        
BEGIN                                                              
 UPDATE [EmployeeAttendanceMaster]          
 SET                                            
  EmployeeID    = IsNull(@EmployeeID     , EmployeeID     ),               
  WorkMinutes  = IsNull(WorkMinutes,0) + IsNull(@WorkMinutes  , 0),              
  FacilityID = IsNull(@FacilityID , FacilityID),              
  OrganizationID     = IsNull(@OrganizationID , OrganizationID ),              
  Note     = IsNull(@Note , Note      ),              
  UpdatedDate     = @currentDateTime               
 WHERE                       
  Id=@Id;                                           
                                          
 SELECT 1; RETURN;                                           
 END 