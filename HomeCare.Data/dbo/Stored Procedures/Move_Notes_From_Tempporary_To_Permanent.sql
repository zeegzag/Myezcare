CREATE PROCEDURE Move_Notes_From_Tempporary_To_Permanent  
@LoggedInID BIGINT,  
@IsDayCare BIT,  
@ReferralsIds varchar(max)=null,  
@ResultRequired bit = 1  
AS  
BEGIN  
  
  
DECLARE @Id INT;  
DECLARE @employeevisitid bigint;  
DROP TABLE IF EXISTS #temp  
SELECT ROW_NUMBER() OVER(ORDER BY N.EmployeeVisitID ASC) AS Id, N.EmployeeVisitID into #temp FROM Notes_Temporary N  
WHERE CreatedBy = @LoggedInID AND IsDeleted = 0 AND ( @ReferralsIds IS NULL OR LEN(@ReferralsIds)=0 OR ReferralID in (SELECT val FROM GETCSVTABLE(@ReferralsIds)) )  
  
Select @Id=Count(*) From #Temp  
While @Id > 0  
Begin  
DECLARE @resultid BIGINT;  
SELECT @employeevisitid = EmployeeVisitID from #temp where Id=@Id  
  
  
IF(@IsDayCare=1)  
BEGIN  
EXEC [ADC].[API_AddNoteByCareType] @employeevisitid, 0, null, @ResultRequired  
END  
ELSE  
BEGIN  
EXEC [API_AddNoteByCareType] @employeevisitid, 0, null, @ResultRequired  
END  
  
SET @Id = (@Id - 1)  
End  
  
  
EXEC HC_RefreshAndGroupingNotes @ResultRequired  
  
  
END  
  