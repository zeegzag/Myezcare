CREATE PROCEDURE [dbo].[SaveCommonNote]       
 @EmployeeID bigint=NULL,      
 @ReferralID bigint=NULL,      
 @IsEdit bit=0,      
 @CommonNoteID bigint=NULL,      
 @NoteDetail nvarchar(max),    
 @catId bigint,      
 @LoggedinUserID bigint,    
 @RoleID nvarchar(100),    
 @EmployeesID nvarchar(100),  
 @isPrivate bit =1  
AS      
BEGIN      
 IF (@IsEdit=1)      
  BEGIN      
   UPDATE [dbo].[CommonNotes]      
   SET Note=@NoteDetail,UpdatedBy=@LoggedinUserID,UpdatedDate=GETUTCDATE(),CategoryID=@catId,  RoleID=@roleID,EmployeesID=@EmployeesID,isPrivate=@isPrivate    
   WHERE CommonnoteID=@CommonNoteID      
  END      
 ELSE      
  BEGIN      
   IF (@EmployeeID is null OR @EmployeeID=0)      
    INSERT INTO [dbo].[CommonNotes](EmployeeID,ReferralID,Note,IsDeleted,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,RoleID,EmployeesID,CategoryID,isPrivate)      
    VALUES (NULL,@ReferralID,@NoteDetail,0,@LoggedinUserID,GETUTCDATE(),@LoggedinUserID,GETUTCDATE(),@RoleID,@EmployeesID,@catId,@isPrivate)      
   ELSE      
    INSERT INTO [dbo].[CommonNotes](EmployeeID,ReferralID,Note,IsDeleted,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,RoleID,EmployeesID,CategoryID,isPrivate)      
    VALUES (@EmployeeID,NULL,@NoteDetail,0,@LoggedinUserID,GETUTCDATE(),@LoggedinUserID,GETUTCDATE(),@RoleID,@EmployeesID,@catId,@isPrivate)      
  END      
END