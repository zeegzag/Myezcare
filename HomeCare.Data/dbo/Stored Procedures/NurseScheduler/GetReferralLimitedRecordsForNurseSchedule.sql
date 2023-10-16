USE [Live_AHSAPPO]
GO

/****** Object:  StoredProcedure [dbo].[GetReferralLimitedRecordsForNurseSchedule]    Script Date: 12/8/2020 11:02:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ali H
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetReferralLimitedRecordsForNurseSchedule]                      
 @AssigneeID bigint= 0,                    
 @ClientName varchar(100) = NULL,                     
 @PayorID bigint = 0,                    
 @NotifyCaseManagerID int = -1,                    
 @ChecklistID INT = -1,                    
 @ClinicalReviewID INT = -1,                    
 @CaseManagerID int = 0,                    
 @ServiceID int = -1,                    
 @AgencyID bigint=0,                    
 @AgencyLocationID bigint =0,                    
 @ReferralStatusID bigint =0,                    
 @IsSaveAsDraft int=-1,                    
 @AHCCCSID varchar(20)=null,                    
 @CISNumber varchar(20)=null,                    
 @IsDeleted BIGINT =-1,                    
 @SortExpression VARCHAR(100),                      
 @SortType VARCHAR(10),                                   
 @ParentName varchar(100)=null,            
 @ParentPhone varchar(100)=null,            
 @CaseManagerPhone varchar(100)=null,            
 @LanguageID bigint =0,          
 @RegionID bigint=0,    
 @DDType_PatientSystemStatus INT = 12 ,
 @employeeId int =0        
AS                    
BEGIN                        
 ;WITH CTEReferralList AS                    
 (                     
 SELECT  *,COUNT(t1.ReferralID) OVER() AS Count FROM                     
  (                    
  SELECT ROW_NUMBER() OVER (ORDER BY               
              
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN CONVERT(varchar(50),t.AHCCCSID) END END ASC,                                                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN CONVERT(varchar(50),t.AHCCCSID) END END DESC,                   
                   
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CISNumber' THEN CAST(t.CISNumber AS bigint) END END ASC,                                                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CISNumber' THEN CAST(t.CISNumber AS bigint) END END DESC,             
              
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Gender' THEN CONVERT(char(1),t.Gender) END END ASC,                                                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Gender' THEN CONVERT(char(1),t.Gender) END END DESC,                    
                        
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Age' THEN t.Dob END END ASC,      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Age' THEN t.Dob END END DESC,      
                    
    CASE WHEN @SortType = 'ASC' THEN                    
      CASE                    
      WHEN @SortExpression = 'ClientName' THEN t.Name            
   WHEN @SortExpression = 'NickName' THEN t.ClientNickName                    
      WHEN @SortExpression = 'FaciliatorName' THEN t.FaciliatorName                    
      --WHEN @SortExpression = 'ContractName' THEN t.ContractName              
   WHEN @SortExpression = 'CompanyName' THEN t.CompanyName              
      WHEN @SortExpression = 'Status' THEN t.Status                    
      WHEN @SortExpression = 'AssigneeName' THEN t.AssigneeName                    
   WHEN @SortExpression = 'Address' THEN t.Address      
      WHEN @SortExpression = 'CreatedDate' THEN CONVERT(varchar(10), t.CreatedDate, 105)                    
      WHEN @SortExpression = 'ModifiedDate' THEN CONVERT(varchar(10), t.UpdatedDate, 105)                    
      END                     
    END ASC,                    
    CASE WHEN @SortType = 'DESC' THEN                    
      CASE                     
      WHEN @SortExpression = 'ClientName' THEN t.Name                    
   WHEN @SortExpression = 'NickName' THEN t.ClientNickName        
      WHEN @SortExpression = 'FaciliatorName' THEN t.FaciliatorName                    
      --WHEN @SortExpression = 'ContractName' THEN t.ContractName              
   WHEN @SortExpression = 'CompanyName' THEN t.CompanyName                    
      WHEN @SortExpression = 'Status' THEN t.Status                    
  WHEN @SortExpression = 'AssigneeName' THEN t.AssigneeName      
   WHEN @SortExpression = 'Address' THEN t.Address      
      WHEN @SortExpression = 'CreatedDate' THEN CONVERT(varchar(10), t.CreatedDate, 105)--CONVERT(DateTime, r.CreatedDate)                    
      WHEN @SortExpression = 'ModifiedDate' THEN CONVERT(varchar(10), t.UpdatedDate, 105) --CONVERT(DateTime, r.UpdatedDate)                          
      END                    
    END DESC                    
  ) AS Row,             
    t.* from (select DISTINCT r.ReferralID,r.LastName+', '+r.FirstName as Name,r.ClientID,r.AHCCCSID,r.ReferralStatusID,r.IsSaveAsDraft,r.CISNumber,r.Gender, r.Dob, dbo.GetAge(r.Dob) as Age ,                   
   RS.Status,r.Assignee,e.LastName+', '+e.FirstName as AssigneeName,--rp.PayorID,p.ShortName as ContractName,
   r.PlacementRequirement,                    
   r.CaseManagerID,cm.LastName+', '+cm.FirstName as FaciliatorName,r.ZSPLifeSkills,r.ZSPRespite,r.ZSPCounselling,r.CreatedDate,r.CreatedBy,eself.LastName+', '+eself.FirstName as CreatedName,                    
   a.NickName as CompanyName,al.LocationName,rc.IsCheckListCompleted,rc.ChecklistCompletedBy,er.LastName+' '+er.FirstName as CheckListName,rc.ChecklistCompletedDate,                    
   rsf.IsSparFormCompleted,rsf.SparFormCompletedBy,es.LastName+', '+es.FirstName as ClinicalReviewName,rsf.SparFormCompletedDate,r.NotifyCaseManager,                    
   r.AgencyID,r.AgencyLocationID,r.UpdatedDate,eselfUP.LastName+', '+eselfUP.FirstName as UpdatedName, r.IsDeleted,r.CounselingService,r.LifeSkillsService,r.RespiteService,r.ClientNickName,c.Address,c.City,c.ZipCode,c.State      
   from Referrals r                    
   left join ReferralStatuses RS ON rs.ReferralStatusID=r.ReferralStatusID                   
   left join Employees e on e.EmployeeID=r.Assignee                    
   left join ReferralPayorMappings rp on rp.ReferralID=r.ReferralID and rp.IsActive=1 and rp.IsDeleted=0 AND rp.Precedence=1  
   left join Payors p on p.PayorID=rp.PayorID                    
   left join CaseManagers cm on cm.CaseManagerID=r.CaseManagerID                    
   left join Agencies a on a.AgencyID=cm.AgencyID               
   left join AgencyLocations al on al.AgencyLocationID=r.AgencyLocationID                    
   left join ReferralCheckLists rc on rc.ReferralID=r.ReferralID                    
   left join ReferralSparForms rsf on rsf.ReferralID=r.ReferralID                    
   left join Employees es on es.EmployeeID=rsf.SparFormCompletedBy                    
   left join Employees er on er.EmployeeID=rc.ChecklistCompletedBy                    
   left join Employees eself on eself.EmployeeID=r.CreatedBy            
   left join Employees eselfUP on eselfUP.EmployeeID=r.UpdatedBy                      
   --left join ContactMappings cmp on cmp.ReferralID= r.ReferralID and cmp.ContactTypeID in (1,2)            
   left join ContactMappings cmp on cmp.ReferralID= r.ReferralID and cmp.ContactTypeID = 1        
   left join Contacts c on c.ContactID=cmp.ContactID                  
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR r.IsDeleted=@IsDeleted)                     
  AND ((@ClientName IS NULL OR LEN(r.LastName)=0)         
  --OR r.FirstName+' '+r.LastName LIKE '%' + @ClientName + '%' OR r.LastName+' '+r.FirstName LIKE '%' + @ClientName + '%')                    
    OR        
  ( (r.FirstName LIKE '%'+@ClientName+'%' )OR          
    (r.LastName  LIKE '%'+@ClientName+'%') OR          
    (r.FirstName +' '+r.LastName like '%'+@ClientName+'%') OR          
    (r.LastName +' '+r.FirstName like '%'+@ClientName+'%') OR          
    (r.FirstName +', '+r.LastName like '%'+@ClientName+'%') OR          
    (r.LastName +', '+r.FirstName like '%'+@ClientName+'%'))        
  )           
   AND ((@AHCCCSID IS NULL OR LEN(@AHCCCSID)=0) OR r.AHCCCSID LIKE '%' + @AHCCCSID + '%')               
   AND ((@CISNumber IS NULL OR LEN(@CISNumber)=0) OR r.CISNumber LIKE '%' + @CISNumber + '%')                    
   AND (( CAST(@PayorID AS BIGINT)=0) OR rp.PayorID = CAST(@PayorID AS BIGINT))     
   AND (( CAST(@ReferralStatusID AS BIGINT)=0) OR r.ReferralStatusID = CAST(@ReferralStatusID AS BIGINT))                      
   AND (( CAST(@AssigneeID AS BIGINT)=0) OR r.Assignee = CAST(@AssigneeID AS BIGINT))                    
   AND (( CAST(@ChecklistID AS INT)=-1) OR ( CAST(@ChecklistID AS INT)=0  AND (rc.IsCheckListCompleted=0 OR rc.IsCheckListCompleted is null ) ) OR rc.IsCheckListCompleted = CAST(@ChecklistID AS INT) )           
   AND (( CAST(@ClinicalReviewID AS INT)=-1) OR ( CAST(@ClinicalReviewID AS INT)=0  AND (rsf.IsSparFormCompleted=0 OR rsf.IsSparFormCompleted is null ) ) OR rsf.IsSparFormCompleted = CAST(@ClinicalReviewID AS INT) )         
   AND (( CAST(@IsSaveAsDraft AS INT)=-1) OR r.IsSaveAsDraft = CAST(@IsSaveAsDraft AS INT))                    
   AND (( CAST(@NotifyCaseManagerID AS INT)=-1) OR r.NotifyCaseManager = CAST(@NotifyCaseManagerID AS INT))                     
   AND (( CAST(@CaseManagerID AS BIGINT)=0) OR r.CaseManagerID = CAST(@CaseManagerID AS BIGINT))                    
   AND (( CAST(@ServiceID AS bigint)=-1)                    
   OR (CAST(@ServiceID AS bigint) = 0 and r.RespiteService = 1)                     
   OR (CAST(@ServiceID AS bigint) = 1 and r.LifeSkillsService = 1)                    
   OR (CAST(@ServiceID AS bigint) = 2 and r.CounselingService = 1)        
   OR (CAST(@ServiceID AS bigint) = 3 and r.ConnectingFamiliesService = 1))                    
   AND (( CAST(@AgencyID AS BIGINT)=0) OR r.AgencyID = CAST(@AgencyID AS BIGINT))                    
   AND (( CAST(@AgencyLocationID AS BIGINT)=0) OR r.AgencyLocationID = CAST(@AgencyLocationID AS BIGINT))                       
           
   AND (        
   (@ParentName IS NULL OR LEN(c.LastName)=0)         
   OR (        
       (c.FirstName LIKE '%'+@ParentName+'%' )OR          
    (c.LastName  LIKE '%'+@ParentName+'%') OR          
    (C.FirstName +' '+C.LastName like '%'+@ParentName+'%') OR          
    (C.LastName +' '+C.FirstName like '%'+@ParentName+'%') OR          
    (C.FirstName +', '+C.LastName like '%'+@ParentName+'%') OR          
    (C.LastName +', '+C.FirstName like '%'+@ParentName+'%')))        
             
   AND ((@ParentPhone IS NULL OR LEN(@ParentPhone)=0) OR (c.Phone1 LIKE '%' + @ParentPhone + '%') OR (c.Phone2 LIKE '%' + @ParentPhone + '%'))        
   AND ((@CaseManagerPhone IS NULL OR LEN(@CaseManagerPhone)=0) OR cm.Phone LIKE '%' + @CaseManagerPhone + '%')                    
   AND (( CAST(@LanguageID AS BIGINT)=0) OR r.LanguageID = CAST(@LanguageID AS BIGINT))                    
   AND (( CAST(@RegionID AS BIGINT)=0) OR r.RegionID= CAST(@RegionID AS BIGINT))      
  -- AND r.ReferralID =  (SELECT DISTINCT ReferralID FROM ScheduleMasters WHERE EmployeeID = @AssigneeID)           
 ) as t              
               
   ) AS t1 )                    
                     
 SELECT * FROM CTEReferralList      
                 
END
GO

