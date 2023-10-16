CREATE PROCEDURE [dbo].[SaveDDmaster]
	@DDMasterID BIGINT,
	@ItemType INT,
	@Title NVARCHAR(2000),
	@Value NVARCHAR(2000),
	@loggedInUserId BIGINT,
	@SystemID VARCHAR(100)
AS
BEGIN
	IF EXISTS (SELECT TOP 1 DDMasterID FROM DDMaster WHERE ItemType=@ItemType AND Title=@Title AND DDMasterID != @DDMasterID)
		BEGIN
			SELECT -1 RETURN;
		END
		       
	-- If edit mode                    
	IF(@DDMasterID=0)                    
		BEGIN                    
			INSERT INTO DDMaster(ItemType,Title,Value,ParentID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)  
			VALUES (@ItemType,@Title,@Value,0,GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@loggedInUserId,@SystemID);  
		END                    
	ELSE                    
		BEGIN                    
			UPDATE DDMaster                     
			SET                          
			ItemType=@ItemType,                    
			Title=@Title,
			Value = @Value,
			UpdatedBy=@loggedInUserId,                    
			UpdatedDate=GETUTCDATE(),                    
			SystemID=@SystemID  
			WHERE DDMasterID=@DDMasterID;                    
		END
		SELECT 1; RETURN;             
END