-- execute [GetReferralTimeSlotsPageModel]  1,10013
  
CREATE PROCEDURE [dbo].[GetReferralTimeSlotsPageModel]        
 @DDType_CareType INT = 1,     
 @ReferralID bigint  =NULL     
AS        
BEGIN    
 select EmployeeID,LastName+', '+FirstName as EmployeeName,IsDeleted from Employees where IsDeleted=0 order by LastName ASC
 SELECT Value=ReferralID, Name=LastName+', ' +FirstName  FROM Referrals WHERE IsDeleted=0 ORDER BY LastName ASC  
   
   
     IF(@ReferralID >0)    
  BEGIN    
      
     declare @caretyis varchar(500)      
 set @caretyis = (select CareTypeIds FROM Referrals where [ReferralID] = @ReferralID)      
 select distinct Name=Title,Value=DDMasterID from DDMaster WHERE ',' + @caretyis + ',' LIKE '%,' + CAST(DDMasterID as VARCHAR(500)) + ',%';    
    
      
  END    
  ELSE    
  BEGIN    
      SELECT Name=Title,Value=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_CareType    
     END    
END 
GO  