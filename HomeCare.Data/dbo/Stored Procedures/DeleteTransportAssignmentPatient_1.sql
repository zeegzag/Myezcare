  
CREATE PROCEDURE [dbo].[DeleteTransportAssignmentPatient]                    
(  
  @TransportID bigint = 0,                        
  @AssigneeID bigint = 0,                        
  @ClientName varchar(100) = NULL,                        
  @PayorID bigint = 0,                        
  @NotifyCaseManagerID int = -1,                        
  @ChecklistID int = -1,                        
  @ClinicalReviewID int = -1,                        
  @CaseManagerID int = 0,                        
  @ServiceID int = -1,                        
  @AgencyID bigint = 0,                        
  @AgencyLocationID bigint = 0,                        
  @ReferralStatusID bigint = 0,                        
  @IsSaveAsDraft int = -1,                        
  @AHCCCSID varchar(20) = NULL,                        
  @CISNumber varchar(20) = NULL,                        
  @IsDeleted bigint = -1,                        
  @SortExpression varchar(100) = '',                        
  @SortType varchar(10) = 'ASC',                        
  @FromIndex int=1,                        
  @PageSize int=10,                        
  @ParentName varchar(100) = NULL,                        
  @ParentPhone varchar(100) = NULL,                        
  @CaseManagerPhone varchar(100) = NULL,                        
  @LanguageID bigint = 0,                        
  @RegionID bigint = 0,                        
  @DDType_PatientSystemStatus int = 12,                        
  @EmployeeId int = 0,                        
  @ServicetypeId int = 0,                        
  @RecordAccess int = 0,                        
  @ServerDateTime nvarchar(100) = NULL,                        
  @Groupdids nvarchar(max) = NULL,                  
  @CareTypeID nvarchar(max) = null,  
  @ListOfIdsInCsv varchar(300),                      
  @IsShowList bit,                      
  @loggedInID BIGINT                      
  )  
AS                      
BEGIN                          
                       
 IF(LEN(@ListOfIdsInCsv)>0)                      
 BEGIN                      
                       
  UPDATE TransportAssignPatient  SET IsDeleted=CASE IsNull(IsDeleted,0) WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE()       
  WHERE TransportAssignPatientID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))                       
                          
 END                      
                      
 IF(@IsShowList=1)                      
 BEGIN                      
  --EXEC [dbo].[TransportAssignmentList] @FacilityID, @VehicleID, @RouteCode, @StartDate, @EndDate, @Attendent, @IsDeleted, @SortExpression, @SortType, @FromIndex, @PageSize                      
EXEC [dbo].GetTransportAssignPatient   
    @TransportID  
  , @AssigneeID  
  , @ClientName  
  , @PayorID  
  , @NotifyCaseManagerID  
  , @ChecklistID  
  , @ClinicalReviewID  
  , @CaseManagerID  
  , @ServiceID  
  , @AgencyID  
  , @AgencyLocationID  
  , @ReferralStatusID  
  , @IsSaveAsDraft  
  , @AHCCCSID  
  , @CISNumber  
  , @IsDeleted  
  , @SortExpression  
  , @SortType  
  , @FromIndex  
  , @PageSize  
  , @ParentName  
  , @ParentPhone  
  , @CaseManagerPhone  
  , @LanguageID  
  , @RegionID  
  , @DDType_PatientSystemStatus  
  , @EmployeeId  
  , @ServicetypeId  
  , @RecordAccess  
  , @ServerDateTime  
  , @Groupdids  
  , @CareTypeID  
 END                      
END