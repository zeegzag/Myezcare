--CreatedBy: Abhishek Gautam  
--CreatedDate: 25 sept 2020  
--Description: Add/Update of CMS485
  
  
--  EXEC CMS485AddUpdate               
CREATE PROCEDURE [dbo].[CMS485AddUpdate]                          
 @Cms485ID bigint = 0,                
 @JsonData nvarchar(MAX) = '',                
 @ReferralID BIGINT = 0,              
 @EmployeeID BIGINT= 0,              
 @IsDeleted BIT = 0              
               
AS                
BEGIN                
                
IF(@Cms485ID=0)                
 BEGIN              
  Insert into CMS485 (JsonData,EmployeeID,ReferralID,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy) values (@JsonData,0,@ReferralID,@IsDeleted,GETDATE(),1,GETDATE(),1)                        
              
   SELECT 1; RETURN;              
 END              
ELSE                
 BEGIN              
  UPDATE  CMS485 SET              
  JsonData=@JsonData,              
  EmployeeID=@EmployeeID,              
  ReferralID=@ReferralID,              
  IsDeleted=@IsDeleted,              
  UpdatedDate=GETDATE(),              
  UpdatedBy=1                
  where Cms485ID = @Cms485ID;              
              
   SELECT 1; RETURN;              
 END              
END