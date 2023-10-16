CREATE TRIGGER [dbo].[CommonNotes_UPDATE]
       ON [dbo].[CommonNotes]
AFTER UPDATE
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

 
       SELECT @CommonNoteID = INSERTED.CommonNoteID FROM INSERTED
	   SELECT @EmployeeID = INSERTED.EmployeeID FROM INSERTED
	   SELECT @ReferralID = INSERTED.ReferralID FROM INSERTED
	   SELECT @Note = INSERTED.Note FROM INSERTED
	   SELECT @IsDeleted = INSERTED.IsDeleted FROM INSERTED
	   SELECT @CreatedBy = INSERTED.CreatedBy FROM INSERTED
	   SELECT @CreatedDate = INSERTED.CreatedDate FROM INSERTED
	   SELECT @UpdatedDate = INSERTED.UpdatedDate FROM INSERTED
	   SELECT @UpdatedBy = INSERTED.UpdatedBy FROM INSERTED
	   SELECT @RoleID = INSERTED.RoleID FROM INSERTED
	   SELECT @EmployeesID = INSERTED.EmployeesID FROM INSERTED
	   SELECT @CategoryID = INSERTED.CategoryID FROM INSERTED
 
       INSERT INTO JO_COMMONNOTES
       VALUES(@CommonNoteID, @EmployeeID,@ReferralID,@Note,@IsDeleted,@CreatedBy,@CreatedDate,@UpdatedDate,@UpdatedBy,@RoleID,@EmployeesID,@CategoryID,'U',GETDATE())
END