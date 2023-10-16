CREATE PROCEDURE [dbo].[DeleteTransportAssignment]                  
@FacilityID VARCHAR(100) = NULL,                               
@VehicleID VARCHAR(100) = NULL,         
@RouteCode VARCHAR(100) = NULL,        
@StartDate VARCHAR(10) = NULL,          
@EndDate VARCHAR(100) = NULL,                              
@Attendent VARCHAR(100) = NULL,                                                    
@IsDeleted BIGINT = -1,                                
@SortExpression VARCHAR(100) = '',                                  
@SortType VARCHAR(10) = 'DESC',                                
@FromIndex INT=1,                                
@PageSize INT  =10,                    
@ListOfIdsInCsv varchar(300),                    
@IsShowList bit,                    
@loggedInID BIGINT                    
AS                    
BEGIN                        
                     
 IF(LEN(@ListOfIdsInCsv)>0)                    
 BEGIN                    
                     
  UPDATE Transport SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE()     
  WHERE TransportID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))                     
                        
 END                    
                    
 IF(@IsShowList=1)                    
 BEGIN                    
  EXEC [dbo].[TransportAssignmentList] @FacilityID, @VehicleID, @RouteCode, @StartDate, @EndDate, @Attendent, @IsDeleted, @SortExpression, @SortType, @FromIndex, @PageSize                    
 END                    
END 