-- EXEC [GetPayorIdentificationNumber] @PayorID=12      
create PROCEDURE [dbo].[GetPayorIdentificationNumber]      
@PayorID BIGINT           
AS                       
BEGIN                      
           
      
 SELECT P.PayorIdentificationNumber 
 FROM Payors P           
 WHERE P.PayorID=@PayorID 
 AND P.IsDeleted=0
 END 