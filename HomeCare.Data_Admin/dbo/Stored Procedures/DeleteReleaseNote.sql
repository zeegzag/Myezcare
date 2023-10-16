CREATE PROCEDURE [dbo].[DeleteReleaseNote]                  
@Title NVARCHAR(200)=null,
@Description NVARCHAR(MAX)=null,
@StartDate DATETIME=null,
@EndDate DATETIME=null,           
@IsDeleted BIGINT = -1,                         
@SORTEXPRESSION NVARCHAR(100),                  
@SORTTYPE NVARCHAR(10),                
@FROMINDEX INT,                
@PAGESIZE INT,          
@ListOfIdsInCSV  varchar(300)=null,                    
@IsShowList bit,              
@loggedInID BIGINT                   
AS                    
BEGIN                        
                    
	IF(LEN(@ListOfIdsInCSV)>0)                    
	BEGIN                  
		UPDATE ReleaseNotes 
		SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE()      
		WHERE
		ReleaseNoteID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))                         
	END    
	                
	IF(@IsShowList=1)                    
	BEGIN   
		EXEC GetReleaseNoteList @Title,@Description,@StartDate,@EndDate,@IsDeleted,@SortExpression,@SortType ,@FromIndex,@PageSize                  
	END                    
END