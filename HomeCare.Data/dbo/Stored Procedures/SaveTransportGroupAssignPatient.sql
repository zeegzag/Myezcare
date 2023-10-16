----TransportGroup      
----TransportGroupAssignPatient      
CREATE PROCEDURE [dbo].[SaveTransportGroupAssignPatient]                  
@TransportGroupID bigint = 0,      
@ListOfIdsInCsv varchar(300),      
@loggedInID BIGINT                          
AS                          
BEGIN                       
 IF(LEN(@ListOfIdsInCsv)>0)                          
 BEGIN                 
 insert into [fleet].TransportGroupAssignPatient      
 (      
 TransportGroupID,      
 ReferralID,      
 CreatedDate,      
 CreatedBy,      
 UpdatedDate,      
 UpdatedBy      
 )      
 SELECT       
 @TransportGroupID,      
 ReferralIDs.val,       
 GETDATE(),      
 @loggedInID,      
 GETDATE(),      
 @loggedInID      
 FROM GetCSVTable(@ListOfIdsInCsv) ReferralIDs      
 INNER JOIN Referrals r (nolock)      
 on r.ReferralID = ReferralIDs.val      
 left join [fleet].TransportGroupAssignPatient TGAP (nolock)      
 on TGAP.ReferralID = ReferralIDs.val      
 and TGAP.TransportGroupID = @TransportGroupID       
 and IsNull(TGAP.IsDeleted,0) = 0    
 where       
 TGAP.TransportGroupAssignPatientID is null      
      
       
     SELECT 1; RETURN;                             
                              
 END                          
                          
    SELECT 0; RETURN;                             
END 