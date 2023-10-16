CREATE PROCEDURE [dbo].[GetEdi277FileForProcess]
@GetUpload277FileProcessStatus INT,
@SetUpload277FileProcessStatus INT
AS
BEGIN

 SELECT * FROM EDI277Files WHERE Upload277FileProcessStatus=@GetUpload277FileProcessStatus
 UPDATE EDI277Files SET Upload277FileProcessStatus=@SetUpload277FileProcessStatus WHERE Upload277FileProcessStatus=@GetUpload277FileProcessStatus

END