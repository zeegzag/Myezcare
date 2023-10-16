-- EXEC SetAddFacilityHousePage @FacilityID = '1'        
CREATE PROCEDURE [dbo].[GetParentFacilityHouse]    
@FacilityID BIGINT        
AS        
BEGIN         
 SELECT FacilityID as ParentFacilityID,FacilityName,FacilityBillingName,Address,City,State,ZipCode,Phone,Cell,RegionID,County,GSA,BadCapacity,PrivateRoomCount,SiteType,  
ProviderType,Licensure,LicensureRenewalDate,FirePermitDate,NPI,AHCCCSID,EIN,FacilityColorScheme,IsDeleted   
 ,(SELECT    
     STUFF((SELECT ',' + CAST(s.PayorID AS VARCHAR)    
     FROM FacilityApprovedPayors s WHERE FacilityID = CAST(@FacilityID AS BIGINT)    
     ORDER BY s.PayorID    
     FOR XML PATH('')),1,1,'')) AS SetSelectedPayors    
 FROM Facilities WHERE  FacilityID = CAST(@FacilityID AS BIGINT)       
 END      
    
    
