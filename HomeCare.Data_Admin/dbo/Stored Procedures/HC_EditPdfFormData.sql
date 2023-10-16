CREATE PROCEDURE [dbo].[HC_EditPdfFormData]    
(    
 @EBFormId int,    
 @OrganizationId int,    
 @FormData nvarchar(max),    
 @FormDataId int,    
 @ReferralId INT,    
 @EmployeeId int,    
 @UserId INT,    
 @EBrigFormId NVARCHAR(1000),  
   
 @OriginalEBFormID   NVARCHAR(1000),  
 @SubSectionID    BIGINT,  
 @FormName NVARCHAR(MAX),  
 @UpdateFormName NVARCHAR(MAX),  
 @EbriggsFormMppingID BIGINT=0  
)    
as    
    
BEGIN    
    
IF( @EmployeeId<1)    
BEGIN    
 SET @EmployeeId = NULL;    
END    
    
BEGIN TRY     
 BEGIN TRAN    
 DECLARE @DBName NVARCHAR(1000)    
    
 select @DBName = DBName FROM dbo.Organizations WHERE OrganizationID = @OrganizationId    
    
 declare @NewFormId int    
 select @EBFormId AS EBFormId    

 
    
 IF( CONVERT(BIGINT,@EBrigFormId) = 0)    
 BEGIN    
      
  INSERT INTO OrganizationFormData    
  (    
   EBFormId,    
   FormId,    
   OrganizationId,    
   FormData,    
   --TypeId ,    
   CreatedDate     
  )    
  SELECT     
   @OriginalEBFormID,    
   @EBFormId,    
   @OrganizationId,    
   @FormData,    
   --@TypeId,    
   GETUTCDATE()    
    
  SET @NewFormId = SCOPE_IDENTITY()    
 END    
 ELSE    
 BEGIN    
  UPDATE OrganizationFormData    
  SET FormData = @FormData,    
  ModifiedOn = GETUTCDATE()    
  WHERE     
   OrganizationFormDataId = @EBrigFormId     
 END    

    
    
 IF( @NewFormId>0)    
 BEGIN    
  DECLARE @Query NVARCHAR(MAX) = '';    
    
  IF(@SubSectionID>0)
  BEGIN
  PRINT @SubSectionID
  
		SET @Query =  'EXEC '+@DBName+'.dbo.HC_SavedNewHtmlFormWithSubsection @ReferralID ='''+ CONVERT(VARCHAR(100),@ReferralId)+''', 
		@EmployeeID='''+CONVERT(VARCHAR(100),ISNULL(@EmployeeId,''))+''', 
		@SubSectionID ='''+ CONVERT(VARCHAR(100),@SubSectionID)+''', @EBriggsFormID = '''+CONVERT(VARCHAR(100),@NewFormId)+''', 
		@OriginalEBFormID = '''+CONVERT(VARCHAR(100),@OriginalEBFormID)+''', @FormName = '''+CONVERT(VARCHAR(100),@FormName)+''', @UpdateFormName = '''+@UpdateFormName+''',
		@FormId ='''+ CONVERT(VARCHAR(100),@EBFormId)+''',@EbriggsFormMppingID='''+  CONVERT(VARCHAR(100),@EbriggsFormMppingID) +''',
		@LoggedInID = '''+ CONVERT(VARCHAR(100),@UserId) +''', @SystemID = ''192.168.1.32''' ;  
    
		EXEC sp_executesql @Query  
		 
		--PRINT (@Query)
   
  END
  ELSE
  BEGIN
     
	   SET @Query = 'insert into '+@DBName+'.dbo.EbriggsFormMppings(EBriggsFormID,OriginalEBFormID,FormId,ReferralID,EmployeeID,CreatedDate,UpdatedDate,UpdatedBy,CreatedBy,IsDeleted)';    
	   SET @Query+=' Values('''+CAST(@NewFormId AS VARCHAR(100))+''','''+CAST(@OriginalEBFormID AS VARCHAR(100))+''','+CAST(@EBFormId AS VARCHAR(100))+','+CAST(@ReferralId AS VARCHAR(100))  
	   +','+CONVERT(VARCHAR(100),ISNULL(@EmployeeId,''))+',GETUTCDATE(),GETUTCDATE(),'+CAST(@UserId AS VARCHAR(100))+','+CAST(@UserId AS VARCHAR(100))+',0)';    
	   
	    EXEC sp_executesql @Query   
	   -- PRINT @Query  
	    
  END
  
  
  
  
  
 END    
    
    
    
 SELECT @@ROWCOUNT AS RowsAffected     
    
 COMMIT    
    
 END TRY    
    
 BEGIN CATCH    
  ROLLBACK    
    
  DECLARE @ErrorMessage NVARCHAR(4000);    
    DECLARE @ErrorSeverity INT;    
    DECLARE @ErrorState INT;    
      
    SELECT     
     @ErrorMessage = ERROR_MESSAGE(),    
     @ErrorSeverity = ERROR_SEVERITY(),    
     @ErrorState = ERROR_STATE();    
    
    RAISERROR (@ErrorMessage, -- Message text.    
         @ErrorSeverity, -- Severity.    
         @ErrorState -- State.    
         );    
    
    
 END CATCH    
END