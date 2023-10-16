-- EXEC GetNoteClientList @SortExpression='ReferralName',@SortType='ASC',@FromIndex=1,@PageSize=100 ,@AHCCCSID='A12346549'  
CREATE PROCEDURE [dbo].[GetNoteClientList]    
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
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
 ;WITH CTENoteClientList AS  
 (   
 SELECT *,COUNT(t1.ReferralID) OVER() AS Count FROM   
  (  
  SELECT ROW_NUMBER() OVER (ORDER BY   
      
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralName' THEN ReferralName END END ASC,  
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralName' THEN ReferralName END END DESC,  
  
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN AHCCCSID END END ASC,  
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN AHCCCSID END END DESC,  
  
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Payor' THEN Payor END END ASC,  
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Payor' THEN Payor END END DESC,  
  
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Population' THEN Population END END ASC,  
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Population' THEN Population END END DESC,  
  
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Title' THEN Title END END ASC,  
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Title' THEN Title END END DESC,  
  
  --CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FacilityName' THEN FacilityName END END ASC,  
  --CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FacilityName' THEN FacilityName END END DESC,  
  
    
  
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'StarDate' THEN CONVERT(datetime, StartDate, 103) END END ASC,  
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'StarDate' THEN CONVERT(datetime, StartDate, 103) END END DESC  
  
       
       
      
  ) AS Row,   
  --ReferralID,FacilityID,FacilityName, ReferralName,AHCCCSID,CISNumber,Payor,Population,Title ,StartDate--,NoteID  
  ReferralID, ReferralName,AHCCCSID,CISNumber,Payor,Population,Title ,StartDate--,NoteID  
   FROM (   
   --SELECT DISTINCT R.ReferralID,F.FacilityID,F.FacilityName,  dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) as ReferralName,R.AHCCCSID,R.CISNumber,Payor=P.ShortName,R.Population,R.Title ,   
   SELECT DISTINCT R.ReferralID,  dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) as ReferralName,R.AHCCCSID,R.CISNumber,Payor=P.ShortName,R.Population,R.Title ,   
   StartDate=MIN(N.ServiceDate),NoteID=MAX(ISNUll(N.NoteID,0))  
     
   FROM Notes N  
   INNER JOIN Referrals R ON R.ReferralID=N.ReferralID  
   INNER JOIN ReferralPayorMappings RP ON RP.ReferralID=R.ReferralID AND RP.IsActive=1  
   INNER JOIN Payors P ON P.PayorID=RP.PayorID  
   LEFT JOIN Facilities F ON F.FacilityID=N.RenderingProviderID  
   LEFT JOIN Regions RG ON RG.RegionID=R.RegionID  
   LEFT JOIN Employees eu on EU.EmployeeID=N.UpdatedBy  
   LEFT JOIN Departments DEP on DEP.DepartmentID=EU.DepartmentID  
   LEFT JOIN BatchNotes BN on BN.NoteID=n.NoteID --AND  BN.ParentBatchNoteID IS NULL AND BN.BatchNoteStatusID not in(7)  
   LEFT JOIN Batches B on B.BatchID=BN.BatchID   
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR N.IsDeleted=@IsDeleted) AND ((CAST(@IsBillable AS BIGINT)=-1) OR N.IsBillable=@IsBillable)   
   AND ((@AHCCCSID IS NULL OR LEN(@AHCCCSID)=0) OR n.AHCCCSID LIKE '%' + @AHCCCSID + '%')  
   AND ((@NoteKind IS NULL OR LEN(@NoteKind)=0) OR n.OtherNoteType =  @NoteKind )  
   --AND (( CAST(@BillingProviderID AS BIGINT)=0) OR (n.BillingProviderID = CAST(@BillingProviderID AS BIGINT)))  
     
   -- CHNAGE AS BILLING Provider to Renderring Provider as ASked by client  
   AND (( CAST(@BillingProviderID AS BIGINT)=0) OR (n.RenderingProviderID = CAST(@BillingProviderID AS BIGINT)))  
   AND (( CAST(@RegionID AS BIGINT)=0) OR (RG.RegionID = CAST(@RegionID AS BIGINT)))  
     
   AND (( CAST(@DepartmentID AS BIGINT)=0) OR DEP.DepartmentID = CAST(@DepartmentID AS BIGINT))   
   --AND (( CAST(@ServiceCodeID AS BIGINT)=0) OR n.ServiceCodeID = CAST(@ServiceCodeID AS BIGINT))   
   AND ((@CISNumber IS NULL OR LEN(@CISNumber)=0) OR n.CISNumber LIKE '%' + @CISNumber + '%')  
   AND (((@SearchText IS NULL OR LEN(@SearchText)=0) OR n.NoteDetails LIKE '%' + @SearchText + '%')  
   OR ((@SearchText IS NULL OR LEN(@SearchText)=0) OR n.ActionPlan LIKE '%' + @SearchText + '%')  
   OR ((@SearchText IS NULL OR LEN(@SearchText)=0) OR n.Assessment LIKE '%' + @SearchText + '%'))  
   -- AND (( CAST(@ServiceCodeID AS BIGINT)=0) OR n.ServiceCodeID = CAST(@ServiceCodeID AS BIGINT))    
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
      
    GROUP BY R.ReferralID,  R.LastName,R.FirstName,R.MiddleName ,R.AHCCCSID,R.CISNumber,P.ShortName,R.Population,R.Title   
    --GROUP BY R.ReferralID,F.FacilityID,F.FacilityName,  R.LastName,R.FirstName ,R.AHCCCSID,R.CISNumber,P.ShortName,R.Population,R.Title   
  ) AS t  
    
  ) AS t1  
 )  
   
 SELECT * FROM CTENoteClientList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)   
  
END  