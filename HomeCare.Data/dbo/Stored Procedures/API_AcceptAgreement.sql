-- =============================================  
-- Author:  Sameer M.  
-- Create date: 2021-07-08  
-- Description: Accept mobile agreement  
-- =============================================  
CREATE PROCEDURE [dbo].[API_AcceptAgreement]  
 -- Add the parameters for the stored procedure here  
 @EmployeeID   bigint,  
 @Latitude   float,  
 @Longitude   float,  
 @SystemID   varchar(100)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
      
  Update dbo.Employees Set  
   IsTermsConditionMobileAccepted = 1,  
   UpdatedBy = @EmployeeID,  
   UpdatedDate = getdate(),  
   Latitude = @Latitude,  
   Longitude = @Longitude,  
   SystemID = @SystemID  
  
  WHERE EmployeeID = @EmployeeID       
  
  
  Select TermsConditionMobile = IsNull(TermsConditionMobile, '')  from OrganizationSettings  
   
END