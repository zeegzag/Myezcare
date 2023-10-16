CREATE PROCEDURE [dbo].[GetScheduleAggregatorLogs]        
  @StartDate DATE = NULL,        
  @EndDate DATE = NULL,        
  @Name VARCHAR(MAX) = NULL,        
  @Address VARCHAR(MAX) = NULL,        
  @EmployeeID BIGINT = 0,        
  @SortExpression VARCHAR(MAX),        
  @SortType VARCHAR(MAX),        
  @FromIndex INT,        
  @PageSize INT,    
  @ClaimProcessor  VARCHAR(MAX) = NULL,     
  @LastSent DATE = NULL,    
  @Status VARCHAR(MAX) = NULL,      
  @IsVisit BIT = 1  
AS        
BEGIN        
     DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()      
  
  DECLARE @IsWaitingForResponse BIT = (SELECT CASE WHEN (','+ ISNULL(@Status,'') +',' LIKE '%,-2,%') THEN 1 ELSE NULL END)  
        
  ;WITH CTEScheduleMasterList        
  AS (        
    SELECT *,        
      COUNT(t1.ScheduleID) OVER () AS Count        
    FROM (        
      SELECT ROW_NUMBER() OVER (        
          ORDER BY CASE         
              WHEN @SortType = 'ASC'        
                THEN CASE         
                    WHEN @SortExpression = 'StartDate'        
                      THEN CONVERT(DATETIME, sm.StartDate, 103)        
                    END        
              END ASC,        
            CASE         
              WHEN @SortType = 'DESC'        
                THEN CASE         
                    WHEN @SortExpression = 'StartDate'        
                      THEN CONVERT(DATETIME, sm.StartDate, 103)        
                    END        
              END DESC,        
            CASE         
              WHEN @SortType = 'ASC'        
                THEN CASE         
                    WHEN @SortExpression = 'EndDate'        
                      THEN CONVERT(DATETIME, sm.EndDate, 103)        
                    END        
              END ASC,        
            CASE         
              WHEN @SortType = 'DESC'        
                THEN CASE         
                    WHEN @SortExpression = 'EndDate'        
                      THEN CONVERT(DATETIME, sm.EndDate, 103)        
                    END        
              END DESC,        
            CASE         
              WHEN @SortType = 'ASC'        
                THEN CASE         
                    WHEN @SortExpression = 'Medicaid'        
                      THEN AGI.BeneficiaryNumber        
                    END        
              END ASC,        
            CASE         
              WHEN @SortType = 'DESC'        
                THEN CASE         
                    WHEN @SortExpression = 'Medicaid'        
                      THEN AGI.BeneficiaryNumber        
                    END        
              END DESC,        
            CASE         
              WHEN @SortType = 'ASC'        
                THEN CASE         
                    WHEN @SortExpression = 'Name'        
                      THEN r.LastName        
                    END        
              END ASC,        
            CASE         
              WHEN @SortType = 'DESC'        
                THEN CASE         
                    WHEN @SortExpression = 'Name'        
                      THEN r.LastName        
                    END        
              END DESC,        
            CASE         
              WHEN @SortType = 'ASC'        
                THEN CASE         
                    WHEN @SortExpression = 'EmployeeName'        
                      THEN E.LastName        
                    END        
              END ASC,        
            CASE         
              WHEN @SortType = 'DESC'        
                THEN CASE         
                    WHEN @SortExpression = 'EmployeeName'        
                      THEN E.LastName        
                    END        
              END DESC,        
            CASE         
              WHEN @SortType = 'ASC'        
                THEN CASE         
                    WHEN @SortExpression = 'Address'        
                      THEN c.Address        
                    END        
              END ASC,        
            CASE         
              WHEN @SortType = 'DESC'                        THEN CASE         
                    WHEN @SortExpression = 'Address'        
                      THEN c.Address        
                    END        
              END DESC,        
            CASE         
              WHEN @SortType = 'ASC'        
                THEN CASE         
                    WHEN @SortExpression = 'Aggregator'        
                      THEN SDL.Aggregator        
                    END        
              END ASC,        
            CASE         
              WHEN @SortType = 'DESC'        
                THEN CASE         
                    WHEN @SortExpression = 'Aggregator'        
                      THEN SDL.Aggregator       
                    END        
              END DESC,        
            CASE         
              WHEN @SortType = 'ASC'        
                THEN CASE         
                    WHEN @SortExpression = 'LastSent'        
                      THEN LD.LastSent     
                    END        
              END ASC,        
            CASE         
              WHEN @SortType = 'DESC'        
                THEN CASE         
                    WHEN @SortExpression = 'LastSent'        
                      THEN LD.LastSent    
                    END        
              END DESC,        
            CASE         
              WHEN @SortType = 'ASC'        
                THEN CASE         
                    WHEN @SortExpression = 'LastStatus'        
                      THEN SDL.IsSuccess      
                    END        
              END ASC,        
            CASE         
              WHEN @SortType = 'DESC'        
                THEN CASE         
                    WHEN @SortExpression = 'LastStatus'        
                      THEN SDL.IsSuccess        
                    END        
              END DESC        
          ) AS Row,        
        sm.ScheduleID,        
        sm.ReferralID,        
        sm.EmployeeID,        
        EmployeeName = dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),        
        PatAddress = c.Address + ', ' + c.City + ', ' + c.STATE + ' - ' + c.ZipCode,        
        sm.StartDate,        
        sm.EndDate,        
        AGI.BeneficiaryNumber [Medicaid],        
        dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) AS Name,        
  SDL.Aggregator,        
  LD.LastSent,        
  SDL.IsSuccess LastStatus,  
  SDL.IsWaitingForResponse,  
  CAST(ISNULL(CASE WHEN EV.EmployeeVisitID IS NOT NULL THEN 1 END, 0) AS BIT) IsVisit  
      FROM ScheduleMasters sm        
   OUTER APPLY         
   (        
  SELECT * FROM [dbo].[GetAggregatorInfo](SM.ScheduleID)         
   ) AGI        
      INNER JOIN Referrals r        
