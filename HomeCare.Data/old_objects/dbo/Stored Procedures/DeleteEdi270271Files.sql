CREATE PROCEDURE [dbo].[DeleteEdi270271Files]
@FileType VARCHAR(100),            
@FileName VARCHAR(100)=null,            
@Comment VARCHAR(100)=null,            
@PayorID BIGINT=0,            
@ServiceID  VARCHAR(100)=null,              
@ClientName VARCHAR(100)=NULL,            
@EligibilityCheckDate VARCHAR(100)=NULL,            
@Upload271FileProcessStatus INT,            
@IsDeleted INT,
@AllServiceText VARCHAR(MAX),
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
   UPDATE Edi270271Files SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@LoggedInID as BIGINT) ,UpdatedDate=GETUTCDATE() WHERE  Edi270271FileID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))
 END        
 IF(@IsShowList=1)        
 BEGIN        
  EXEC GetEdi270271FileList  @FileType ,@FileName ,@Comment,@PayorID,@ServiceID,@ClientName,@EligibilityCheckDate,@Upload271FileProcessStatus,@IsDeleted,@AllServiceText,@SortExpression,@SortType ,@FromIndex,@PageSize      
 END        
END        
