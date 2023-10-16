CREATE PROCEDURE [dbo].[GetPayorServiceCodeMappingList]
	@PayorID bigint=null,
	@ServiceCode varchar(30)=null,
	@ModifierID bigint=0,
	@PosID bigint=0,
	@POSStartDate date=null,
	@POSEndDate date=null,
	@IsDeleted BIGINT = -1,
	@SORTEXPRESSION VARCHAR(100),
	@SORTTYPE VARCHAR(10),
	@FROMINDEX INT,
	@PAGESIZE INT
AS                                        
BEGIN                                          
	;WITH CTEPayor AS                                    
	(                                         
		SELECT *,COUNT(P1.PayorServiceCodeMappingID) OVER() AS COUNT FROM                                    
		(                                        
			SELECT ROW_NUMBER() OVER (ORDER BY
				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PayorServiceCodeMappingID' THEN PSM.PayorServiceCodeMappingID END END ASC,
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PayorServiceCodeMappingID' THEN PSM.PayorServiceCodeMappingID END END DESC,

				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'POSStartDate' THEN  CONVERT(date, PSM.POSStartDate, 105) END END ASC,
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'POSStartDate' THEN  CONVERT(date, PSM.POSStartDate, 105) END END DESC,
            
				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'POSEndDate' THEN  CONVERT(date, PSM.POSEndDate, 105) END END ASC,
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'POSEndDate' THEN  CONVERT(date, PSM.POSEndDate, 105) END END DESC,
            
				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Rate' THEN CAST(PSM.Rate AS decimal)  END END ASC,
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Rate' THEN CAST(PSM.Rate AS decimal)  END END DESC,
        
				CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'BillingUnitLimit' THEN CAST(PSM.BillingUnitLimit AS INT)  END END ASC,
				CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'BillingUnitLimit' THEN CAST(PSM.BillingUnitLimit AS INT)  END END DESC,

				CASE WHEN @SORTTYPE = 'ASC' THEN
					CASE
						WHEN @SORTEXPRESSION = 'ServiceCode' THEN SC.ServiceCode
						WHEN @SORTEXPRESSION = 'ModifierName' THEN M.ModifierName
						WHEN @SORTEXPRESSION = 'PosName' THEN POS.PosName
					END
				END ASC,
				CASE WHEN @SORTTYPE = 'DESC' THEN
					CASE
						WHEN @SORTEXPRESSION = 'ServiceCode' THEN SC.ServiceCode
						WHEN @SORTEXPRESSION = 'ModifierName' THEN M.ModifierName
						WHEN @SORTEXPRESSION = 'PosName' THEN POS.PosName
						--WHEN @SortExpression = 'Rate' THEN CAST(PSM.Rate AS decimal)
					END
				END DESC                                      
			) AS ROW,SC.CareType,PSM.PayorServiceCodeMappingID,PSM.ModifierID,PSM.Rate,PSM.POSID,M.ModifierName,POS.PosName,PSM.POSStartDate,PSM.POSEndDate,SC.ServiceName,
			SC.ServiceCodeID,SC.ServiceCode,SC.Description,SC.ServiceCodeType,SC.UnitType,SC.MaxUnit,SC.DailyUnitLimit,SC.PerUnitQuantity,SC.IsBillable,SC.HasGroupOption,
			SC.ServiceCodeStartDate,SC.ServiceCodeEndDate,P.PayorName,P.PayorID,PSM.IsDeleted, PSM.BillingUnitLimit,
			ServiceCodeExpired=Case When (GetDate()-1 > SC.ServiceCodeEndDate) then 1 else 0 end,
			ServiceCodeMappingExpired=Case When (GetDate()-1  > PSM.POSEndDate) then 1 else 0 end                 
			FROM  PayorServiceCodeMapping PSM
			left Join ServiceCodes SC on PSM.ServiceCodeID=SC.ServiceCodeID
			left Join Payors P on P.PayorID=PSM.PayorID
			left Join Modifiers M on m.ModifierID=psm.ModifierID
			left Join PlaceOfServices POS on POS.PosID=psm.PosID
			WHERE((@IsDeleted=-1) OR (PSM.IsDeleted=@IsDeleted))
			AND ((@ModifierID=0) or PSM.ModifierID=@ModifierID)
			AND (@PosID = 0 or PSM.PosID= @PosID)
			AND (@PayorID =(case when @PayorID=0 then @PayorID else PSM.PayorID  end))
			AND ((@ServiceCode IS NULL) OR (SC.ServiceCode LIKE '%' + @ServiceCode+ '%'))
			AND ((@POSStartDate is null OR PSM.POSStartDate >= @POSStartDate) and (@POSEndDate is null OR PSM.POSEndDate<= @POSEndDate))
		) AS P1
	)
	SELECT * FROM CTEPayor WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)
END