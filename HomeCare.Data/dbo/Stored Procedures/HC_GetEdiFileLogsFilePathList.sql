CREATE Procedure [dbo].[HC_GetEdiFileLogsFilePathList]  
@ListOfIdsInCSV varchar(4000)  
AS
BEGIN

select *  from EdiFileLogs where EdiFileLogID in(select val from GetCSVTable(@ListOfIdsInCSV))  

END