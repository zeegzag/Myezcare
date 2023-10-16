-- EXEC  HC_RefreshAndGroupingNotes        
CREATE PROCEDURE [dbo].[HC_RefreshAndGroupingNotes]
AS
BEGIN

  BEGIN TRY

    EXEC HC_CreateChildGroupNotes
    EXEC HC_RefreshBatchNotes

    SELECT
      1;
  END TRY
  BEGIN CATCH
    SELECT
      0;
  END CATCH


END