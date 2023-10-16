--CreatedBy: Abhishek Gautam  
--CreatedDate: 10 sept 2020  
--Description: Add/Update DMAS97AB form  
  
  
--  EXEC Dmas97AbAddUpdate           
CREATE PROCEDURE [dbo].[Dmas97AbAddUpdate]            
 -- Add the parameters for the stored procedure here            
 @Dmas97ID bigint = 0,            
 @JsonData nvarchar(MAX) = '',            
  @ReferralID BIGINT = 0,          
  @EmployeeID BIGINT= 0,          
  @IsDeleted BIT = 0          
           
AS            
BEGIN            
            
IF(@Dmas97ID=0)            
 BEGIN          
  Insert into Dmas97ab (JsonData,EmployeeID,ReferralID,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy) values (@JsonData,0,@ReferralID,@IsDeleted,GETDATE(),1,GETDATE(),1)            
  --SELECT @Dmas97ID = SCOPE_IDENTITY()          
          
   SELECT 1; RETURN;          
 END          
ELSE            
 BEGIN          
  UPDATE  Dmas97ab SET          
  JsonData=@JsonData,          
  EmployeeID=@EmployeeID,          
  ReferralID=@ReferralID,          
  IsDeleted=@IsDeleted,          
  UpdatedDate=GETDATE(),          
  UpdatedBy=1            
  where Dmas97ID = @Dmas97ID;          
          
   SELECT 1; RETURN;          
 END          
END