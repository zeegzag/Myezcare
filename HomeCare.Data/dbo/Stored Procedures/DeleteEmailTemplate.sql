CREATE PROCEDURE [dbo].[DeleteEmailTemplate]      
@EmailTemplateID BIGINT=0,                  
@EmailTemplateName VARCHAR(100)=null,                  
@EmailTemplateSubject VARCHAR(100)=null,  
@Token varchar(100)=null,          
@IsDeleted BIGINT = -1,                 
@SortExpression nvarchar(100),            
@SortType nvarchar(10),                  
@FromIndex int,                  
@PageSize int,            
@ListOfIdsInCSV  varchar(300),              
@IsShowList bit,        
@loggedInID BIGINT             
AS              
BEGIN                  
              
 IF(LEN(@ListOfIdsInCSV)>0)              
 BEGIN            
   UPDATE EmailTemplates SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE()        
   WHERE EmailTemplateID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))                   
 END              
 IF(@IsShowList=1)              
 BEGIN              
  EXEC GetEmailTemplateList @EmailTemplateID ,@EmailTemplateName,@EmailTemplateSubject ,@Token,@IsDeleted,@SortExpression,@SortType,@FromIndex,@PageSize            
 END              
END