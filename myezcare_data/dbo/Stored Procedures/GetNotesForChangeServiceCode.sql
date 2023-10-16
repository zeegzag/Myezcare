-- EXEC GetNotesForChangeServiceCode @ClientName = '', @PayorID = '2', @CreatedBy = '', @ServiceCodeID = '11', @PosID = '99', @EndDate = '2017/07/18', @StartDate = '2017/07/01'
CREATE PROCEDURE [dbo].[GetNotesForChangeServiceCode]    
@ClientName varchar(20)=null,        
@StartDate Date=null,        
@EndDate Date=null,        
@PayorID bigint = 0,        
@PosID bigint = 0,        
@ServiceCodeID bigint = 0,
@CreatedBy bigint = 0  
AS        
BEGIN        

         SELECT  R. ReferralID,R.LastName+', '+R.FirstName as ReferralName, n.NoteID,n.CalculatedUnit,StartTime,EndTime,
		 n.CalculatedAmount,n.IsBillable,n.MarkAsComplete,n.PayorShortName,(N.ServiceCode + CASE WHEN M.ModifierCode IS NOT NULL THEN ' - ' + M.ModifierCode ELSE '' END) AS ServiceCode,
		 n.ServiceName,
		 es.UserName as SignatureBy,e.UserName as CreatedByUserName,eu.UserName as UpdatedByUserName,
		 n.CreatedDate,n.SignatureDate,n.UpdatedDate,
		 n.IsDeleted
		 --,
		 -- (SELECT  STUFF((SELECT TOP 1 ', ' + Convert(varchar(50),ISNULL(B.BatchID,''))
			--FROM BatchNotes BN
			--INNER  JOIN Batches B on B.BatchID=BN.BatchID  where BN.NoteID=n.NoteID AND  B.IsDeleted=0	 Order BY B.BatchID
			--FOR XML PATH('')),1,1,'')) AS BatchDetails
		 from Notes n
		 inner join Referrals r on r.ReferralID=n.ReferralID		 
		 left join Modifiers m on m.ModifierID=n.ModifierID
		 left join Employees e on e.EmployeeID=n.CreatedBy
		 left join Employees eu on eu.EmployeeID=n.UpdatedBy
		 LEFT JOIN SignatureLogs SL ON SL.NoteID=N.NoteID and SL.IsActive=1 and n.MarkAsComplete=1
		 LEFT JOIN Employees es on es.EmployeeID=SL.SignatureBy
		 --LEFT JOIN BatchNotes BN on BN.NoteID=n.NoteID
		 --LEFT JOIN Batches B on B.BatchID=BN.BatchID 
		 WHERE (n.ServiceCodeID IS NOT NULL AND n.ServiceCodeID>0)
			AND ( ( LEN(@ServiceCodeID)=0 OR @ServiceCodeID=0) OR n.ServiceCodeID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ServiceCodeID)) )		  	
			AND ( ( LEN(@PosID)=0 OR @PosID=0) OR n.PosID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@PosID)) )		  	
			
			AND ( (LEN(@CreatedBy)=0 OR @CreatedBy=0) OR n.CreatedBy IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@CreatedBy)) )
			AND ((@ClientName IS NULL) OR 
				 (        
					  R.FirstName LIKE '%'+@ClientName+'%' OR        
					  R.LastName  LIKE '%'+@ClientName+'%' OR        
					  R.FirstName +' '+r.LastName like '%'+@ClientName+'%' OR        
					  R.LastName +' '+r.FirstName like '%'+@ClientName+'%' OR        
					  R.FirstName +', '+r.LastName like '%'+@ClientName+'%' OR        
					  R.LastName +', '+r.FirstName like '%'+@ClientName+'%'        
				 ))        
		   AND (( CAST(@PayorID AS BIGINT)=0) OR N.PayorID = CAST(@PayorID AS BIGINT))        
		   AND (((@StartDate IS NULL) OR (N.ServiceDate BETWEEN @StartDate and @EndDate)) OR ((@EndDate IS NULL) OR (N.ServiceDate  BETWEEN  @StartDate and @EndDate))     )           
		   Order by Name asc      				  	

END

