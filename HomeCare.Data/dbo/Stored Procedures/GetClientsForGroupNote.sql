--EXEC GetClientsForGroupNote @ClientName = '', @PayorID = '0', @FacilityID = '0' ,@StartDate='2016-09-24',@EndDate='2016-09-26'        
--EXEC GetClientsForGroupNote @ClientName = 'Abraham', @PayorID = '0', @FacilityID = '0', @PageSize = '100'  
CREATE PROCEDURE [dbo].[GetClientsForGroupNote]          
@ClientName varchar(20)=null,          
@StartDate Date=null,          
@EndDate Date=null,          
@PayorID bigint = 0,          
@FacilityID bigint = 0,          
@ContactTypeID bigint,          
@IgnoreClientID varchar(200)=null,      
@PageSize int  =100        
      
AS          
BEGIN          
     DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
	 
select distinct top (@PageSize) R.ReferralID,dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) as Name,R.AHCCCSID,R.CISNumber,C.Phone1,C.Address,RPM.PayorID,P.ShortName as PayorName,          
(SELECT  STUFF((SELECT ',' + CONVERT(CHAR(20), RD.ReferralDXCodeMappingID) +'|'+ CONVERT(CHAR(20), D.DXCodeID) + '|'+D.DXCodeName+'|'+CONVERT(CHAR(20), RD.StartDate,101)+          
'|'+ISNULL(CONVERT(CHAR(20), RD.EndDate,101),'')+'|'+CONVERT(CHAR(20), RD.Precedence)+'|'+D.DxCodeType+'|'+DT.DxCodeShortName+'|'+D.DXCodeWithoutDot          
   FROM ReferralDXCodeMappings RD                
  INNER JOIN DxCodes D ON D.DXCodeID=RD.DXCodeID          
  INNER JOIN DxCodeTypes DT on DT.DxCodeTypeID=D.DxCodeType          
  --left join NoteDXCodeMappings nd on ND.ReferralDxCodeMappingID=RD.ReferralDxCodeMappingID AND ND.ReferralID=RD.ReferralID AND ND.NoteID=@NoteID          
  WHERE RD.ReferralID = R.ReferralID and RD.IsDeleted=0   AND RD.Precedence=1           
  order by case when RD.Precedence is null then 1 else 0 end,  RD.Precedence              
   FOR XML PATH('')),1,1,'')) AS StrDxCodes --,          
          
 --  (SELECT  STUFF((SELECT ',' + CONVERT(CHAR(20), F.FacilityID) +'|'+ CONVERT(CHAR(20), F.FacilityName)          
 --  from FacilityApprovedPayors FAP                
 --inner join Facilities F on F.FacilityID=FAP.FacilityID and F.IsDeleted=0 and ParentFacilityID=0          
 --where FAP.PayorID=          
 --   (select p.PayorID from Referrals RE LEFT join ReferralPayorMappings rp on rp.ReferralID=RE.ReferralID and rp.IsActive=1 and rp.IsDeleted=0                
 --    LEFT join Payors p on p.PayorID=rp.PayorID where RE.ReferralID=R.ReferralID)               
 --  FOR XML PATH('')),1,1,'')) AS StrFacilities          
  
 from Referrals R          
    left join ContactMappings CM on CM.ReferralID=R.ReferralID and ContactID=@ContactTypeID --(CM.IsDCSLegalGuardian=1 OR CM.IsPrimaryPlacementLegalGuardian=1)          
 left join Contacts C on CM.ContactID=C.ContactID          
 left join ScheduleMasters SM on SM.ReferralID=R.ReferralID and SM.IsDeleted = 0           
 left join ReferralPayorMappings RPM on RPM.ReferralID=R.ReferralID and RPM.IsActive=1          
 left join Payors P on P.PayorID=RPM.PayorID          
 WHERE            
     ((@ClientName IS NULL) OR           
     (          
  R.FirstName LIKE '%'+@ClientName+'%' OR          
  R.LastName  LIKE '%'+@ClientName+'%' OR          
  R.FirstName +' '+r.LastName like '%'+@ClientName+'%' OR          
  R.LastName +' '+r.FirstName like '%'+@ClientName+'%' OR          
  R.FirstName +', '+r.LastName like '%'+@ClientName+'%' OR          
  R.LastName +', '+r.FirstName like '%'+@ClientName+'%'          
     ))          
   AND          
      (((@StartDate IS NULL) OR (StartDate BETWEEN @StartDate and @EndDate)) OR ((@EndDate IS NULL) OR (EndDate BETWEEN  @StartDate and @EndDate))     )        
    AND R.IsDeleted=0              
    AND (( CAST(@PayorID AS BIGINT)=0) OR RPM.PayorID = CAST(@PayorID AS BIGINT))          
    AND ((@IgnoreClientID IS NULL) OR (R.ReferralID not in (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@IgnoreClientID))))          
    AND (( CAST(@FacilityID AS BIGINT)=0) OR SM.FacilityID=@FacilityID)           
  Order by Name asc  
 --select F.FacilityID Value,F.FacilityName Name from FacilityApprovedPayors FAP                
 --inner join Facilities F on F.FacilityID=FAP.FacilityID and (F.IsDeleted=0) and ParentFacilityID=0          
 --where FAP.PayorID=@PayorID          
END  