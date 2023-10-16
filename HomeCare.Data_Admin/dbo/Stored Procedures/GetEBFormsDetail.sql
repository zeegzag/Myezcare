CREATE PROCEDURE [dbo].[GetEBFormsDetail]    
 -- Add the parameters for the stored procedure here    
 @EBFormId nvarchar(max)    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
    -- Insert statements for procedure here    
 Select *, EBCategoryIDs = EBCategoryID from EBForms where EBFormID = @EBFormId    
    
 select * from EBMarkets    
    
 select * from EBCategories    
  
 select * from Organizations WHERE IsDeleted !=1  
END