-- EXEC GetServiceCodes @ReferralID = '87', @ServiceCodeTypeID = '1', @ServiceDate = '2016-08-23', @SearchText = '', @PageSize = '10'          
CREATE PROCEDURE [dbo].[GetSiblingPageReferrals]          
@SearchText varchar(max)=null,          
@IgnoreIds varchar(max),  
@PageSize int          
AS          
BEGIN          
          
select Top(@PageSize) R.ReferralID,R.LastName+', '+ R.FirstName as Name,R.AHCCCSID,R.CISNumber,C.Phone1,C.Address,C.Email,C.LastName+' '+C.FirstName as ParentName     
,r.Gender, dbo.GetAge(R.Dob) as Age  ,rs.Status    
from Referrals R          
 left join ContactMappings CM on CM.ReferralID=R.ReferralID and (CM.IsDCSLegalGuardian=1 OR CM.IsPrimaryPlacementLegalGuardian=1)          
 left join ReferralStatuses RS on rs.ReferralStatusID=r.ReferralStatusID       
 left join Contacts C on CM.ContactID=C.ContactID  
 WHERE            
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
   (R.AHCCCSID LIKE '%' + @SearchText+ '%') OR (R.CISNumber LIKE '%' + @SearchText+ '%'))  
 AND      
   R.ReferralID NOT IN (  
   SELECT val  
   FROM [dbo].[GetCSVTable](@IgnoreIds))  
  AND R.IsDeleted=0 
END
