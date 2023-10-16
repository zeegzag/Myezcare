CREATE PROCEDURE [dbo].[GetNpiDetails]      
      
      
AS       
BEGIN      
      
select TaxID = BillingProvider_REF02_ReferenceIdentification,Submitter_NM109_IdCode from OrganizationSettings        
--select top 1 CONVERT(date , RecievedTime) as RecievedTime from LatestERAs  order by RecievedTime desc      
select top 1 DATEADD(DAY, -30, CONVERT(date , ISNULL(RecievedTime,GETDATE()))) as RecievedTime from LatestERAs  order by RecievedTime desc      
END    
  
