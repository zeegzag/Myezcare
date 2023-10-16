-- exec [HC_GetPayorServiceCodeListForAutoCompleter] 'test'               
CREATE PROCEDURE [dbo].[HC_GetPayorServiceCodeListForAutoCompleter]                        
 @SearchText VARCHAR(MAX),                      
 @PageSize int=10                  
AS                        
BEGIN                        
 SELECT TOP (@PageSize)        
 ServiceCodeID,ServiceName,Description,IsBillable,        
 ServiceCode = SC.ServiceCode +            
    CASE WHEN (SC.ModifierID IS NULL OR SC.ModifierID='' ) THEN '' ELSE ' -'+                  
    STUFF(                      
    (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                      
    FROM Modifiers M  where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID)) AND M.IsDeleted=0                     
    FOR XML PATH ('')), 1, 1, '')                  
    END                    
 FROM ServiceCodes SC             
 LEFT JOIN Modifiers M ON M.ModifierID=SC.ModifierID AND M.IsDeleted=0                  
 WHERE                       
 (                      
 ServiceCode LIKE '%'+@SearchText+'%' OR                      
 ServiceName LIKE '%'+@SearchText+'%' --OR        
 )                    
 AND SC.IsDeleted=0                  
END
