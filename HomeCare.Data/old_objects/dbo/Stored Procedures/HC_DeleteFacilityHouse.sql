CREATE PROCEDURE [dbo].[HC_DeleteFacilityHouse]  
@FacilityName VARCHAR(100)=NULL,  
@County VARCHAR(100)=NULL,  
@RegionID BIGINT=0,  
@Phone varchar(15)=NULL,  
@NPI VARCHAR(10)=NULL,  
@AHCCCSID varchar(10)=NULL,  
@EIN varchar(9)=NULL,  
@PayorApproved VARCHAR(15)=NULL,  
@AgencyID BIGINT = 0,
@IsDeleted BIGINT=-1,  
@SortExpression VARCHAR(100)=NULL,    
@SortType VARCHAR(10)=NULL,  
@FromIndex INT,  
@PageSize INT,  
@ListOfIdsInCsv varchar(300),  
@IsShowList bit,  
@loggedInID BIGINT  
AS  
BEGIN      
   
 IF(LEN(@ListOfIdsInCsv)>0)  
 BEGIN  
  --IF EXISTS (SELECT * FROM ScheduleMasters WHERE FacilityID IN (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV)))  
  --BEGIN   
  -- SELECT NULL;  
  -- RETURN NULL;  
  --END  
  --ELSE  
  --BEGIN   
  UPDATE Facilities SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE() WHERE FacilityID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))   
  --END      
 END  
  
 IF(@IsShowList=1)  
 BEGIN  
  EXEC HC_GetFacilityHouseList @FacilityName,@County,@RegionID,@Phone,@NPI,@AHCCCSID,@EIN,@PayorApproved,@AgencyID,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize  
 END  
END
