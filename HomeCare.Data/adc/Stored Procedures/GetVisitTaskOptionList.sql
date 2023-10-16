
CREATE PROCEDURE [adc].[GetVisitTaskOptionList]
@ReferralID bigint=0
as
begin
SET NOCOUNT ON  
Declare @TaskOption nvarchar(max),
@DefaultTaskOption nvarchar(max),
@VisitTaskID bigint;
declare @temp table(
VisitTaskID bigint,
TaskOption nvarchar(max),
DefaultTaskOption nvarchar(max)
)
DECLARE EMP_CURSOR CURSOR  
 STATIC  FOR  
--SELECT VisitTaskID ,TaskOption,DefaultTaskOption FROM VisitTasks 
    
 SELECT distinct VT.VisitTaskID, VT.TaskOption,VT.DefaultTaskOption
 FROM ReferralTaskMappings RTM      
 INNER JOIN VisitTasks VT ON VT.VisitTaskID=RTM.VisitTaskID      
 WHERE VT.IsDeleted=0 AND RTM.IsDeleted=0 AND RTM.ReferralID=@ReferralID  

OPEN EMP_CURSOR  
FETCH NEXT FROM EMP_CURSOR INTO  @VisitTaskID ,@TaskOption,@DefaultTaskOption 
WHILE @@FETCH_STATUS = 0  
BEGIN  
	insert into @temp  select @VisitTaskID, val ,@DefaultTaskOption from GetCSVTable(@TaskOption)
	
--PRINT  'EMP_ID: ' + CONVERT(NVARCHAR(MAX),@VisitTaskID)+  '  EMP_NAME '+@TaskOption 
FETCH  FROM EMP_CURSOR INTO  @VisitTaskID ,@TaskOption,@DefaultTaskOption


END  
CLOSE EMP_CURSOR  
select VisitTaskID,tt.TaskOption as TaskOptionID, dm.Title  as TaskOption,DefaultTaskOption
from @temp tt
inner join DDMaster dm on dm.DDMasterID=tt.TaskOption
     
DEALLOCATE EMP_CURSOR

end