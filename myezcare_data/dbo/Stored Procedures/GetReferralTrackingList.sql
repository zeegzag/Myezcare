-- EXEC GetReferralTrackingList @ReferralDate = '2016/12/01', @ReferralToDate = '2016/12/31', @ReferralTrackingComment = '', @IsDeleted = '-1', @SortExpression = 'ClientName', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'
CREATE PROCEDURE [dbo].[GetReferralTrackingList]  
 @ReferralTrackingComment varchar(100) = NULL,             
 @ClientName varchar(100) = NULL,             
 @PayorID bigint = 0,            
 @CaseManagerID int = 0,            
 @AgencyID bigint=0,            
 @ReferralStatusID bigint =0,            
 @AHCCCSID varchar(20)=null,            
 @IsDeleted BIGINT =-1, 
       
 @CMNotifiedDate date=null,
 @ReferralDate date=null,
 @CreatedDate datetime=null,
 @ChecklistDate datetime=null,
 @SparDate datetime=null,

 @CMNotifiedToDate date=null,
 @ReferralToDate date=null,
 @CreatedToDate datetime=null,
 @ChecklistToDate datetime=null,
 @SparToDate datetime=null,
      
 @SortExpression VARCHAR(100),              
 @SortType VARCHAR(10),            
 @FromIndex INT,            
 @PageSize INT 
