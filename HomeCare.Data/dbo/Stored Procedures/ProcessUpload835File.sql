CREATE PROCEDURE [dbo].[ProcessUpload835File]  
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
@ListOfIdsInCSV  varchar(500),  
@Upload835FileStatus  varchar(2),      
@IsShowList bit  
AS          
BEGIN              
          
 IF(LEN(@ListOfIdsInCSV)>0)          
 BEGIN        
   Update Upload835Files SET IsProcessed=1, Upload835FileProcessStatus=@Upload835FileStatus  WHERE  Upload835FileID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))  
 END          
 IF(@IsShowList=1)          
 BEGIN          
  EXEC GetUpload835FileList @Upload835FileID,@PayorID,@FileName, @FilePath,@Comment,@A835TemplateType,@Upload835FileProcessStatus,@SortExpression,@SortType ,@FromIndex,@PageSize        
 END          
END