CREATE PROCEDURE [dbo].[Rpt_GetNotePageReferrals]
@SearchText varchar(20)=null,  
@PageSize int  
AS  
BEGIN  
  
select Top(@PageSize) R.ReferralID,R.LastName+', '+ R.FirstName as Name,R.AHCCCSID,R.CISNumber,C.Phone1,C.Address from Referrals R  
    inner join ContactMappings CM on CM.ReferralID=R.ReferralID and (CM.IsDCSLegalGuardian=1 OR CM.IsPrimaryPlacementLegalGuardian=1)  
 inner join Contacts C on CM.ContactID=C.ContactID  
 WHERE    
     (@SearchText IS NULL) OR (   
     (R.FirstName LIKE '%'+@SearchText+'%' OR  
      R.LastName  LIKE '%'+@SearchText+'%' OR  
      R.FirstName +' '+r.LastName like '%'+@SearchText+'%' OR  
      R.LastName +' '+r.FirstName like '%'+@SearchText+'%' OR  
      R.FirstName +', '+r.LastName like '%'+@SearchText+'%' OR  
      R.LastName +', '+r.FirstName like '%'+@SearchText+'%') 
      OR  
     (R.AHCCCSID LIKE '%' + @SearchText+ '%') OR (R.CISNumber LIKE '%' + @SearchText+ '%'))
END