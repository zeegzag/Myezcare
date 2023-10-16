CREATE PROCEDURE [dbo].[HC_GetSavedHtmlFormContent]
@EbriggsFormMppingID BIGINT=NULL
AS      
BEGIN      

 SELECT Value=HTMLFormContent FROm EbriggsFormMppings WHERE EbriggsFormMppingID=@EbriggsFormMppingID
    
      
END