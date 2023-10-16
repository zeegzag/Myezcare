                    
CREATE PROCEDURE [dbo].[SaveTransportAssignPatient]      
(    
@TransportAssignPatientID BIGINT = 0,                    
@ReferralID BIGINT = 0,                    
@TransportID BIGINT = 0,                    
@StartDate DateTime = NULL,                     
@EndDate DateTime = NULL,                     
@Note VARCHAR(255) = NULL,                    
@OrganizationID BIGINT = 0,                     
@loggedInUserID BIGINT = 0 ,  
@IsBillable BIT = 0  
)    
AS                      
 IF(@TransportAssignPatientID=0)                     
BEGIN                    
                    
INSERT INTO TransportAssignPatient     
(    
 ReferralID    ,    
 TransportID    ,    
 Startdate    ,    
 EndDate     ,    
 Note     ,    
 CreatedDate    ,    
 CreatedBy    ,    
 UpdatedDate    ,    
 UpdatedBy  ,  
 IsBillable  
)                     
VALUES     
(    
 @ReferralID   ,    
 @TransportID  ,    
 @Startdate   ,    
 @EndDate   ,    
 @Note    ,    
 getdate()   ,    
 @loggedInUserID  ,    
 getdate()   ,    
 @loggedInUserID  ,  
 @IsBillable  
);                    
                     
 SELECT 1; RETURN;                     
END                    
                    
 ELSE                                                  
 BEGIN                    
                    
    UPDATE TransportAssignPatient                                                   
   SET                      
   ReferralID = @ReferralID   ,    
   TransportID = @TransportID  ,    
   Startdate = @Startdate   ,    
   EndDate  = @EndDate   ,    
   Note   = @Note    ,       
   UpdatedDate = getdate()   ,    
   UpdatedBy = @loggedInUserID  ,  
   IsBillable = @IsBillable  
    WHERE TransportAssignPatientID=@TransportAssignPatientID;                     
                    
 SELECT 1; RETURN;                     
 END 