Declare @max int;
Declare @sort int;
select @max = MAX( DDMasterTypeID ),
	   @sort = MAX(SortOrder)
	   FROM lu_DDMasterTypes;

INSERT INTO lu_DDMasterTypes (DDMasterTypeID, Name, SortOrder, IsDisplayValue, ParentID)
								VALUES(@max+1, 'Relation Type', @sort+1, 0, 0)

