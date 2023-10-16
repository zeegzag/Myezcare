

--CreatedBy: Chirag Jagad
--CreatedDate:06 Feb 2020
--Description: For get report master data by Id
-- exec GetReportMasterById 1
CREATE PROCEDURE [dbo].[GetReportMasterById]
@ReportId bigint=0
AS
BEGIN
SELECT * FROM [ReportMaster] 
WHERE ReportID = @ReportId AND IsDeleted = 0 AND IsActive = 1
END