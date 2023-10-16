CREATE PROCEDURE [dbo].[SaveDDmaster]  
 @DDMasterID BIGINT,  
 @ItemType INT,  
 @Title NVARCHAR(MAX),  
 @Value NVARCHAR(MAX),
 --@Value2 NVARCHAR(MAX),
 @ChildTaskIds VARCHAR(MAX),
 @IsMultiSelect BIT=0,
 @loggedInUserId BIGINT,  
 @SystemID VARCHAR(100)  
AS  
BEGIN  
 IF EXISTS (SELECT TOP 1 DDMasterID FROM DDMaster WHERE ItemType=@ItemType AND Title=@Title AND DDMasterID != @DDMasterID)  
  BEGIN  
   SELECT -1 RETURN;  
  END  
           
 DECLARE @TempDDMasterID BIGINT=@DDMasterID;

 IF(@DDMasterID=0)                      
  BEGIN                      
   INSERT INTO DDMaster(ItemType,Title,Value,ParentID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)    
   VALUES (@ItemType,@Title,@Value,0,GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@loggedInUserId,@SystemID);    

   SET @TempDDMasterID=@@IDENTITY;
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



  --  EXEC SaveDDmaster @DDMasterID = '29', @ItemType = '9', @Title = 'Test Visit', @Value = '', @ChildTaskIds = '', @IsMultiSelect = 'True', @loggedInUserId = '1', @SystemID = '::1'

  -- @@IDENTITY
  
  IF(@IsMultiSelect=1)
   BEGIN
     IF(@ChildTaskIds='')
	    UPDATE DDMaster SET ParentID=0 WHERE ParentID=@TempDDMasterID
	 ELSE
	  BEGIN
        UPDATE DDMaster SET ParentID=0 WHERE ParentID=@TempDDMasterID AND DDMasterID NOT IN (SELECT RESULT FROM dbo.CSVtoTableWithIdentity(@ChildTaskIds,','))  
        UPDATE DDMaster SET ParentID=@TempDDMasterID WHERE DDMasterID IN (SELECT RESULT FROM dbo.CSVtoTableWithIdentity(@ChildTaskIds,','))
	  END
   END
  ELSE
   BEGIN
    IF(@ChildTaskIds='')
	 UPDATE DDMaster SET ParentID=0 WHERE DDMasterID=@TempDDMasterID -- AND DDMasterID NOT IN (SELECT RESULT FROM dbo.CSVtoTableWithIdentity(@ChildTaskIds,','))  
    ELSE
     UPDATE DDMaster SET ParentID=@ChildTaskIds WHERE DDMasterID=@TempDDMasterID

   END

  SELECT 1; RETURN;               
END
