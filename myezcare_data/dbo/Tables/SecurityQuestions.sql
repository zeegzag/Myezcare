CREATE TABLE [dbo].[SecurityQuestions] (
    [SecurityQuestionID] BIGINT        NOT NULL,
    [Question]           VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_SecurityQuestions] PRIMARY KEY CLUSTERED ([SecurityQuestionID] ASC)
);

