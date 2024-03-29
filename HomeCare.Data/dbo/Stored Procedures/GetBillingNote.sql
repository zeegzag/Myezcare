CREATE PROCEDURE [dbo].[GetBillingNote]    
  @BatchID bigint=0  
  
AS    
BEGIN    
select bn.BillingNoteID,bn.BatchID,bn.BillingNote,bn.CreatedDate,bn.CreatedBy,bn.UpdatedDate,bn.UpdatedBy,bn.IsDeleted,e.FirstName,e.LastName From BillingNote bn  
inner join employees e on e.EmployeeID=bn.UpdatedBy  
where bn.BatchID=@BatchID AND bn.IsDeleted=0  
ORDER BY BillingNoteID DESC  
END 