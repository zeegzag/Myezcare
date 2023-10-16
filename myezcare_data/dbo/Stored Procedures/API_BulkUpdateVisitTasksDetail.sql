Create PROCEDURE [dbo].[API_BulkUpdateVisitTasksDetail]
 @BulkType nvarchar(max),            
 @VisitTaskIDList NVARCHAR(max),        
 @loggedInID BIGINT,
 @VisitTaskValue NVARCHAR(max)
 
AS                        
BEGIN            
	declare @setBit bit=null;
	declare @id bigint=null

  if(@BulkType IS NOT NULL and @BulkType!='')
	Begin 
		if(@BulkType ='VisitType')
			begin
			set @id=cast(@VisitTaskValue as bigint)
				UPDATE VisitTasks set  VisitType=@id,UpdatedDate=GETUTCDATE(),UpdatedBy=@loggedInID where VisitTaskID in (select val from dbo.f_split(@VisitTaskIDList, ','))
			END

		else if(@BulkType ='VisitTaskType')
		
			UPDATE VisitTasks set  VisitTaskType=@VisitTaskValue ,UpdatedDate=GETUTCDATE(),UpdatedBy=@loggedInID where VisitTaskID in (select val from dbo.f_split(@VisitTaskIDList, ','))
	
		
		else if(@BulkType ='CareType')
			UPDATE VisitTasks set  CareType=CASt(@VisitTaskValue as bigint) ,UpdatedDate=GETUTCDATE(),UpdatedBy=@loggedInID where VisitTaskID in (select val from dbo.f_split(@VisitTaskIDList, ','))
		
		else if(@BulkType ='Category')
			UPDATE VisitTasks set  VisitTaskCategoryID=cast(@VisitTaskValue as bigint) ,UpdatedDate=GETUTCDATE(),UpdatedBy=@loggedInID where VisitTaskID in (select val from dbo.f_split(@VisitTaskIDList, ','))
		
		else if(@BulkType ='IsDefault')
			begin
				if(@VisitTaskValue='Yes')
					set @setBit=1
				else 
					set @setBit=0
				UPDATE VisitTasks set  IsDefault=@setBit ,UpdatedDate=GETUTCDATE(),UpdatedBy=@loggedInID where VisitTaskID in (select val from dbo.f_split(@VisitTaskIDList, ','))
			end
		
		else if(@BulkType ='IsRequired')
			begin
				if(@VisitTaskValue='Yes')
					set @setBit=1
				else 
					set @setBit=0
				UPDATE VisitTasks set  IsRequired=@setBit ,UpdatedDate=GETUTCDATE(),UpdatedBy=@loggedInID where VisitTaskID in (select val from dbo.f_split(@VisitTaskIDList, ','))
			end
	End
              
  SELECT 1; RETURN;            
END 
