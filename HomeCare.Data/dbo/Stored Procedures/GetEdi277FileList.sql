CREATE PROCEDURE [dbo].[GetEdi277FileList]                              
@FileType VARCHAR(100),              
@FileName VARCHAR(100)=null,              
@Comment VARCHAR(100)=null,              
@PayorID BIGINT=0,              
@Upload277FileProcessStatus INT,              
@IsDeleted INT,  
@SORTEXPRESSION NVARCHAR(100),               
@SORTTYPE NVARCHAR(10),              
@FROMINDEX INT,                              
@PAGESIZE INT                                
AS                              
BEGIN        
 DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()  
PRINT CAST(@IsDeleted AS int)  
                          
;WITH CTEEdi277FileLog AS                          
 (                               
  SELECT *,COUNT(T1.Edi277FileID) OVER() AS COUNT FROM                          
  (                              
   SELECT ROW_NUMBER() OVER (ORDER BY                          
                          
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Edi277FileID' THEN Edi277FileID END END ASC,                          
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Edi277FileID' THEN Edi277FileID END END DESC,  
  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AddedDate' THEN  CONVERT(datetime,CreatedDate, 105)   END END ASC,                                            
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AddedDate' THEN  CONVERT(datetime, CreatedDate, 105)   END END DESC,           
   
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FileSize' THEN  cast(FileSize as decimal)   END END ASC,          
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FileSize' THEN  cast(FileSize as decimal)     END END DESC,                                           
   
  CASE WHEN @SORTTYPE = 'ASC' THEN              
     CASE          
    
      WHEN @SORTEXPRESSION = 'Payor' THEN Payor  
   WHEN @SORTEXPRESSION = 'FileName' THEN FileName          
      WHEN @SORTEXPRESSION = 'AddedBy' THEN LastName  
   WHEN @SORTEXPRESSION = 'Comment' THEN Comment  
    END          
    END ASC,          
    CASE WHEN @SORTTYPE = 'DESC' THEN          
      CASE          
   WHEN @SORTEXPRESSION = 'Payor' THEN Payor  
      WHEN @SORTEXPRESSION = 'FileName' THEN FileName          
      WHEN @SORTEXPRESSION = 'AddedBy' THEN LastName  
   WHEN @SORTEXPRESSION = 'Comment' THEN Comment          
    END           
  
       END DESC             
      
    ) AS ROW,  * FROM (  
         
     SELECT  EFL.*,P.ShortName AS Payor,  
     EMP.FirstName,EMP.LastName,dbo.GetGenericNameFormat(EMP.FirstName,EMP.MiddleName, EMP.LastName,@NameFormat) AS StrDisplayName,EFL.CreatedDate as AddedDate  
     FROM  Edi277Files EFL          
     INNER JOIN Payors P ON P.PayorID=EFL.PayorID  
     INNER JOIN Employees EMP on EMP.EmployeeID=EFL.CreatedBy                    
     WHERE  
     FileType = @FileType AND  
     ( CAST(@IsDeleted AS int)=-1 OR EFL.IsDeleted=Convert(bit,@IsDeleted)) AND  
     ((@FileName IS NULL OR LEN(@FileName)=0) OR EFL.FileName LIKE '%' + @FileName+ '%')  AND  
     ((@Comment IS NULL OR LEN(@Comment)=0) OR EFL.Comment LIKE '%' + @Comment+ '%')  AND  
     ((CAST(@Upload277FileProcessStatus AS BIGINT)=-1 OR CAST(@Upload277FileProcessStatus AS BIGINT)=0) OR (EFL.Upload277FileProcessStatus=@Upload277FileProcessStatus OR EFL.Upload277FileProcessStatus IS NULL) ) AND  
      (@PayorID=0 OR EFL.PayorID =@PayorID) AND  
     1=1                           
     ) AS T  
  
   ) AS T1          
 )          
 SELECT * FROM CTEEdi277FileLog WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)END  