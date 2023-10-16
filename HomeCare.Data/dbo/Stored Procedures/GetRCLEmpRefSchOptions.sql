
-- EXEC GetRCLEmpRefSchOptions @ReferralID = '1951', @SortExpression = 'Employee', @SortType = 'ASC', @FromIndex = '1', @PageSize = '10000', @SortIndexArray = 'Preferences DESC'
CREATE PROCEDURE [dbo].[GetRCLEmpRefSchOptions]              
 @ReferralID BIGINT = 0,                                                 
 @EmployeeName VARCHAR(MAX)='',                    
 @MileRadius BIGINT=NULL,                    
 @StrSkillList VARCHAR(MAX) = NULL,                                    
 @StrPreferenceList VARCHAR(MAX) = NULL,                
 @PreferenceType_Prefernce VARCHAR(100)='Preference',              
 @PreferenceType_Skill VARCHAR(100)='Skill',                 
 @SortExpression NVARCHAR(100),                                  
 @SortType NVARCHAR(10),                                
 @FromIndex INT,                                
 @PageSize INT,              
 @SortIndexArray VARCHAR(MAX)
AS                                
BEGIN
	DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
	DECLARE @CurrentPermanentEmployeeID BIGINT = 0
	
	IF EXISTS(SELECT 1 FROM ReferralCaseLoads WHERE ReferralID = @ReferralID AND CaseLoadType = 'Permanent' AND EndDate IS NULL)
	BEGIN
		SELECT
			@CurrentPermanentEmployeeID = EmployeeID
		FROM
			ReferralCaseLoads
		WHERE 
			ReferralID = @ReferralID 
			AND CaseLoadType = 'Permanent' 
			AND EndDate IS NULL
	END
 
	IF(@StrSkillList IS NULL OR LEN(@StrSkillList)=0)              
	SET @StrSkillList=NULL;              
	             
	IF(@StrPreferenceList IS NULL OR LEN(@StrPreferenceList)=0)              
	SET @StrPreferenceList=NULL;              
	             
	DECLARE @InfiniteEndDate DATE='2099-12-31';              
	DECLARE @PatientPayorID BIGINT = 0;          
	DECLARE @EmployeeID BIGINT = 0;              
	DECLARE @EmployeeTSDateID BIGINT=0;              
	DECLARE @ReferralTSDateID BIGINT=0;  
	              
	DECLARE @TotalPrefeCount BIGINT=0;              
	DECLARE @TotalSkillCount BIGINT=0;              
	             
	SELECT        
	@TotalPrefeCount=SUM(CASE WHEN PR.KeyType=@PreferenceType_Prefernce THEN 1 ELSE 0 END) OVER (PARTITION BY RP.ReferralID),          
	@TotalSkillCount=SUM(CASE WHEN PR.KeyType=@PreferenceType_Skill THEN 1 ELSE 0 END) OVER (PARTITION BY RP.ReferralID)              
	FROM ReferralPreferences  RP              
	INNER JOIN Preferences PR ON PR.PreferenceID=RP.PreferenceID              
	WHERE ReferralID=@ReferralID              
	             
	IF(@TotalPrefeCount=0) SET @TotalPrefeCount=1;              
	IF(@TotalSkillCount=0) SET @TotalSkillCount=1;              
	             
	
	DECLARE @TempGetEmpoyeePreferences Table(              
	   ReferralID BIGINT,              
	   RefPreferenceID BIGINT,              
	   EmpPreferenceID  BIGINT,              
	   EmployeeID BIGINT,              
	   FirstName VARCHAR(5000),              
	   LastName VARCHAR(5000),
	   EmployeeName VARCHAR(5000),
	   IsDeleted BIT,              
	   OrderRank INT,              
	   PreferencesMatchPercent INT,              
	   SkillsMatchPercent INT,              
	   EmpLatLong GeoGraphy,              
	   KeyType VARCHAR(MAX)              
	)       
	          
	;WITH CTETempGetEmpoyeePreferences AS               
	(              
		 SELECT ReferralID,RefPreferenceID,EmpPreferenceID,EmployeeID,FirstName,LastName,EmployeeName,
		 IsDeleted,OrderRank,  
		 PreferencesMatchPercent,SkillsMatchPercent,EmpLatLong,KeyType
		 FROM 
		 (              
			  SELECT R.ReferralID,PE.KeyType,              
			  RefPreferenceID=RP.PreferenceID,EmpPreferenceID=EP.PreferenceID,E.EmployeeID,E.FirstName,E.LastName,dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName,   
			  E.IsDeleted, EmpLatLong=[dbo].GetGeoFromLatLng(E.Latitude,E.Longitude),              
			  PreferencesMatchPercent=  
			  SUM(CASE WHEN PE.KeyType=@PreferenceType_Prefernce AND RP.PreferenceID IS NOT NULL THEN 1 ELSE 0 END) OVER  
			  ( PARTITION BY EP.EmployeeID) * 100/@TotalPrefeCount ,              
			  SkillsMatchPercent=  
			  SUM(CASE WHEN PE.KeyType=@PreferenceType_Skill AND RP.PreferenceID IS NOT NULL THEN 1 ELSE 0 END) OVER  
			  ( PARTITION BY EP.EmployeeID) * 100/@TotalSkillCount ,              
			  OrderRank= DENSE_RANK() OVER(PARTITION BY  EP.EmployeeID ORDER BY R.ReferralID DESC ,EP.EmployeePreferenceID DESC)                
			  FROM 
					Employees E     
					LEFT JOIN EmployeePreferences EP ON E.EmployeeID=EP.EmployeeID              
					LEFT JOIN ReferralPreferences RP ON RP.PreferenceID=EP.PreferenceID AND RP.ReferralID=@ReferralID              
					LEFT JOIN Preferences PE ON PE.PreferenceID=EP.PreferenceID              
					LEFT JOIN (SELECT ReferralID=@ReferralID) R ON  R.ReferralID=RP.ReferralID              
			  WHERE 
					E.EmployeeID != @CurrentPermanentEmployeeID
					AND
					(
						(@EmployeeName IS NULL OR LEN(@EmployeeName)=0 OR E.FirstName IS NULL)               
						OR              
						(
							(E.FirstName LIKE '%'+@EmployeeName+'%' )OR                
							(E.LastName  LIKE '%'+@EmployeeName+'%') OR                
							(E.FirstName +' '+E.LastName like '%'+@EmployeeName+'%') OR                
							(E.LastName +' '+E.FirstName like '%'+@EmployeeName+'%') OR                
							(E.FirstName +', '+E.LastName like '%'+@EmployeeName+'%') OR                
							(E.LastName +', '+E.FirstName like '%'+@EmployeeName+'%')
						)
					)
					AND
					(  
						(@StrSkillList IS NULL  AND @StrPreferenceList IS NULL ) OR              
						(
							(@StrSkillList IS NULL  AND @StrPreferenceList IS NOT NULL ) 
							AND (EP.PreferenceID IN (SELECT CONVERT(BIGINT, VAL) FROM GetCSVTable(@StrPreferenceList)) )
						) OR              
						(
							(@StrSkillList IS NOT NULL  AND @StrPreferenceList IS NULL) AND (EP.PreferenceID IN (SELECT CONVERT(BIGINT, VAL) FROM GetCSVTable(@StrSkillList)) )
						) OR              
						(
							(@StrSkillList IS NOT NULL AND @StrPreferenceList IS NOT NULL) AND               
							((EP.PreferenceID IN (SELECT CONVERT(BIGINT, VAL) FROM GetCSVTable(@StrPreferenceList))) 
							OR (EP.PreferenceID IN (SELECT CONVERT(BIGINT, VAL) FROM GetCSVTable(@StrSkillList)) ))              
						)
					)
		  ) AS Temp WHERE Temp.OrderRank=1               
	)              
	             
	INSERT INTO @TempGetEmpoyeePreferences              
	SELECT * FROM CTETempGetEmpoyeePreferences              
	           
	UPDATE @TempGetEmpoyeePreferences SET ReferralID=@ReferralID            
	             
	-- EXEC GetRCLEmpRefSchOptions @ReferralID = '1951', @EmployeeID = '0', @StartDate = '2018/03/05', @EndDate = '2018/03/18', @SortExpression = 'EmployeeDayOffID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50' ,@SortIndexArray ='Skills ASC, Preferences DESC, Miles ASC'             
	             
	--SELECT * FROM @TempGetEmpoyeePreferences              
	                
	Declare @Item1 varchar(max), @Item2 varchar(max), @Item3 varchar(max), @Item4 varchar(max);  
	               
	select @Item1=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=1                
	select @Item2=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=2                
	select @Item3=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=3                
	                
	;WITH CTEEmployeeTSList AS
	(                                 
		SELECT *,COUNT(t1.EmployeeID) OVER() AS Count FROM                                 
		(                                
			 SELECT ROW_NUMBER() OVER (ORDER BY                                 
							
			 CASE WHEN 'Skills ASC' = @Item1  THEN  TBL1.SkillsMatchPercent  END  ASC,   
			 CASE WHEN 'Skills DESC' = @Item1  THEN  TBL1.SkillsMatchPercent  END DESC,  
									 
			 CASE WHEN 'Preferences ASC'=@Item1 THEN  TBL1.PreferencesMatchPercent  END  ASC,   
			 CASE WHEN 'Preferences DESC'=@Item1  THEN  TBL1.PreferencesMatchPercent  END DESC,   
						
			 CASE WHEN 'Miles ASC'=@Item1 THEN  TBL1.Distance END  ASC,  
			 CASE WHEN 'Miles DESC'=@Item1  THEN TBL1.Distance END DESC, 
						 
			 CASE WHEN  'Skills ASC'= @Item2  THEN  TBL1.SkillsMatchPercent  END  ASC,   
			 CASE WHEN  'Skills DESC' = @Item2  THEN  TBL1.SkillsMatchPercent  END DESC,  
									   
			 CASE WHEN  'Preferences ASC'=@Item2 THEN  TBL1.PreferencesMatchPercent  END  ASC,   
			 CASE WHEN 'Preferences DESC'=@Item2  THEN  TBL1.PreferencesMatchPercent  END DESC,   
						
			 CASE WHEN  'Miles ASC'=@Item2 THEN  TBL1.Distance END  ASC,  
			 CASE WHEN  'Miles DESC'=@Item2  THEN TBL1.Distance END DESC, 
						 
			 CASE WHEN  'Skills ASC'= @Item3  THEN  TBL1.SkillsMatchPercent  END  ASC,   
			 CASE WHEN  'Skills DESC' = @Item3 THEN  TBL1.SkillsMatchPercent  END DESC,  
									
			 CASE WHEN  'Preferences ASC'=@Item3 THEN  TBL1.PreferencesMatchPercent  END  ASC,   
			 CASE WHEN  'Preferences DESC'=@Item3  THEN  TBL1.PreferencesMatchPercent  END DESC,    
					
			 CASE WHEN  'Miles ASC'=@Item3 THEN  TBL1.Distance END  ASC,  
			 CASE WHEN 'Miles DESC'=@Item3  THEN TBL1.Distance END DESC,  
								   
			 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Employee' THEN TBL1.FirstName END END ASC,                                
			 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Employee' THEN TBL1.FirstName END END DESC           
							  
			 ) AS Row, * 
			 FROM 
			 (                      
				 SELECT * FROM 
				 (              
					  SELECT  E.*,              
					  Distance=              
					  CASE WHEN EmpLatLong IS NULL OR C.Latitude IS NULL OR C.Longitude IS NULL THEN NULL ELSE               
					  (EmpLatLong.STDistance([dbo].GetGeoFromLatLng(C.Latitude,C.Longitude)) * 0.000621371) END
					  FROM @TempGetEmpoyeePreferences E              
					  LEFT JOIN ContactMappings CM ON CM.ReferralID = E.ReferralID AND CM.ContactTypeID=1              
					  LEFT JOIN Contacts C ON C.ContactID= CM.ContactID               
				 ) AS TEMP WHERE 1=1 AND               
				 (@MileRadius IS NULL OR Distance < @MileRadius)              
			 )   AS TBL1               
		) AS t1    
	)                                
	                          
	SELECT * FROM CTEEmployeeTSList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)   
END
