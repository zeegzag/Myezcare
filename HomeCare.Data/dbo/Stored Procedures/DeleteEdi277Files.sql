CREATE PROCEDURE [dbo].[DeleteEdi277Files]
@FileType VARCHAR(100),            
@FileName VARCHAR(100)=null,            
@Comment VARCHAR(100)=null,            
@PayorID BIGINT=0,            
@Upload277FileProcessStatus INT,            
@IsDeleted INT,
@SORTEXPRESSION NVARCHAR(100),             
@SORTTYPE NVARCHAR(10),            
@FROMINDEX INT,                            
@PAGESIZE INT ,
@ListOfIdsInCSV  varchar(300),        
@IsShowList bit,
@LoggedInID BIGINT
AS        
BEGIN            
        
 IF(LEN(@ListOfIdsInCSV)>0)        
 BEGIN      
   UPDATE Edi277Files SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@LoggedInID as BIGINT) ,UpdatedDate=GETUTCDATE() WHERE  Edi277FileID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))
 END        
 IF(@IsShowList=1)        
 BEGIN        
  EXEC GetEdi277FileList  @FileType ,@FileName ,@Comment,@PayorID,@Upload277FileProcessStatus,@IsDeleted,@SortExpression,@SortType ,@FromIndex,@PageSize      
 END        
END