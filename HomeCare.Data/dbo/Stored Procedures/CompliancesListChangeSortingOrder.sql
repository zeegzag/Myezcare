--   CompliancesListChangeSortingOrder  -5,2,1,      
create PROC [dbo].[CompliancesListChangeSortingOrder]      
@ComplianceID BIGINT=0,      
@originID BIGINT=0,      
@destinationID BIGINT=0      
      
      
AS      
BEGIN   
declare @Temp BIGINT=0      
declare @DestComplianceID BIGINT=0      
set @Temp=@destinationID      
select @DestComplianceID=ComplianceID from Compliances where SortingID=@destinationID      
      
UPDATE Compliances set SortingID=@destinationID where SortingID=@originID and ComplianceID=@ComplianceID      
UPDATE Compliances set SortingID=@originID where SortingID=@destinationID and ComplianceID=@DestComplianceID      
  --RETURN SELECT 1;    
END      
      
    --SELECT * FROM Compliances ORDER BY SortingID 
GO