AS            
BEGIN   
           
 ;WITH CTEReferralList AS            
 (             
 

 SELECT  *,COUNT(t1.ReferralID) OVER() AS Count FROM             
  (            
  SELECT ROW_NUMBER() OVER (ORDER BY       
      
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN CONVERT(varchar(50),t.AHCCCSID) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN CONVERT(varchar(50),t.AHCCCSID) END END DESC,           
           
      
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Gender' THEN CONVERT(char(1),t.Gender) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Gender' THEN CONVERT(char(1),t.Gender) END END DESC,            
                
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Age' THEN CAST(t.Age AS decimal) END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Age' THEN CAST(t.Age AS decimal) END END DESC,     

    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN CONVERT(datetime, t.CreatedDate, 103)  END END ASC,                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN CONVERT(datetime, t.CreatedDate, 103)  END END DESC, 
	
	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CheckListDate' THEN CONVERT(datetime, t.ChecklistCompletedDate, 103)  END END ASC,                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CheckListDate' THEN CONVERT(datetime, t.ChecklistCompletedDate, 103)  END END DESC, 

	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'SPARDate' THEN CONVERT(datetime, t.SparFormCompletedDate, 103)  END END ASC,                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'SPARDate' THEN CONVERT(datetime, t.SparFormCompletedDate, 103)  END END DESC, 

	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralDate' THEN CONVERT(datetime, t.ReferralDate, 103)  END END ASC,                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralDate' THEN CONVERT(datetime, t.ReferralDate, 103)  END END DESC, 

	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CMNotified' THEN CONVERT(datetime, t.NotifyCaseManagerDate, 103)  END END ASC,                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CMNotified' THEN CONVERT(datetime, t.NotifyCaseManagerDate, 103)  END END DESC, 
	  
            
    CASE WHEN @SortType = 'ASC' THEN            
      CASE            
      WHEN @SortExpression = 'ClientName' THEN t.Name            
      WHEN @SortExpression = 'FaciliatorName' THEN t.FaciliatorName            
      WHEN @SortExpression = 'ContractName' THEN t.ContractName      
      WHEN @SortExpression = 'CompanyName' THEN t.CompanyName      
      WHEN @SortExpression = 'Status' THEN t.Status            
      WHEN @SortExpression = 'AssigneeName' THEN t.AssigneeName            
      --WHEN @SortExpression = 'CreatedDate' THEN CONVERT(datetime, t.CreatedDate, 103)            
   --   WHEN @SortExpression = 'ModifiedDate' THEN CONVERT(datetime, t.UpdatedDate, 103)
	  --WHEN @SortExpression = 'CheckListDate' THEN CONVERT(datetime, t.ChecklistCompletedDate, 103) 
	  --WHEN @SortExpression = 'SPARDate' THEN CONVERT(datetime, t.SparFormCompletedDate, 103) 
	  --WHEN @SortExpression = 'ReferralDate' THEN CONVERT(datetime, t.ReferralDate, 103) 	             
	  --WHEN @SortExpression = 'CMNotified' THEN CONVERT(datetime, t.NotifyCaseManagerDate, 103) 	             
	  WHEN @SortExpression = 'Comment' THEN t.ReferralTrackingComment	             
      END             
    END ASC,            
    CASE WHEN @SortType = 'DESC' THEN            
      CASE             
      WHEN @SortExpression = 'ClientName' THEN t.Name            
      WHEN @SortExpression = 'FaciliatorName' THEN t.FaciliatorName            
      WHEN @SortExpression = 'ContractName' THEN t.ContractName      
   WHEN @SortExpression = 'CompanyName' THEN t.CompanyName            
      WHEN @SortExpression = 'Status' THEN t.Status            
      WHEN @SortExpression = 'AssigneeName' THEN t.AssigneeName            
   --   WHEN @SortExpression = 'CreatedDate' THEN CONVERT(datetime, t.CreatedDate, 103)--CONVERT(DateTime, r.CreatedDate)            
   --   WHEN @SortExpression = 'ModifiedDate' THEN CONVERT(datetime, t.UpdatedDate, 103) --CONVERT(DateTime, r.UpdatedDate) 
	  --WHEN @SortExpression = 'CheckListDate' THEN CONVERT(datetime, t.ChecklistCompletedDate, 103)      
	  --WHEN @SortExpression = 'SPARDate' THEN CONVERT(datetime, t.SparFormCompletedDate, 103) 
	  --WHEN @SortExpression = 'ReferralDate' THEN CONVERT(datetime, t.ReferralDate, 103) 	             
	  --WHEN @SortExpression = 'CMNotified' THEN CONVERT(datetime, t.NotifyCaseManagerDate, 103) 	
	  WHEN @SortExpression = 'Comment' THEN t.ReferralTrackingComment                                     
      END            
    END DESC            
  ) AS Row,     
    t.* from (
	
   select DISTINCT r.ReferralID,r.LastName+', '+r.FirstName as Name,r.AHCCCSID,r.Gender, dbo.GetAge(r.Dob) as Age ,           
   r.ReferralStatusID,rs.Status,e.LastName+', '+e.FirstName as AssigneeName,p.ShortName as ContractName,
   cm.LastName+', '+cm.FirstName as FaciliatorName,r.CreatedDate,eself.LastName+', '+eself.FirstName as CreatedName,            
   a.NickName as CompanyName,er.LastName+' '+er.FirstName as CheckListName,rc.ChecklistCompletedDate, es.LastName+', '+es.FirstName as ClinicalReviewName,
   rsf.SparFormCompletedDate,r.NotifyCaseManager,            
   r.UpdatedDate,eselfUP.LastName+', '+eselfUP.FirstName as UpdatedName, r.IsDeleted, r.ReferralDate,r.NotifyCaseManagerDate,r.ReferralTrackingComment
   
   from Referrals r            
   left join ReferralStatuses RS on rs.ReferralStatusID=r.ReferralStatusID            
   left join Employees e on e.EmployeeID=r.Assignee            
   left join ReferralPayorMappings rp on rp.ReferralID=r.ReferralID and rp.IsActive=1 and rp.IsDeleted=0            
   left join Payors p on p.PayorID=rp.PayorID            
   left join CaseManagers cm on cm.CaseManagerID=r.CaseManagerID            
   left join Agencies a on a.AgencyID=r.AgencyID       
   left join AgencyLocations al on al.AgencyLocationID=r.AgencyLocationID            
   left join ReferralCheckLists rc on rc.ReferralID=r.ReferralID            
   left join ReferralSparForms rsf on rsf.ReferralID=r.ReferralID            
   left join Employees es on es.EmployeeID=rsf.SparFormCompletedBy            
   left join Employees er on er.EmployeeID=rc.ChecklistCompletedBy            
   left join Employees eself on eself.EmployeeID=r.CreatedBy    
   left join Employees eselfUP on eselfUP.EmployeeID=r.UpdatedBy              
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR r.IsDeleted=@IsDeleted)             
  AND ((@ClientName IS NULL OR LEN(r.LastName)=0) 
    OR
  ( (r.FirstName LIKE '%'+@ClientName+'%' )OR  
    (r.LastName  LIKE '%'+@ClientName+'%') OR  
    (r.FirstName +' '+r.LastName like '%'+@ClientName+'%') OR  
    (r.LastName +' '+r.FirstName like '%'+@ClientName+'%') OR  
    (r.FirstName +', '+r.LastName like '%'+@ClientName+'%') OR  
    (r.LastName +', '+r.FirstName like '%'+@ClientName+'%'))
  )   
   AND ((@AHCCCSID IS NULL OR LEN(@AHCCCSID)=0) OR r.AHCCCSID LIKE '%' + @AHCCCSID + '%')            
   AND (( CAST(@PayorID AS BIGINT)=0) OR rp.PayorID = CAST(@PayorID AS BIGINT))            
   AND (( CAST(@ReferralStatusID AS BIGINT)=0) OR r.ReferralStatusID = CAST(@ReferralStatusID AS BIGINT))              
   AND (( CAST(@AgencyID AS BIGINT)=0) OR r.AgencyID = CAST(@AgencyID AS BIGINT))  
   AND (( CAST(@CaseManagerID AS BIGINT)=0) OR r.CaseManagerID = CAST(@CaseManagerID AS BIGINT))      
   
   AND ((@CMNotifiedDate is null OR r.NotifyCaseManagerDate >= @CMNotifiedDate) AND  (@CMNotifiedToDate is null OR r.NotifyCaseManagerDate <= @CMNotifiedToDate))
   AND ((@ReferralDate is null OR r.ReferralDate >= @ReferralDate)  AND (@ReferralToDate is null OR r.ReferralDate <= @ReferralToDate))  
   
   AND ((@CreatedDate is null OR r.CreatedDate >= @CreatedDate)  AND (@CreatedToDate is null OR r.CreatedDate <= @CreatedToDate))  
   AND ((@ChecklistDate is null OR rc.ChecklistCompletedDate >= @ChecklistDate)  AND (@ChecklistToDate is null OR rc.ChecklistCompletedDate  <= @ChecklistToDate))  
   AND ((@SparDate is null OR rsf.SparFormCompletedDate >= @SparDate)  AND (@SparToDate is null OR rsf.SparFormCompletedDate <= @SparToDate)) 
    
   --AND (@CreatedDate is null OR (r.CreatedDate>=@CreatedDate AND r.CreatedDate<=@CreatedDate+1))
   --AND (@ChecklistDate is null OR (rc.ChecklistCompletedDate>=@ChecklistDate AND rc.ChecklistCompletedDate<=@ChecklistDate+1)) 
   --AND (@SparDate is null OR (rsf.SparFormCompletedDate>=@SparDate AND rsf.SparFormCompletedDate<=@SparDate+1)) 
   AND  ((@ReferralTrackingComment IS NULL OR LEN(@ReferralTrackingComment)=0) OR r.ReferralTrackingComment LIKE '%' + @ReferralTrackingComment + '%')  
 ) as t      
       
   ) AS t1 )   
             
 SELECT * FROM CTEReferralList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)      
         
END
