
--add region option 
 begin
  DECLARE 
  @maxid int,
  @maxSortOrder int,
  @MAXDDMastrID int

 set @maxid = (SELECT MAX(DDMasterTypeID) FROM lu_DDMasterTypes) + 1;
 set @maxSortOrder = (SELECT MAX(SortOrder) FROM lu_DDMasterTypes) + 1 ; 
 set @MAXDDMastrID =  (SELECT MAX(DDMasterID) FROM DDMaster) + 1 ; 

 insert into lu_DDMasterTypes values(@maxid,'Region' , @maxSortOrder , 1 , 0);


 insert into DDMaster (ItemType , Title , Value,ParentID,SortOrder,Remark , IsDeleted , CreatedDate , CreatedBy , UpdatedDate , UpdatedBy,SystemID)
 select  @maxid ,r.RegionName , r.RegionCode , 0 ,null ,null,0,GETDATE() ,1 , GETDATE() , 1 ,'205220234.249'  from Regions r

 
end

--end add region option


