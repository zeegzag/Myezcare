-- EXEC GetServiceCodes @ReferralID = '87', @ServiceCodeTypeID = '1', @ServiceDate = '2016-08-23', @SearchText = '', @PageSize = '10'    
CREATE PROCEDURE [dbo].[GetNotePageReferrals]    
@SearchText varchar(20)=null,    
@PageSize int    
AS    
BEGIN    
   DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
select Top(@PageSize) R.ReferralID,dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) as Name,R.AHCCCSID,R.CISNumber,C.Phone1,C.Address from Referrals R    
 left join ContactMappings CM on CM.ReferralID=R.ReferralID and (CM.IsDCSLegalGuardian=1 OR CM.IsPrimaryPlacementLegalGuardian=1)    
 left join Contacts C on CM.ContactID=C.ContactID    
 WHERE    R.IsDeleted=0 AND  
     (@SearchText IS NULL) OR (     
     (    
    R.FirstName LIKE '%'+@SearchText+'%' OR    
    R.LastName  LIKE '%'+@SearchText+'%' OR    
    R.FirstName +' '+r.LastName like '%'+@SearchText+'%' OR    
    R.LastName +' '+r.FirstName like '%'+@SearchText+'%' OR    
    R.FirstName +', '+r.LastName like '%'+@SearchText+'%' OR    
    R.LastName +', '+r.FirstName like '%'+@SearchText+'%'    
   )    
   OR    
   (R.AHCCCSID LIKE '%' + @SearchText+ '%') OR (R.CISNumber LIKE '%' + @SearchText+ '%') )    
      
 END    
    
--select IsSaveAsDraft, *  from Referrals  
  
--select IsDCSLegalGuardian,IsPrimaryPlacementLegalGuardian,* from ContactMappings  
  