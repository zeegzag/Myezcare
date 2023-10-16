 create procedure DeleteAllergy  
 (  
 @id bigint  
  
 )  
  
 as  
 begin    
 if @id>0  
 begin  
      update Allergy set IsDeleted=1 where id=@id  
 end  
 end