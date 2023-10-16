CREATE PROCEDURE  [dbo].[Rpt_GetClientSatus]                                     
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
 @CISNumber varchar(20)=null ,                  
 @IsDeleted bigint   =-1                       
AS                                          
BEGIN                                 
 SELECT  R.LastName+', '+R.FirstName as ClientName,r.CISNumber,A.NickName as Agency,P.PayorName as Payor, RG.RegionName as Region,                        
                      CONVERT(VARCHAR(10),CONVERT(datetime,r.LastAttendedDate,1),111) as Last_Attended_Date        
                      ,RS.Status                                    
  from Referrals R                                          
   left join ReferralStatuses RS on RS.ReferralStatusID=R.ReferralStatusID                                          
   left join Employees E on E.EmployeeID=R.Assignee                                          
   left join ReferralPayorMappings RP on RP.ReferralID=R.ReferralID and RP.IsActive=1 and RP.IsDeleted=0                                          
   left join Payors P on P.PayorID=RP.PayorID                                          
   left join CaseManagers CM on CM.CaseManagerID=R.CaseManagerID                                          
   left join Agencies A on A.AgencyID=R.AgencyID                                          
   left join AgencyLocations AL on AL.AgencyLocationID=R.AgencyLocationID                                          
   left join ReferralCheckLists RC on RC.ReferralID=R.ReferralID                                          
   left join ReferralSparForms RSF on RSF.ReferralID=R.ReferralID                                          
   left join Employees ES on ES.EmployeeID=RSF.SparFormCompletedBy                                          
   left join Employees ER on er.EmployeeID=rc.ChecklistCompletedBy                                          
   left join Employees EMP on EMP.EmployeeID=r.CreatedBy                                            
   left join Regions RG on R.RegionID=RG.RegionID                                    
   WHERE                                     
       ((CAST(@IsDeleted AS BIGINT)=-1) OR R.IsDeleted=@IsDeleted)                                        
   AND (( CAST(@AgencyID AS BIGINT)=0) OR R.AgencyID = CAST(@AgencyID AS BIGINT))                                          
   AND (( CAST(@RegionID AS BIGINT)=0) OR R.RegionID= CAST(@RegionID AS BIGINT))                                    
   AND (( CAST(@ReferralStatusID AS BIGINT)=0) OR R.ReferralStatusID = CAST(@ReferralStatusID AS BIGINT))                                            
   AND ((@StartDate is null OR R.ReferralDate>= @StartDate) and (@EndDate is null OR R.ReferralDate<= @EndDate))                              
   AND ((@ClientName IS NULL OR LEN(@ClientName)=0) OR R.FirstName+' '+r.LastName LIKE '%' + @ClientName + '%' OR R.LastName+' '+R.FirstName LIKE '%' + @ClientName + '%')                                          
   AND ((@AHCCCSID IS NULL OR LEN(@AHCCCSID)=0) OR r.AHCCCSID LIKE '%' + @AHCCCSID + '%')                               
   AND ((@CISNumber IS NULL OR LEN(@CISNumber)=0) OR r.CISNumber LIKE '%' + @CISNumber + '%')                                    
   AND (( CAST(@PayorID AS BIGINT)=0) OR RP.PayorID = CAST(@PayorID AS BIGINT))                                    
   AND (( CAST(@AssigneeID AS BIGINT)=0) OR R.Assignee = CAST(@AssigneeID AS BIGINT))                                          
   AND (( CAST(@ChecklistID AS INT)=-1) OR ( CAST(@ChecklistID AS INT)=0  AND (rc.IsCheckListCompleted=0 OR rc.IsCheckListCompleted is null ) ) OR rc.IsCheckListCompleted = CAST(@ChecklistID AS INT) )                                         
   AND (( CAST(@ClinicalReviewID AS INT)=-1) OR ( CAST(@ClinicalReviewID AS INT)=0  AND (rsf.IsSparFormCompleted=0 OR rsf.IsSparFormCompleted is null ) ) OR rsf.IsSparFormCompleted = CAST(@ClinicalReviewID AS INT) )                                      
   AND (( CAST(@IsSaveAsDraft AS INT)=-1) OR r.IsSaveAsDraft = CAST(@IsSaveAsDraft AS INT))                                          
   AND (( CAST(@NotifyCaseManagerID AS INT)=-1) OR r.NotifyCaseManager = CAST(@NotifyCaseManagerID AS INT))
   AND (( CAST(@CaseManagerID AS BIGINT)=0) OR r.CaseManagerID = CAST(@CaseManagerID AS BIGINT))
   AND (( CAST(@ServiceID AS bigint)=-1)
   OR   (CAST(@ServiceID AS bigint) = 0 and r.RespiteService = 1)
   OR   (CAST(@ServiceID AS bigint) = 1 and r.LifeSkillsService = 1)
   OR   (CAST(@ServiceID AS bigint) = 2 and r.CounselingService = 1))
   AND ((CAST(@AgencyLocationID AS BIGINT)=0) OR r.AgencyLocationID = CAST(@AgencyLocationID AS BIGINT))
   ORDER BY R.LastName ASC
      
END