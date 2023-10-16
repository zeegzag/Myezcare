
CREATE procedure [dbo].[GetUpload835FilePathList]
@ListOfIdsInCSV varchar(4000)
as
select *  from Upload835Files where Upload835FileID in(select val from GetCSVTable(@ListOfIdsInCSV))
