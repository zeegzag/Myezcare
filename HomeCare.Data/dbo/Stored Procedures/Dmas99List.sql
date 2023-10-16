--CreatedBy: Abhishek Gautam  
--CreatedDate: 10 sept 2020  
--Description: Get List DMAS99 form  
  
  
--   exeC [Dmas99List]                
CREATE PROCEDURE [dbo].[Dmas99List]                
@ReferralID bigint = 0                  
AS                    
BEGIN                                
              
  --SELECT * from DMAS99 WHERE IsDeleted=0            
    SELECT d.Dmas99ID, EmployeeName = dbo.GetGeneralNameFormat(e.FirstName,e.LastName), d.JsonData, d.CreatedDate    
  FROM  DMAS99 d    
  INNER JOIN Employees e on e.EmployeeID=d.CreatedBy    
  WHERE d.IsDeleted=0 AND d.ReferralID=@ReferralID   
             
select 0 return;  
                       
END