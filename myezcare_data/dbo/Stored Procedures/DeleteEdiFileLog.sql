CREATE PROCEDURE [dbo].[DeleteEdiFileLog]
@EdiFileLogID BIGINT=0,                
@EdiFileTypeID bigint=0,                
@FileName VARCHAR(255)=null,                
@FilePath VARCHAR(255)=null,                
@IsDeleted bit = 0,               
@SORTEXPRESSION NVARCHAR(100),                
@SORTTYPE NVARCHAR(10),                
@FROMINDEX INT,                
@PAGESIZE INT,
@ListOfIdsInCSV  varchar(300),        
@IsShowList bit
AS        
BEGIN            
        
 IF(LEN(@ListOfIdsInCSV)>0)        
 BEGIN      
   Delete EdiFileLogs  WHERE  EdiFileLogID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))
 END        
 IF(@IsShowList=1)        
 BEGIN        
  EXEC GetEdiFileLogList @EdiFileLogID,@EdiFileTypeID,@FileName, @FilePath,@SortExpression,@SortType ,@FromIndex,@PageSize      
 END        
END        
