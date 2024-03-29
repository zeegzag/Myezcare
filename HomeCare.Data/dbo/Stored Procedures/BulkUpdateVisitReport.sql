
CREATE PROCEDURE [dbo].[BulkUpdateVisitReport]  
 @BulkType nvarchar(max),                  
 @EmployeeVisitIDList NVARCHAR(max),              
 @loggedInID BIGINT,      
 @VisitTaskValue NVARCHAR(max) 
   
AS                          
BEGIN              
 declare @setBit bit=null;  
 declare @id bigint=null  
          SELECT * 
INTO #tempTable
FROM (
   select EmployeeVisitID,ScheduleID from EmployeeVisits where EmployeeVisitID in (select val from dbo.f_split(@EmployeeVisitIDList, ',')) 
) AS x


  if(@BulkType IS NOT NULL and @BulkType!='')  
 Begin   
  if(@BulkType ='CareType')  
   begin  
   UPDATE ScheduleMasters set  CareTypeId=CASt(@VisitTaskValue as bigint) ,UpdatedDate=GETUTCDATE(),UpdatedBy=@loggedInID where ScheduleID in (select ScheduleID from #tempTable)      
    SELECT 1; RETURN; 
   END  
  
  else if(@BulkType ='AuthorizationCode')  
    BEGIN
     UPDATE ScheduleMasters set  ReferralBillingAuthorizationID=CASt(@VisitTaskValue as bigint) ,UpdatedDate=GETUTCDATE(),UpdatedBy=@loggedInID where ScheduleID in (select ScheduleID from #tempTable)   
    SELECT 2; RETURN
    END
  else if(@BulkType ='Payor') 
  BEGIN 
  UPDATE ScheduleMasters set  PayorID=CASt(@VisitTaskValue as bigint) ,UpdatedDate=GETUTCDATE() where ScheduleID in (select ScheduleID from #tempTable)      
      SELECT 3; RETURN; 
	  END
  
 End  

 
                
  SELECT 1; RETURN;              
END
