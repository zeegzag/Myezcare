-- EXEC GetNotePageServiceCode @ReferralID = '3681', @ServiceCodeTypeID = '1', @ServiceDate = '2018/04/29', @SearchText = '', @LoggedInId = '1', @CredentialBHP = '', @PageSize = '10'
CREATE PROCEDURE [dbo].[GetNotePageServiceCode]    
@ReferralID bigint,    
@ServiceCodeTypeID int,    
@ServiceDate date,    
@SearchText varchar(20)=null,    
@LoggedInId bigint, 
@CredentialBHP varchar(MAX),
@PageSize int    
AS    
BEGIN    

	DECLARE @EmployeeCredentialBHP VARCHAR(MAX) 
	SELECT @EmployeeCredentialBHP=CredentialID FROM Employees WHERE EmployeeID=@LoggedInId 


    Declare @PayorID bigint =0    
    select @PayorID=PayorID from ReferralPayorMappings where ReferralID=@ReferralID  AND IsActive=1  


    
 SELECT DISTINCT Top(@PageSize) PSM.ServiceCodeID,SC.ServiceCode + ISNULL(' - '+M.ModifierCode,'') as ServiceCode,SC.ModifierID, ISNULL(M.ModifierName,'') AS ModifierName,
 ServiceName,Description,ServiceCodeType,psm.UnitType,  
 psm.MaxUnit,psm.DailyUnitLimit,psm.PerUnitQuantity,IsBillable,HasGroupOption,ServiceCodeStartDate,ServiceCodeEndDate, sc.RandomGroupID, 
      SC.IsDeleted  
      from PayorServiceCodeMapping psm    
 INNER JOIN ServiceCodes sc on sc.ServiceCodeID=psm.ServiceCodeID     
 LEFT JOIN Modifiers M on M.ModifierID=SC.ModifierID  
 WHERE (psm.PayorID=@PayorID) AND psm.IsDeleted=0  AND (@ServiceDate >= psm.POSStartDate and @ServiceDate<= psm.POSEndDate)    
 AND (@ServiceDate >= sc.ServiceCodeStartDate and @ServiceDate<= sc.ServiceCodeEndDate)  AND sc.ServiceCodeType=@ServiceCodeTypeID    
 AND (     
     (@SearchText IS NULL) OR ( (SC.ServiceCode LIKE '%' + @SearchText+ '%') OR (SC.ServiceName LIKE '%' + @SearchText+ '%') )    
 )  
  AND ( 
          (@EmployeeCredentialBHP IS NULL) OR 
		  ( 
		    ( @EmployeeCredentialBHP=@CredentialBHP AND (ModifierCode IS NULL OR ModifierCode!='HN')) OR 
		    ( @EmployeeCredentialBHP!=@CredentialBHP AND (ModifierCode IS NULL OR ModifierCode!='HO') )
		  )  
	  )    
 
 PRINT @EmployeeCredentialBHP
 PRINT @CredentialBHP
 PRINT @LoggedInId
    
 
END 

-- BHPP-2, BHT-4, BHP-7
-- SELECT EmployeeID, CredentialID FROM Employees WHERE EmployeeID=1 
-- EXEC GetNotePageServiceCode @ReferralID = '2892', @ServiceCodeTypeID = '1', @ServiceDate = '2017/06/09', @SearchText = '', @LoggedInId = '2', @CredentialBHP = 'BHP', @PageSize = '1000'