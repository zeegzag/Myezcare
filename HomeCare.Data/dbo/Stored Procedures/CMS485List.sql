--CreatedBy: Abhishek Gautam  
--CreatedDate: 25 sept 2020  
--Description: Get List CMS485 form  
  
--   Exec CMS485List                  
CREATE PROCEDURE [dbo].[CMS485List]                  
@ReferralID bigint = 0                    
AS                      
BEGIN                                    
                      
  SELECT d.Cms485ID, EmployeeName = dbo.GetGeneralNameFormat(e.FirstName,e.LastName), d.JsonData, CreatedDate=convert(date,d.CreatedDate)            
  FROM  CMS485 d            
  INNER JOIN Employees e on e.EmployeeID=d.CreatedBy            
  WHERE d.IsDeleted=0 AND d.ReferralID=@ReferralID           
                     
select 0 return;             
                         
END