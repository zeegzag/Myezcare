--  exec st.CreateEmpGroup '5t11'  
CREATE PROCEDURE st.CreateEmpGroup  
@GroupName NVARCHAR(MAX)=NULL,
@ReferralID BIGINT=0,
@IsEditMode BIT=0
AS --select @ReferralID = '10150', @GroupName = 'ABC1ggggggggggg', @IsEditMode = 'True'  
BEGIN  
	DECLARE    @DDMasterID BIGINT  
	DECLARE    @ItemType INT  
	DECLARE    @DDMasterTypeID BIGINT  
	SELECT @DDMasterID=DDMasterID,@ItemType=ItemType FROM DDMaster WHERE Title LIKE '%'+@GroupName+'%'  
	SELECT @DDMasterTypeID=DDMasterTypeID FROM lu_DDMasterTypes WHERE Name like '%Employee Group%'--DDMasterTypeID = (SELECT ItemType FROM DDMaster WHERE Title LIKE '%'+@GroupName+'%')  
    
	IF(@IsEditMode='True')
	BEGIN
			 IF(@DDMasterID IS NOT NULL)  
				BEGIN  
					 --UPDATE DDMaster SET Title=@GroupName,UpdatedDate=GETDATE() WHERE DDMasterID=@DDMasterID  
					 --UPDATE Referrals set GroupIDs=@DDMasterID where ReferralID=@ReferralID
					 select  DDMasterID AS Value,Title AS Name from DDMaster where IsDeleted=0 AND Title LIKE '%'+@GroupName+'%'
				    
				 END  
			 ELSE  
				 BEGIN  
					 INSERT INTO  DDMaster(ItemType,Title,Value,ParentID,SortOrder,Remark,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)  
					VALUES(@DDMasterTypeID,@GroupName,NULL,0,NULL,NULL,0,GETDATE(),1,GETDATE(),1,NULL)  
					SELECT @DDMasterID=DDMasterID,@ItemType=ItemType FROM DDMaster WHERE Title LIKE '%'+@GroupName+'%' 
					UPDATE Referrals set GroupIDs=@DDMasterID where ReferralID=@ReferralID
					select  DDMasterID AS Value,Title AS Name from DDMaster where IsDeleted=0 AND Title LIKE '%'+@GroupName+'%' 
					
				 END 
	END
	ELSE 
		BEGIN
	 IF(@DDMasterID IS NOT NULL)                      
	 BEGIN                                  
	select  DDMasterID AS Value,Title AS Name from DDMaster where IsDeleted=0 AND Title LIKE '%'+@GroupName+'%'  

	 END 
	 ELSE
	 BEGIN
					INSERT INTO  DDMaster(ItemType,Title,Value,ParentID,SortOrder,Remark,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)  
					VALUES(@DDMasterTypeID,@GroupName,NULL,0,NULL,NULL,0,GETDATE(),1,GETDATE(),1,NULL)  
					SELECT @DDMasterID=DDMasterID,@ItemType=ItemType FROM DDMaster WHERE Title LIKE '%'+@GroupName+'%' 
					UPDATE Referrals set GroupIDs=@DDMasterID where ReferralID=@ReferralID
					select  DDMasterID AS Value,Title AS Name from DDMaster where IsDeleted=0 AND Title LIKE '%'+@GroupName+'%' 
	END
		END

	
 
END