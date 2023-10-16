-- exec GetServiceCodeListForAutoCompleter '',12    
CREATE PROCEDURE [dbo].[GetServiceCodeListForAutoCompleter]      
 @SearchText VARCHAR(MAX),    
 @PageSize int=10,
 @ServiceCodeTypeID bigint=0,
 @LoggedInId bigint,
 @CredentialBHP varchar(MAX)        
AS      
BEGIN      
 SET NOCOUNT ON;     

 DECLARE @EmployeeCredentialBHP VARCHAR(MAX) 
 SELECT @EmployeeCredentialBHP=CredentialID FROM Employees WHERE EmployeeID=@LoggedInId 

  
 select TOP (@PageSize)ServiceCodeID,ServiceCode + ISNULL(M.ModifierCode,'') as ServiceCode,SC.ModifierID,ISNULL(M.ModifierName,'') AS ModifierName,
 ServiceName,Description,ServiceCodeType,UnitType,MaxUnit,DailyUnitLimit,PerUnitQuantity,IsBillable,HasGroupOption,ServiceCodeStartDate,ServiceCodeEndDate,  
 SC.IsDeleted,CheckRespiteHours  from ServiceCodes SC   
 LEFT join Modifiers M ON M.ModifierID=SC.ModifierID AND M.IsDeleted=0  
 WHERE       
   (      
    ServiceCode LIKE '%'+@SearchText+'%' OR      
    ServiceName LIKE '%'+@SearchText+'%'     
   )    
   AND   GetDate() < SC.ServiceCodeEndDate AND SC.IsDeleted=0  
   AND (@ServiceCodeTypeID=0 OR SC.ServiceCodeType=@ServiceCodeTypeID)  
   
   AND ( 
          (@EmployeeCredentialBHP IS NULL) OR 
		  ( 
		    ( @EmployeeCredentialBHP=@CredentialBHP AND (ModifierCode IS NULL OR ModifierCode!='HN')) OR 
		    ( @EmployeeCredentialBHP!=@CredentialBHP AND (ModifierCode IS NULL OR ModifierCode!='HO') )
		  )  
	  )    


END