--CreatedBy: Chirag Jagad  
--CreatedDate:02 June 2020  
--Description: For get Report Master list  
-- exec GetReportMasterList ''  
CREATE PROCEDURE [dbo].[GetReportMasterList]  
AS  
BEGIN  
SELECT * FROM ReportMaster   
WHERE IsDeleted = 0  
ORDER BY ReportID DESC;  
END  
 -- SELECT * FROM ReportMaster