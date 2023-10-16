CREATE procedure [dbo].[GetEDIFileDeleteList]  
@Days int  
as  
SELECT  DATEDIFF(day,CreatedDate,getdate()),* FROM  EdiFilelogs where   EdiFileTypeID in(3,4) AND  (DATEDIFF(day,CreatedDate,getdate()) >@Days)




