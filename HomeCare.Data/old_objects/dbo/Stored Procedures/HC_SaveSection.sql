CREATE PROCEDURE [dbo].[HC_SaveSection]
@SectionName NVARCHAR(1000),
@Color NVARCHAR(1000),
@DocumentSection BIGINT,
@CurrentDate DATETIME,          
@LoggedInID BIGINT,            
@SystemID VARCHAR(MAX)           
AS            
BEGIN            
IF EXISTS (SELECT TOP 1 DDMasterID FROM DDMaster WHERE Title=@SectionName AND ItemType=@DocumentSection)
BEGIN            
 SELECT -1 AS Result;
 SELECT SectionID=DDMasterID,SectionName=Title,Color=Value FROM DDMaster 
	WHERE ItemType=@DocumentSection AND IsDeleted=0;
	RETURN;
END
 INSERT INTO DDMaster(ItemType,Title,Value,ParentID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)
 VALUES (@DocumentSection,@SectionName,@Color,0,@CurrentDate,@LoggedInID,@CurrentDate,@LoggedInID,@SystemID)
 SELECT 1 AS Result;
 SELECT SectionID=DDMasterID,SectionName=Title,Color=Value FROM DDMaster 
	WHERE ItemType=@DocumentSection AND IsDeleted=0;
	 RETURN;
END
