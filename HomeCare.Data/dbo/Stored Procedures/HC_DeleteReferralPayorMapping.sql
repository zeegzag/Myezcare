-- [HC_DeleteReferralPayorMapping] 14232,null,null,null,null,null,'PayorName','ASC',1,100,null,1
CREATE PROCEDURE [dbo].[HC_DeleteReferralPayorMapping]                  
@ReferralID bigint=null,                                         
@PayorName nvarchar(50)=null,                                      
@Precedence int=0,  
@IsDeleted int = -1,                        
@PayorEffectiveDate date=null,                                              
@PayorEffectiveEndDate date=null,       
           
@SORTEXPRESSION VARCHAR(100),                                        
@SORTTYPE VARCHAR(10),                                      
@FROMINDEX INT,                                      
@PAGESIZE INT  ,             
@ListOfIdsInCSV varchar(300)=null,                              
@IsShowList bit,
@BeneficiaryTypeID bigint=null 
AS                              
BEGIN                                  
    
DECLARE @tempAllRecords TABLE (RPMID bigint,PayorID bigint,StartDate Date,EndDate Date,Precedence int,IsDeleted bit);
DECLARE @tempCSVRecords TABLE (RPMID bigint,PayorID bigint,StartDate Date,EndDate Date,Precedence int,IsDeleted bit);
DECLARE @idsForUpdate varchar(max)=null;
DECLARE @idsForNotupdate varchar(max)=null;
	                          
 IF(LEN(@ListOfIdsInCSV)>0)                              
 BEGIN 
 
INSERT INTO @tempAllRecords
SELECT ReferralPayorMappingID,PayorID,PayorEffectiveDate,PayorEffectiveEndDate,Precedence,IsDeleted
FROM ReferralPayorMappings 
WHERE ReferralID=@ReferralID AND IsDeleted=0

INSERT INTO @tempCSVRecords
SELECT ReferralPayorMappingID,PayorID,PayorEffectiveDate,PayorEffectiveEndDate,Precedence,IsDeleted
FROM ReferralPayorMappings 
WHERE ReferralPayorMappingID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))
 
 
 -- update 
SELECT @idsForUpdate = COALESCE(@idsForUpdate + ',', '') + CAST(TCSV.RPMID AS VARCHAR(10))
FROM @tempCSVRecords TCSV
LEFT JOIN @tempAllRecords TA ON TA.RPMID != TCSV.RPMID AND TCSV.Precedence = TA.Precedence AND 
((TA.StartDate BETWEEN TCSV.StartDate AND TCSV.EndDate) OR (TA.EndDate BETWEEN TCSV.StartDate AND TCSV.EndDate))
WHERE TA.RPMID IS NULL 
--SELECT @idsForUpdate

-- Not Update
SELECT @idsForNotupdate = COALESCE(@idsForNotupdate + ',', '') + CAST(TCSV.RPMID AS VARCHAR(10))
FROM @tempCSVRecords TCSV
LEFT JOIN @tempAllRecords TA ON TA.RPMID != TCSV.RPMID AND TCSV.Precedence = TA.Precedence AND  
((TA.StartDate BETWEEN TCSV.StartDate AND TCSV.EndDate) OR (TA.EndDate BETWEEN TCSV.StartDate AND TCSV.EndDate))
WHERE TA.RPMID IS NOT NULL 
--SELECT @idsForNotupdate
 
                            
  UPDATE ReferralPayorMappings SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END                         
  WHERE ReferralPayorMappingID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@idsForUpdate))     
                                
 END                              
 IF(@IsShowList=1)                              
 BEGIN                              
  EXEC HC_GetReferralPayorMappingList @ReferralID,@PayorName,@Precedence,@IsDeleted,@PayorEffectiveDate,@PayorEffectiveEndDate,@SORTEXPRESSION,@SORTTYPE,@FROMINDEX,@PAGESIZE 
  
  IF(@idsForNotupdate IS NOT NULL)
	SELECT 1 AS DuplicateRecords;
  ELSE
	SELECT 0 AS DuplicateRecords;
         
 END                              
END