CREATE PROCEDURE [dbo].[DeleteScheduleBatchService]  
@ScheduleBatchServiceName varchar(100)=null,    
@ScheduleBatchServiceType varchar(100)=null,    
@ScheduleBatchServiceStatus varchar(100)=null,    
@AddedBy varchar(100)=null,    
@FilePath varchar(100)=null,    
@SORTEXPRESSION VARCHAR(100),                          
@SORTTYPE VARCHAR(10),                          
@FROMINDEX INT,                          
@PAGESIZE INT ,                         
@ListOfIdsInCSV  varchar(300),            
@IsShowList bit    
AS            
BEGIN                
            
 IF(LEN(@ListOfIdsInCSV)>0)            
 BEGIN          
   Delete ScheduleBatchServices  WHERE  ScheduleBatchServiceID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))    
 END            
 IF(@IsShowList=1)            
 BEGIN            
  EXEC GetScheduleBatchServiceList @ScheduleBatchServiceName,@ScheduleBatchServiceType,@ScheduleBatchServiceStatus,@AddedBy,@FilePath,@SORTEXPRESSION,@SORTTYPE ,@FROMINDEX,@PAGESIZE          
 END            
END