--  exec [GetReferralEmail] '27,35'  
CREATE PROCEDURE [dbo].[GetReferralEmail]  
@ReferralIDs varchar(max)=null   
        
AS          
BEGIN              
        SELECT       
    CM.ReferralID ,C.Email  
           
  FROM Contacts C        
  INNER JOIN ContactMappings CM        
    ON CM.ContactID = C.ContactID        
  INNER JOIN ContactTypes CT        
    ON CT.ContactTypeID = CM.ContactTypeID        
    AND CT.IsDeleted = 0            
  WHERE CM.REFERRALID in (select CONVERT(BIGINT,VAL)   from GetCSVTable(@ReferralIDs))   
END          

