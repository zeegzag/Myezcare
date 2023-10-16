--updatedBy                     UpdatedDate                   Description  
--Akhilesh                      02-jul-2019               change from EmployeeID to EmployeesID in where condition   
-- exec [GetCareType] '4296',''  
CREATE PROCEDURE [dbo].[GetCareType] 
      
AS       
BEGIN      

select * from DDMaster where ItemType=1 and IsDeleted=0 
      
END
