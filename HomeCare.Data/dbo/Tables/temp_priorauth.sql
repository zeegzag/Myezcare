CREATE TABLE [dbo].[temp_priorauth] (
    [FirstName]                 NVARCHAR (50) NOT NULL,
    [last_name]                 NVARCHAR (50) NOT NULL,
    [Medicaid]                  VARCHAR (MAX) NOT NULL,
    [Prior_Auth]                VARCHAR (MAX) NOT NULL,
    [Diagonse_Code]             NVARCHAR (50) NOT NULL,
    [Service_Code]              NVARCHAR (50) NOT NULL,
    [Modifier]                  NVARCHAR (50) NULL,
    [StartDate]                 DATETIME2 (7) NOT NULL,
    [Endate]                    DATETIME2 (7) NOT NULL,
    [Rate]                      VARCHAR (MAX) NOT NULL,
    [Type_Visit_Time_Flat_Rate] VARCHAR (MAX) NULL,
    [svccodeid]                 INT           NULL,
    [modifierid]                INT           NULL
);

