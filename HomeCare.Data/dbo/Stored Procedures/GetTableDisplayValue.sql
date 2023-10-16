--  EXEC GetTableDisplayValue @TableName = 'Payors', @ValueBefore = '0', @ValueAfter = '2'
CREATE PROCEDURE [dbo].[GetTableDisplayValue]
@TableName varchar(100),
@ValueBefore varchar(100),
@ValueAfter varchar(100)
AS
BEGIN

IF(@TableName='Regions')
BEGIN
IF(CONVERT(bigint,@ValueBefore)=0)
 SELECT ValueBefore='(null)',ValueAfter=R2.RegionName from Regions R2 WHERE R2.RegionID=CONVERT(bigint,@ValueAfter)
ELSE IF(CONVERT(bigint,@ValueAfter)=0)
 SELECT ValueBefore=R2.RegionName,ValueAfter='(null)' from Regions R2 WHERE R2.RegionID=CONVERT(bigint,@ValueBefore)
ELSE
 SELECT ValueBefore=R1.RegionName,ValueAfter=R2.RegionName from Regions R1
CROSS JOIN Regions  R2 WHERE R1.RegionID=CONVERT(bigint,@ValueBefore) AND  R2.RegionID=CONVERT(bigint,@ValueAfter)
END




IF(@TableName='Languages')
BEGIN
IF(CONVERT(bigint,@ValueBefore)=0)
 SELECT ValueBefore='(null)',ValueAfter=R2.Name from Languages R2 WHERE R2.LanguageID=CONVERT(bigint,@ValueAfter)
ELSE IF(CONVERT(bigint,@ValueAfter)=0)
 SELECT ValueBefore=R2.Name,ValueAfter='(null)' from Languages R2 WHERE R2.LanguageID=CONVERT(bigint,@ValueBefore)
ELSE
 SELECT ValueBefore=R1.Name,ValueAfter=R2.Name from Languages R1
CROSS JOIN Languages R2 WHERE R1.LanguageID=CONVERT(bigint,@ValueBefore) AND  R2.LanguageID=CONVERT(bigint,@ValueAfter)
END



IF(@TableName='ReferralStatuses')
BEGIN
IF(CONVERT(bigint,@ValueBefore)=0)
 SELECT ValueBefore='(null)',ValueAfter=R2.Status from ReferralStatuses R2 WHERE R2.ReferralStatusID=CONVERT(bigint,@ValueAfter)
ELSE IF(CONVERT(bigint,@ValueAfter)=0)
 SELECT ValueBefore=R2.Status,ValueAfter='(null)' from ReferralStatuses R2 WHERE R2.ReferralStatusID=CONVERT(bigint,@ValueBefore)
ELSE
 SELECT ValueBefore=R1.Status,ValueAfter=R2.Status from ReferralStatuses R1
CROSS JOIN ReferralStatuses  R2 WHERE R1.ReferralStatusID=CONVERT(bigint,@ValueBefore) AND  R2.ReferralStatusID=CONVERT(bigint,@ValueAfter)
END


IF(@TableName='Employees')
BEGIN
IF(CONVERT(bigint,@ValueBefore)=0)
 SELECT ValueBefore='(null)',ValueAfter=R2.LastName+', '+R2.FirstName from Employees R2 WHERE R2.EmployeeID=CONVERT(bigint,@ValueAfter)
ELSE IF(CONVERT(bigint,@ValueAfter)=0)
 SELECT ValueBefore=R2.LastName+', '+R2.FirstName,ValueAfter='(null)' from Employees R2 WHERE R2.EmployeeID=CONVERT(bigint,@ValueBefore)
ELSE
	 SELECT ValueBefore=R1.LastName+', '+R1.FirstName,ValueAfter=R2.LastName+', '+R2.FirstName from Employees R1
CROSS JOIN Employees  R2 WHERE R1.EmployeeID=CONVERT(bigint,@ValueBefore) AND  R2.EmployeeID=CONVERT(bigint,@ValueAfter)
END



IF(@TableName='TransportLocations')
BEGIN
IF(CONVERT(bigint,@ValueBefore)=0)
 SELECT ValueBefore='(null)',ValueAfter=R2.Location from TransportLocations R2 WHERE R2.TransportLocationID=CONVERT(bigint,@ValueAfter)
