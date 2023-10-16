CREATE PROCEDURE [dbo].[SaveEmployeeAttendanceDetail]                            
(                          
@Id      int = 0,          
@AttendanceMasterId  int = null,          
@Type     int = null,          
@TypeDetail    int = null      ,    
@Note nvarchar(max)=null    
    
)                          
AS      
      
DECLARE @currentDateTime DATETIME = (SELECT CAST((SWITCHOFFSET(GETUTCDATE(), (SELECT current_utc_offset FROM sys.time_zone_info WHERE name=(      
SELECT TimeZone FROM OrganizationSettings)))) AS DATETIME))      
 IF(@Id=0)                                           
BEGIN                                          
      
  INSERT INTO [EmployeeAttendanceDetail]           
  (                          
 AttendanceMasterId          
,[Type]          
,TypeDetail          
,CreatedDate          
,UpdatedDate       
,Note    
  )                                           
  VALUES                           
  (                          
   @AttendanceMasterId          
 , @Type          
 , @TypeDetail          
 , @currentDateTime                
 , @currentDateTime     
 ,@Note    
  );                                          
                                             
   SELECT 1; RETURN;                                           
END                      
ELSE                                                                        
BEGIN                                                              
 UPDATE [EmployeeAttendanceDetail]          
 SET                                            
  [Type]    = IsNull(@Type , [Type]     ),               
  TypeDetail  = IsNull(@TypeDetail  , TypeDetail),          
  UpdatedDate     = @currentDateTime           ,    
  Note = IsNull (@Note,Note)    
 WHERE                       
  Id=@Id;                                           
                                          
 SELECT 1; RETURN;                                           
 END 