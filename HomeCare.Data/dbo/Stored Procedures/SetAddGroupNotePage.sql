    
CREATE PROCEDURE [dbo].[SetAddGroupNotePage]                
@LoggedInUserID bigint                
AS                
BEGIN                
     DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()     
               
 ----Get dx codes                
 --SELECT RD.ReferralDXCodeMappingID,D.DXCodeID,D.DXCodeName,D.Description,ND.NoteDXCodeMappingID,RD.StartDate,RD.EndDate,RD.Precedence          
 --FROM ReferralDXCodeMappings RD                
 --INNER JOIN DxCodes D ON D.DXCodeID=RD.DXCodeID               
 --left join NoteDXCodeMappings nd on ND.ReferralDXCodeMappingID=RD.ReferralDXCodeMappingID and ND.ReferralID=RD.ReferralID           
 --WHERE RD.ReferralID = @ReferralID and RD.IsDeleted=0              
 --order by case when RD.Precedence is null then 1 else 0 end,  RD.Precedence           
                
 select * from ServiceCodeTypes                
                
 SELECT Signature=es.SignaturePath,SignatureName=dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat),SignatureLogID=0,EmployeeID =e.EmployeeID          
 FROM Employees e            
 INNER JOIN EmployeeSignatures es ON es.EmployeeSignatureID=e.EmployeeSignatureID            
 WHERE e.EmployeeID=@LoggedInUserID                 
           
           
           
           
           
             
           
               
              
 select F.FacilityID Value,F.FacilityName Name from Facilities F where (F.IsDeleted=0 AND ParentFacilityID=0) ORDER BY FacilityName ASC          
          
 select F.FacilityID Value,F.FacilityName Name from Facilities F ORDER BY FacilityName ASC          
      
 select F.TransportLocationID Value,F.Location Name from TransportLocations F ORDER BY Location ASC          
 --AND FAP.PayorID=(select p.PayorID from Referrals R LEFT join ReferralPayorMappings rp on rp.ReferralID=r.ReferralID and rp.IsActive=1 and rp.IsDeleted=0                
 --LEFT join Payors p on p.PayorID=rp.PayorID where r.ReferralID=@ReferralID)                
          
          
          
           
             
 select * from ServiceCodes where ServiceCodeID=0             
          
 select PayorID as Value,ShortName As Name from Payors where IsDeleted=0 order by ShortName ASC          
             
 select EmployeeID Value,dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat) Name from Employees where IsActive=1 and IsDeleted=0 Order By LastName ASC               
          
 select * from PlaceOfServices Where IsDeleted=0          
          
           
SELECT * FROM NoteSentences WHERE IsDeleted=0 ORDER BY NoteSentenceTitle ASC          
        
        
        
        
SELECT Value=RegionID,Name=RegionName FROM Regions              
SELECT Value=ReferralStatusID,Name=Status FROM ReferralStatuses              
        
        
END