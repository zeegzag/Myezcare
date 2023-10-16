CREATE TRIGGER [dbo].[CommonNotes_DELETE]
       ON [dbo].[CommonNotes]
AFTER DELETE
AS
BEGIN
       SET NOCOUNT ON;
 
		DECLARE @CommonNoteID bigint,
		@EmployeeID bigint,
		@ReferralID bigint,
		@Note nvarchar(max),
		@IsDeleted bit,
		@CreatedBy bigint,
		@CreatedDate datetime,
		@UpdatedDate datetime,
		@UpdatedBy bigint,
		@RoleID nvarchar(50),
		@EmployeesID nvarchar(50),
		@CategoryID bigint
 
       SELECT @CommonNoteID = DELETED.CommonNoteID FROM DELETED
	   SELECT @EmployeeID = DELETED.EmployeeID FROM DELETED
	   SELECT @ReferralID = DELETED.ReferralID FROM DELETED
	   SELECT @Note = DELETED.Note FROM DELETED
	   SELECT @IsDeleted = DELETED.IsDeleted FROM DELETED
	   SELECT @CreatedBy = DELETED.CreatedBy FROM DELETED
	   SELECT @CreatedDate = DELETED.CreatedDate FROM DELETED
	   SELECT @UpdatedDate = DELETED.UpdatedDate FROM DELETED
	   SELECT @UpdatedBy = DELETED.UpdatedBy FROM DELETED
	   SELECT @RoleID = DELETED.RoleID FROM DELETED
	   SELECT @EmployeesID = DELETED.EmployeesID FROM DELETED
	   SELECT @CategoryID = DELETED.CategoryID FROM DELETED
 
       INSERT INTO JO_COMMONNOTES
       VALUES(@CommonNoteID, @EmployeeID,@ReferralID,@Note,@IsDeleted,@CreatedBy,@CreatedDate,@UpdatedDate,@UpdatedBy,@RoleID,@EmployeesID,@CategoryID,'D',GETDATE())
END