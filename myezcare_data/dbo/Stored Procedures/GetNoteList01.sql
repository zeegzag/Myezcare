-- SP_HELPTEXT GetNoteList
CREATE PROCEDURE [dbo].[GetNoteList01]
	@IsBillable int= -1,
	@AHCCCSID varchar(100) = NULL,	
	@CISNumber varchar(50) = NULL,
	--@ServiceCodeID int = 0,		
	@ServiceCodeIDs varchar(MAX),	
	@ReferralID bigint =0,
	@ServiceDateStart date=null,
	@ServiceDateEnd date=null,	
	@ServiceCodeTypeID int=-1,
	@NoteKind varchar(30)=null,
	@IsCompleted int=-1,
	@SearchText varchar(50)=null,
	@BatchID bigint=0,
	@NoteID bigint=0,
	@BillingProviderID bigint=0,
	@RegionID bigint=0,
	@DepartmentID bigint=0,
	@PayorID bigint=0,
	@CreatedByIDs varchar(MAX),	
	@AssigneeID bigint=0,	
	@IsDeleted BIGINT =-1,
	@AllowEditStatuses varchar(200)=null,
	@SortExpression VARCHAR(100),  
	@SortType VARCHAR(10),
	@FromIndex INT,
	@PageSize INT	
AS
BEGIN    
	;WITH CTENoteList AS
	( 
	SELECT *,COUNT(t1.NoteID) OVER() AS Count FROM 
		(
		SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY 
				
				    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'NoteID' THEN NoteID END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'NoteID' THEN NoteID END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ServiceDate' THEN CONVERT(datetime, ServiceDate, 103) END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ServiceDate' THEN CONVERT(datetime, ServiceDate, 103) END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN CONVERT(datetime, CreatedDate, 103) END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN CONVERT(datetime, CreatedDate, 103) END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ModifiedDate' THEN CONVERT(datetime, UpdatedDate) END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ModifiedDate' THEN CONVERT(datetime, UpdatedDate) END END DESC,


					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'SpokeTo' THEN SpokeTo END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'SpokeTo' THEN SpokeTo END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Payor' THEN PayorShortName END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Payor' THEN PayorShortName END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'IsBillable' THEN IsBillable END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'IsBillable' THEN IsBillable END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Unit' THEN CalculatedUnit END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Unit' THEN CalculatedUnit END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Amount' THEN CalculatedAmount END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Amount' THEN CalculatedAmount END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'StartDate' THEN CONVERT(datetime, StartTime, 103) END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'StartDate' THEN CONVERT(datetime, StartTime, 103) END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EndDate' THEN CONVERT(datetime, EndTime, 103) END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EndDate' THEN CONVERT(datetime, EndTime, 103) END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedBy' THEN CreatedByUserName END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedBy' THEN CreatedByUserName END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'UpdatedBy' THEN UpdatedByUserName END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'UpdatedBy' THEN UpdatedByUserName END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'SignatureDate' THEN SignatureDate END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'SignatureDate' THEN SignatureDate END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'SignatureBy' THEN SignatureBy END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'SignatureBy' THEN SignatureBy END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ServiceCode' THEN ServiceName END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ServiceCode' THEN ServiceName END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ServiceType' THEN ServiceCodeTypeName END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ServiceType' THEN ServiceCodeTypeName END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'BillingProvider' THEN BillingProviderName END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'BillingProvider' THEN BillingProviderName END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'RenderingProvider' THEN RenderingProviderName END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'RenderingProvider' THEN RenderingProviderName END END DESC,

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralName' THEN ReferralName END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralName' THEN ReferralName END END DESC--,

					--CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Batch' THEN BN.BatchID END END ASC,
					--CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Batch' THEN BN.BatchID END END DESC
					
				
		) AS Row,

		* FROM (
		SELECT DISTINCT n.NoteID,--BN.BatchID,case when B.IsSent=1 then 1 else 0 end as IsSent,
		 n.ActionPlan,r.AHCCCSID,n.Assessment,n.CalculatedUnit,r.CISNumber,n.ContinuedDX,n.MonthlySummaryIds,
		 n.CreatedBy,n.CreatedDate,n.DailyUnitLimit,n.Description,n.BillingProviderName,n.RenderingProviderName,
		 n.EndMile,n.EndTime,n.HasGroupOption,n.IsBillable,n.MarkAsComplete,n.MaxUnit,n.ModifierID,n.NoteDetails,n.OtherNoteType,n.PayorID,n.PayorServiceCodeMappingID,
		 n.PerUnitQuantity,n.POSDetail,n.POSEndDate,n.PosID,n.POSStartDate,n.Rate,n.ReferralID,n.Relation,n.ServiceCode,n.ServiceCodeEndDate,n.Source,
		 e.UserName as CreatedByUserName,
		 eu.UserName as UpdatedByUserName,
		 n.ServiceCodeID,n.ServiceCodeStartDate,n.ServiceCodeType,n.ServiceDate,n.ServiceName,n.SignatureDate,n.SpokeTo,n.StartMile,n.StartTime,n.CalculatedAmount,
		 es.UserName as SignatureBy,R.LastName+', '+R.FirstName as ReferralName,
		 n.SystemID,n.ZarephathService,n.PayorShortName,n.UnitType,n.UpdatedBy,n.UpdatedDate,pos.PosName,m.ModifierName,n.IsDeleted,
		 CASE when n.CreatedBy > 0 then e.LastName+', '+e.FirstName else n.Source END as Name,
		 sct.ServiceCodeTypeName,AllowEdit=1,N.AttachmentURL,N.NoteComments
		 
		 --CASE WHEN (select count(*) from BatchNotes where NoteID=n.NoteID AND BatchNoteStatusID not in(SELECT CAST(VAL AS BIGINT) 
		 -- FROM GETCSVTABLE(@AllowEditStatuses))) >0 then 0 else 1 end As AllowEdit,
		 --,
		 -- (SELECT  STUFF((SELECT TOP 1 ', ' + Convert(varchar(50),ISNULL(B.BatchID,''))
			--FROM BatchNotes BN
			--INNER  JOIN Batches B on B.BatchID=BN.BatchID  where BN.NoteID=n.NoteID AND  B.IsDeleted=0	 Order BY B.BatchID
			--FOR XML PATH('')),1,1,'')) AS BatchDetails

		  --BatchNoteStatusID!=1 AND BatchNoteStatusID!=5
		 from Notes n
		 inner join Referrals r on r.ReferralID=n.ReferralID		 
		 left join Modifiers m on m.ModifierID=n.ModifierID
		 left join Employees e on e.EmployeeID=n.CreatedBy
		 left join Employees eu on eu.EmployeeID=n.UpdatedBy
		 left join Departments dep on dep.DepartmentID=eu.DepartmentID
		 left join PlaceOfServices pos on pos.PosID=n.PosID	
		 left join ServiceCodeTypes sct on sct.ServiceCodeTypeID=n.ServiceCodeType
		 LEFT JOIN SignatureLogs SL ON SL.NoteID=N.NoteID and SL.IsActive=1 and n.MarkAsComplete=1
		 LEFT JOIN Employees es on es.EmployeeID=SL.SignatureBy
		 LEFT JOIN Facilities F ON F.FacilityID=N.RenderingProviderID
		 LEFT JOIN Regions RG ON RG.RegionID=R.RegionID
		 LEFT JOIN BatchNotes BN on BN.NoteID=n.NoteID
		 LEFT JOIN Batches B on B.BatchID=BN.BatchID 
		 WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR n.IsDeleted=@IsDeleted) AND ((CAST(@IsBillable AS BIGINT)=-1) OR n.IsBillable=@IsBillable)
			AND ((@AHCCCSID IS NULL OR LEN(@AHCCCSID)=0) OR n.AHCCCSID LIKE '%' + @AHCCCSID + '%')
			AND ((@NoteKind IS NULL OR LEN(@NoteKind)=0) OR n.OtherNoteType =  @NoteKind )
			--AND (( CAST(@BillingProviderID AS BIGINT)=0) OR (n.BillingProviderID = CAST(@BillingProviderID AS BIGINT)  ))--OR n.RenderingProviderID = CAST(@BillingProviderID AS BIGINT)) )	

			-- CHNAGE AS BILLING Provider to Renderring Provider as ASked by client
			AND (( CAST(@BillingProviderID AS BIGINT)=0) OR (n.RenderingProviderID = CAST(@BillingProviderID AS BIGINT)))
			AND (( CAST(@RegionID AS BIGINT)=0) OR (RG.RegionID = CAST(@RegionID AS BIGINT)))

			AND (( CAST(@DepartmentID AS BIGINT)=0) OR dep.DepartmentID = CAST(@DepartmentID AS BIGINT))	
			--AND (( CAST(@ServiceCodeID AS BIGINT)=0) OR n.ServiceCodeID = CAST(@ServiceCodeID AS BIGINT))	
			AND ((@CISNumber IS NULL OR LEN(@CISNumber)=0) OR n.CISNumber LIKE '%' + @CISNumber + '%')
			AND (((@SearchText IS NULL OR LEN(@SearchText)=0) OR n.NoteDetails LIKE '%' + @SearchText + '%')
			OR ((@SearchText IS NULL OR LEN(@SearchText)=0) OR n.ActionPlan LIKE '%' + @SearchText + '%')
			OR ((@SearchText IS NULL OR LEN(@SearchText)=0) OR n.Assessment LIKE '%' + @SearchText + '%'))
			--AND (( CAST(@ServiceCodeID AS BIGINT)=0) OR n.ServiceCodeID = CAST(@ServiceCodeID AS BIGINT))		
			AND (( LEN(@ServiceCodeIDs)=0) OR n.ServiceCodeID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ServiceCodeIDs)) )		  	
			AND (( CAST(@BatchID AS BIGINT)=0) OR BN.BatchID = CAST(@BatchID AS BIGINT))						
			AND (( CAST(@NoteID AS BIGINT)=0) OR N.NoteID = CAST(@NoteID AS BIGINT))						
			AND ((CAST(@ReferralID AS BIGINT)=0) OR  n.ReferralID = CAST(@ReferralID AS BIGINT))	
			AND (( CAST(@ServiceCodeTypeID AS BIGINT)=-1) OR n.ServiceCodeType = CAST(@ServiceCodeTypeID AS BIGINT))		
			AND (( CAST(@PayorID AS BIGINT)=-1) OR n.PayorID = CAST(@PayorID AS BIGINT))		
			
			AND (( CAST(@IsCompleted AS BIGINT)=-1) OR n.MarkAsComplete = @IsCompleted)		
			AND ((@ServiceDateStart is null OR CONVERT(Date,N.ServiceDate) >= @ServiceDateStart)) and ((@ServiceDateEnd is null OR Convert(date,N.ServiceDate) <= @ServiceDateEnd))
			--AND (( CAST(@EmployeeID AS BIGINT)=0) OR  n.CreatedBy = CAST(@EmployeeID AS BIGINT))	
			AND (
			      (LEN(@CreatedByIDs)=0) OR n.CreatedBy IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@CreatedByIDs))
				  OR (( CAST(@AssigneeID AS BIGINT)=0) OR  n.NoteAssignee = CAST(@AssigneeID AS BIGINT))
				 )			  	
		 ) AS t2
	   ) AS t1	
	)
	
	SELECT * FROM CTENoteList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)	

END
