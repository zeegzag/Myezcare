CREATE PROCEDURE [dbo].[GetAddProcess277Model]
AS
BEGIN

SELECT Name=PayorName, Value=PayorID FROM Payors P WHERE  P.IsDeleted=0 AND IsBillingActive=1 ORDER BY  ShortName ASC

END
