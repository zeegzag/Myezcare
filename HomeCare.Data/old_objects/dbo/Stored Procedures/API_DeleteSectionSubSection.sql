CREATE PROCEDURE [dbo].[API_DeleteSectionSubSection]      
@ComplianceID BIGINT,
@ServerCurrentDateTime DATETIME,
@LoggedInID BIGINT,
@SystemID VARCHAR(MAX)
AS                              
BEGIN
  UPDATE Compliances SET IsDeleted=1,UpdatedBy=@LoggedInID,UpdatedDate=@ServerCurrentDateTime,SystemID=@SystemID    
  WHERE ComplianceID=@ComplianceID

  SELECT 1;
END
