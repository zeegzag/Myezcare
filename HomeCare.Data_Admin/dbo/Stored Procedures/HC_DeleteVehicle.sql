CREATE PROCEDURE [dbo].[HC_DeleteVehicle]              
@VIN_Number VARCHAR(100) = NULL,                       
@TransportService VARCHAR(100) = NULL,  
@ContactID VARCHAR(10) = NULL,  
@Attendent VARCHAR(100) = NULL,      
@Model VARCHAR(100) = NULL,                      
@BrandName VARCHAR(100) = NULL,                       
@Color VARCHAR(100) = NULL,             
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
                 
  UPDATE Vehicles SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE() WHERE VehicleID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))                 
                    
 END                
                
 IF(@IsShowList=1)                
 BEGIN                
  EXEC HC_GetVehicleList @VIN_Number, @TransportService, @ContactID, @Attendent, @Model, @BrandName, @Color, @IsDeleted, @SortExpression, @SortType, @FromIndex, @PageSize                
 END                
END 