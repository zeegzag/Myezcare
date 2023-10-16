CREATE proc [dbo].[HC_GetPdfFormPath]  
(  
 @EBFormID nvarchar(1000)  
)  
AS  
BEGIN  
 declare @PdfPath nvarchar(max)  
  
 SELECT @PdfPath = InternalFormPath FROM dbo.EBForms  
 WHERE  FormId = @EBFormid 
  
 SELECT @PdfPath AS FormPath  
END