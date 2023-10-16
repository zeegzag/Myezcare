
CREATE PROCEDURE [dbo].[GetReferralTaskMapping]   
 -- Add the parameters for the stored procedure here  
 @ReferralID int,  
 @VisitTaskType VARCHAR(100),  
 @CareType VARCHAR(100)  
AS  
BEGIN  
 SET NOCOUNT ON;  

 EXEC [dbo].[UpdateReferralTaskMappingDaysForIsDefault] @ReferralID, @VisitTaskType, @CareType  
  
 CREATE TABLE #MappedTasksTemp(  
  VisitTaskID int,  
  VisitTaskCategoryId int,  
  VisitTaskSubCategoryID nvarchar(100),  
  VisitTaskType nvarchar(100),  
  Days nvarchar(100),  
  Frequency nvarchar(100),  
  Comment nvarchar(100),  
  isDefault bit,  
  CareTypeID int,  
  VisitTaskDetail nvarchar(100)  
 )  
  
 IF @CareType = 0  
 BEGIN  
  insert into #MappedTasksTemp(  
   VisitTaskID,  
   VisitTaskCategoryId,  
   VisitTaskSubCategoryID,  
   VisitTaskType,   
   Days,  
   Frequency,  
   Comment, isDefault, CareTypeID, VisitTaskDetail)  
  select   
   vt.VisitTaskID,  
   vt.VisitTaskCategoryID,   
   vt.VisitTaskSubCategoryID,   
   vt.VisitTaskType,  
   rtm.Days,  
   rtm.Frequency,  
   rtm.Comment, vt.IsDefault, vt.CareType, vt.VisitTaskDetail  
  from visittasks vt  
  join ReferralTaskMappings rtm on vt.VisitTaskID = rtm.VisitTaskID  
  where rtm.ReferralID=@ReferralID and rtm.isdeleted=0 and vt.IsDeleted=0 and vt.VisitTaskType = @VisitTaskType 
 END  
 ELSE  
 BEGIN  
 insert into #MappedTasksTemp(  
  VisitTaskID,  
  VisitTaskCategoryId,  
  VisitTaskSubCategoryID,  
  VisitTaskType,   
  Days,  
  Frequency,  
  Comment, isDefault,CareTypeID,VisitTaskDetail)  
 select   
  vt.VisitTaskID,  
  vt.VisitTaskCategoryID,   
  vt.VisitTaskSubCategoryID,   
  vt.VisitTaskType,  
  rtm.Days,  
  rtm.Frequency,  
  rtm.Comment, vt.IsDefault,vt.CareType,vt.VisitTaskDetail  
 from visittasks vt  
 join ReferralTaskMappings rtm on vt.VisitTaskID = rtm.VisitTaskID  
 where rtm.ReferralID=@ReferralID and rtm.isdeleted=0 and vt.IsDeleted=0 and vt.VisitTaskType = @VisitTaskType  
 and vt.CareType = @CareType  
 END  
  
IF @VisitTaskType ='Task'
BEGIN
 select   
  vtc.VisitTaskCategoryID,   
  vtc.VisitTaskCategoryName,   
  vtc.VisitTaskCategoryType,  
  vt.VisitTaskID,   
  vt.VisitTaskType,   
  vtc.ParentCategoryLevel,  
  CASE  
    WHEN vtc.ParentCategoryLevel IS NULL THEN vt.Days --'0'  
    ELSE vt.Days  
END AS Days,  
  vt.Frequency,  
  vt.Comment,  
  vt.IsDefault,  
  vt.CareTypeID,vt.VisitTaskDetail  
 from VisitTaskCategories  vtc  
 join #MappedTasksTemp vt on vt.VisitTaskCategoryID = vtc.VisitTaskCategoryID or   
 vtc.VisitTaskCategoryID  = vt.VisitTaskSubCategoryID  
 where vt.VisitTaskType = @VisitTaskType 
  END
 
 IF @VisitTaskType ='Conclusion'
 BEGIN
  select   
  vtc.VisitTaskCategoryID,   
  vtc.VisitTaskCategoryName,   
  vtc.VisitTaskCategoryType,  
  vt.VisitTaskID,   
  vt.VisitTaskType,   
  vtc.ParentCategoryLevel,  
