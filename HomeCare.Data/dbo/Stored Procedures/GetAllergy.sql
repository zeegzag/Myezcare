
-- exec GetAllergy 60053,'',''

 CREATE procedure [dbo].[GetAllergy]  
  (  
  @patient bigint,  
  @allergy nvarchar(max)=null,  
  @status bit  
  )  
  AS  
  BEGIN  
  declare @cnt int;  
  set @cnt=0;  
  
   if len(@allergy)<2  
   BEGIN  
    select a.id,a.Allergy,(case when a.Status=1 then 'Observed' else 'Historical' end)as[StatusName],
	a.Reaction,convert(varchar,a.CreatedOn,101)as[CreatedOn], a.Comment ,dd.DDMasterID as ReportedBy,dd.Title as 'ReportedByName',a.Status as Status  
      from Allergy a   
      inner join DDMaster dd on dd.DDMasterID=a.ReportedBy inner join lu_DDMasterTypes lu on dd.ItemType=lu.DDMasterTypeID  
       where a.Patient=@patient and a.IsDeleted=0 and lu.Name='Reported By'  
    
   END  
  
    if len(@allergy)>2  
   BEGIN  
    select a.id,a.Allergy,(case when a.Status=1 then 'Observed' else 'Historical' end)as[StatusName],
	a.Reaction,convert(varchar,a.CreatedOn,101)as[CreatedOn],a.Comment ,dd.DDMasterID as ReportedBy,dd.Title as 'ReportedByName',a.Status as Status 
        from Allergy a   
        inner join DDMaster dd on dd.DDMasterID=a.ReportedBy inner join lu_DDMasterTypes lu on dd.ItemType=lu.DDMasterTypeID  
        where a.Patient=@patient and a.IsDeleted=0  
     and a.allergy like '%'+@allergy+'%' and lu.Name='Reported By'  
   end  
  END