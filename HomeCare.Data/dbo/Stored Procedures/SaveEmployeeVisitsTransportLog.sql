--EmployeeVisitsTransportLog            
CREATE PROCEDURE [dbo].[SaveEmployeeVisitsTransportLog]                  
(                
@Id       bigint = 0,    
@EmployeeID     bigint = NULL,    
@TransportGroupID   bigint = NULL,    
@TransportAssignPatientID bigint = NULL,    
@VisitDate     datetime = NULL,    
@Starttime     datetime = NULL,    
@Endtime     datetime = NULL,    
@IsDeleted     bit = NULL,      
@loggedInUserID    BIGINT = 0             
            
)                
AS                                  
 IF(@Id=0)                                 
BEGIN                                
                                
  INSERT INTO EmployeeVisitsTransportLog                
  (                
    EmployeeID     ,    
 TransportGroupID   ,    
 TransportAssignPatientID ,    
 VisitDate     ,    
 Starttime     ,    
 Endtime      ,    
 IsDeleted     ,    
 CreatedDate           
  )                                 
  VALUES                 
  (                
 @EmployeeID     ,    
 @TransportGroupID   ,    
 @TransportAssignPatientID ,    
 @VisitDate     ,    
 @Starttime     ,    
 @Endtime     ,    
 @IsDeleted     ,            
 GETDATE()           
  );                                
                                   
   SELECT 1; RETURN;                                 
END            
ELSE                                                              
BEGIN                                                    
 UPDATE EmployeeVisitsTransportLog                                                               
 SET                                  
  EmployeeID    = IsNull(@EmployeeID     , EmployeeID     ),     
  TransportGroupID   = IsNull(@TransportGroupID   , TransportGroupID    ),    
  TransportAssignPatientID = IsNull(@TransportAssignPatientID , TransportAssignPatientID  ),    
  VisitDate     = IsNull(@VisitDate     , VisitDate      ),    
  Starttime     = IsNull(@Starttime     , Starttime      ),    
  Endtime     = IsNull(@Endtime      , Endtime      ),    
  IsDeleted     = IsNull(@IsDeleted     , IsDeleted      )    
 WHERE             
  Id=@Id;                                 
                                
 SELECT 1; RETURN;                                 
 END 