vt.Days,   
  vt.Frequency,  
  vt.Comment,  
  vt.IsDefault,  
  vt.CareTypeID,vt.VisitTaskDetail  
 from VisitTaskCategories  vtc  
 join #MappedTasksTemp vt on vt.VisitTaskCategoryID = vtc.VisitTaskCategoryID or   
 vtc.VisitTaskCategoryID  = vt.VisitTaskSubCategoryID  
 where vt.VisitTaskType = @VisitTaskType
 END
  
 CREATE TABLE #NonMappedTasksTemp(  
  VisitTaskID int,  
  VisitTaskCategoryId int,  
  VisitTaskSubCategoryID nvarchar(100),  
  VisitTaskType nvarchar(100),  
  Days nvarchar(100),  
  Frequency nvarchar(100),  
  Comment nvarchar(100),  
  IsDefault bit,  
  CareTypeID int,  
  VisitTaskDetail nvarchar(100)  
 )  
  
 IF @CareType = 0  
 BEGIN  
  insert into #NonMappedTasksTemp(  
   VisitTaskID,  
   VisitTaskCategoryId,  
   VisitTaskSubCategoryID,  
   VisitTaskType,   
   Days,  
   Frequency,  
   Comment,IsDefault, CareTypeID,VisitTaskDetail)  
  select   
   vt.VisitTaskID,  
   vt.VisitTaskCategoryID,   
   vt.VisitTaskSubCategoryID,   
   vt.VisitTaskType,  
   0,  
   0,  
   '',  
    vt.IsDefault, vt.CareType,vt.VisitTaskDetail  
  from visittasks vt  
  where vt.VisitTaskID not in   
  (select VisitTaskID from #MappedTasksTemp)  
  and vt.IsDeleted = 0 and vt.VisitTaskType = @VisitTaskType  
 END  
 ELSE  
 BEGIN  
 insert into #NonMappedTasksTemp(  
   VisitTaskID,  
   VisitTaskCategoryId,  
   VisitTaskSubCategoryID,  
   VisitTaskType,   
   Days,  
   Frequency,  
   Comment,IsDefault,CareTypeID, VisitTaskDetail)  
  select   
   vt.VisitTaskID,  
   vt.VisitTaskCategoryID,   
   vt.VisitTaskSubCategoryID,   
   vt.VisitTaskType,  
   0,  
   0,  
   '',  
    vt.IsDefault,  
    vt.CareType,vt.VisitTaskDetail  
  from visittasks vt  
  where vt.VisitTaskID not in   
  (select VisitTaskID from #MappedTasksTemp)  
  and vt.IsDeleted = 0 and vt.VisitTaskType = @VisitTaskType  
  and vt.CareType = @CareType  
 END  
  
  
  
  select   
   vtc.VisitTaskCategoryID,   
   vtc.VisitTaskCategoryName,   
   vtc.VisitTaskCategoryType,  
   vtt.VisitTaskID,   
   vtt.VisitTaskType,   
   vtc.ParentCategoryLevel,  
   vtt.Days,  
   vtt.Frequency,  
   vtt.Comment, vtt.IsDefault, vtt.CareTypeID,vtt.VisitTaskDetail  
  from VisitTaskCategories  vtc  
  join #NonMappedTasksTemp vtt on vtt.VisitTaskCategoryId = vtc.VisitTaskCategoryID  
  or vtt.VisitTaskSubCategoryID  = vtc.VisitTaskCategoryID  
  and  vtt.VisitTaskType =  @VisitTaskType  
    
  
  
 DROP TABLE #MappedTasksTemp  
 DROP TABLE #NonMappedTasksTemp  
  
   
    
END  