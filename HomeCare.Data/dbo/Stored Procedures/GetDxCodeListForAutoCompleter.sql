-- EXEC GetDxCodeListForAutoCompleter '','','10'
CREATE PROCEDURE [dbo].[GetDxCodeListForAutoCompleter]  
 -- Add the parameters for the stored procedure here  
 @SearchText VARCHAR(MAX),  
 @IgnoreIds VARCHAR(MAX),
 @PazeSize int
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 select TOP (@PazeSize) DC.*,DT.DxCodeShortName as DxCodeShortName from DXCodes DC
 inner join DxCodeTypes DT on DT.DxCodeTypeID=DC.DxCodeType
 WHERE   
   (  
    DXCodeName LIKE '%'+@SearchText+'%' OR  
	DXCodeWithoutDot LIKE '%'+@SearchText+'%' OR  
    [Description]  LIKE '%'+@SearchText+'%'  OR DT.DxCodeShortName LIKE '%'+@SearchText+'%'
   )  
   AND  
   DXCodeID NOT IN (  
   SELECT val  
   FROM [dbo].[GetCSVTable](@IgnoreIds)  
   )
   AND
   IsDeleted!=1;  
  
END