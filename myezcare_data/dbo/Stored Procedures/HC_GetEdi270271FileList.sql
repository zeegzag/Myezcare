CREATE PROCEDURE [dbo].[HC_GetEdi270271FileList]
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
@PAGESIZE INT                                
AS                              
BEGIN        
  
PRINT CAST(@IsDeleted AS int)  
                          
;WITH CTEEdiFileLog AS                          
 (                               
  SELECT *,COUNT(T1.Edi270271FileID) OVER() AS COUNT FROM                          
  (                              
   SELECT ROW_NUMBER() OVER (ORDER BY                          
                          
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Edi270271FileID' THEN Edi270271FileID END END ASC,                          
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Edi270271FileID' THEN Edi270271FileID END END DESC,  
   
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EligibilityCheckDate' THEN  CONVERT(date,EligibilityCheckDate, 105)   END END ASC,                                            
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EligibilityCheckDate' THEN  CONVERT(date, EligibilityCheckDate, 105)   END END DESC,   
  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AddedDate' THEN  CONVERT(date,CreatedDate, 105)   END END ASC,                                            
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AddedDate' THEN  CONVERT(date, CreatedDate, 105)   END END DESC,           
   
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FileSize' THEN  cast(FileSize as decimal)   END END ASC,          
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FileSize' THEN  cast(FileSize as decimal)     END END DESC,                                           
   
  CASE WHEN @SORTTYPE = 'ASC' THEN              
     CASE          
    
      WHEN @SORTEXPRESSION = 'Payors' THEN Payors  
   WHEN @SORTEXPRESSION = 'Services' THEN ServiceIDs  
   WHEN @SORTEXPRESSION = 'ClientName' THEN Name  
   WHEN @SORTEXPRESSION = 'FileName' THEN FileName          
      WHEN @SORTEXPRESSION = 'AddedBy' THEN LastName  
    END          
    END ASC,          
    CASE WHEN @SORTTYPE = 'DESC' THEN          
      CASE          
   WHEN @SORTEXPRESSION = 'Payors' THEN Payors  
   WHEN @SORTEXPRESSION = 'Services' THEN ServiceIDs  
   WHEN @SORTEXPRESSION = 'ClientName' THEN Name  
      WHEN @SORTEXPRESSION = 'FileName' THEN FileName          
      WHEN @SORTEXPRESSION = 'AddedBy' THEN LastName          
    END           
  
       END DESC             
      
    ) AS ROW,  * FROM (  
         
     SELECT  EFL.*, Payors=  (SELECT  STUFF((SELECT ', ' +P.ShortName FROM Payors P  
     INNER JOIN (select CAST(val AS BIGINT) PayorID from GetCSVTable(EFL.PayorIDs)) Temp ON TEMP.PayorID=P.PayorID  
     ORDER BY P.ShortName ASC                                                          
      FOR XML PATH('')),1,1,'')) ,  
  
      ReferralStatuses=  (SELECT  STUFF((SELECT ', ' + R.Status FROM ReferralStatuses R  
         INNER JOIN (select CAST(val AS BIGINT) ReferralStatusID from GetCSVTable(EFL.ReferralStatusIDs)) Temp ON TEMP.ReferralStatusID=R.ReferralStatusID  
         ORDER BY R.Status ASC                                                          
      FOR XML PATH('')),1,1,'')) ,  
  
     EMP.FirstName,EMP.LastName,EFL.CreatedDate as AddedDate  
     FROM  Edi270271Files EFL          
     INNER JOIN Employees EMP on EMP.EmployeeID=EFL.CreatedBy                    
     WHERE  
     FileType = @FileType AND  
     ( CAST(@IsDeleted AS int)=-1 OR EFL.IsDeleted=Convert(bit,@IsDeleted)) AND  
     ((@FileName IS NULL OR LEN(@FileName)=0) OR EFL.FileName LIKE '%' + @FileName+ '%')  AND  
     ((@Comment IS NULL OR LEN(@Comment)=0) OR EFL.Comment LIKE '%' + @Comment+ '%')  AND  
     ((@ClientName IS NULL OR LEN(@ClientName)=0) OR EFL.Name LIKE '%' + @ClientName+ '%')  AND  
     ((@ServiceID IS NULL OR LEN(@ServiceID)=0 OR @ServiceID=@AllServiceText) OR EFL.ServiceIDs LIKE '%' + @ServiceID+ '%')  AND  
     ((CAST(@Upload271FileProcessStatus AS BIGINT)=-1 OR CAST(@Upload271FileProcessStatus AS BIGINT)=0) OR (EFL.Upload271FileProcessStatus=@Upload271FileProcessStatus OR EFL.Upload271FileProcessStatus IS NULL) ) AND  
     (@EligibilityCheckDate IS NULL OR LEN(@EligibilityCheckDate)=0 OR EFL.EligibilityCheckDate=CONVERT(DATE,@EligibilityCheckDate))  AND  
     (@PayorID=0 OR @PayorID in (select CAST(val AS BIGINT) from GetCSVTable(EFL.PayorIDs))) AND  
     1=1                           
     ) AS T  
  
   ) AS T1          
 )          
 SELECT * FROM CTEEdiFileLog WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)END
