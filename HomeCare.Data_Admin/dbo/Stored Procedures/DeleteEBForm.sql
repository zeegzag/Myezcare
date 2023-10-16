/*
Created by : Neeraj Sharma
Created Date: 14 August 2020
Updated by :
Updated Date :

Purpose: This stored procedure is used to Delete EBForms Data

*/
CREATE PROCEDURE [dbo].[DeleteEBForm]    
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