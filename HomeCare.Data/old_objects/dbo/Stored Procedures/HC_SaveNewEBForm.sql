-- EXEC HC_SaveNewEBForme @ReferralID = '1', @EmployeeID = '0', @EBriggsFomrID = '5beff06dd5b2df342c97c4fe', @LoggedInID = '1', @SystemID = '7C05070EAE7B'        
CREATE PROCEDURE [dbo].[HC_SaveNewEBForm]          
@ReferralID BIGINT=NULL,        
@EmployeeID BIGINT=NULL,        
@EBriggsFormID NVARCHAR(MAX)=NULL,        
@OriginalEBFormID NVARCHAR(MAX)=NULL,        
@FormId NVARCHAR(MAX)=NULL,  
@SubSectionID BIGINT,  
@EbriggsFormMppingID BIGINT=0,       
@HTMLFormContent NVARCHAR(MAX)=NULL,        
@LoggedInID BIGINT=NULL,        
@SystemID NVARCHAR(100)=NULL        
AS          
BEGIN          
    
    
              
  IF(@HTMLFormContent = '' OR LEN(@HTMLFormContent)=0) SET @HTMLFormContent = NULL        
  IF(@ReferralID = 0) SET @ReferralID = NULL        
  IF(@EmployeeID = 0) SET @EmployeeID = NULL        
      
      
  IF(@EbriggsFormMppingID>0)    
    BEGIN     -- INTERNAL FORMS UPDATE    
   UPDATE EbriggsFormMppings SET         
   ReferralID=@ReferralID, EmployeeID=@EmployeeID, UpdatedDate=GETUTCDATE(), UpdatedBy=@LoggedInID, SystemID=@SystemID , HTMLFormContent=@HTMLFormContent         
   WHERE EbriggsFormMppingID=@EbriggsFormMppingID
   
   Select @EbriggsFormMppingID;
       
    END    
  ELSE IF EXISTS ( SELECT TOP 1 EbriggsFormMppingID FROM EbriggsFormMppings WHERE OriginalEBFormID=@OriginalEBFormID AND FormId=@FormId AND      
              EBriggsFormID=@EBriggsFormID AND (ReferralID=@ReferralID OR EmployeeID=@EmployeeID) AND LEN(@HTMLFormContent)=0 )        
      
   BEGIN    -- EBRIGGS FORMS UPDATE    
    UPDATE EbriggsFormMppings SET         
    ReferralID=@ReferralID, EmployeeID=@EmployeeID, UpdatedDate=GETUTCDATE(), UpdatedBy=@LoggedInID, SystemID=@SystemID --, HTMLFormContent=@HTMLFormContent         
    WHERE OriginalEBFormID=@OriginalEBFormID AND FormId=@FormId AND  EBriggsFormID=@EBriggsFormID AND (ReferralID=@ReferralID OR EmployeeID=@EmployeeID)        
   END        
        
  ELSE        
   BEGIN        
        
   INSERT INTO EbriggsFormMppings(EBriggsFormID,OriginalEBFormID,FormId,ReferralID,EmployeeID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted,HTMLFormContent,  
   SubSectionID)    
   SELECT @EBriggsFormID,@OriginalEBFormID,@FormId,@ReferralID, @EmployeeID, GETUTCDATE(), @LoggedInID, GETUTCDATE(), @LoggedInID,@SystemID,0,@HTMLFormContent,  
   @SubSectionID  
   
   SELECT @@IDENTITY;
        
   END        
        
        
          
END
