 CREATE procedure [dbo].[GetAllergyDDL]  
  (  
 @param nvarchar(max)  
  )  
  as  
  begin     
        select d.Title as Title,d.DDMasterID AS Value  
  from DDMaster d  
 inner join  lu_DDMasterTypes ld on ld.DDMasterTypeID=d.ItemType  
  where d.ItemType=ld.DDMasterTypeID   
        and ld.Name='Reported By' order by d.Title  
  end