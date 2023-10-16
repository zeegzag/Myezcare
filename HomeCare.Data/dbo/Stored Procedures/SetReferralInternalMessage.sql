CREATE PROCEDURE [dbo].[SetReferralInternalMessage]  
@EmployeeID BIGINT,  
@ReferralID BIGINT,  
@ReferralInternalMessageID BIGINT=0,  
@Assignee BIGINT=0,  
@IsDelete BIT=0,  
@SortExpression NVARCHAR(100),    
@SortType NVARCHAR(10),  
@FromIndex INT,  
@PageSize INT  
AS  
BEGIN  
  
IF @IsDelete=1   
BEGIN  
 UPDATE ReferralInternalMessages SET IsDeleted=1 WHERE ReferralInternalMessageID=@ReferralInternalMessageID;  
END  
  
  
;WITH CTEReferralInternalMessage AS  
 (   
  SELECT *,COUNT(T1.ReferralInternalMessageID) OVER() AS Count FROM   
  (  
   SELECT ROW_NUMBER() OVER (ORDER BY   
    CASE WHEN @SortType = 'ASC' THEN  CASE WHEN @SortExpression = 'ReferralInternalMessageID' THEN t.ReferralInternalMessageID END  END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralInternalMessageID' THEN t.ReferralInternalMessageID END       END DESC,  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Note' THEN t.Note END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Note' THEN t.Note END END DESC,  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Assignee' THEN t.AssigneeName END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN  CASE WHEN @SortExpression = 'Assignee' THEN t.AssigneeName END END DESC,  
    CASE WHEN @SortType = 'ASC' THEN  CASE WHEN @SortExpression = 'Status' THEN t.IsResolved END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Status' THEN t.IsResolved END END DESC,  
	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN Convert(datetime,t.CreatedDate,103) END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedDate' THEN Convert(datetime,t.CreatedDate,103) END END DESC, 
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedBy' THEN t.CreatedByName END END ASC,  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedBy' THEN t.CreatedByName END END DESC  

   ) AS ROW,  
   t.* FROM  
   (  
   SELECT emp.EmployeeID,rim.ReferralInternalMessageID, rim.Note, ref.ReferralID, rim.Assignee, emp.LastName + ', ' + emp.FirstName AS AssigneeName, rim.IsResolved,  
     emp1.LastName + ', ' + emp1.FirstName AS CreatedByName, rim.CreatedBy, rim.CreatedDate,  
     CASE WHEN emp.EmployeeID=@EmployeeID THEN 1 ELSE 0 END AS CanResolve  
    FROM Referrals ref   
    JOIN ReferralInternalMessages rim  ON ref.referralid=rim.referralid  
    JOIN Employees emp ON emp.EmployeeID=rim.Assignee  
    join employees emp1 on emp1.EmployeeID=rim.CreatedBy  
   WHERE ref.ReferralID=@ReferralID  
   and (((@Assignee =0) or (@Assignee =-1)) OR rim.Assignee = @Employeeid )          
   and rim.IsDeleted=0  
  ) AS T) AS T1)  
   
 SELECT * FROM CTEReferralInternalMessage WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)   
END