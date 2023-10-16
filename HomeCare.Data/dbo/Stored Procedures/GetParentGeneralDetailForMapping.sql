CREATE PROCEDURE [dbo].[GetParentGeneralDetailForMapping]  
 @DDMasterTypeID BIGINT,  
 @IsFetchParentRecord BIT,  
 @ParentID BIGINT  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 IF (@DDMasterTypeID = 0)  
  BEGIN  
   SET @DDMasterTypeID = NULL  
  END  
  
 SELECT DDMasterTypeID,Name FROM dbo.lu_DDMasterTypes ldt WHERE ldt.ParentID=@DDMasterTypeID  
  
 IF(@IsFetchParentRecord = 0)  
  BEGIN  
   SELECT DDMasterID,Title FROM DDMaster d WHERE d.ItemType=@DDMasterTypeID AND IsDeleted=0
  END  
 ELSE  
  BEGIN  
   SELECT DDMasterID,Title,ParentID FROM DDMaster d WHERE d.ItemType=@DDMasterTypeID AND (d.ParentID = 0 OR d.ParentID=@ParentID) AND IsDeleted=0
  END  
END