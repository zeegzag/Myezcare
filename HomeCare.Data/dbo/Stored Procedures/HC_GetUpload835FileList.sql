CREATE PROCEDURE [dbo].[HC_GetUpload835FileList]              
@Upload835FileID BIGINT=0,                            
@PayorID BIGINT=0,                            
@FileName VARCHAR(100)=null,                            
@FilePath NVARCHAR(MAX)=null,                     
@Comment NVARCHAR(MAX)=null,                  
@A835TemplateType NVARCHAR(MAX)=null,                  
@Upload835FileProcessStatus int=-1,                       
@SORTEXPRESSION NVARCHAR(100),                             
@SORTTYPE NVARCHAR(10),                            
@FROMINDEX INT,                                            
@PAGESIZE INT                                              
AS                                            
BEGIN    
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
;WITH CTEEdiFileLog AS                                        
 (                                             
  SELECT *,COUNT(T1.Upload835FileID) OVER() AS COUNT FROM                                        
  (                                            
   SELECT ROW_NUMBER() OVER (ORDER BY                                        
                                        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Upload835FileID' THEN Upload835FileID END END ASC,                                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Upload835FileID' THEN Upload835FileID END END DESC,                            
                             
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AddedDate' THEN  CONVERT(date,UF.CreatedDate, 105)     END END ASC,                                                          
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AddedDate' THEN  CONVERT(date, UF.CreatedDate, 105)     END END DESC,                                                          
                      
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PayorID' THEN  cast(UF.PayorID as bigint)   END END ASC,                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PayorID' THEN  cast(UF.PayorID as bigint)     END END DESC,                        
                    
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'BatchID' THEN  cast(BatchID as bigint)   END END ASC,                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'BatchID' THEN  cast(BatchID as bigint)     END END DESC,                        
                     
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FileSize' THEN  cast(FileSize as decimal)   END END ASC,                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FileSize' THEN  cast(FileSize as decimal)     END END DESC,                        
                        
                        
    CASE WHEN @SORTTYPE = 'ASC' THEN                            
     CASE                        
      WHEN @SORTEXPRESSION = 'FileName' THEN FileName                       
   WHEN @SORTEXPRESSION = 'A835TemplateType' THEN A835TemplateType                       
      WHEN @SORTEXPRESSION = 'Comment' THEN Comment                        
      WHEN @SORTEXPRESSION = 'AddedBy' THEN FirstName                        
    END                        
    END ASC,                        
    CASE WHEN @SORTTYPE = 'DESC' THEN                        
      CASE                        
      WHEN @SORTEXPRESSION = 'FileName' THEN FileName                 
   WHEN @SORTEXPRESSION = 'A835TemplateType' THEN A835TemplateType                            
      WHEN @SORTEXPRESSION = 'Comment' THEN Comment                        
      WHEN @SORTEXPRESSION = 'AddedBy' THEN FirstName                        
    END                        
    END DESC                         
    ) AS ROW,     UF.A835TemplateType,                   
   UF.Upload835FileID,UF.PayorID,P.ShortName AS PayorName,UF.BatchID,UF.Comment, UF.FileName, UF.FilePath,UF.IsDeleted,UF.IsProcessed, 
   UF.Upload835FileProcessStatus,UF.FileSize,UF.CreatedDate as AddedDate,EMP.FirstName,EMP.LastName,  StrDisplayName  =dbo.GetGenericNameFormat(EMP.FirstName,EMP.MiddleName, EMP.LastName,@NameFormat),           
   UF.ReadableFilePath , UF.EraMappedBatches    ,  UF.LogFilePath          
   FROM  Upload835Files UF                
   INNER JOIN Payors P on P.PayorID=UF.PayorID                       
   INNER JOIN Employees EMP on EMP.EmployeeID=UF.CreatedBy                                  
   WHERE                                        
   ((@FileName IS NULL OR LEN(@FileName)=0) OR UF.FileName LIKE '%' + @FileName+ '%')                     
   AND ((@Comment IS NULL OR LEN(@Comment)=0) OR UF.Comment LIKE '%' + @Comment+ '%')                        
   AND ((@A835TemplateType IS NULL OR LEN(@A835TemplateType)=0 OR @A835TemplateType='-1') OR UF.A835TemplateType LIKE '%' + @A835TemplateType+ '%')                      
   AND ((@PayorID =0) or UF.PayorID = @PayorID )                     
   AND ((CAST(@Upload835FileProcessStatus AS BIGINT)=-1) OR UF.Upload835FileProcessStatus=@Upload835FileProcessStatus)                         
   ) AS T1                        
 )                        
 SELECT * FROM CTEEdiFileLog WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)END 