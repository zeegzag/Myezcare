/*
Created by : Pallav Saxena
Create Date : 12/19/2019
Purose: Get the name of the organization by twillio number, there are messages that come in as notifications where the users notify why can't they clock in or clock out and need to get that looked into but the agency name in the 
notification is missing so this stored procedure is created as a stop gap.
*/

CREATE PROCEDURE GetOrgNameFromTwillioNumber(@PHONENUMBER VARCHAR(20) )
AS
BEGIN
DECLARE @ORGDBNAME NVARCHAR(100)
DECLARE @ORGID INT
DECLARE @STRSQL NVARCHAR(MAX)
--DECLARE @PHONENUMBER VARCHAR(20) ='7576901781'
DECLARE @RC INT
DECLARE @SITENAME NVARCHAR(50)
DECLARE @ParmDefinition nvarchar(500);
DECLARE CURORG  CURSOR FOR SELECT ORGANIZATIONID FROM ORGANIZATIONS WHERE ISACTIVE=1 AND ISDELETED=0
OPEN CURORG
FETCH NEXT FROM CURORG INTO @ORGID

WHILE @@FETCH_STATUS = 0  
    BEGIN
		SELECT @ORGDBNAME=DBNAME FROM ORGANIZATIONS  WHERE ORGANIZATIONID=@ORGID
		SET @STRSQL ='SELECT @retvalOUT = [SiteName] FROM '+@ORGDBNAME+'.[dbo].[OrganizationSettings] WHERE TwilioFromNo='''+@PHONENUMBER+''''
		SET @ParmDefinition = N'@retvalOUT NVARCHAR(50) OUTPUT';

		--PRINT @STRSQL
		EXEC  SP_EXECUTESQL @STRSQL, @ParmDefinition, @retvalOUT=@SITENAME OUTPUT;
		SET @RC=@@ROWCOUNT
		IF @RC>0 BEGIN SELECT @SITENAME END --ELSE SELECT 'NO ORG FOUND'
        FETCH NEXT FROM CURORG  INTO @ORGID  
    END;

		
CLOSE CURORG;
DEALLOCATE CURORG;
END