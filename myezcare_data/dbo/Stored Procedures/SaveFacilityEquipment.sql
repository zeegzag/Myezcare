-- SaveFacilityEquipment 35
CREATE PROCEDURE [dbo].[SaveFacilityEquipment]            
@FacilityID BIGINT,
@LoggedInUserId BIGINT,
@SystemID VARCHAR(100),
@EquipmentIDs VARCHAR(MAX)
AS            
BEGIN
	IF OBJECT_ID('tempdb..#SelectEquipments') IS NOT NULL DROP TABLE #SelectEquipments
	
	SELECT   
		Temp.*  
	INTO  
		#SelectEquipments 
	FROM   
	(  
		SELECT  
			CAST(Val AS BIGINT) AS DDMasterID  
		FROM  
			GetCSVTable(@EquipmentIDs)
	) AS Temp 
	
	IF(LEN(@EquipmentIDs) > 0)
	BEGIN
		MERGE FacilityEquipments FE
		USING  
			#SelectEquipments SE
		ON
			FE.FacilityID = @FacilityID  
			AND FE.DDMasterID = SE.DDMasterID
		WHEN NOT MATCHED BY TARGET THEN  
		INSERT  
		(  
			FacilityID,  
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
			@FacilityID,  
			SE.DDMasterID,  
			@LoggedInUserId,
			GETDATE(),  
			GETDATE(),  
			@LoggedInUserId,  
			@SystemID,
			0
		)  
		WHEN NOT MATCHED BY SOURCE  
			AND FE.FacilityID = @FacilityID 
		THEN  
			DELETE;
	END
	
	SELECT 1 AS FacilityID
END
