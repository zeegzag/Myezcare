CREATE PROCEDURE [dbo].[API_GetAppointedPatients]      
 @ServerCurrentDate DATE,                                                                
 @EmployeeId bigint                                                                
AS                                                                
BEGIN      
       
 DECLARE @MaxDate DATE='2019-12-31';      
      
 SELECT DISTINCT RC.ReferralCaseloadID,RC.ReferralID,dbo.GetGeneralNameFormat(R.FirstName,R.LastName) AS PatientName,C.Email,Phone=C.Phone1,ImageUrl=R.ProfileImagePath,      
 --CTS.CareTypeTimeSlotID,
 FirstCharOfName=LEFT(R.FirstName, 1)    
 FROM ReferralCaseloads RC      
 LEFT JOIN CareTypeTimeSlots CTS ON CTS.ReferralID=RC.ReferralID      
 INNER JOIN Referrals R ON R.ReferralID=RC.ReferralID      
 INNER JOIN ContactMappings CM ON CM.ReferralID=RC.ReferralID AND CM.ContactTypeID=1      
 INNER JOIN Contacts C ON C.ContactID=CM.ContactID AND C.IsDeleted=0  
 WHERE RC.EmployeeID=@EmployeeId AND RC.IsDeleted=0 AND (@ServerCurrentDate >= CONVERT(DATE,RC.StartDate) AND       
 @ServerCurrentDate <= CONVERT(DATE,ISNULL(RC.EndDate,@MaxDate)))      
      
END
