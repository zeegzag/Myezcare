CREATE TABLE [dbo].[EmpCovidSurvey] (
    [CovidSurveyID] BIGINT   IDENTITY (1, 1) NOT NULL,
    [EmployeeID]    BIGINT   NULL,
    [QuestionID]    BIGINT   NULL,
    [AnswersID]     BIGINT   NULL,
    [IsDeleted]     BIT      NULL,
    [CreatedDate]   DATETIME NULL,
    [CreatedBy]     BIGINT   NULL,
    [UpdatedDate]   DATETIME NULL,
    [UpdatedBy]     BIGINT   NULL,
    CONSTRAINT [PK_EmpCovidSurvey] PRIMARY KEY CLUSTERED ([CovidSurveyID] ASC)
);

