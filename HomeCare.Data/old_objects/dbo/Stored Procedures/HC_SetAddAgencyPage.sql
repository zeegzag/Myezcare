-- alter region add agancy to get regions from ddmaster table 
CREATE PROCEDURE [dbo].[HC_SetAddAgencyPage]    
@AgencyID BIGINT      
AS      
BEGIN   
-- Added region list
 select DDMasterID as RegionID,Title as RegionName 
 from DDMaster dm
 inner join lu_DDMasterTypes lu on lu.DDMasterTypeID=dm.ItemType
  where lu.Name='Region'    

   select *  from States;    
  select * from  Agencies WHERE  AgencyID= @AgencyID ;      
  SELECT [TaxonomyID]    
      ,[AgencyID]    
      ,[Code]    
      ,[Description]    
      ,[IsPrimary]    
      ,[State]    
      ,[License]    
      ,[TaxonomyGroup]    
      ,[IsDeleted]    
      ,[CreatedDate]    
      ,[CreatedBy]    
      ,[UpdatedDate]    
      ,[UpdatedBy]    
  FROM [dbo].[AgencyTaxonomies] WHERE [AgencyID]=@AgencyID    
 END 