ELSE IF(CONVERT(bigint,@ValueAfter)=0)
 SELECT ValueBefore=R2.Location,ValueAfter='(null)' from TransportLocations R2 WHERE R2.TransportLocationID=CONVERT(bigint,@ValueBefore)
ELSE
SELECT ValueBefore=R1.Location,ValueAfter=R2.Location from TransportLocations R1
CROSS JOIN TransportLocations  R2 WHERE R1.TransportLocationID=CONVERT(bigint,@ValueBefore) AND  R2.TransportLocationID=CONVERT(bigint,@ValueAfter)
END


IF(@TableName='FrequencyCodes')
BEGIN
IF(CONVERT(bigint,@ValueBefore)=0)
 SELECT ValueBefore='(null)',ValueAfter=R2.Code from FrequencyCodes R2 WHERE R2.FrequencyCodeID=CONVERT(bigint,@ValueAfter)
ELSE IF(CONVERT(bigint,@ValueAfter)=0)
 SELECT ValueBefore=R2.Code,ValueAfter='(null)' from FrequencyCodes R2 WHERE R2.FrequencyCodeID=CONVERT(bigint,@ValueBefore)
ELSE
SELECT ValueBefore=R1.Code,ValueAfter=R2.Code from FrequencyCodes R1
CROSS JOIN FrequencyCodes  R2 WHERE R1.FrequencyCodeID=CONVERT(bigint,@ValueBefore) AND  R2.FrequencyCodeID=CONVERT(bigint,@ValueAfter)
END




IF(@TableName='Agencies')
BEGIN
IF(CONVERT(bigint,@ValueBefore)=0)
 SELECT ValueBefore='(null)',ValueAfter=R2.NickName from Agencies R2 WHERE R2.AgencyID=CONVERT(bigint,@ValueAfter)
ELSE IF(CONVERT(bigint,@ValueAfter)=0)
 SELECT ValueBefore=R2.NickName,ValueAfter='(null)' from Agencies R2 WHERE R2.AgencyID=CONVERT(bigint,@ValueBefore)
ELSE
SELECT ValueBefore=R1.NickName,ValueAfter=R2.NickName from Agencies R1
CROSS JOIN Agencies  R2 WHERE R1.AgencyID=CONVERT(bigint,@ValueBefore) AND  R2.AgencyID=CONVERT(bigint,@ValueAfter)
END



IF(@TableName='CaseManagers')
BEGIN
IF(CONVERT(bigint,@ValueBefore)=0)
 SELECT ValueBefore='(null)',ValueAfter=R2.LastName+', '+R2.FirstName from CaseManagers R2 WHERE R2.CaseManagerID=CONVERT(bigint,@ValueAfter)
ELSE IF(CONVERT(bigint,@ValueAfter)=0)
 SELECT ValueBefore=R2.LastName+', '+R2.FirstName,ValueAfter='(null)' from CaseManagers R2 WHERE R2.CaseManagerID=CONVERT(bigint,@ValueBefore)
ELSE
SELECT ValueBefore=R1.LastName+', '+R1.FirstName,ValueAfter=R2.LastName+', '+R2.FirstName from CaseManagers R1
CROSS JOIN CaseManagers  R2 WHERE R1.CaseManagerID=CONVERT(bigint,@ValueBefore) AND  R2.CaseManagerID=CONVERT(bigint,@ValueAfter)
END




IF(@TableName='ReferralSources')
BEGIN
IF(CONVERT(bigint,@ValueBefore)=0)
 SELECT ValueBefore='(null)',ValueAfter=R2.ReferralSourceName from ReferralSources R2 WHERE R2.ReferralSourceID=CONVERT(bigint,@ValueAfter)
ELSE IF(CONVERT(bigint,@ValueAfter)=0)
 SELECT ValueBefore=R2.ReferralSourceName,ValueAfter='(null)' from ReferralSources R2 WHERE R2.ReferralSourceID=CONVERT(bigint,@ValueBefore)
