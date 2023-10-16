-- EXEC Rpt_GetRequestClientList
CREATE procedure [dbo].[Rpt_GetRequestClientList]   
 @RegionID bigint=0,                                                                                                                           
 @ReferralStatusID bigint =0,                                                                                                                           
 @ClientName varchar(100) = NULL,                                                                                                                           
 @IsDeleted bigint=-1                                                           
AS                                                                      
BEGIN                          
 DECLARE @CurrentDate Date=CAST(GETDATE() AS DATE) ;                                            
 SELECT   R.ReferralID, R.LastName+', '+R.FirstName as ClientName, 
 
 (SELECT  STUFF((SELECT ',' + FC.DXCodeName                                                                 
           FROM ReferralDXCodeMappings F join DXCodes Fc on F.DXCodeID=FC.DXCodeID                                                                
           WHERE F.ReferralID=r.ReferralID                                                                
           FOR XML PATH('')),1,1,'')) AS DXCode,  

 R.AHCCCSID, R.CISNumber, P.ShortName as Payor,A.NickName as Agency,
 CM.FirstName+', '+ CM.LastName as CaseManager, convert(varchar(10), R.Dob, 101) AS Birthdate, 
 PCNT.Address+', '+PCNT.City +', '+ PCNT.State+ '-' + PCNT.ZipCode as PlacementFullAddress
 

     from Referrals R   
	 left join ReferralPayorMappings RP on RP.ReferralID=R.ReferralID and RP.IsActive=1 and RP.IsDeleted=0                                          
     left join Payors P on P.PayorID=RP.PayorID                                                                                                        
     left join CaseManagers CM on CM.CaseManagerID=R.CaseManagerID                                                                      
     left join Agencies A on A.AgencyID=R.AgencyID                                                                      
     left join ContactMappings PCT on PCT.ReferralID=r.ReferralID AND PCT.ContactTypeID=1         
     left join Contacts PCNT on PCNT.ContactID=PCT.ContactID        
   WHERE      --  R.referralID=2841 and                                                        
  ((CAST(@IsDeleted AS BIGINT)=-1) OR R.IsDeleted=@IsDeleted)                                                                            
   AND (( CAST(@RegionID AS BIGINT)=0) OR R.RegionID= CAST(@RegionID AS BIGINT))                                                                      
   AND (( CAST(@ReferralStatusID AS BIGINT)=0) OR R.ReferralStatusID = CAST(@ReferralStatusID AS BIGINT))           
   AND ((@ClientName IS NULL OR LEN(r.LastName)=0) 
    OR
  ( (r.FirstName LIKE '%'+@ClientName+'%' )OR  
    (r.LastName  LIKE '%'+@ClientName+'%') OR  
    (r.FirstName +' '+r.LastName like '%'+@ClientName+'%') OR  
    (r.LastName +' '+r.FirstName like '%'+@ClientName+'%') OR  
    (r.FirstName +', '+r.LastName like '%'+@ClientName+'%') OR  
    (r.LastName +', '+r.FirstName like '%'+@ClientName+'%'))
  )   
   ORDER BY R.LastName ASC     
   
   
   
   SELECT FacilityBillingName, AHCCCSID, NPI FROM Facilities WHERE FacilityName Like '%Outpatient Main%'
                              
END