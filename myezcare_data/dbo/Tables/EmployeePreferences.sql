CREATE TABLE [dbo].[EmployeePreferences] (
    [EmployeePreferenceID] BIGINT IDENTITY (1, 1) NOT NULL,
    [EmployeeID]           BIGINT NOT NULL,
    [PreferenceID]         BIGINT NOT NULL,
    CONSTRAINT [PK_EmployeePreferences] PRIMARY KEY CLUSTERED ([EmployeePreferenceID] ASC),
    CONSTRAINT [FK_EmployeePreferences_Employees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID]),
    CONSTRAINT [FK_EmployeePreferences_Preferences] FOREIGN KEY ([PreferenceID]) REFERENCES [dbo].[Preferences] ([PreferenceID])
);