ELSE
SELECT ValueBefore=R1.ReferralSourceName,ValueAfter=R2.ReferralSourceName from ReferralSources R1
CROSS JOIN ReferralSources  R2 WHERE R1.ReferralSourceID=CONVERT(bigint,@ValueBefore) AND  R2.ReferralSourceID=CONVERT(bigint,@ValueAfter)
END



IF(@TableName='DXCodes')
BEGIN
IF(CONVERT(bigint,@ValueBefore)=0)
 SELECT ValueBefore='(null)',ValueAfter=R2.DXCodeName from DXCodes R2 WHERE R2.DXCodeID=CONVERT(bigint,@ValueAfter)
ELSE IF(CONVERT(bigint,@ValueAfter)=0)
 SELECT ValueBefore=R2.DXCodeName,ValueAfter='(null)' from DXCodes R2 WHERE R2.DXCodeID=CONVERT(bigint,@ValueBefore)
ELSE
 SELECT ValueBefore=R1.DXCodeName,ValueAfter=R2.DXCodeName from DXCodes R1
 CROSS JOIN DXCodes  R2 WHERE R1.DXCodeID=CONVERT(bigint,@ValueBefore) AND  R2.DXCodeID=CONVERT(bigint,@ValueAfter)
END



IF(@TableName='ROITypes')
BEGIN
IF(CONVERT(bigint,@ValueBefore)=0)
 SELECT ValueBefore='(null)',ValueAfter=CASE WHEN @ValueAfter='1' THEN 'Verbal' WHEN @ValueAfter='2' THEN 'Written' ELSE '(null)' END  ;
ELSE IF(CONVERT(bigint,@ValueAfter)=0)
 SELECT ValueBefore=CASE WHEN @ValueAfter='1' THEN 'Verbal' WHEN @ValueAfter='2' THEN 'Written' ELSE '(null)' END ,ValueAfter='(null)';
ELSE
 SELECT ValueBefore=CASE WHEN @ValueAfter='1' THEN 'Verbal' WHEN @ValueAfter='2' THEN 'Written' ELSE '(null)' END ,
 ValueAfter=CASE WHEN @ValueAfter='1' THEN 'Verbal' WHEN @ValueAfter='2' THEN 'Written' ELSE '(null)' END ;
END


IF(@TableName='ContactTypes')
BEGIN
IF(CONVERT(bigint,@ValueBefore)=0)
 SELECT ValueBefore='(null)',ValueAfter=R2.ContactTypeName from ContactTypes R2 WHERE R2.ContactTypeID=CONVERT(bigint,@ValueAfter)
ELSE IF(CONVERT(bigint,@ValueAfter)=0)
 SELECT ValueBefore=R2.ContactTypeName,ValueAfter='(null)' from ContactTypes R2 WHERE R2.ContactTypeID=CONVERT(bigint,@ValueBefore)
ELSE
 SELECT ValueBefore=R1.ContactTypeName,ValueAfter=R2.ContactTypeName from ContactTypes R1
 CROSS JOIN ContactTypes  R2 WHERE R1.ContactTypeID=CONVERT(bigint,@ValueBefore) AND  R2.ContactTypeID=CONVERT(bigint,@ValueAfter)
END


