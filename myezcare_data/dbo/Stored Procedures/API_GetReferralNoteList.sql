-- exec API_GetReferralNoteList 1,100,null,null,1951,''
CREATE PROCEDURE [dbo].[API_GetReferralNoteList]          
 @FromIndex INT,              
 @ToIndex INT,              
 @SortType NVARCHAR(10),    
 @SortExpression NVARCHAR(100),                      
 @ReferralID BIGINT,      
 @NoteDetail NVARCHAR(100)             
AS              
BEGIN              
            
IF(@SortExpression IS NULL OR @SortExpression ='')              
BEGIN              
	SET @SortExpression = 'CreatedDate'              
	SET @SortType='DESC'              
END              
   
    
;WITH CTENoteList AS                  
 (                       
  SELECT ROW_NUMBER() OVER               
    (ORDER BY              
       CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN T.AddedDate END              
       END ASC,              
       CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN T.AddedDate END              
       END DESC            
    ) AS Row,*,
	COUNT(T.CommonNoteID) OVER() AS Count              
  FROM              
  (              
	  SELECT 
	  cn.CommonNoteID,
	  NoteDetail=cn.Note,
	  AddedBy=E.LastName+', '+ E.FirstName,   
	  AddedDate=cn.CreatedDate   
	  FROM dbo.CommonNotes cn      
	  INNER JOIN Employees E ON E.EmployeeID = cn.CreatedBy      
	  WHERE 
	  cn.ReferralID=@ReferralID AND cn.IsDeleted=0 AND (@NoteDetail IS NULL OR LEN(@NoteDetail)=0 OR cn.Note=@NoteDetail)
  ) AS T              
 )              
                 
  SELECT * FROM CTENoteList WHERE ROW BETWEEN @FromIndex AND @ToIndex              
                
END
