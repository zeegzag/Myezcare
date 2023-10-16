CREATE PROCEDURE [dbo].[HC_GetEdiFileLogList]                                
@EdiFileLogID BIGINT=0,                
@EdiFileTypeID bigint=0,                
@FileName VARCHAR(100)=null,                
@FilePath NVARCHAR(MAX)=null,                
@SORTEXPRESSION NVARCHAR(100),                 
@SORTTYPE NVARCHAR(10),                
@FROMINDEX INT,                                
@PAGESIZE INT                                  
AS                                
BEGIN                                  
;WITH CTEEdiFileLog AS                            
 (                                 
  SELECT *,COUNT(T1.EdiFileLogID) OVER() AS COUNT FROM                            
  (                                
   SELECT ROW_NUMBER() OVER (ORDER BY                            
                            
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EdiFileLogID' THEN EdiFileLogID END END ASC,                            
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EdiFileLogID' THEN EdiFileLogID END END DESC,                
                 
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AddedDate' THEN  CONVERT(date,EFL.CreatedDate, 105)     END END ASC,                                              
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AddedDate' THEN  CONVERT(date, EFL.CreatedDate, 105)     END END DESC,                                              
                
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'BatchID' THEN  cast(BatchID as bigint)   END END ASC,            
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'BatchID' THEN  cast(BatchID as bigint)    END END DESC,            
         
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FileSize' THEN  cast(EFL.FileSize as decimal)   END END ASC,            
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FileSize' THEN  cast(EFL.FileSize as decimal)     END END DESC,            
            
            
    CASE WHEN @SORTTYPE = 'ASC' THEN                
     CASE            
      WHEN @SORTEXPRESSION = 'FileName' THEN EFL.FileName            
      WHEN @SORTEXPRESSION = 'EdiFileTypeName' THEN EFT.EdiFileTypeName            
      WHEN @SORTEXPRESSION = 'AddedBy' THEN EMP.FirstName            
    END            
    END ASC,            
    CASE WHEN @SORTTYPE = 'DESC' THEN            
      CASE            
      WHEN @SORTEXPRESSION = 'FileName' THEN EFL.FileName            
      WHEN @SORTEXPRESSION = 'EdiFileTypeName' THEN EFT.EdiFileTypeName            
      WHEN @SORTEXPRESSION = 'AddedBy' THEN EMP.FirstName            
    END            
    END DESC             
    ) AS ROW,            
   EFL.EdiFileLogID,EFL.EdiFileTypeID,EFL.FileName,EFL.FilePath,EFT.EdiFileTypeName,EMP.FirstName,EMP.LastName,EFL.CreatedDate as AddedDate,EFL.BatchID,EFL.FileSize            
   FROM  EdiFilelogs EFL            
   LEFT JOIN EdiFileTypes EFT on EFT.EdiFileTypeID=EFL.EdiFileTypeID            
   LEFT JOIN Employees EMP on EMP.EmployeeID=EFL.CreatedBy                      
   WHERE                             
   ((@FileName IS NULL OR LEN(@FileName)=0) OR EFL.FileName LIKE '%' + @FileName+ '%')            
   AND ((@EdiFileTypeID =0) or EFL.EdiFileTypeID = @EdiFileTypeID )            
   ) AS T1            
 )            
 SELECT * FROM CTEEdiFileLog WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)END
