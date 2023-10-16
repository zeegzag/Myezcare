--EXEC SaveCommonNote @EmployeeID=NULL, @ReferralID = '4005', @NoteDetail = 'sdacv saev', @LoggedInUserID = '1'  
--EXEC SaveCommonNote @EmployeeID='204', @ReferralID = null, @NoteDetail = 'Note2', @LoggedInUserID = '1'  
CREATE PROCEDURE [dbo].[SaveCommonNote]   
 @EmployeeID bigint=NULL,  
 @ReferralID bigint=NULL,  
 @IsEdit bit=0,  
 @CommonNoteID bigint=NULL,  
 @NoteDetail nvarchar(2000),  
 @LoggedinUserID bigint,
 @RoleID nvarchar(100),
 @EmployeesID nvarchar(100)  
AS  
BEGIN  
 IF (@IsEdit=1)  
  BEGIN  
   UPDATE [dbo].[CommonNotes]  
   SET Note=@NoteDetail,UpdatedBy=@LoggedinUserID,UpdatedDate=GETUTCDATE()  
   WHERE CommonnoteID=@CommonNoteID  
  END  
 ELSE  
  BEGIN  
   IF (@EmployeeID is null OR @EmployeeID=0)  
    INSERT INTO [dbo].[CommonNotes](EmployeeID,ReferralID,Note,IsDeleted,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,RoleID,EmployeesID)  
    VALUES (NULL,@ReferralID,@NoteDetail,0,@LoggedinUserID,GETUTCDATE(),@LoggedinUserID,GETUTCDATE(),@RoleID,@EmployeesID)  
   ELSE  
    INSERT INTO [dbo].[CommonNotes](EmployeeID,ReferralID,Note,IsDeleted,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,RoleID,EmployeesID)  
    VALUES (@EmployeeID,NULL,@NoteDetail,0,@LoggedinUserID,GETUTCDATE(),@LoggedinUserID,GETUTCDATE(),@RoleID,@EmployeesID)  
  END  
END
