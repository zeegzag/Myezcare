--CreatedBy      CreatedDate      Description    
--Vishwas       30-Dec-2019      For VisitTask Cloning    
CREATE PROCEDURE [dbo].[CloneTask]
  @VisitTaskType VARCHAR(100) = NULL,
  @VisitTaskDetail VARCHAR(100) = NULL,
  @ServiceCode VARCHAR(100) = NULL,
  @VisitTaskCategoryID BIGINT = 0,
  @VisitTaskVisitTypeID BIGINT = 0,
  @VisitTaskCareTypeID BIGINT = 0,
  @IsDeleted INT = - 1,
  @SortExpression NVARCHAR(100),
  @SortType NVARCHAR(10),
  @FromIndex INT,
  @PageSize INT,
  @ListOfIdsInCsv VARCHAR(300),
  @IsShowList BIT,
  @loggedInID BIGINT,
  @TargetCareType NVARCHAR(max) = NULL,
  @TargetServiceCode NVARCHAR(max) = NULL
AS
BEGIN
  SET @ServiceCode = CAST(@TargetServiceCode AS INT)
  SET @VisitTaskCareTypeID = CAST(@TargetCareType AS INT)

  IF (LEN(@ListOfIdsInCsv) > 0)
  BEGIN
    INSERT INTO VisitTasks (
      [VisitTaskType],
      [VisitTaskDetail],
      [IsDeleted],
      [CreatedDate],
      [CreatedBy],
      [UpdatedDate],
      [UpdatedBy],
      [SystemID],
      [ServiceCodeID],
      [IsRequired],
      [MinimumTimeRequired],
      [IsDefault],
      [VisitTaskCategoryID],
      [VisitTaskSubCategoryID],
      [SendAlert],
      [VisitType],
      [CareType],
      [Frequency],
      [TaskCode]
      )
    SELECT VisitTaskType,
      VisitTaskDetail,
      IsDeleted,
      GETDATE(),
      CreatedBy,
      GETDATE(),
      UpdatedBy,
      SystemID,
      @ServiceCode,
      IsRequired,
      MinimumTimeRequired,
      IsDefault,
      VisitTaskCategoryID,
      VisitTaskSubCategoryID,
      SendAlert,
      VisitType,
      @VisitTaskCareTypeID,
      Frequency,
      TaskCode
    FROM VisitTasks
    WHERE VisitTaskID IN (
        SELECT CAST(Val AS VARCHAR(100))
        FROM GetCSVTable(@ListOfIdsInCsv)
        )
  END

  SELECT *
  FROM VisitTasks
  WHERE VisitTaskID IN (
      SELECT CAST(Val AS VARCHAR(100))
      FROM GetCSVTable(@ListOfIdsInCsv)
      )
    --IF(@IsShowList=1)                
    --BEGIN                
    --	EXEC GetVisitTaskList @VisitTaskType,@VisitTaskDetail,@ServiceCode,@VisitTaskCategoryID,@VisitTaskVisitTypeID,@VisitTaskCareTypeID,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize                
    --END  
END
