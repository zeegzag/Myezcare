--EXEC GetLSTeamMemberCaseloadList @ReferralStatusID = '1,14,9', @ClientName = 'graciela Espinoza', @LoggedInID = '1',  @ViewAllPermission = '1', @IsDeleted = '0', @SortExpression = 'ClientName', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'


CREATE PROCEDURE [dbo].[GetLSTeamMemberCaseloadList]  
 @ClientName varchar(100) = NULL,             
 @ParentName varchar(100) = NULL,
 @CaseManagerID int = 0,       
 @PayorID bigint = 0,    
 @ReferralStatusID varchar(max) =null,            
 @AgencyID bigint=0,            
 @AHCCCSID varchar(20)=null,            
 @ServiceFromDate date=null,
 @ServiceToDate date=null,     
 @CFServiceFromDate date=null,
 @CFServiceToDate date=null,     
 @ReferralFromDate date=null,
 @ReferralToDate date=null,     
 @ACFromDate date=null,
 @ACToDate date=null,   
 @OMCompletedFromDate date=null,
 @OMCompletedToDate date=null,   
 @OMNextDueFromDate date=null,
 @OMNextDueToDate date=null,   
 @ReferralLSTMCaseloadsComment varchar(500)=null,
 @ViewAllPermission bit,
 @ServiceID int = -1,   
 @LoggedInID bigint = 0,   
 @IsDeleted BIGINT =-1, 
 @SortExpression VARCHAR(100),              
 @SortType VARCHAR(10),            
 @FromIndex INT,            
 @PageSize INT ,
 @CaseLoadID bigint=0
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
                
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Age' THEN  CONVERT(datetime, t.Dob, 103)END END ASC,                                        
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Age' THEN CONVERT(datetime, t.Dob, 103)  END END DESC, 
	
	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralDate' THEN CONVERT(datetime, t.ReferralDate, 103)  END END ASC,                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralDate' THEN CONVERT(datetime, t.ReferralDate, 103)  END END DESC, 
	
	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ServicePlanDue' THEN CONVERT(datetime, t.ServicePlanDue, 103)  END END ASC,                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ServicePlanDue' THEN CONVERT(datetime, t.ServicePlanDue, 103)  END END DESC, 

	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CFServicePlanDue' THEN CONVERT(datetime, t.CFServicePlanDue, 103)  END END ASC,                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CFServicePlanDue' THEN CONVERT(datetime, t.CFServicePlanDue, 103)  END END DESC, 
	
	
	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ACServicePlanDue' THEN CONVERT(datetime, t.ACServicePlanDue, 103)  END END ASC,                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ACServicePlanDue' THEN CONVERT(datetime, t.ACServicePlanDue, 103)  END END DESC,     

	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'OMLastCompleted' THEN CONVERT(datetime, t.OMLastCompleted, 103)  END END ASC,                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'OMLastCompleted' THEN CONVERT(datetime, t.OMLastCompleted, 103)  END END DESC,

	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'OMNextDue' THEN CONVERT(datetime, t.OMNextDue, 103) END END ASC,                  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'OMNextDue' THEN CONVERT(datetime, t.OMNextDue, 103)  END END DESC,
	     
      
            
    CASE WHEN @SortType = 'ASC' THEN            
      CASE            
 WHEN @SortExpression = 'ClientName' THEN t.Name            
	  WHEN @SortExpression = 'Parent' THEN t.Parent   
      WHEN @SortExpression = 'FaciliatorName' THEN t.FaciliatorName            
      WHEN @SortExpression = 'ContractName' THEN t.ContractName      
      WHEN @SortExpression = 'CompanyName' THEN t.CompanyName      
      WHEN @SortExpression = 'Status' THEN t.Status            
      WHEN @SortExpression = 'AssigneeName' THEN t.AssigneeName 
	  WHEN @SortExpression = 'ReferralLSTMCaseloadsComment' THEN t.ReferralLSTMCaseloadsComment            
	  WHEN @SortExpression = 'CaseLoads' THEN t.CaseLoads            
      END             
    END ASC,            

    CASE WHEN @SortType = 'DESC' THEN            
      CASE             
      WHEN @SortExpression = 'ClientName' THEN t.Name  
	  WHEN @SortExpression = 'Parent' THEN t.Parent                      
      WHEN @SortExpression = 'FaciliatorName' THEN t.FaciliatorName            
      WHEN @SortExpression = 'ContractName' THEN t.ContractName      
	  WHEN @SortExpression = 'CompanyName' THEN t.CompanyName            
      WHEN @SortExpression = 'Status' THEN t.Status            
      WHEN @SortExpression = 'AssigneeName' THEN t.AssigneeName            
	  WHEN @SortExpression = 'ReferralLSTMCaseloadsComment' THEN t.ReferralLSTMCaseloadsComment   
	  WHEN @SortExpression = 'CaseLoads' THEN t.CaseLoads                     	  
      END            
    END DESC,
   
    CASE WHEN @SortExpression != 'ClientName' THEN t.Name END  ASC                                             
	            
  ) AS Row,     
    t.* from (
	
   select DISTINCT r.ReferralID,r.LastName+', '+r.FirstName as Name,r.LastName,r.AHCCCSID,r.Gender, r.Dob ,dbo.GetAge(r.Dob) as Age ,rs.Status,e.EmployeeID AS Assignee,  
   e.LastName+', '+e.FirstName as AssigneeName,
   p.ShortName as ContractName,cm.LastName+', '+cm.FirstName as FaciliatorName,
   a.NickName as CompanyName,r.IsDeleted, r.ReferralDate, R.ZSPLifeSkillsExpirationDate AS ServicePlanDue, 
   R.ZSPConnectingFamiliesExpirationDate AS CFServicePlanDue, R.ConnectingFamiliesService AS EnrollToCFServicePlan, 
   R.ACAssessmentExpirationDate AS ACServicePlanDue,PCNT.FirstName+' '+PCNT.LastName as Parent ,
   (select MAX(OutcomeMeasurementDate) from ReferralOutcomeMeasurements where ReferralID=R.ReferralID) as OMLastCompleted,
   DATEADD(Month,6,(select MAX(OutcomeMeasurementDate) from ReferralOutcomeMeasurements where ReferralID=R.ReferralID))  as OMNextDue,

   (SELECT STUFF((SELECT '|' + E.LastName + ', '+E.FirstName 
			FROM Employees E         
			inner join ReferralCaseloads ircl on e.EmployeeID = ircl.EmployeeID   
   where ReferralID =r.ReferralID         
   FOR XML PATH('')),1,1,'') ) CaseLoads ,R.ReferralLSTMCaseloadsComment ,
   CM.Phone as CMPhone,CM.Email as CMEmail,
   PCNT.Phone1 as ParentPhone1,PCNT.Phone2 as ParentPhone2,PCNT.Email as ParentEmail      


   from Referrals r            
   inner join ReferralCaseloads RCL on RCL.ReferralID= r.ReferralID
   left join ReferralStatuses RS on rs.ReferralStatusID=r.ReferralStatusID            
   left join Employees e on e.EmployeeID=r.Assignee            
   left join ReferralPayorMappings rp on rp.ReferralID=r.ReferralID and rp.IsActive=1 and rp.IsDeleted=0            
   left join Payors p on p.PayorID=rp.PayorID            
   left join CaseManagers cm on cm.CaseManagerID=r.CaseManagerID            
   left join Agencies a on a.AgencyID=r.AgencyID
   left join ContactMappings PCT on PCT.ReferralID=r.ReferralID AND PCT.ContactTypeID=1             
   left join Contacts PCNT on PCNT.ContactID=PCT.ContactID     
     
   WHERE 
   ((CAST(@IsDeleted AS BIGINT)=-1) OR r.IsDeleted=@IsDeleted)   
   AND (@ReferralLSTMCaseloadsComment IS NULL OR ((r.ReferralLSTMCaseloadsComment  LIKE '%'+@ReferralLSTMCaseloadsComment+'%'))) 
   AND ((@ClientName IS NULL OR LEN(r.LastName)=0) 
			OR
		   ((r.FirstName LIKE '%'+@ClientName+'%' )OR  
			(r.LastName  LIKE '%'+@ClientName+'%') OR  
			(r.FirstName +' '+r.LastName like '%'+@ClientName+'%') OR  
			(r.LastName +' '+r.FirstName like '%'+@ClientName+'%') OR  
			(r.FirstName +', '+r.LastName like '%'+@ClientName+'%') OR  
			(r.LastName +', '+r.FirstName like '%'+@ClientName+'%'))
		   ) 
   AND ((@ParentName IS NULL OR LEN(r.LastName)=0) 
		OR
	   ((PCNT.FirstName LIKE '%'+@ParentName+'%' )OR  
		(PCNT.LastName  LIKE '%'+@ParentName+'%') OR  
		(PCNT.FirstName +' '+PCNT.LastName like '%'+@ParentName+'%') OR  
		(PCNT.LastName +' '+PCNT.FirstName like '%'+@ParentName+'%') OR  
		(PCNT.FirstName +', '+PCNT.LastName like '%'+@ParentName+'%') OR  
		(PCNT.LastName +', '+PCNT.FirstName like '%'+@ParentName+'%'))
	   )  
   AND ((@AHCCCSID IS NULL OR LEN(@AHCCCSID)=0) OR r.AHCCCSID LIKE '%' + @AHCCCSID + '%')            
   AND (( CAST(@PayorID AS BIGINT)=0) OR rp.PayorID = CAST(@PayorID AS BIGINT))            
   --AND (@ReferralStatusID IS NULL OR r.ReferralStatusID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ReferralStatusID)))      

   AND (( CAST(@AgencyID AS BIGINT)=0) OR r.AgencyID = CAST(@AgencyID AS BIGINT))  
   AND (( CAST(@CaseManagerID AS BIGINT)=0) OR r.CaseManagerID = CAST(@CaseManagerID AS BIGINT))      
   AND ((@ReferralFromDate is null OR r.ReferralDate >= @ReferralFromDate)  AND (@ReferralToDate is null OR r.ReferralDate <= @ReferralToDate))  
   AND ((@ServiceFromDate is null OR r.ZSPLifeSkillsExpirationDate >= @ServiceFromDate)  AND (@ServiceToDate is null OR r.ZSPLifeSkillsExpirationDate <= @ServiceToDate))  
   AND ((@CFServiceFromDate is null OR r.ZSPConnectingFamiliesExpirationDate >= @CFServiceFromDate)  AND (@CFServiceToDate is null OR r.ZSPConnectingFamiliesExpirationDate <= @CFServiceToDate))  
   AND ((@ACFromDate is null OR r.ACAssessmentExpirationDate >= @ACFromDate)  AND (@ACToDate is null OR r.ACAssessmentExpirationDate <= @ACToDate))  
   AND (@ViewAllPermission=1 OR  (( @LoggedInID =0) OR RCL.EmployeeID = @LoggedInID)  )
   --AND (( CAST(@ServiceID AS bigint)=-1)  		   		   
		 --  OR (CAST(@ServiceID AS bigint) = 0 and r.RespiteService = 1)    
		 --  OR (CAST(@ServiceID AS bigint) = 1 and r.LifeSkillsService = 1)    
		 --  OR (CAST(@ServiceID AS bigint) = 2 and r.CounselingService = 1)
		 --  OR (CAST(@ServiceID AS bigint) = 3 and r.ConnectingFamiliesService = 1))
   AND (( CAST(@CaseLoadID AS BIGINT)=0) OR RCL.EmployeeID in(@CaseLoadID))            
 ) as t  
   
   WHERE
    ((@OMCompletedFromDate is null OR t.OMLastCompleted >= @OMCompletedFromDate)  AND (@OMCompletedToDate is null OR t.OMLastCompleted <= @OMCompletedToDate))  
   AND ((@OMNextDueFromDate is null OR t.OMNextDue >= @OMNextDueFromDate)  AND (@OMNextDueToDate is null OR t.OMNextDue <= @OMNextDueToDate))  
         
       
   ) AS t1 )   
             
 SELECT * FROM CTEReferralList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)      
         
END
