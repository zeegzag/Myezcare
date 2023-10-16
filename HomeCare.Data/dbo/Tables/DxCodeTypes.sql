CREATE TABLE [dbo].[DxCodeTypes] (
    [DxCodeTypeID]    VARCHAR (50)  NOT NULL,
    [DxCodeTypeName]  VARCHAR (100) NOT NULL,
    [DxCodeShortName] VARCHAR (50)  NULL,
    [DxCodeTypeOrder] INT           NOT NULL,
    CONSTRAINT [PK_DxCodeTypes] PRIMARY KEY CLUSTERED ([DxCodeTypeID] ASC)
);

