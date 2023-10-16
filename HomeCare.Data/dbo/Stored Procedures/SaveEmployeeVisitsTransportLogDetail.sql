    
 --EmployeeVisitsTransportLogDetail    
 CREATE PROCEDURE [dbo].[SaveEmployeeVisitsTransportLogDetail]                  
(                
@Id        bigint  = 0,    
@EmployeeVisitsTransportLogId bigint  = null,    
@ReferralID      bigint  = null,    
@ClockInTime     datetime = null,    
@ClockOutTime     datetime = null,    
@Latitude      float  = null,    
@Longitude      float  = null      
)                
AS                                  
 IF(@Id=0)                                 
BEGIN                                
                                
  INSERT INTO EmployeeVisitsTransportLogDetail                
  (                
    EmployeeVisitsTransportLogId ,    
 ReferralID      ,    
 ClockInTime      ,    
 ClockOutTime     ,    
 Latitude      ,    
 Longitude          
  )                                 
  VALUES                 
  (                
 @EmployeeVisitsTransportLogId ,    
 @ReferralID      ,    
 @ClockInTime     ,    
 @ClockOutTime     ,    
 @Latitude      ,    
 @Longitude          
  );                                
                                   
   SELECT 1; RETURN;                                 
END            
ELSE                                                              
BEGIN                                                    
 UPDATE EmployeeVisitsTransportLogDetail                                                               
 SET                                  
  EmployeeVisitsTransportLogId = IsNull(@EmployeeVisitsTransportLogId  , EmployeeVisitsTransportLogId),     
  ReferralID     = IsNull(@ReferralID      , ReferralID     ),    
  ClockInTime     = IsNull(@ClockInTime      , ClockInTime     ),    
  ClockOutTime     = IsNull(@ClockOutTime      , ClockOutTime    ),    
  Latitude      = IsNull(@Latitude       , Latitude     ),    
  Longitude          = IsNull(@Longitude           , Longitude     )    
    
 WHERE             
  Id=@Id;                                 
                                
 SELECT 1; RETURN;                                 
 END 