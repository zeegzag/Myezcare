--EXEC API_IVR_ValidateIvrCode @MobileNumber = N'8866884636', @IVRPin = N'7777', @IVRBypassPermission = N'Mobile_IVR_Bypass_ClockInOut'    
CREATE PROCEDURE [dbo].[API_IVR_ValidateIvrCode]        
 @MobileNumber VARCHAR(20),        
 @IVRPin VARCHAR(4),    
 @AutoApprovedIVRBypassPermission VARCHAR(50),  
 @ApprovalRequiredIVRBypassPermission VARCHAR(100),
 @PatientPhoneNo varchar(max)  
AS  
--select @MobileNumber = N'7008533174', @IVRPin = N'1111', @AutoApprovedIVRBypassPermission = N'Mobile_IVR_Bypass_ClockInOut', @ApprovalRequiredIVRBypassPermission = N'Mobile_ApprovalRequired_IVR_Bypass_ClockInOut'      
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
     
 DECLARE @RoleID BIGINT;    
 DECLARE @PermissionID BIGINT; 
 DECLARE @IsVerified BIGINT=0;   
     Select @IsVerified=count(*) from referrals r
	 inner join ContactMappings cm on cm.ReferralID=r.ReferralID 
	 inner join Contacts c on c.ContactID=cm.ContactID and (c.Phone1=@PatientPhoneNo OR c.Phone2=@PatientPhoneNo OR c.OtherPhone=@PatientPhoneNo)
	IF( @IsVerified =0 OR @IsVerified IS NULL)  
	begin
	 SELECT @RoleID=RoleID FROM Employees WHERE MobileNumber=@MobileNumber AND IVRPin=@IVRPin    
    
 SELECT P.PermissionID FROM RolePermissionMapping RPM    
 INNER JOIN Permissions P ON P.PermissionID=RPM.PermissionID    
 WHERE RPM.RoleID=@RoleID AND (P.PermissionCode=@AutoApprovedIVRBypassPermission OR P.PermissionCode=@ApprovalRequiredIVRBypassPermission) AND RPM.IsDeleted=0    
       
	end

	ELSE
	BEGIN 
 SELECT TOP 1 P.PermissionID FROM Permissions P WHERE (P.PermissionCode=@AutoApprovedIVRBypassPermission OR P.PermissionCode=@ApprovalRequiredIVRBypassPermission)
 
 
 	END
 
    SELECT e.EmployeeID,e.FirstName,e.LastName    
 FROM dbo.Employees e         
 WHERE e.MobileNumber=@MobileNumber AND e.IVRPin=@IVRPin    
END  