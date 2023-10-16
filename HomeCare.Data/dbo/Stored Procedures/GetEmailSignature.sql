CREATE procedure [dbo].[GetEmailSignature]  
(  
  @ReferralId bigint  
)  
as  
begin  
         
       select '<br/><br/><br/><br/><strong>'+e.Name +'</strong><br/>'+Description as [EmailSignature]   from  EmployeeEmailSignature e where e.EmployeeID=@ReferralId;  
  
end