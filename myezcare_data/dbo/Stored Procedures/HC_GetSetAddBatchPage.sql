-- exec [HC_GetSetAddBatchPage] 144  
CREATE PROCEDURE [dbo].[HC_GetSetAddBatchPage]              
@BatchID BIGINT                        
AS                         
                       
BEGIN        
                                
select *  from  Batches where BatchID=@BatchID;         
           
select *  from BatchTypes where IsDeleted=0;        
      
select * from Payors where IsDeleted=0 AND IsBillingActive=1  AND PayorInvoiceType=1 order by PayorName;       
     
 select ServiceCodeID,      
 ServiceCode =  
 CASE WHEN DM.Title IS NOT NULL THEN DM.Title + ' - ' ELSE '' END +  
 SC.ServiceCode +          
 case          
 when SC.ModifierID is null           
 then ''          
 else         
 ' -'+          
 STUFF(              
 (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)              
 FROM Modifiers M              
 where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))              
 FOR XML PATH (''))              
 , 1, 1, '')          
 end            
 from ServiceCodes SC     
 left join DDMaster DM on DM.DDMasterID = SC.CareType        
 LEFT join Modifiers M ON M.ModifierID=SC.ModifierID AND M.IsDeleted=0        
      
END
