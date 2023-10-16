  --  Exec [API].[GetNotesCategory] 
CREATE PROCEDURE  [API].[GetNotesCategory] 
 
AS
BEGIN  
SELECT d.Title as Title,d.DDMasterID AS Value  
  FROM DDMaster d  
 inner join  lu_DDMasterTypes ld on ld.DDMasterTypeID=d.ItemType   
        and ld.Name='Note Category' order by d.Title  
END  