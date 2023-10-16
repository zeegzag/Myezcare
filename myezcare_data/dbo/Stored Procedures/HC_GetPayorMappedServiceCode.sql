
CREATE PROCEDURE [dbo].[HC_GetPayorMappedServiceCode]  
@PayorID BIGINT ,  
@SearchText VARCHAR(20)=NULL,        
@PageSize INT   
AS                   
BEGIN                  
   
 DECLARE @InfinateDate DATE='2099/12/31';  
 DECLARE @ServiceDate  DATE= GETDATE();  
   
        
 SELECT DISTINCT Top(@PageSize)PSM.ServiceCodeID,ServiceName,Description,ServiceCodeType,IsBillable,  
 ServiceCodeStartDate,ServiceCodeEndDate,          
 SC.IsDeleted,CheckRespiteHours,      
 ServiceCode = SC.ServiceCode +          
 CASE WHEN SC.ModifierID is null OR LEN(SC.ModifierID)=0 THEN ''          
  ELSE ' -'+          
  STUFF( (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)              
  FROM Modifiers M              
  WHERE M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))              
  FOR XML PATH ('')), 1, 1, '')          
 END  
 FROM PayorServiceCodeMapping PSM       
 INNER JOIN ServiceCodes SC on SC.ServiceCodeID=PSM.ServiceCodeID         
 LEFT JOIN Modifiers M on M.ModifierID=SC.ModifierID      
 WHERE (psm.PayorID=@PayorID) AND PSM.IsDeleted=0  AND ( (@ServiceDate >= PSM.POSStartDate AND @ServiceDate<= PSM.POSEndDate)  OR (@ServiceDate < PSM.POSStartDate))      
 AND (         
     (@SearchText IS NULL OR LEN(@SearchText)=0) OR ( (SC.ServiceCode LIKE '%' + @SearchText+ '%') OR (SC.ServiceName LIKE '%' + @SearchText+ '%') )        
 )  
  
END