ON r.ReferralID = sm.ReferralID        
      INNER JOIN ContactMappings CM        
        ON CM.ReferralID = sm.ReferralID        
          AND CM.ContactTypeID = 1        
      INNER JOIN Contacts c        
        ON c.ContactID = CM.ContactID        
      INNER JOIN Employees E        
        ON E.EmployeeID = sm.EmployeeID        
      OUTER APPLY (        
        SELECT TOP 1 SDL.Aggregator, SDL.UpdatedDate, SDL.CreatedDate, SDL.IsSuccess, SDL.IsWaitingForResponse        
        FROM [Admin_Myezcare_Live].[dbo].[ScheduleDataEventProcessLogs] SDL        
        WHERE SDL.ScheduleID = SM.ScheduleID        
          AND SDL.OrganizationID = [dbo].[GetOrgId]()        
  ORDER BY SDL.CreatedDate DESC        
        ) SDL        
      CROSS APPLY (  
        SELECT [dbo].[GetOrgDateTime](ISNULL(SDL.UpdatedDate, SDL.CreatedDate)) AS LastSent  
      ) LD  
      LEFT JOIN [dbo].[EmployeeVisits] EV ON EV.ScheduleID = SM.ScheduleID AND EV.IsDeleted = 0  
      WHERE (sm.IsDeleted = 0)        
        AND (        
          @EmployeeID = 0        
          OR sm.EmployeeID = @EmployeeID        
          )        
        AND (        
          (        
            @Name IS NULL        
            OR LEN(@Name) = 0        
            )        
          OR (        
            (r.FirstName LIKE '%' + @Name + '%')        
            OR (r.LastName LIKE '%' + @Name + '%')        
            OR (r.FirstName + ' ' + r.LastName LIKE '%' + @Name + '%')        
            OR (r.LastName + ' ' + r.FirstName LIKE '%' + @Name + '%')        
            OR (r.FirstName + ', ' + r.LastName LIKE '%' + @Name + '%')        
            OR (r.LastName + ', ' + r.FirstName LIKE '%' + @Name + '%')        
            )        
          )        
        AND (        
          (        
            @Address IS NULL        
            OR LEN(@Address) = 0        
            )        
          OR (        
            (c.Address LIKE '%' + @Address + '%')        
            OR (c.City LIKE '%' + @Address + '%')        
            OR (c.STATE LIKE '%' + @Address + '%')        
            OR (c.ZipCode = @Address)        
            )        
          )        
        AND (        
          (        
            @StartDate IS NULL        
            OR (CONVERT(DATE, SM.StartDate) >= @StartDate)        
            )        
          AND (        
            @EndDate IS NULL        
            OR (CONVERT(DATE, SM.EndDate) <= @EndDate)        
            )        
          )  
        AND (@Status IS NULL OR   
    (SDL.CreatedDate IS NOT NULL  
     AND ((ISNULL(CAST(SDL.IsWaitingForResponse AS INT), 0) = 0 AND ISNULL(CAST(SDL.IsSuccess AS INT), -1) IN (SELECT CAST(val AS INT) FROM dbo.GetCSVTable(@Status)))  
     OR ISNULL(CAST(SDL.IsWaitingForResponse AS INT), 0) = @IsWaitingForResponse)  
    ))  
        AND (@ClaimProcessor IS NULL OR SDL.Aggregator = @ClaimProcessor)  
        AND (@LastSent IS NULL OR CONVERT(DATE, LD.LastSent) = @LastSent)  
      ) AS t1     
      WHERE (@IsVisit IS NULL OR t1.IsVisit = @IsVisit)  
    )        
  SELECT *        
  FROM CTEScheduleMasterList        
  WHERE ROW BETWEEN ((@PageSize * (@FromIndex - 1)) + 1        
          )        
      AND (@PageSize * @FromIndex)        
END