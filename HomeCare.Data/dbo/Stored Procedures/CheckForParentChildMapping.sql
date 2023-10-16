-- EXEC CheckForParentChildMapping @DDMasterTypeID = '9', @DDMasterID = '0'
-- EXEC CheckForParentChildMapping @DDMasterTypeID = '1', @DDMasterID = '1'
CREATE PROCEDURE [dbo].[CheckForParentChildMapping]      
 @DDMasterTypeID BIGINT,      
 @DDMasterID BIGINT      
AS      
BEGIN      

SET NOCOUNT ON;      

DECLARE @IsParentItemSelected BIT=1;    
DECLARE @ItemType BIGINT=0;  
DECLARE @TempParentChildTable TABLE (DDMasterTypeID BIGINT, Name VARCHAR(100), IsChild BIT , SortOrder INT)  
  
 INSERT INTO @TempParentChildTable  
 SELECT DDMasterTypeID,Name,0,SortOrder FROM lu_DDMasterTypes  WHERE ParentID=@DDMasterTypeID  ORDER BY SortOrder ASC  

   
 IF NOT EXISTS(SELECT 1 FROM @TempParentChildTable)  
 BEGIN  

 
 SET @IsParentItemSelected=0;
  
 INSERT INTO @TempParentChildTable  
 SELECT   DDMasterTypeID,Name,1,SortOrder FROM lu_DDMasterTypes WHERE DDMasterTypeID IN (SELECT  ParentID FROM lu_DDMasterTypes  WHERE DDMasterTypeID=@DDMasterTypeID) ORDER BY SortOrder ASC  
 END  
  
  
 DECLARE @TotalCount BIGINT=0;
 SELECT @TotalCount=COUNT(*) FROM @TempParentChildTable

SELECT * FROM @TempParentChildTable

IF (@TotalCount=1)  
BEGIN

IF( @IsParentItemSelected = 1 )
BEGIN
	SELECT DM.DDMasterID,DM.Title,DM.ParentID  FROM DDMaster DM 
	INNER JOIN @TempParentChildTable T ON T.DDMasterTypeID=DM.ItemType
	WHERE (DM.ParentID = 0 OR DM.ParentID=@DDMasterID) AND IsDeleted=0    
END
ELSE
BEGIN
    
	SELECT DISTINCT DM.DDMasterID,DM.Title,DM1.ParentID FROM DDMaster DM 
	INNER JOIN @TempParentChildTable T ON T.DDMasterTypeID=DM.ItemType
	LEFT JOIN DDMaster DM1 ON DM1.ParentID=DM.DDMasterID AND DM1.DDMasterID=@DDMasterID
	WHERE DM.IsDeleted=0    -- (DM.ParentID = 0 OR DM.DDMasterID=@DDMasterID) AND 
END


SELECT IsMultiSelect=@IsParentItemSelected

END
ELSE
BEGIN
 SELECT DDMaster=NULL, Title=NULL,ParentID=NULL WHERE 1=2 ;
 SELECT IsMultiSelect=0;
END
  
END