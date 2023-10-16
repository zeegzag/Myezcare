CREATE PROCEDURE [notif].[EmployeeCovidSurvey]  
  @NotificationConfigurationID bigint,  
  @NotificationEventID bigint,  
  @FromDateTime datetime,  
  @ToDateTime datetime,  
  @BaseURL nvarchar(100)  
AS  
BEGIN  
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
  DECLARE @EmployeeCovidSurvey [notif].[EventDataTable];  
  
    ;  
    WITH QuessAns  
    AS (SELECT  
      CSQ.QuestionID,  
      CSQ.Question,  
      CASE  
        WHEN ECS.AnswersID = 1 THEN 'Yes'  
        ELSE 'No'  
      END [Answer],  
      ECS.EmployeeID,  
      ECS.CovidSurveyID,  
      ECS.CreatedDate,  
      ECS.UpdatedDate  
    FROM [dbo].[CovidSurveyQuestions] CSQ  
    INNER JOIN [dbo].[EmpCovidSurvey] ECS  
      ON ECS.QuestionID = CSQ.QuestionID  
      AND ECS.IsDeleted = 0  
    WHERE CSQ.IsDeleted = 0),  
    QA  
    AS (SELECT  
      'Q' + CONVERT(nvarchar(max), QuestionID) [ColumnName],  
      Question [ColumnValue],  
      EmployeeID,  
      CovidSurveyID,  
      CreatedDate,  
      UpdatedDate  
    FROM QuessAns  
    UNION ALL  
    SELECT  
      'A' + CONVERT(nvarchar(max), QuestionID) [ColumnName],  
      Answer [ColumnValue],  
      EmployeeID,  
      CovidSurveyID,  
      CreatedDate,  
      UpdatedDate  
    FROM QuessAns),  
    CovidSurvey  
    AS (SELECT  
      EmployeeID,  
      MIN(CovidSurveyID) CovidSurveyID,  
      MIN(CreatedDate) CreatedDate,  
      MIN(UpdatedDate) UpdatedDate,  
      MIN(Q1) Q1,  
      MIN(Q2) Q2,  
      MIN(Q3) Q3,  
      MIN(Q4) Q4,  
      MIN(A1) A1,  
      MIN(A2) A2,  
      MIN(A3) A3,  
      MIN(A4) A4  
    FROM (SELECT  
      *  
    FROM QA) t  
    PIVOT (  
    MIN(ColumnValue)  
    FOR ColumnName IN ([Q1], [Q2], [Q3], [Q4], [A1], [A2], [A3], [A4])) AS pivot_table  
    GROUP BY EmployeeID)  
  INSERT INTO @EmployeeCovidSurvey  
    SELECT  
      REF.[RefID],  
      REF.[RefTableName],  
      TD.[Data] [TemplateData],  
      NULL [DefaultRecipients]  
    FROM [CovidSurvey] ECS  
    CROSS APPLY (SELECT  
      ECS.[CovidSurveyID] [RefID],  
      '[dbo].[EmpCovidSurvey]' [RefTableName]) REF  
    LEFT JOIN [notif].[Notifications] N  
      ON N.RefID = REF.RefID  
      AND N.RefTableName = REF.RefTableName  
      AND N.NotificationEventID = @NotificationEventID  
      AND N.NotificationConfigurationID = @NotificationConfigurationID  
    CROSS APPLY (SELECT  
      dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) [EmployeeName],  
      CONVERT(varchar, ECS.CreatedDate, 0) [CreatedDate],  
      CONVERT(varchar, ECS.UpdatedDate, 0) [UpdatedDate]  
    FROM [dbo].[Employees] E  
    WHERE E.[EmployeeID] = ECS.[EmployeeID]) ED  
    CROSS APPLY (SELECT (SELECT  
        ED.[EmployeeName] [##EmployeeName##],  
        ED.[CreatedDate] [##CreatedDate##],  
        ED.[UpdatedDate] [##UpdatedDate##],  
        ECS.[Q1] [##Q1##],  
        ECS.[Q2] [##Q2##],  
        ECS.[Q3] [##Q3##],  
        ECS.[Q4] [##Q4##],  
        ECS.[A1] [##A1##],  
        ECS.[A2] [##A2##],  
        ECS.[A3] [##A3##],  
        ECS.[A4] [##A4##],  
        'Covid Survey' [##EventName##]  
        FOR JSON PATH    
      )  
      [Data]) TD  
    WHERE N.NotificationID IS NULL  
    AND (ECS.[CreatedDate] BETWEEN @FromDateTime AND @ToDateTime  
    OR ECS.[UpdatedDate] BETWEEN @FromDateTime AND @ToDateTime);  
  
  SELECT  
    *  
  FROM @EmployeeCovidSurvey;  
  
  
END