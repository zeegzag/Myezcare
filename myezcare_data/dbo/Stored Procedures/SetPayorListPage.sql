  
CREATE PROCEDURE [dbo].[SetPayorListPage]
AS
BEGIN
 SELECT DISTINCT PayorTypeID,PayorTypeName FROM PayorTypes order by PayorTypeName ASC
 SELECT 0;  
 SELECT 0;  
END 
