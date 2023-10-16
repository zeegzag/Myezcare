    
CREATE PROCEDURE [dbo].[GetGroupTaskOptionList]    
 @VisitTaskType NVARCHAR(30),   
 -- @VisitTaskIDs NVARCHAR(max)  
 @CareType BIGINT=0      
AS
begin    
SET NOCOUNT ON      
Declare @TaskOption nvarchar(max),    
@DefaultTaskOption nvarchar(max),    
@VisitTaskID bigint,    
@VisitTaskDetail nvarchar(max);    
declare @temp table(    
VisitTaskID bigint,    
VisitTaskDetail nvarchar(max),    
TaskOption nvarchar(max),    
DefaultTaskOption nvarchar(max)    
)    
DECLARE EMP_CURSOR CURSOR      
 STATIC  FOR      
    
   SELECT v.VisitTaskID,v.VisitTaskDetail,v.TaskOption,v.DefaultTaskOption        
  FROM VisitTasks v        
  WHERE v.VisitTaskType = @VisitTaskType    AND v.IsDeleted = 0 AND v.caretype = @CareType     
    
OPEN EMP_CURSOR      
FETCH NEXT FROM EMP_CURSOR INTO  @VisitTaskID ,@VisitTaskDetail,@TaskOption,@DefaultTaskOption     
WHILE @@FETCH_STATUS = 0      
BEGIN      
 insert into @temp  select @VisitTaskID,@VisitTaskDetail, val ,@DefaultTaskOption from GetCSVTable(@TaskOption)    
    
FETCH  FROM EMP_CURSOR INTO  @VisitTaskID ,@VisitTaskDetail,@TaskOption,@DefaultTaskOption    
    
    
END      
CLOSE EMP_CURSOR      
select VisitTaskID,tt.VisitTaskDetail as VisitTaskDetail ,tt.TaskOption as TaskOptionID, dm.Title  as TaskOption,DefaultTaskOption    
from @temp tt    
inner join DDMaster dm on dm.DDMasterID=tt.TaskOption    
  --  where VisitTaskID =@VisitTaskIDs     
DEALLOCATE EMP_CURSOR    
END