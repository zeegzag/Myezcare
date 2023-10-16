
  --     EXEC SaveAllergy @Id=0, @Allergy = 'iyopi', @Status = '1', @Reaction = 'iopp', @Comment = 'khk;', @CreatedBy = '1', @Patient = '60053', @ReportedBy = '60136'
  CREATE procedure [dbo].[SaveAllergy]  
  (  
  @Id bigint=0,
 @Allergy nvarchar(max),  
 @Status nvarchar(max),  
 @Reaction nvarchar(max),  
 @Comment nvarchar(max),  
 @CreatedBy bigint,  
 @Patient bigint,  
 @ReportedBy nvarchar(max)  
  )  
  as  
  
  begin  
  if(@Id>0)
  begin
       update  Allergy set
	  Allergy=  @Allergy ,
	  Status=@Status,
	  Reaction=@Reaction,
	  Comment=@Comment,
	  Patient=@Patient,
	  CreatedBy=@CreatedBy,
	  ReportedBy=@ReportedBy
	  where Id=@Id
  
  end
  else
    begin  
	  insert into Allergy(Allergy,Status,Reaction,Comment,CreatedBy,Patient,CreatedOn,ReportedBy,IsDeleted)  
     values(@Allergy,@Status,@Reaction,@Comment,@CreatedBy,@Patient,getdate(),@ReportedBy,0) 
    end  
  end