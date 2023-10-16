--CreatedBy: Abhishek Gautam
--CreatedDate: 10 sept 2020
--Description: Delete DMAS97AB form

CREATE PROC [dbo].[DeleteDMAS97AB]
@Dmas97ID BIGINT=0
AS
BEGIN

UPDATE DMAS97AB SET IsDeleted=1 WHERE Dmas97ID=@Dmas97ID
SELECT 1 RETURN;
--SELECT * FROM DMAS97AB where IsDeleted=0
END