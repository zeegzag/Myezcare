-- EXEC CheckDomainNameExists 'asapcarestage'
CREATE PROCEDURE [dbo].[CheckDomainNameExists]
@DomainName VARCHAR(100)
AS    
BEGIN
	IF EXISTS
	(
		SELECT
			1
		FROM
			Organizations O
		WHERE
			O.DomainName = @DomainName
	)
	BEGIN
		SELECT 1 AS IsSuccess
	END
	ELSE
	BEGIN
		SELECT 0 AS IsSuccess
	END
END