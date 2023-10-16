CREATE PROCEDURE [dbo].[DELETETRANSPORTLOCATION]    
@TransportLocationId bigint=0,          
@Location varchar(255)=null,    
@LocationCode varchar(255)=null,    
@State nvarchar(100)=null,          
@Address nvarchar(255)=null,          
@Phone nvarchar(100)=null,
@IsDeleted BIGINT = -1,         
@RegionID bigint=0,
@SortExpression nvarchar(100),    
@SortType nvarchar(10),          
@FromIndex int,          
@PageSize int,    
@ListOfIdsInCSV  varchar(300),      
@IsShowList bit,
@loggedInID BIGINT     
AS      
BEGIN          
      
 IF(LEN(@ListOfIdsInCSV)>0)      
 BEGIN    
 --IF EXISTS (SELECT * FROM ScheduleMasters WHERE DropOffLocation IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@LISTOFIDSINCSV)) OR PickUpLocation IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@LISTOFIDSINCSV)))
	--OR EXISTS (SELECT * FROM Referrals WHERE DropOffLocation IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@LISTOFIDSINCSV)) OR PickUpLocation IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@LISTOFIDSINCSV)))
 -- BEGIN       
 --  SELECT NULL;      
 --  RETURN NULL;      
 -- END      
 -- ELSE      
 -- BEGIN      
   UPDATE TransportLocations SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE()
   WHERE TransportLocationID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))           
  --END      
 END      
 IF(@IsShowList=1)      
 BEGIN      
  EXEC GetTransportaLocationList @TransportLocationId ,@Location ,@LocationCode,@State,@Address,@Phone,@IsDeleted,@RegionID,@SortExpression,@SortType ,@FromIndex,@PageSize    
 END      
END      
    
