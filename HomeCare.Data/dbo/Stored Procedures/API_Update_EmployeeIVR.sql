-- =============================================
-- Author:		Sameer M.
-- Create date: 2021-07-08
-- Description:	Update IVR Pin for an employee
-- =============================================
CREATE PROCEDURE [dbo].[API_Update_EmployeeIVR]
	-- Add the parameters for the stored procedure here
	@UserName		nvarchar(50),                
	@IVRPin			varchar(max),
	@LoggedInID		bigint,
	@SystemID		varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
    
	 Update dbo.Employees Set
	 IVRPin = @IVRPin,
	 UpdatedBy = @LoggedInID,
	 UpdatedDate = getdate(),
	 SystemID = @SystemID

	 WHERE UserName=@UserName     

	 Select 1
 
END