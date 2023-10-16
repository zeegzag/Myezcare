
-- EXEC  HC_RefreshAndGroupingNotes        
CREATE PROCEDURE [dbo].[HC_RefreshAndGroupingNotes]
    @ResultRequired bit = 1
AS
BEGIN

  BEGIN TRY

    EXEC HC_CreateChildGroupNotes
    EXEC HC_RefreshBatchNotes

    IF (@ResultRequired = 1)              
    BEGIN              
      SELECT
        1;              
    END
  END TRY
  BEGIN CATCH
    IF (@ResultRequired = 1)              
    BEGIN              
      SELECT
        0;              
    END
  END CATCH


END