IF(@TableName='Contacts')
BEGIN
  IF(CONVERT(bigint,@ValueBefore)=0)
	SELECT ValueBefore='(null)',
	ValueAfter=R2.FirstName+' '+R2.LastName +
	CASE WHEN R2.Email IS NULL THEN '' ELSE ' | '+R2.Email END +
	CASE WHEN R2.Address IS NULL THEN '' ELSE ' | '+R2.Address END +
	CASE WHEN R2.City IS NULL THEN '' ELSE ' | '+R2.City END +
	CASE WHEN R2.State IS NULL THEN '' ELSE ' | '+R2.State END +
	CASE WHEN R2.Zipcode IS NULL THEN '' ELSE ' | '+R2.Zipcode END +
	CASE WHEN R2.Phone1 IS NULL THEN '' ELSE ' | '+R2.Phone1 END +
	CASE WHEN R2.Phone2 IS NULL THEN '' ELSE ' | '+R2.Phone2 END +
	' | '+L.Name from Contacts R2 INNER JOIN Languages L ON L.LanguageID=R2.LanguageID
	WHERE R2.ContactID=CONVERT(bigint,@ValueAfter)
  
  ELSE IF(CONVERT(bigint,@ValueAfter)=0)
	SELECT ValueBefore=R2.FirstName+' '+R2.LastName +
	CASE WHEN R2.Email IS NULL THEN '' ELSE ' | '+R2.Email END +
	CASE WHEN R2.Address IS NULL THEN '' ELSE ' | '+R2.Address END +
	CASE WHEN R2.City IS NULL THEN '' ELSE ' | '+R2.City END +
	CASE WHEN R2.State IS NULL THEN '' ELSE ' | '+R2.State END +
	CASE WHEN R2.Zipcode IS NULL THEN '' ELSE ' | '+R2.Zipcode END +
	CASE WHEN R2.Phone1 IS NULL THEN '' ELSE ' | '+R2.Phone1 END +
	CASE WHEN R2.Phone2 IS NULL THEN '' ELSE ' | '+R2.Phone2 END +
	' | '+L.Name 
	,ValueAfter='(null)'
	from Contacts R2 INNER JOIN Languages L ON L.LanguageID=R2.LanguageID
	WHERE R2.ContactID=CONVERT(bigint,@ValueBefore)
  
  ELSE 
   
   SELECT ValueBefore=R1.FirstName+' '+R1.LastName +
	CASE WHEN R1.Email IS NULL THEN '' ELSE ' | '+R1.Email END +
	CASE WHEN R1.Address IS NULL THEN '' ELSE ' | '+R1.Address END +
	CASE WHEN R1.City IS NULL THEN '' ELSE ' | '+R1.City END +
	CASE WHEN R1.State IS NULL THEN '' ELSE ' | '+R1.State END +
	CASE WHEN R1.Zipcode IS NULL THEN '' ELSE ' | '+R1.Zipcode END +
	CASE WHEN R1.Phone1 IS NULL THEN '' ELSE ' | '+R1.Phone1 END +
	CASE WHEN R1.Phone2 IS NULL THEN '' ELSE ' | '+R1.Phone2 END  + ' | '+L1.Name ,

	ValueAfter=R2.FirstName+' '+R2.LastName +
	CASE WHEN R2.Email IS NULL THEN '' ELSE ' | '+R2.Email END +
	CASE WHEN R2.Address IS NULL THEN '' ELSE ' | '+R2.Address END +
	CASE WHEN R2.City IS NULL THEN '' ELSE ' | '+R2.City END +
	CASE WHEN R2.State IS NULL THEN '' ELSE ' | '+R2.State END +
	CASE WHEN R2.Zipcode IS NULL THEN '' ELSE ' | '+R2.Zipcode END +
	CASE WHEN R2.Phone1 IS NULL THEN '' ELSE ' | '+R2.Phone1 END +
	CASE WHEN R2.Phone2 IS NULL THEN '' ELSE ' | '+R2.Phone2 END + ' | '+L2.Name 
	from Contacts R1 
	CROSS JOIN Contacts R2 
	INNER JOIN Languages L1 ON L1.LanguageID=R1.LanguageID
	INNER JOIN Languages L2 ON L2.LanguageID=R2.LanguageID
	WHERE R1.ContactID=CONVERT(bigint,@ValueBefore) AND  R2.ContactID=CONVERT(bigint,@ValueAfter)

END




IF(@TableName='Payors')
BEGIN
IF(CONVERT(bigint,@ValueBefore)=0)
 SELECT ValueBefore='(null)',ValueAfter=R2.ShortName from Payors R2 
 WHERE R2.PayorID=CONVERT(bigint,@ValueAfter)
ELSE IF(CONVERT(bigint,@ValueAfter)=0)
 SELECT ValueBefore=R2.ShortName,ValueAfter='(null)' from Payors R2 
 WHERE R2.PayorID=CONVERT(bigint,@ValueBefore)
ELSE

 SELECT ValueBefore=R1.ShortName,ValueAfter=R2.ShortName from Payors R1
CROSS JOIN Payors  R2 WHERE R1.PayorID=CONVERT(bigint,@ValueBefore) AND  R2.PayorID=CONVERT(bigint,@ValueAfter)

END


END