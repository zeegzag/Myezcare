  
--CreatedBy Chirag Jagad  
-- CreatedDate: 06/02/2020  
--Description: Form Delete report master  
-- Exec DeleteReportMaster 1  
CREATE PROCEDURE [dbo].[DeleteReportMaster]      
@ReportID BIGINT,      
@IsDeleted BIT      
AS                                
 BEGIN                                
  UPDATE ReportMaster SET IsDeleted = @IsDeleted WHERE ReportID = @ReportID      
  SELECT 1; RETURN;      
END