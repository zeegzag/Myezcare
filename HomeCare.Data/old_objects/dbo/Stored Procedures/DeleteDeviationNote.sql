
--CreatedBy: Akhilesh
--CreatedDate:17/3/2020
--Description: for delete deviation note
-- exec DeleteDeviationNote '30' 1

CREATE PROCEDURE [dbo].[DeleteDeviationNote]
 @ListOfIdsInCsv varchar(300),
  @loggedInID BIGINT =1
 --@SortExpression NVARCHAR(100),                          
 --@SortType NVARCHAR(10),                        
 --@FromIndex INT,                        
 --@PageSize INT,                        
                        
                        
     
AS
BEGIN
IF(LEN(@ListOfIdsInCsv)>0 )                        
BEGIN           
DELETE FROM SaveDeviationNote WHERE DeviationNoteID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv)) 
-- SELECT 1; RETURN;                
                 
END
SELECT * FROM SaveDeviationNote
END
GO
