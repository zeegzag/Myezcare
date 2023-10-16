-- exec [GetSetAddBatchPage] 0
CREATE PROCEDURE [dbo].[GetSetAddBatchPage]        
@BatchID BIGINT                  
AS                   
                 
BEGIN                            
select *  from  Batches where BatchID=@BatchID;        
select *  from BatchTypes where IsDeleted=0;  
select * from Payors where IsDeleted=0 AND IsBillingActive=1  order by PayorName;        
SELECT FacilityID as BillingProviderID, FacilityName From  Facilities WHERE ParentFacilityID=0 AND IsDeleted=0  ORDER BY FacilityName ASC  
select ServiceCodeID,ServiceCode from ServiceCodes 
--SELECT ServiceCodeID,ServiceCode = ServiceCode + CASE WHEN M.ModifierCode IS NOT NULL THEN ' : '+M.ModifierCode ELSE '' END  FROM ServiceCodes SC LEFT JOIN Modifiers M ON M.ModifierID=SC.ModifierID  
END