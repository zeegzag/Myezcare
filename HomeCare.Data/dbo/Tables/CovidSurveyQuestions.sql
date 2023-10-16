CREATE TABLE [dbo].[CovidSurveyQuestions] (
    [QuestionID]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [Question]    NVARCHAR (MAX) NULL,
    [IsDeleted]   BIT            NULL,
    [CreatedDate] DATETIME       NULL,
    [CreatedBy]   BIGINT         NULL,
    [UpdatedDate] DATETIME       NULL,
    [UpdatedBy]   BIGINT         NULL,
    CONSTRAINT [PK_CovidSurveyQuestions] PRIMARY KEY CLUSTERED ([QuestionID] ASC)
);

