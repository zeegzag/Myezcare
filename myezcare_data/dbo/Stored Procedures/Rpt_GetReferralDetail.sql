--exec Rpt_GetReferralDetail                
CREATE procedure [dbo].[Rpt_GetReferralDetail]                                
 @RegionID bigint=0,                              
 @AgencyID bigint=0,                               
 @ReferralStatusID bigint =0,                              
 @StartDate date=null,                                      
 @EndDate date=null,                                      
 @AssigneeID bigint= 0,                              
 @ClientName varchar(100) = NULL,                                       
 @PayorID bigint = 0,                              
 @NotifyCaseManagerID int = -1,                                      
 @ChecklistID INT = -1,                                      
 @ClinicalReviewID INT = -1,                                      
 @CaseManagerID int = 0,                                      
 @ServiceID int = -1,                                            
 @AgencyLocationID bigint =0,                                    
 @IsSaveAsDraft int=-1,                                      
 @AHCCCSID varchar(20)=null,                                      
 @CISNumber varchar(20)=null,                  
 @IsDeleted bigint =0                              
AS                                      
BEGIN                                          
                                  
 SELECT  r.LastName+', '+r.FirstName as ClientName,a.NickName as Agency,P.PayorName as Payor, al.LocationName as AgencyLocation,                         
 CONVERT(VARCHAR(10),CONVERT(datetime,r.ReferralDate,1),111) as ReferralDate                
                 
 ,RS.Status                                
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
   left join Regions rg on r.RegionID=rg.RegionID                                
   WHERE                                 
  ((CAST(@IsDeleted AS BIGINT)=-1) OR r.IsDeleted=@IsDeleted)                                             
   AND (( CAST(@AgencyID AS BIGINT)=0) OR r.AgencyID = CAST(@AgencyID AS BIGINT))                                            
   AND (( CAST(@RegionID AS BIGINT)=0) OR r.RegionID= CAST(@RegionID AS BIGINT))                                      
   AND (( CAST(@ReferralStatusID AS BIGINT)=0) OR r.ReferralStatusID = CAST(@ReferralStatusID AS BIGINT))                                              
   AND ((@StartDate is null OR r.ReferralDate>= @StartDate) and (@EndDate is null OR r.ReferralDate<= @EndDate))                              
   AND ((@ClientName IS NULL OR LEN(@ClientName)=0) OR r.FirstName+' '+r.LastName LIKE '%' + @ClientName + '%' OR r.LastName+' '+r.FirstName LIKE '%' + @ClientName + '%')                                      
   AND ((@AHCCCSID IS NULL OR LEN(@AHCCCSID)=0) OR r.AHCCCSID LIKE '%' + @AHCCCSID + '%')         
   AND ((@CISNumber IS NULL OR LEN(@CISNumber)=0) OR r.CISNumber LIKE '%' + @CISNumber + '%')                                
   AND (( CAST(@PayorID AS BIGINT)=0) OR rp.PayorID = CAST(@PayorID AS BIGINT))                                
   AND (( CAST(@AssigneeID AS BIGINT)=0) OR r.Assignee = CAST(@AssigneeID AS BIGINT))                                      
   AND (( CAST(@ChecklistID AS INT)=-1) OR ( CAST(@ChecklistID AS INT)=0  AND (rc.IsCheckListCompleted=0 OR rc.IsCheckListCompleted is null ) ) OR rc.IsCheckListCompleted = CAST(@ChecklistID AS INT) )                                     
   AND (( CAST(@ClinicalReviewID AS INT)=-1) OR ( CAST(@ClinicalReviewID AS INT)=0  AND (rsf.IsSparFormCompleted=0 OR rsf.IsSparFormCompleted is null ) ) OR rsf.IsSparFormCompleted = CAST(@ClinicalReviewID AS INT) )                                  
   AND (( CAST(@IsSaveAsDraft AS INT)=-1) OR r.IsSaveAsDraft = CAST(@IsSaveAsDraft AS INT))                                      
   AND (( CAST(@NotifyCaseManagerID AS INT)=-1) OR r.NotifyCaseManager = CAST(@NotifyCaseManagerID AS INT))                                     
   AND (( CAST(@CaseManagerID AS BIGINT)=0) OR r.CaseManagerID = CAST(@CaseManagerID AS BIGINT))                         
   AND (( CAST(@ServiceID AS bigint)=-1)                                 
   OR (CAST(@ServiceID AS bigint) = 0 and r.RespiteService = 1)                                       
   OR (CAST(@ServiceID AS bigint) = 1 and r.LifeSkillsService = 1)                                      
   OR (CAST(@ServiceID AS bigint) = 2 and r.CounselingService = 1))                      
   AND (( CAST(@AgencyLocationID AS BIGINT)=0) OR r.AgencyLocationID = CAST(@AgencyLocationID AS BIGINT))           
   ORDER BY R.LastName ASC                                 
                              
END 
