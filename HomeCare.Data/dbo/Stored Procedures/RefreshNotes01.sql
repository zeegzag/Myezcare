CREATE PROCEDURE [dbo].[RefreshNotes01]
AS
BEGIN 

BEGIN TRY  

 EXEC CreateChildGroupNotes
 EXEC RefreshBatchNotes
 SELECT 1;
END TRY
BEGIN CATCH
  SELECT 0;
END CATCH

                             
END