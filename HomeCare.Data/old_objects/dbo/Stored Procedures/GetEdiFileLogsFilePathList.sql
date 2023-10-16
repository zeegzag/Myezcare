CREATE procedure [dbo].[GetEdiFileLogsFilePathList]
@ListOfIdsInCSV varchar(4000)
as
select *  from EdiFileLogs where EdiFileLogID in(select val from GetCSVTable(@ListOfIdsInCSV))
