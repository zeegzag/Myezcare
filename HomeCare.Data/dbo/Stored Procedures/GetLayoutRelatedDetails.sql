  
--EXEC GetLayoutRelatedDetails @PageSize = '50', @Assignee = '1', @CreatedBY = '1', @AssigneeLastCheckTime = '2018/12/04 11:37:04', @ResolvedLastCheckTime = '2018/12/04 11:37:04', @PendingVisitLastCheckTime = '2018/12/04 17:07:04'      
      
CREATE PROCEDURE [dbo].[GetLayoutRelatedDetails]          
@PageSize int=50,          
@Assignee BIGINT=0,          
@CreatedBY BIGINT=0,          
@AssigneeLastCheckTime DATETIME=NULL,          
@ResolvedLastCheckTime DATETIME=NULL,      
@PendingVisitLastCheckTime DATETIME=NULL      
          
AS          
   DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()        
BEGIN          
          
 SELECT TOP(@PageSize)  R.ReferralID,ClientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),R.AHCCCSID,InternalMessage= RIM.Note,          
 AssigneeID=EA.EmployeeID, Assignee=dbo.GetGenericNameFormat(EA.FirstName,EA.MiddleName, EA.LastName,@NameFormat), CreatedID=EC.EmployeeID,CreatedBYUserName=EC.UserName, CreatedBY= dbo.GetGenericNameFormat(EC.FirstName,EC.MiddleName, EC.LastName,@NameFormat), RIM.CreatedDate, RIM.ReferralInternalMessageID           
 FROM ReferralInternalMessages RIM          
 INNER JOIN Referrals R ON R.ReferralID=RIM.ReferralID          
 INNER JOIN Employees EA ON EA.EmployeeID=RIM.Assignee          
 INNER JOIN Employees EC ON EC.EmployeeID=RIM.CreatedBy          
 WHERE RIM.Assignee=@Assignee AND RIM.IsResolved=0 AND RIM.IsDeleted=0 Order BY RIM.ReferralInternalMessageID DESC          
        
          
          
          
 SELECT COUNT(RIM.ReferralInternalMessageID) FROM ReferralInternalMessages RIM WHERE RIM.Assignee=@Assignee AND RIM.IsResolved=0 AND RIM.IsDeleted=0          
          
 SELECT COUNT(RIM.ReferralInternalMessageID) FROM ReferralInternalMessages RIM WHERE  (@AssigneeLastCheckTime IS NULL OR RIM.CreatedDate>=@AssigneeLastCheckTime) AND RIM.Assignee=@Assignee AND RIM.IsResolved=0 AND RIM.IsDeleted=0          
          
 SELECT COUNT(RIM.ReferralInternalMessageID) FROM ReferralInternalMessages RIM WHERE  (@ResolvedLastCheckTime IS NULL OR RIM.ResolveDate>=@ResolvedLastCheckTime) AND RIM.CreatedBy=@CreatedBY AND RIM.IsResolved=1 AND RIM.IsDeleted=0          
          
 --SELECT COUNT(EmployeeVisitID) FROM EmployeeVisits WHERE ActionTaken=1 AND ((@PendingVisitLastCheckTime IS NULL OR ClockInTime>=@PendingVisitLastCheckTime) AND IsByPassClockIn=1)      
      
 SELECT COUNT(EmployeeVisitID) FROM EmployeeVisits WHERE ActionTaken=1 AND (((@PendingVisitLastCheckTime IS NULL OR ClockInTime>=@PendingVisitLastCheckTime) AND (IsByPassClockIn=1 OR   
IsApprovalRequired=1)) OR ((@PendingVisitLastCheckTime IS NULL OR ClockOutTime>=@PendingVisitLastCheckTime) AND ((IsByPassClockOut=1 AND IsByPassClockIn=0) OR IsApprovalRequired=1) ))    and isNotified=0  
 --SELECT COUNT(EmployeeVisitID) FROM EmployeeVisits WHERE ActionTaken=1 AND ((('2019/10/16 14:36:18' IS NULL OR ClockInTime>='2019/10/16 14:36:18') AND (IsByPassClockIn=1 OR IsApprovalRequired=1)) OR  
 -- (('2019/10/16 14:36:18' IS NULL OR ClockOutTime>='2019/10/16 14:36:18') AND ((IsByPassClockOut=1 AND IsByPassClockIn=0) OR IsApprovalRequired=1) ))      
  update employeeVisits set isNotified=1       WHERE ActionTaken=1 AND (((@PendingVisitLastCheckTime IS NULL OR ClockInTime>=@PendingVisitLastCheckTime) AND (IsByPassClockIn=1 OR   
IsApprovalRequired=1)) OR ((@PendingVisitLastCheckTime IS NULL OR ClockOutTime>=@PendingVisitLastCheckTime) AND ((IsByPassClockOut=1 AND IsByPassClockIn=0) OR IsApprovalRequired=1) ))    and isNotified=0  
          
 SELECT TOP(@PageSize)  R.ReferralID,ClientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),R.AHCCCSID,N.NoteComments,          
 AssigneeBy=dbo.GetGenericNameFormat(EAB.FirstName,EAB.MiddleName, EAB.LastName,@NameFormat), N.NoteID , N.NoteAssignedDate          
 FROM NOTES N          
 INNER JOIN Referrals R ON R.ReferralID=N.ReferralID          
 INNER JOIN Employees EA ON EA.EmployeeID=N.NoteAssignee AND N.NoteAssignee=@Assignee          
 INNER JOIN Employees EAB ON EAB.EmployeeID=N.NoteAssignedBy          
 WHERE N.IsDeleted=0 AND N.MarkAsComplete=0 AND R.IsDeleted=0          
 ORDER BY NoteID DESC          
          
 SELECT AssignedBillingNotesCount=COUNT(N.NoteID) FROM  NOTES N WHERE N.NoteAssignee=@Assignee AND N.IsDeleted=0 AND N.MarkAsComplete=0          
          
          
END 