-- =============================================  
-- Author:  Kundan Kumar Rai  
-- Create date: 2 June 2020  
-- Description: To fetch modifiers by Service Code  
-- =============================================  
CREATE PROCEDURE [dbo].[HC_GetModifierByServiceCode]  
 @ServiceCodeID bigint  
AS  
BEGIN  
IF(@ServiceCodeID>0)
BEGIN
 SELECT M.ModifierID,M.ModifierName,M.ModifierCode  
  FROM Modifiers M INNER JOIN ServiceCodes SC ON M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))  
  WHERE SC.ServiceCodeID=@ServiceCodeID 
  END
ELSE
BEGIN
SELECT M.ModifierID,M.ModifierName,M.ModifierCode  
  FROM Modifiers M INNER JOIN ServiceCodes SC ON M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))  
 -- WHERE SC.ServiceCodeID=@ServiceCodeID 
END
END