CREATE Procedure [dbo].[DMTGetReferralData]        
@AHCCCSID varchar(100)        
--,@FirstName varchar(100),        
--@LastName varchar(100),        
--@Dob datetime        
as                       
select R.ReferralID,R.AHCCCSID,R.FirstName,R.LastName,ISNULL(RDU.UploadStatus,0)as UploadStatus,R.Dob from        
Referrals R      
left JOIN ReferralDocumentUploadStatuses RDU On RDU.AHCCCSID=R.AHCCCSID      
where      
R.AHCCCSID=@AHCCCSID    
 --AND R.FirstName=@FirstName AND R.LastName=@LastName AND R.Dob=@Dob      
      
 --exec GetReferralData 'A12315646','NIsarg','Shah','2015-12-28'