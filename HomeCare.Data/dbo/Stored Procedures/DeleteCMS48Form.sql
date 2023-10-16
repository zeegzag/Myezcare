--CreatedBy: Abhishek Gautam  
--CreatedDate: 25 sept 2020  
--Description: Delete CMS485 form 
  
  --Exec DeleteCMS48Form  
    
CREATE PROC DeleteCMS48Form    
@Cms485ID BIGINT=0    
AS    
BEGIN    
    
UPDATE CMS485 SET IsDeleted=1 WHERE Cms485ID=@Cms485ID    
SELECT 1 RETURN;    
   
END