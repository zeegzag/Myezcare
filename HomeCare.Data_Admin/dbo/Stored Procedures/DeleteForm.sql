CREATE PROCEDURE [dbo].[DeleteForm]  
@MarketID BIGINT=null,                    
@FormCategoryID BIGINT=NULL,                    
@FormName VARCHAR(20)=NULL,    
@FormNumber VARCHAR(20)=NULL,    
@IsDeleted BIGINT = -1,                   
@SORTEXPRESSION NVARCHAR(100),            
@SORTTYPE NVARCHAR(10),          
@FROMINDEX INT,          
@PAGESIZE INT,  
@ListOfIdsInCSV  varchar(MAX)=null,            
@IsShowList bit,      
@loggedInID BIGINT           
AS            
BEGIN                
            
 IF(LEN(@ListOfIdsInCSV)>0)            
 BEGIN          
   UPDATE EbForms SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE()      
   WHERE EBFormID IN (SELECT VAL FROM GETCSVTABLE(@ListOfIdsInCSV))                 
 END            
 IF(@IsShowList=1)            
 BEGIN            
  EXEC GetFormList @MarketID,@FormCategoryID,@FormName,@FormNumber,@IsDeleted,@SORTEXPRESSION,@SORTTYPE,@FROMINDEX,@PAGESIZE  
  
   END            
END