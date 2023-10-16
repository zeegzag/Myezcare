CREATE PROCEDURE [dbo].[SetAddParentPage]   
@ContactID BIGINT=0  
AS  
BEGIN  
 SELECT * FROM Contacts WHERE ContactID=@ContactID;  
 SELECT * FROM Languages ORDER BY Name ASC;  
 SELECT * FROM ContactTypes WHERE IsDeleted=0 Order BY OrderNumber
END
