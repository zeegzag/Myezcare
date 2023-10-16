CREATE PROCEDURE [dbo].[HC_DeleteUpload835File]    
@Upload835FileID BIGINT=0,                  
@PayorID BIGINT=0,                                
@FileName VARCHAR(255)=null,                          
@FilePath VARCHAR(255)=null,           
@Comment NVARCHAR(MAX)=null,        
@A835TemplateType NVARCHAR(MAX)=null,                            
@IsDeleted bit = 0,                   
@Upload835FileProcessStatus int=-1,                   
@SORTEXPRESSION NVARCHAR(100),                          
@SORTTYPE NVARCHAR(10),                          
@FROMINDEX INT,                          
@PAGESIZE INT,          
@ListOfIdsInCSV  varchar(300),                  
@IsShowList bit,      
@UnProcess int          
AS                  
BEGIN                      
                  
 IF(LEN(@ListOfIdsInCSV)>0)                  
 BEGIN                
   Delete Upload835Files  WHERE  Upload835FileID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV)) AND Upload835FileProcessStatus=@UnProcess AND BatchID IS NULL          
 END                  
 IF(@IsShowList=1)                  
 BEGIN                  
  EXEC HC_GetUpload835FileList @Upload835FileID,@PayorID,@FileName,@FilePath,@Comment,@A835TemplateType, @Upload835FileProcessStatus ,@SortExpression,@SortType ,@FromIndex,@PageSize                
 END                  
END
