CREATE TABLE [dbo].[JO_FrequencyCodes] (
    [JO_FrequencyCodeID]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [FrequencyCodeID]     BIGINT        NOT NULL,
    [Code]                VARCHAR (50)  NOT NULL,
    [Description]         VARCHAR (500) NOT NULL,
    [DefaultScheduleDays] INT           NULL,
    [Action]              CHAR (1)      NOT NULL,
    [ActionDate]          DATETIME      NOT NULL,
    CONSTRAINT [PK_JO_FrequencyCodes] PRIMARY KEY CLUSTERED ([JO_FrequencyCodeID] ASC)
);

