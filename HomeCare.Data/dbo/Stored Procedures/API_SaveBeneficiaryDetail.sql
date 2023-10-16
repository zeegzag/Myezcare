CREATE PROCEDURE [dbo].[API_SaveBeneficiaryDetail]  
 @EmployeeVisitID BIGINT,          
 @EmployeeID BIGINT,          
 @PlaceOfService VARCHAR(200),        
 @HHA_PCA_NP VARCHAR(200)         
AS                    
BEGIN
  Update EmployeeVisits   
  SET PlaceOfService=@PlaceOfService,HHA_PCA_NP=@HHA_PCA_NP,  
  UpdatedDate=GETUTCDATE(),  
  UpdatedBy=@EmployeeID  
  WHERE EmployeeVisitID=@EmployeeVisitID  
END