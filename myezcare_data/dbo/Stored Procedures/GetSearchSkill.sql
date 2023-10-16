CREATE PROCEDURE [dbo].[GetSearchSkill]    
@SearchText varchar(20)=null,      
@PageSize int,
@PreferenceType VARCHAR(100)       
AS      
BEGIN    
    
SELECT DISTINCT Top(@PageSize) PreferenceName,PreferenceID FROM  Preferences  
WHERE (@SearchText IS NULL) OR (PreferenceName LIKE '%'+@SearchText+'%' )  AND KeyType=@PreferenceType AND IsDeleted=0
       
END


--SELECT * FROM EmployeePreferences
