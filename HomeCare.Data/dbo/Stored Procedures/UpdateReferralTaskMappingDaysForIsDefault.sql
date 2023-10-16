CREATE PROCEDURE [dbo].[UpdateReferralTaskMappingDaysForIsDefault]  
  
@ReferralID int,    
@VisitTaskType VARCHAR(100),    
@CareType VARCHAR(100)    
AS  
BEGIN  
IF EXISTS (SELECT  rtm.VisitTaskID FROM ReferralTaskMappings rtm  
inner join VisitTasks vt on vt.VisitTaskID=rtm.VisitTaskID  
WHERE ReferralID=@ReferralID and rtm.IsDeleted=0 and vt.IsDefault=1 )  
   BEGIN                              
print '-1' ;                              
   END  
else  
     begin  
insert into ReferralTaskMappings ([VisitTaskID], [IsRequired], [IsDeleted], [ReferralID], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [Frequency], [Comment], [Days])  
  
     select  
   vt.VisitTaskID,    
   0,  
   0,  
   @ReferralID,  
   GETUTCDATE(),  
   1,  
   GETUTCDATE(),  
   null,  
   1,  
   0,  
   null,  
   '1,2,3,4,5,6,7'  
  from visittasks vt   
  where vt.IsDeleted=0 and vt.VisitTaskType = @VisitTaskType  and vt.IsDefault=1  
    end  
end  