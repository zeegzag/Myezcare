-- EXEC GetAuditLogList @ParentKeyFieldID = '0', @UpdatedBy = '', @Table = '', @ActionName = '', @SortExpression = 'AuditLogID', @SortType = 'DESC', @FromIndex = '1', @PageSize = '30'
CREATE  PROCEDURE [dbo].[GetAuditLogList]                            
@ParentKeyFieldID bigint=0,
@UpdatedBy varchar(100)='',
@Table varchar(100)='',
@ActionName varchar(100)='',
@FromDate  datetime=null,     
@ToDate  datetime=null,      
@SORTEXPRESSION NVARCHAR(100),             
@SORTTYPE NVARCHAR(10),            
@FROMINDEX INT,                            
@PAGESIZE INT                              
AS                            
BEGIN                              
;WITH CTEAuditLog AS                        
 (                             
  SELECT *,COUNT(T1.AuditLogID) OVER() AS COUNT FROM                        
  (                            
   SELECT ROW_NUMBER() OVER (ORDER BY   
   
                        
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AuditLogID' THEN CONVERT(bigint,A.AuditLogID)  END END ASC,                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AuditLogID' THEN CONVERT(bigint,A.AuditLogID)  END END DESC,  
                        
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedByName' THEN LastName END END ASC,                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedByName' THEN LastName END END DESC,  
 
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN CONVERT(datetime,A.CreatedDate, 127)   END END ASC,                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN CONVERT(datetime,A.CreatedDate, 127)   END END DESC ,  
 
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'DataModel' THEN DataModel END END ASC,                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'DataModel' THEN DataModel END END DESC,  
 
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AuditActionType' THEN AuditActionType   END END ASC,                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AuditActionType' THEN AuditActionType   END END DESC 
 
  
               
    ) AS ROW,        
   
		 A.AuditLogID,A.ParentKeyFieldID,A.ChildKeyFieldID,A.AuditActionType,A.DateTimeStamp,A.DataModel,A.Changes,A.ValueBefore,A.ValueAfter,A.CreatedDate
		,A.CreatedBy,A.UpdatedDate,A.UpdatedBy,A.SystemID, CE.LastName+', '+CE.FirstName AS CreatedByName, CE.UserName 
		From AuditLogTable A
		INNER JOIN Employees CE ON CE.EmployeeID=A.CreatedBy
		WHERE A.ParentKeyFieldID=@ParentKeyFieldID
		AND (   (@UpdatedBy IS NULL OR LEN(@UpdatedBy)=0) OR  
				((CE.FirstName LIKE '%'+@UpdatedBy+'%' )OR            
				(CE.LastName  LIKE '%'+@UpdatedBy+'%') OR            
				(CE.FirstName +' '+CE.LastName like '%'+@UpdatedBy+'%') OR            
				(CE.LastName +' '+CE.FirstName like '%'+@UpdatedBy+'%') OR            
				(CE.FirstName +', '+CE.LastName like '%'+@UpdatedBy+'%') OR            
				(CE.LastName +', '+CE.FirstName like '%'+@UpdatedBy+'%'))   )
	
		AND  ( (@Table IS NULL OR LEN(@Table)=0) OR A.DataModel like '%'+@Table+'%' )
		AND  ( (@ActionName IS NULL OR LEN(@ActionName)=0) OR A.AuditActionType like '%'+@ActionName+'%' )
		AND ((@FromDate is null OR A.UpdatedDate >= @FromDate) and (@ToDate is null OR A.UpdatedDate <= @ToDate)) 
           
   ) AS T1        
 )        
 SELECT * FROM CTEAuditLog WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)END             


 --select * from AuditLogTable
