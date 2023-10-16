--EXEC HC_GetInvoiceList @IsPatientLogin = 0, @LoggedInID = 1, @RoleID = 1, @InvoiceStatus = '', @IsDeleted = '0', @SortExpression = 'ReferralInvoiceID', @SortType = 'DESC', @FromIndex = '1', @PageSize = '50'  
--EXEC HC_GetInvoiceList @InvoiceStatus = '', @IsDeleted = '0', @SortExpression = 'ReferralInvoiceID', @SortType = 'DESC', @FromIndex = '1', @PageSize = '50'    
CREATE PROCEDURE [dbo].[HC_GetInvoiceList]
  @PatientName varchar(100) = NULL,
  @InvoiceDate date = NULL,
  @InvoiceAmount decimal = NULL,
  @PaidAmount decimal = NULL,
  @InvoiceStatus int = NULL,
  @RoleID bigint,
  @IsPatientLogin bit,
  @LoggedInID bigint,
  @IsDeleted bigint = -1,
  @SortExpression nvarchar(100),
  @SortType nvarchar(10),
  @FromIndex int,
  @PageSize int
AS
BEGIN
  ;
  WITH CTEInvoiceList
  AS (SELECT
    *,
    COUNT(T1.ReferralInvoiceID) OVER () AS Count
  FROM (SELECT
    ROW_NUMBER() OVER (ORDER BY

    CASE
      WHEN @SortType = 'DESC' THEN CASE
          WHEN @SortExpression = 'InvoiceDate' THEN InvoiceDate
        END
    END ASC,
    CASE
      WHEN @SortType = 'ASC' THEN CASE
          WHEN @SortExpression = 'InvoiceDate' THEN InvoiceDate
        END
    END DESC,

    CASE
      WHEN @SortType = 'DESC' THEN CASE
          WHEN @SortExpression = 'InvoiceAmount' THEN PayAmount
        END
    END ASC,
    CASE
      WHEN @SortType = 'ASC' THEN CASE
          WHEN @SortExpression = 'InvoiceAmount' THEN PayAmount
        END
    END DESC,

    CASE
      WHEN @SortType = 'DESC' THEN CASE
          WHEN @SortExpression = 'PaidAmount' THEN PaidAmount
        END
    END ASC,
    CASE
      WHEN @SortType = 'ASC' THEN CASE
          WHEN @SortExpression = 'PaidAmount' THEN PaidAmount
        END
    END DESC,

    CASE
      WHEN @SortType = 'DESC' THEN CASE
          WHEN @SortExpression = 'InvoiceStatus' THEN InvoiceStatus
        END
    END ASC,
    CASE
      WHEN @SortType = 'ASC' THEN CASE
          WHEN @SortExpression = 'InvoiceStatus' THEN InvoiceStatus
        END
    END DESC,

    CASE
      WHEN @SortType = 'DESC' THEN CASE
          WHEN @SortExpression = 'PatientName' THEN Rfrl.LastName
        END
    END ASC,
    CASE
      WHEN @SortType = 'ASC' THEN CASE
          WHEN @SortExpression = 'PatientName' THEN Rfrl.LastName
        END
    END DESC

    ) AS Row,
    ReferralInvoiceID,
	R.ReferralID,
    InvoiceDate,
    InvoiceAmount = ISNULL(PayAmount, 0),
    PaidAmount = ISNULL(PaidAmount, 0),
    InvoiceStatus,
    R.IsDeleted,
    PatientName = dbo.GetGeneralNameFormat(Rfrl.FirstName, Rfrl.LastName),
	CareTypeName = DD.Title,
	R.ServiceStartDate,
	R.ServiceEndDate,
	RIT.*
  FROM ReferralInvoices R
  CROSS APPLY (
	SELECT
	  SUM(RIT.ServiceTime) ServiceTime
	FROM ReferralInvoiceTransactions RIT
	WHERE RIT.ReferralInvoiceID = R.ReferralInvoiceID
	  AND RIT.IsDeleted = 0
  ) RIT
  INNER JOIN Referrals Rfrl
    ON Rfrl.ReferralID = R.ReferralID
  LEFT JOIN DDMaster DD ON DD.DDMasterID=R.CareTypeID     
  WHERE --((CAST(@IsDeleted AS BIGINT)=-1) OR R.IsDeleted=@IsDeleted)    
  R.IsDeleted = 0
  AND ((@PatientName IS NULL
  OR LEN(Rfrl.LastName) = 0)
  OR (
  (Rfrl.FirstName LIKE '%' + @PatientName + '%')
  OR (Rfrl.LastName LIKE '%' + @PatientName + '%')
  OR (Rfrl.FirstName + ' ' + Rfrl.LastName LIKE '%' + @PatientName + '%')
  OR (Rfrl.LastName + ' ' + Rfrl.FirstName LIKE '%' + @PatientName + '%')
  OR (Rfrl.FirstName + ', ' + Rfrl.LastName LIKE '%' + @PatientName + '%')
  OR (Rfrl.LastName + ', ' + Rfrl.FirstName LIKE '%' + @PatientName + '%')))
  AND ((@InvoiceStatus IS NULL
  OR LEN(@InvoiceStatus) = 0
  OR @InvoiceStatus = 0)
  OR R.InvoiceStatus = @InvoiceStatus)
  AND ((@InvoiceDate IS NULL
  OR LEN(@InvoiceDate) = 0)
  OR CONVERT(date, R.InvoiceDate) = @InvoiceDate)
  AND ((@InvoiceAmount IS NULL
  OR LEN(@InvoiceAmount) = 0)
  OR ISNULL(R.PayAmount, 0) = @InvoiceAmount)
  AND ((@PaidAmount IS NULL
  OR LEN(@PaidAmount) = 0)
  OR ISNULL(R.PaidAmount, 0) = @PaidAmount)
  --AND (@RoleID=1 OR (@RoleID=26 AND Rfrl.ReferralID=@LoggedInID))   
  AND (@RoleID = 1
  OR (@IsPatientLogin = 1
  AND Rfrl.ReferralID = @LoggedInID))) AS T1)

  SELECT
    *
  FROM CTEInvoiceList
  WHERE ROW BETWEEN ((@PageSize * (@FromIndex - 1)) + 1) AND (@PageSize * @FromIndex)
END