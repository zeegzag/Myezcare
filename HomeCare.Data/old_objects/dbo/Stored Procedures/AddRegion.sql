CREATE PROCEDURE [dbo].[AddRegion]              
@RegionName VARCHAR(50)    
AS                
BEGIN    
    
 DECLARE @RegionID BIGINT;    
        
 INSERT INTO Regions (RegionName,RegionDescription,RegionCode) VALUES(@RegionName,@RegionName,@RegionName)    
 SET @RegionID=@@IDENTITY    
    
 SELECT @RegionID;    
            
END
