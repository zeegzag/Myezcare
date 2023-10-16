CREATE  procedure [dbo].[GetCommonList]
(
@Name varchar(max)
)
as
begin
select d.Title as Title,d.DDMasterID AS Value
		from DDMaster d
	inner join 	lu_DDMasterTypes ld on ld.DDMasterTypeID=d.ItemType
		where d.ItemType=ld.DDMasterTypeID 
		      and ld.Name=@Name
			  and d.isdeleted=0
			   order by d.Title
			  
			  
end