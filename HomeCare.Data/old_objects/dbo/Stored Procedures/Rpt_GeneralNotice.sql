-- EXEC [Rpt_GeneralNotice] 0,0,0,1,0,-1
CREATE procedure [dbo].[Rpt_GeneralNotice]                                                                  
 @RegionID bigint=0,                                                                
 @AgencyID bigint=0,            
 @PayorID bigint = 0,                                                     
 @ReferralStatusID bigint =0,   
 @ReferralID bigint =0,                                                         
 @IsDeleted bigint=-1                                                          
AS                                                                        
BEGIN                            
 DECLARE @CurrentDate Date=CAST(GETDATE() AS DATE) ;                                              
 
 --SELECT  DISTINCT  R.LastName,R.FirstName,R.MiddleName,R.ClientNickName,R.ClientID,R.AHCCCSID,R.CISNumber,dbo.GetAge(R.Dob) Age,                                                    
 --         CONVERT(VARCHAR(10),CONVERT(datetime,R.Dob ,1),111)as Birthdate,R.Population,R.Gender,  PCNT.ContactID,   
 --         PCNT.FirstName +' '+ PCNT.LastName as ParentName,PCNT.Email as ParentEmail,                
 --         PCNT.Phone1 as ParentPhone,PCNT.Address+', '+PCNT.City +', '+ PCNT.State+ '-' + PCNT.ZipCode as ParentFullAddress,                
 --         PCNT.Address as ParentAddress,PCNT.City as ParentCity,  PCNT.State as ParentStateName, PCNT.ZipCode as ParentZipCode

 SELECT  DISTINCT  PCNT.ContactID,   
          PCNT.FirstName +' '+ PCNT.LastName as ParentName,PCNT.Email as ParentEmail,                
          PCNT.Phone1 as ParentPhone,PCNT.Address+', '+PCNT.City +', '+ PCNT.State+ '-' + PCNT.ZipCode as ParentFullAddress,                
          PCNT.Address as ParentAddress,PCNT.City as ParentCity,  PCNT.State as ParentStateName, PCNT.ZipCode as ParentZipCode
                              
     from Referrals R                            
	 left join ReferralPayorMappings rp on rp.ReferralID=R.ReferralID and rp.IsActive=1 and rp.IsDeleted=0                                                 
     left join ContactMappings PCT on PCT.ReferralID=r.ReferralID AND PCT.ContactTypeID=1           
     left join Contacts PCNT on PCNT.ContactID=PCT.ContactID          
   WHERE                                                                   
  ((CAST(@IsDeleted AS BIGINT)=-1) OR r.IsDeleted=@IsDeleted)                                                                              
   AND (( CAST(@AgencyID AS BIGINT)=0) OR r.AgencyID = CAST(@AgencyID AS BIGINT))                                          
   AND (( CAST(@RegionID AS BIGINT)=0) OR r.RegionID= CAST(@RegionID AS BIGINT))                                                                        
   AND (( CAST(@PayorID AS BIGINT)=0) OR rp.PayorID = CAST(@PayorID AS BIGINT))                                                                      
   AND (( CAST(@ReferralStatusID AS BIGINT)=0) OR r.ReferralStatusID = CAST(@ReferralStatusID AS BIGINT))                                                                                
   AND (( CAST(@ReferralID AS BIGINT)=0) OR R.ReferralID = CAST(@ReferralID AS BIGINT))                                                                   
   GROUP BY PCNT.ContactID,  PCNT.FirstName,  PCNT.LastName ,PCNT.Email, PCNT.Phone1,PCNT.Address,PCNT.City, PCNT.State,PCNT.ZipCode,PCNT.City
   --ORDER BY PCNT.FirstName ASC                                  
END 
