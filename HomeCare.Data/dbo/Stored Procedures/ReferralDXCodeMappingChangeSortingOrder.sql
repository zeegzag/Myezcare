       
CREATE PROC [dbo].[ReferralDXCodeMappingChangeSortingOrder]        
@ReferralDXCodeMappingID BIGINT=0,        
@originID BIGINT=0,        
@destinationID BIGINT=0        
        
        
AS        
BEGIN     
declare @Temp BIGINT=0        
declare @DestComplianceID BIGINT=0        
set @Temp=@destinationID        
select @DestComplianceID=ReferralDXCodeMappingID from ReferralDXCodeMappings where Precedence=@destinationID        
        
UPDATE ReferralDXCodeMappings set Precedence=@destinationID where Precedence=@originID and ReferralDXCodeMappingID=@ReferralDXCodeMappingID        
UPDATE ReferralDXCodeMappings set Precedence=@originID where Precedence=@destinationID and ReferralDXCodeMappingID=@DestComplianceID        
  --RETURN SELECT 1;      
END        
        
    --SELECT * FROM Compliances ORDER BY SortingID  