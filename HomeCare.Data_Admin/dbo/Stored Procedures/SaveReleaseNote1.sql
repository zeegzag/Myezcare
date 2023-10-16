
CREATE PROCEDURE [dbo].[SaveReleaseNote1] 
@ReleaseNoteID BIGINT,
@Title NVARCHAR(200),
@Description NVARCHAR(MAX),
@StartDate DATETIME,
@EndDate DATETIME,
@loggedInUserId BIGINT,
@SystemID NVARCHAR(100)
AS
BEGIN

	IF EXISTS (SELECT TOP 1 ReleaseNoteID FROM ReleaseNotes WHERE ((@StartDate BETWEEN StartDate AND EndDate) OR (@EndDate BETWEEN StartDate AND EndDate)) AND ReleaseNoteID != @ReleaseNoteID)  
	BEGIN                  
	SELECT -1 RETURN;                    
	END 
	
	IF(@ReleaseNoteID=0)                        
	BEGIN                        
		INSERT INTO ReleaseNotes                        
		(Title,Description,StartDate,EndDate,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,IsDeleted)
		VALUES                        
		(@Title,@Description,@StartDate,@EndDate,@loggedInUserId,GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@SystemID,0);      
	END                        
	ELSE                        
	BEGIN                        
		UPDATE ReleaseNotes                         
		SET                              
		Title=@Title,                        
		Description=@Description,              
		StartDate=@StartDate,          
		EndDate=@EndDate,          
		UpdatedBy=@loggedInUserId,                        
		UpdatedDate=GETUTCDATE(),                        
		SystemID=@SystemID      
		WHERE ReleaseNoteID=@ReleaseNoteID;                        
	END                        
		
	SELECT 1; RETURN;      

END