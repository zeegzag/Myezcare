CREATE TABLE [dbo].[AdminReps] (
    [AdminID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR (100) NULL,
    [LastName]  NVARCHAR (100) NULL,
    [Email]     NVARCHAR (200) NULL,
    [UserName]  NVARCHAR (200) NULL,
    [Password]  NVARCHAR (200) NULL,
    [IsActive]  BIT            NULL,
    [RoleID]    BIGINT         NULL,
    CONSTRAINT [PK_Admin] PRIMARY KEY CLUSTERED ([AdminID] ASC)
);

