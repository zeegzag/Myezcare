-- SaveServicePlanData 4, '$25/Patient/30Days', '25', '30', '2001,2002,2004', '3001,3003', 1, '192.168.1.1'  
CREATE PROCEDURE [dbo].[SaveServicePlanData]  
@ServicePlanID BIGINT,      
@ServicePlanName NVARCHAR(100),  
@PerPatientPrice FLOAT,  
@SetupFees FLOAT NULL,
@NumberOfDaysForBilling INT,  
@ListOfPermissionIdInCsv VARCHAR(MAX),  
@ListOfMobilePermissionIdInCsv VARCHAR(MAX),
@SelectedComponentIds VARCHAR(MAX),
@UDTServicePlanModules UT_ServicePlanModule READONLY,  
@LoggedInUserId BIGINT,  
@SystemID VARCHAR(20)  
AS      
BEGIN  
 DECLARE @IsEditMode BIT = CASE WHEN @ServicePlanID > 0 THEN 1 ELSE 0 END  
   
 IF OBJECT_ID('tempdb..#WebPermissions') IS NOT NULL DROP TABLE #WebPermissions  
 IF OBJECT_ID('tempdb..#SelectComponents') IS NOT NULL DROP TABLE #SelectComponents
   
 IF EXISTS  
 (  
  SELECT  
   1  
  FROM  
   ServicePlans SP  
  WHERE  
   SP.ServicePlanName = @ServicePlanName  
   AND (@ServicePlanID = 0 OR SP.ServicePlanID != @ServicePlanID)  
 )  
 BEGIN  
    
  SELECT   
   -1 AS TransactionResultId,  
   0 AS TablePrimaryId,  
   'Service Plan with the same name already exists' AS ErrorMessage  
       
  RETURN  
 END  
   
 -- Get Permissions in Temp table  
 SELECT   
  Temp.*  
 INTO  
  #SelectedPermissions  
 FROM   
 (  
  SELECT  
   CAST(Val AS BIGINT) AS PermissionID  
  FROM  
   GetCSVTable(@ListOfPermissionIdInCsv)  
  UNION  
  SELECT   
   CAST(Val AS BIGINT) AS PermissionID  
  FROM   
   GetCSVTable(@ListOfMobilePermissionIdInCsv)  
 ) AS Temp  
 
 
 -- Get Components in Temp table  
 SELECT   
  Temp.*  
 INTO  
  #SelectComponents 
 FROM   
 (  
  SELECT  
   CAST(Val AS BIGINT) AS DDMasterID  
  FROM  
   GetCSVTable(@SelectedComponentIds)
 ) AS Temp  
   
 IF(@IsEditMode = 0)  
 BEGIN  
  INSERT INTO ServicePlans  
  (  
   ServicePlanName,  
   PerPatientPrice,
   SetupFees,
   NumberOfDaysForBilling,  
   CreatedBy,  
   CreatedDate,  
   UpdatedDate,  
   UpdatedBy,  
   SystemID,  
   IsDeleted  
  )  
  VALUES  
  (  
   @ServicePlanName,  
   @PerPatientPrice,  
   @SetupFees,
   @NumberOfDaysForBilling,  
   @LoggedInUserId,  
   GETDATE(),  
   GETDATE(),  
   @LoggedInUserId,    
   @SystemID,  
   0  
  )  
    
  SET @ServicePlanID = @@IDENTITY  
    
  INSERT INTO ServicePlanPermissions  
  (  
   ServicePlanID,  
   PermissionID,  
   CreatedBy,  
   CreatedDate,  
   UpdatedDate,  
   UpdatedBy,  
   SystemID,  
   IsDeleted  
  )  
  SELECT  
   @ServicePlanID,  
   SP.PermissionID,  
   @LoggedInUserId,  
   GETDATE(),  
   GETDATE(),  
   @LoggedInUserId,  
   @SystemID,  
   0  
  FROM  
   #SelectedPermissions SP  
     
  INSERT INTO ServicePlanRates  
  (  
   ServicePlanID,  
   ModuleID,
   ModuleName,  
   MaximumAllowedNumber,  
   CreatedBy,  
   CreatedDate,  
   UpdatedDate,  
   UpdatedBy,  
   SystemID,  
   IsDeleted  
  )  
  SELECT  
   @ServicePlanID,  
   ModuleID,
   ModuleName,  
   MaximumAllowedNumber,  
   @LoggedInUserId,  
   GETDATE(),  
   GETDATE(),  
   @LoggedInUserId,  
   @SystemID,  
   0  
  FROM  
   @UDTServicePlanModules  
   
	INSERT INTO ServicePlanComponents
	SELECT
		@ServicePlanID,
		SC.DDMasterID,
		@LoggedInUserId,  
		GETDATE(),  
		GETDATE(),  
		@LoggedInUserId,  
		@SystemID,  
		0  
	FROM  
	   #SelectComponents SC
   
 END  
 ELSE  
 BEGIN  
  UPDATE ServicePlans  
  SET  
   ServicePlanName = @ServicePlanName,  
   PerPatientPrice = @PerPatientPrice,  
   SetupFees = @SetupFees,
   NumberOfDaysForBilling = @NumberOfDaysForBilling,  
   UpdatedDate = GETDATE(),  
   UpdatedBy = @LoggedInUserId  
  WHERE  
   ServicePlanID = @ServicePlanID  
   
  -- Update Permissions in table  
  MERGE ServicePlanPermissions SPP  
  USING  
   #SelectedPermissions SP  
  ON   
   SPP.ServicePlanID = @ServicePlanID  
   AND SPP.PermissionID = SP.PermissionID  
  WHEN NOT MATCHED BY TARGET THEN  
   INSERT  
   (  
    ServicePlanID,  
    PermissionID,  
    CreatedBy,  
    CreatedDate,  
    UpdatedDate,  
    UpdatedBy,  
    SystemID,  
    IsDeleted  
   )  
   VALUES  
   (  
    @ServicePlanID,  
    SP.PermissionID,  
    @LoggedInUserId,  
    GETDATE(),  
    GETDATE(),  
    @LoggedInUserId,  
    @SystemID,  
    0  
   )  
  WHEN NOT MATCHED BY SOURCE  
   AND SPP.ServicePlanID = @ServicePlanID THEN  
   DELETE;  
   
    -- Update Components in table  
  MERGE ServicePlanComponents SPC
  USING  
   #SelectComponents SC
  ON   
   SPC.ServicePlanID = @ServicePlanID  
   AND SPC.DDMasterID = SC.DDMasterID  
  WHEN NOT MATCHED BY TARGET THEN  
   INSERT  
   (  
    ServicePlanID,  
    DDMasterID,  
    CreatedBy,  
    CreatedDate,  
    UpdatedDate,  
    UpdatedBy,  
    SystemID,  
    IsDeleted  
   )  
   VALUES  
   (  
    @ServicePlanID,  
    SC.DDMasterID,  
    @LoggedInUserId,  
    GETDATE(),  
    GETDATE(),  
    @LoggedInUserId,  
    @SystemID,  
    0  
   )  
  WHEN NOT MATCHED BY SOURCE  
   AND SPC.ServicePlanID = @ServicePlanID THEN  
   DELETE;  
     
  -- Update Service Plan Rates  
  MERGE ServicePlanRates SPR  
  USING  
   @UDTServicePlanModules SPM  
  ON   
   SPR.ServicePlanID = @ServicePlanID  
   AND SPR.ModuleName = SPM.ModuleName  
   AND SPR.ModuleID = SPM.ModuleID 
   WHEN NOT MATCHED BY TARGET THEN  
   INSERT  
   (  
    ServicePlanID, 
	ModuleID,
    ModuleName,  
    MaximumAllowedNumber,  
    CreatedBy,  
    CreatedDate,  
    UpdatedDate,  
    UpdatedBy,  
    SystemID,  
    IsDeleted  
   )  
   VALUES  
   (  
    @ServicePlanID,  
	SPM.ModuleID,
    SPM.ModuleName,  
    SPM.MaximumAllowedNumber,  
    @LoggedInUserId,  
    GETDATE(),  
    GETDATE(),  
    @LoggedInUserId,  
    @SystemID,  
    0  
   )  
  WHEN MATCHED THEN  
   UPDATE SET   
    SPR.ModuleName = SPM.ModuleName,  
    SPR.ModuleID = SPM.ModuleID,
    SPR.MaximumAllowedNumber = SPM.MaximumAllowedNumber,  
    SPR.UpdatedDate = GETDATE(),  
    SPR.UpdatedBy = @LoggedInUserId;  
     
 END  
   
 SELECT   
  1 AS TransactionResultId,  
  @ServicePlanID AS TablePrimaryId  
END