--CreatedBy: Abhishek Gautam  
--CreatedDate: 10 sept 2020  
--Description: Get List DMAS97AB form  
  
  
--   exeC [SetDmas97AbListPage]   60053           
CREATE PROCEDURE [dbo].[SetDmas97AbListPage]              
--@Dmas97Id bigint = NULL,
@ReferralID bigint=0                
AS                  
BEGIN                                
            
  --SELECT * from Dmas97ab WHERE IsDeleted=0          
            
  --SELECT 0;       
  SELECT d.Dmas97ID, EmployeeName = dbo.GetGeneralNameFormat(e.FirstName,e.LastName), d.JsonData, CreatedDate=convert(date,d.CreatedDate)        
  FROM  DMAS97AB d        
  INNER JOIN Employees e on e.EmployeeID=d.CreatedBy        
  WHERE d.IsDeleted=0  AND d.ReferralID=@ReferralID       
                 
select 0 return;         
                     
END