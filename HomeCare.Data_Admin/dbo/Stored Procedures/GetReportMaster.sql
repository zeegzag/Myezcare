--CreatedBy                     UpdatedDate                   Description      
--Vishwas                      29/feb/2020               For get reportmaster list     
-- exec [GetCareType] '4296',''      
CREATE PROCEDURE [dbo].[GetReportMaster]     
          
AS           
BEGIN          
    
select * from ReportMaster where IsDeleted=0    
          
END