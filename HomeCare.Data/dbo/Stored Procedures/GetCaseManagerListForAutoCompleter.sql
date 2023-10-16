 -- EXEC GetCaseManagerListForAutoCompleter @SearchText = '', @PazeSize = '10'
CREATE PROCEDURE [dbo].[GetCaseManagerListForAutoCompleter]  
 @SearchText VARCHAR(MAX),
 @PazeSize int
AS  
BEGIN  
 SET NOCOUNT ON;  
 	select TOP (@PazeSize) CM.CaseManagerID,CM.LastName+', '+CM.FirstName as Name,A.AgencyID,A.NickName,R.RegionName,CM.Phone,CM.Fax,CM.Cell,CM.Email  from CaseManagers CM
	inner join Agencies A on A.AgencyID=CM.AgencyID
	inner join Regions R on R.RegionID=A.RegionID
	where CM.IsDeleted=0
	AND (
		CM.FirstName LIKE '%'+@SearchText+'%' OR
		CM.LastName  LIKE '%'+@SearchText+'%' OR
		CM.FirstName +' '+CM.LastName like '%'+@SearchText+'%' OR
		CM.LastName +' '+CM.FirstName like '%'+@SearchText+'%' OR
		CM.FirstName +', '+CM.LastName like '%'+@SearchText+'%' OR
		CM.LastName +', '+CM.FirstName like '%'+@SearchText+'%' OR
		A.NickName like '%'+@SearchText+'%'
	)
END