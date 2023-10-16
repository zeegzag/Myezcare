-- exec [GetPayorServiceCodeListForAutoCompleter] 'H0' 
CREATE PROCEDURE [dbo].[GetPayorServiceCodeListForAutoCompleter]          
 @SearchText VARCHAR(MAX),        
 @PageSize int=10    
AS          
BEGIN          
 
	select TOP (@PageSize)ServiceCodeID,
	SC.ModifierID,ServiceName,Description,ServiceCodeType,UnitType,    
    MaxUnit,DailyUnitLimit,PerUnitQuantity,IsBillable,HasGroupOption,
	ServiceCodeStartDate,ServiceCodeEndDate,    
    SC.IsDeleted,CheckRespiteHours,
	ServiceCode = SC.ServiceCode +    
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
	LEFT join Modifiers M ON M.ModifierID=SC.ModifierID AND M.IsDeleted=0    
	WHERE         
	(        
	ServiceCode LIKE '%'+@SearchText+'%' OR        
	ServiceName LIKE '%'+@SearchText+'%'       
	)      
	AND SC.IsDeleted=0    
END
