CREATE PROCEDURE [dbo].[ExportNoteList]		
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

					CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralName' THEN Name END END ASC,
					CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralName' THEN Name END END DESC--,

					--CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Batch' THEN BN.BatchID END END ASC,
					--CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Batch' THEN BN.BatchID END END DESC
					
				
		) AS Row,* FROM (
		SELECT DISTINCT
		 n.NoteID,r.AHCCCSID,r.CISNumber,R.LastName+', '+R.FirstName as Name,n.ZarephathService,n.ServiceCode,n.Description,sct.ServiceCodeTypeName,
		 n.CalculatedUnit,n.SpokeTo,n.Relation,n.OtherNoteType,n.Source,n.MarkAsComplete,n.IsBillable,n.ServiceName,
		 
		 
		 CONVERT(VARCHAR(10),CONVERT(datetime,n.CreatedDate,1),111) AS CreatedDate,
		 CONVERT(VARCHAR(10),CONVERT(datetime,n.UpdatedDate,1),111) AS UpdatedDate,
		 CONVERT(VARCHAR(10),CONVERT(datetime,SL.Date,1),111) AS SignatureDate,
		 
		 CONVERT(VARCHAR(10),CONVERT(datetime,n.ServiceCodeStartDate,1),111) AS ServiceCodeStartDate,
		 CONVERT(VARCHAR(10),CONVERT(datetime,n.ServiceCodeEndDate,1),111) AS ServiceCodeEndDate,
		 CONVERT(VARCHAR(10),CONVERT(datetime,n.ServiceDate,1),111) AS ServiceDate,
		 --CONVERT(varchar(15),  CAST(n.StartTime AS TIME), 100) as StartTime,
		 --CONVERT(varchar(15),  CAST(n.EndTime AS TIME), 100) as EndTime,


		 CASE WHEN n.ServiceCodeType=3 THEN '' ELSE CONVERT(varchar(15),  CAST(n.StartTime AS TIME), 100) END  as StartTime,
		 CASE WHEN n.ServiceCodeType=3 THEN '' ELSE CONVERT(varchar(15),  CAST(n.EndTime AS TIME), 100) END  as EndTime,

		 n.PosID,n.PosDetail,
		 n.PayorShortName,n.CalculatedAmount,pos.PosName,m.ModifierName,
		 n.BillingProviderName,n.BillingProviderAddress,n.BillingProviderCity,n.BillingProviderState,n.BillingProviderZipcode,n.BillingProviderEIN,n.BillingProviderNPI,
		 n.RenderingProviderName,n.RenderingProviderAddress,n.RenderingProviderCity,n.RenderingProviderState,n.RenderingProviderZipcode,n.RenderingProviderEIN,n.RenderingProviderNPI,
		 dbo.udf_StripHTML(n.NoteDetails) AS NoteDetails,dbo.udf_StripHTML(n.Assessment) AS Assessment,dbo.udf_StripHTML(n.ActionPlan) AS ActionPlan,
		 e.UserName as CreatedByUserName,eu.UserName as UpdatedByUserName,es.UserName as SignatureBy,

		CASE WHEN n.IssueID>0 THEN 'Yes' ELSE 'No' END AS Issue,
		eia.UserName as Assignee,
		CASE WHEN RM.IsResolved=1 THEN 'Yes' ELSE 'No' END AS IssueResolved,

		n.RandomGroupID

		 
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
		 LEFT JOIN Employees eia on eia.EmployeeID=n.IssueAssignID
		 LEFT JOIN ReferralInternalMessages RM on RM.ReferralInternalMessageID=n.IssueID 
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
			--AND (( CAST(@AssigneeID AS BIGINT)=0) OR  n.NoteAssignee = CAST(@AssigneeID AS BIGINT))	
			--AND (( LEN(@CreatedByIDs)=0) OR n.CreatedBy IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@CreatedByIDs)))		  	
			AND (
			      (LEN(@CreatedByIDs)=0) OR n.CreatedBy IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@CreatedByIDs))
				  OR (( CAST(@AssigneeID AS BIGINT)=0) OR  n.NoteAssignee = CAST(@AssigneeID AS BIGINT))
				 )	
		) AS t2
	   ) AS t1	
		
	
END
