CREATE PROCEDURE [dbo].[HC_SaveEmployeeDayOff]
 @EmployeeDayOffID BIGINT,
 @EmployeeID BIGINT,
 @StartTime DATETIME = NULL,      
 @EndTime DATETIME = NULL,   
 @DayOffStatus NVARCHAR(50),  
 @DayOffTypeID  BIGINT=NULL,
 @EmployeeComment NVARCHAR(1000),
 @loggedInID BIGINT, 
 @SystemID VARCHAR(100)
AS  
BEGIN      

 IF(@EmployeeDayOffID=0)
 BEGIN
   INSERT INTO EmployeeDayOffs VALUES(@EmployeeID,@StartTime,@EndTime,@DayOffStatus,NULL,NULL,@EmployeeComment,NULL,0,
   GetDate(),@loggedInID,GetDate(),@loggedInID,@SystemID,@DayOffTypeID )
 END
 ELSE 
 BEGIN
   UPDATE EmployeeDayOffs
   SET StartTime=@StartTime,EndTime=@EndTime,EmployeeComment=@EmployeeComment,UpdatedDate=GETDATE(),UpdatedBy=@loggedInID,
   DayOffTypeID =@DayOffTypeID 
   WHERE EmployeeDayOffID=@EmployeeDayOffID
 END
 
 SELECT 1;
END
