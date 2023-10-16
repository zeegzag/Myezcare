-- EXEC GetUpload835FilesForAutoComplete @PayorId = '0', @SearchText = 'a', @PageSize = '10'    
CREATE PROCEDURE [dbo].[HC_GetUpload835FilesForAutoComplete]    
@PayorId bigint=0,    
@SearchText varchar(100)=NULL,    
@PageSize bigint=10    
AS    
BEGIN    
 SELECT DISTINCT Top(@PageSize) U8F.*,P.ShortName AS Payor FROM Upload835Files U8F    
 INNER JOIN Payors P ON P.PayorID=U8F.PayorID    
 WHERE ( (@SearchText IS NULL) OR     
         (  (P.PayorName LIKE '%' + @SearchText+ '%') OR (P.ShortName LIKE '%' + @SearchText+ '%')     
      OR (U8F.FileName LIKE '%' + @SearchText+ '%') OR (U8F.Comment LIKE '%' + @SearchText+ '%')     
   )     
       ) AND (@PayorId=0 OR U8F.PayorID=@PayorId)    
 ORDER BY U8F.Upload835FileID DESC    
END
