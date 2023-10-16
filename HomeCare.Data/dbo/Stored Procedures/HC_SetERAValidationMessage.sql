-- EXEC HC_SetERAValidationMessage  
CREATE PROCEDURE HC_SetERAValidationMessage  
@EraID NVARCHAR(MAX),  
@ValidationMessage NVARCHAR(MAX)  
AS  
BEGIN   
  
UPDATE LatestERAs SET ValidationMessage=@ValidationMessage WHERE EraID=@EraID  
  
  
  
  
END  
  
  