-- exec [HC_GetReferralPayorMappingList] 14232,null,0,-1,null,null,null,'ASC',1,10                            
CREATE PROCEDURE [dbo].[HC_GetReferralPayorMappingList] @ReferralID BIGINT = NULL,
  @PayorName NVARCHAR(50) = NULL,
  @Precedence INT = 0,
  @IsDeleted INT = - 1,
  @PayorEffectiveDate DATE = NULL,
  @PayorEffectiveEndDate DATE = NULL,
  @BeneficiaryTypeID BIGINT = 0,
  @SORTEXPRESSION VARCHAR(100),
  @SORTTYPE VARCHAR(10),
  @FROMINDEX INT,
  @PAGESIZE INT
AS
BEGIN
    ;

  WITH CTEReferralPayorMapping
  AS (
    SELECT *,
      COUNT(ReferralPayorMappingID) OVER () AS COUNT
    FROM (
      SELECT ROW_NUMBER() OVER (
          ORDER BY CASE 
              WHEN @SortType = 'ASC'
                THEN CASE 
                    WHEN @SortExpression = 'ReferralPayorMappingID'
                      THEN ReferralPayorMappingID
                    END
              END ASC,
            CASE 
              WHEN @SortType = 'DESC'
                THEN CASE 
                    WHEN @SortExpression = 'ReferralPayorMappingID'
                      THEN ReferralPayorMappingID
                    END
              END DESC,
            CASE 
              WHEN @SortType = 'ASC'
                THEN CASE 
                    WHEN @SortExpression = 'PayorName'
                      THEN PayorName
                    END
              END ASC,
            CASE 
              WHEN @SortType = 'DESC'
                THEN CASE 
                    WHEN @SortExpression = 'PayorName'
                      THEN PayorName
                    END
              END DESC,
            CASE 
              WHEN @SortType = 'ASC'
                THEN CASE 
                    WHEN @SortExpression = 'Precedence'
                      THEN Precedence
                    END
              END ASC,
            CASE 
              WHEN @SortType = 'DESC'
                THEN CASE 
                    WHEN @SortExpression = 'Precedence'
                      THEN Precedence
                    END
              END DESC,
            CASE 
              WHEN @SortType = 'ASC'
                THEN CASE 
                    WHEN @SortExpression = 'StarDate'
                      THEN CONVERT(DATE, RPM.PayorEffectiveDate, 105)
                    END
              END ASC,
            CASE 
              WHEN @SortType = 'DESC'
                THEN CASE 
                    WHEN @SortExpression = 'StarDate'
                      THEN CONVERT(DATE, RPM.PayorEffectiveDate, 105)
                    END
              END DESC,
            CASE 
              WHEN @SortType = 'ASC'
                THEN CASE 
                    WHEN @SortExpression = 'EndDate'
                      THEN CONVERT(DATE, RPM.PayorEffectiveEndDate, 105)
                    END
              END ASC,
            CASE 
              WHEN @SortType = 'DESC'
                THEN CASE 
                    WHEN @SortExpression = 'EndDate'
                      THEN CONVERT(DATE, RPM.PayorEffectiveEndDate, 105)
                    END
              END DESC
          ) AS ROW,
        RPM.ReferralPayorMappingID,
        RPM.PayorID,
        P.PayorName,
        RPM.PayorEffectiveDate,
        RPM.PayorEffectiveEndDate,
        RPM.Precedence,
        RPM.IsDeleted,
        RPM.IsPayorNotPrimaryInsured,
        RPM.InsuredId,
        RPM.InsuredFirstName,
        RPM.InsuredMiddleName,
        RPM.InsuredLastName,
        RPM.InsuredAddress,
        RPM.InsuredCity,
        RPM.InsuredState,
        RPM.InsuredZipCode,
        RPM.InsuredPhone,
        RPM.InsuredPolicyGroupOrFecaNumber,
        RPM.InsuredDateOfBirth,
        RPM.InsuredGender,
        RPM.InsuredEmployersNameOrSchoolName,
        DM.Title AS BeneficiaryType,
        RPM.BeneficiaryTypeID,
        RPM.BeneficiaryNumber,
        RPM.MemberID
      FROM ReferralPayorMappings RPM
      LEFT JOIN Referrals R
        ON R.ReferralID = RPM.ReferralID
      LEFT JOIN Payors P
        ON P.PayorID = RPM.PayorID
      LEFT JOIN DDMaster DM
        ON RPM.BeneficiaryTypeID = DM.DDMasterID
      WHERE (
          @ReferralID = (
            CASE 
              WHEN @ReferralID = 0
                THEN @ReferralID
              ELSE RPM.ReferralID
              END
            )
          )
        AND (
          (@IsDeleted = - 1)
          OR (RPM.IsDeleted = @IsDeleted)
          )
        AND (
          (@BeneficiaryTypeID = 0)
          OR (RPM.BeneficiaryTypeID = @BeneficiaryTypeID)
          )
        AND (
          (@PayorName IS NULL)
          OR (P.PayorName LIKE '%' + @PayorName + '%')
          )
        AND (
          (@Precedence = 0)
          OR RPM.Precedence = @Precedence
          )
        AND (
          (
            @PayorEffectiveDate IS NULL
            OR RPM.PayorEffectiveDate >= @PayorEffectiveDate
            )
          AND (
            @PayorEffectiveEndDate IS NULL
            OR RPM.PayorEffectiveEndDate <= @PayorEffectiveEndDate
            )
          )
      ) AS P1
    )
  SELECT *
  FROM CTEReferralPayorMapping
  WHERE ROW BETWEEN ((@PAGESIZE * (@FROMINDEX - 1)) + 1
          )
      AND (@PAGESIZE * @FROMINDEX)
END
