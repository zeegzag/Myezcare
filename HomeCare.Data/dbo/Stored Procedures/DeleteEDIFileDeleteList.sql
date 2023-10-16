CREATE procedure [dbo].[DeleteEDIFileDeleteList]  
@Days int  
as  
delete  FROM  EdiFilelogs where   EdiFileTypeID in(3,4) AND  (DATEDIFF(day,CreatedDate,getdate()) > @Days)