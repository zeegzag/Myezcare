CREATE PROCEDURE [dbo].[HC_GetFacilityHouseList]  
@FacilityName VARCHAR(100)=NULL,  
@County VARCHAR(100)=NULL,  
@RegionID BIGINT=0,  
@Phone varchar(15)=NULL,  
@NPI VARCHAR(10)=NULL,  
@AHCCCSID varchar(10)=NULL,  
@EIN varchar(9)=NULL,  
@PayorApproved VARCHAR(15)=NULL,  
@AgencyID BIGINT = 0,
@IsDeleted BIGINT = -1,  
@SortExpression VARCHAR(100),    
@SortType VARCHAR(10),  
@FromIndex INT,  
@PageSize INT   
AS  
BEGIN  
  
;WITH CTEEmployeeList AS  
 (   
  SELECT *,COUNT(t1.FacilityID) OVER() AS Count FROM   
  (  
   SELECT ROW_NUMBER() OVER (ORDER BY   
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FacilityID' THEN fac.FacilityID END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FacilityID' THEN fac.FacilityID END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'GSA' THEN GSA END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'GSA' THEN GSA END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Address' THEN fac.Address END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Address' THEN fac.Address END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'RegionName' THEN RegionName END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'RegionName' THEN RegionName END END DESC,  
      
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'LicenseRenewal' THEN CONVERT(datetime, LicensureRenewalDate, 103) END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'LicenseRenewal' THEN CONVERT(datetime, LicensureRenewalDate, 103) END END DESC,  
      
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FirePermit' THEN CONVERT(varchar(10), FirePermitDate, 105) END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FirePermit' THEN CONVERT(varchar(10), FirePermitDate, 105) END END DESC,  
      
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FacilityName' THEN FacilityName END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FacilityName' THEN FacilityName END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'County' THEN County END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'County' THEN County END END DESC,  
      
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'GSA' THEN GSA END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'GSA' THEN GSA END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'BadCapacity' THEN BadCapacity END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'BadCapacity' THEN BadCapacity END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'SiteType' THEN SiteType END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'SiteType' THEN SiteType END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ProviderType' THEN ProviderType END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ProviderType' THEN ProviderType END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Licensure' THEN Licensure  END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Licensure' THEN Licensure  END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Phone' THEN fac.Phone END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Phone' THEN fac.Phone END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'NPI' THEN NPI END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'NPI' THEN NPI END END DESC,  
  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN AHCCCSID END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN AHCCCSID END END DESC,  
        
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PrivateRoomCount' THEN PrivateRoomCount END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PrivateRoomCount' THEN PrivateRoomCount END END DESC        
  ) AS Row,  
    fac.FacilityID, fac.FacilityName, fac.Address, fac.City, fac.State, fac.ZipCode, fac.Phone, fac.Cell, r.RegionName, fac.County, fac.GSA, fac.BadCapacity,   
     fac.PrivateRoomCount,fac.SiteType,fac.ProviderType,fac.Licensure,fac.LicensureRenewalDate,fac.FirePermitDate,fac.NPI,fac.AHCCCSID,fac.EIN,fac.IsDeleted  
     ,(SELECT   
     STUFF((SELECT ', ' + p.PayorName  
     FROM FacilityApprovedPayors s JOIN Payors p on p.PayorID=s.PayorID  
     WHERE FacilityID = fac.FacilityID  
     ORDER BY s.PayorID  
     FOR XML PATH('')),1,1,'')) AS PayorApproved  
   FROM Facilities fac      
    LEFT JOIN Regions r on fac.regionid=r.RegionID  
    LEFT JOIN Agencies A on fac.AgencyID=A.AgencyID
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR fac.IsDeleted=@IsDeleted)    
   AND (( CAST(@AgencyID AS BIGINT)=0 AND A.AgencyID IS NULL) OR A.AgencyID = CAST(@AgencyID AS BIGINT))   
   AND ((@FacilityName IS NULL OR LEN(@FacilityName)=0) OR fac.FacilityName LIKE '%' + @FacilityName + '%')  
   AND ((@County IS NULL OR LEN(@County)=0) OR fac.County LIKE '%' + @County + '%')  
   AND (( CAST(@RegionID AS BIGINT)=0) OR r.RegionID = CAST(@RegionID AS BIGINT))   
   AND (( CAST(@EIN AS BIGINT)=0) OR fac.EIN LIKE '%' +  @EIN +'%')   
   AND (( CAST(@Phone AS BIGINT)=0) OR fac.Phone LIKE '%' + @Phone +'%')   
   AND ((@AHCCCSID IS NULL OR LEN(@AHCCCSID)=0) OR fac.AHCCCSID LIKE '%' + @AHCCCSID + '%')  
   AND ((@NPI IS NULL OR LEN(@NPI)=0) OR fac.NPI LIKE '%' + @NPI + '%')     
  ) AS t1 WHERE ((@PayorApproved IS NULL OR LEN(@PayorApproved)=0) OR PayorApproved LIKE '%' + @PayorApproved + '%')  
 )   
 SELECT * FROM CTEEmployeeList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)   
END
