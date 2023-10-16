CREATE PROCEDURE  [dbo].[Rpt_BehaviorContractTracking]   
 @BXContractStatus int = -1,     
 @WarningStartDate date=null,                                        
 @WarningEndDate date=null, 
 @ClientName varchar(100)= NULL,   
 @RegionID bigint=0,                                                                  
 @AgencyID bigint=0,                                                                   
 @ReferralStatusID bigint =0,                                                                  
 @StartDate date=null,                                                                          
 @EndDate date=null,                                                                          
 @PayorID bigint = 0,                                                                  
 @ServiceID int = -1,  
 @IsDeleted bigint=0                   
AS                                              
BEGIN                                     
 SELECT  R.LastName+', '+R.FirstName as ClientName, CONVERT(VARCHAR(10),CONVERT(datetime,RBC.WarningDate ,1),111)as WarningDate,  
 RBC.WarningReason, CONVERT(VARCHAR(10),CONVERT(datetime,RBC.CaseManagerNotifyDate ,1),111)as CaseManagerNotifyDate,   
 CASE WHEN RBC.IsActive=1 then 'Active' Else'In-Active' END as WarningStatus,  
 RSP.SuspentionType, Convert(varchar(max),RSP.SuspentionLength)+' Days' as SuspentionLength, CONVERT(VARCHAR(10),CONVERT(datetime,RSP.ReturnEligibleDate ,1),111)as ReturnEligibleDate,  
 (select Count(ReferralBehaviorContractID) from ReferralBehaviorContracts where ReferralID=R.ReferralID and IsActive=1 and IsDeleted=0) as ActiveCount          
  
                      
  from Referrals R  
     left join ReferralStatuses RS on rs.ReferralStatusID=r.ReferralStatusID                                                                          
     left join ReferralPayorMappings rp on rp.ReferralID=r.ReferralID and rp.IsActive=1 and rp.IsDeleted=0               
     left join Payors p on p.PayorID=rp.PayorID                                                                          
     left join Agencies a on a.AgencyID=r.AgencyID                                                                          
     left join Regions rg on r.RegionID=rg.RegionID   
     Inner Join ReferralBehaviorContracts RBC on RBC.ReferralID=R.ReferralID                                                                   
     left Join ReferralSuspentions RSP on RSP.ReferralID=R.ReferralID                                                                   
   WHERE  
   ((CAST(@IsDeleted AS BIGINT)=-1) OR r.IsDeleted=@IsDeleted)     AND RBC.IsDeleted=0  AND (RSP.IsDeleted IS NULL OR RSP.IsDeleted=0)                                                                           
   AND (( CAST(@AgencyID AS BIGINT)=0) OR r.AgencyID = CAST(@AgencyID AS BIGINT))                                            
   AND (( CAST(@RegionID AS BIGINT)=0) OR r.RegionID= CAST(@RegionID AS BIGINT))                                                                          
   AND (( CAST(@ReferralStatusID AS BIGINT)=0) OR r.ReferralStatusID = CAST(@ReferralStatusID AS BIGINT))                                                                                  
   AND ((@StartDate is null OR r.ReferralDate>= @StartDate) and (@EndDate is null OR r.ReferralDate<= @EndDate))     
   AND (( CAST(@PayorID AS BIGINT)=0) OR rp.PayorID = CAST(@PayorID AS BIGINT))                                                                                                                                       
   AND  
   ((@ClientName IS NULL OR LEN(r.LastName)=0)       
    OR    
   ((r.FirstName LIKE '%'+@ClientName+'%' )OR      
  (r.LastName  LIKE '%'+@ClientName+'%') OR      
  (r.FirstName +' '+r.LastName like '%'+@ClientName+'%') OR      
  (r.LastName +' '+r.FirstName like '%'+@ClientName+'%') OR      
  (r.FirstName +', '+r.LastName like '%'+@ClientName+'%') OR      
  (r.LastName +', '+r.FirstName like '%'+@ClientName+'%'))    
    )  
    AND (( CAST(@ServiceID AS bigint)=-1)    
   OR   (CAST(@ServiceID AS bigint) = 0 and r.RespiteService = 1)    
   OR   (CAST(@ServiceID AS bigint) = 1 and r.LifeSkillsService = 1)    
   OR   (CAST(@ServiceID AS bigint) = 2 and r.CounselingService = 1)
   OR  (CAST(@ServiceID AS bigint) = 3 and r.ConnectingFamiliesService = 1))    
   AND ((CAST(@BXContractStatus AS BIGINT)=-1) OR RBC.IsActive=@BXContractStatus) 
   AND ((@WarningStartDate is null OR RBC.WarningDate>= @WarningStartDate) and (@WarningEndDate is null OR RBC.WarningDate<= @WarningEndDate))    
    
   ORDER BY R.LastName ASC , RBC.WarningDate Desc   
END