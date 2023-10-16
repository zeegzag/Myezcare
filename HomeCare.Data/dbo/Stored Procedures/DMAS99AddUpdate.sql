--CreatedBy: Abhishek Gautam  
--CreatedDate: 10 sept 2020  
--Description: Add/Update DMAS99 form  
  
  
--  EXEC [DMAS99AddUpdate] 1           
CREATE PROCEDURE [dbo].[DMAS99AddUpdate]            
 -- Add the parameters for the stored procedure here            
 @Dmas99ID bigint = 0,            
 @JsonData nvarchar(MAX) = '',            
 @ReferralID BIGINT = 0,          
 @EmployeeID BIGINT= 0,          
 @IsDeleted BIT = 0          
           
AS            
BEGIN            
            
IF(@Dmas99ID=0)            
 BEGIN          
  Insert into DMAS99 (JsonData,EmployeeID,ReferralID,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy) values (@JsonData,0,@ReferralID,@IsDeleted,GETDATE(),1,GETDATE(),1)            
  --SELECT @Dmas97ID = SCOPE_IDENTITY()          
          
   SELECT 1; RETURN;          
 END          
ELSE            
 BEGIN          
  UPDATE DMAS99 SET          
  JsonData=@JsonData,          
  EmployeeID=@EmployeeID,          
  ReferralID=@ReferralID,          
  IsDeleted=@IsDeleted,          
  UpdatedDate=GETDATE(),          
  UpdatedBy=1            
  where Dmas99ID = @Dmas99ID;          
          
   SELECT 1; RETURN;          
 END          
END