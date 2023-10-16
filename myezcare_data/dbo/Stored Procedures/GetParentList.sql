-- EXEC GetParentList @SortExpression='CaseManagerID',@SortType='ASC',@FromIndex='1',@PageSize='10000'
CREATE PROCEDURE [dbo].[GetParentList]  
 @Name VARCHAR(MAX) = NULL,  
 @Email VARCHAR(MAX) = NULL,  
 @Address VARCHAR(MAX) = NULL,  
 @City VARCHAR(MAX) = NULL,  
 @ZipCode VARCHAR(MAX) = NULL,  
 @Phone VARCHAR(MAX) = NULL,  
 @ContactTypeID BIGINT = 0,  
 @IsDeleted BIGINT = -1,  
 @SortExpression NVARCHAR(100),    
 @SortType NVARCHAR(10),  
 @FromIndex INT,  
 @PageSize INT  
AS  
BEGIN  
 ;WITH CTEParentist AS  
 (   
  SELECT *,COUNT(T1.ContactID) OVER() AS Count FROM   
  (  
   SELECT ROW_NUMBER() OVER (ORDER BY   
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ContactID' THEN ContactID END END ASC,  
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ContactID' THEN ContactID END END DESC,  

   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralCount' THEN ReferralCount END END ASC,  
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralCount' THEN ReferralCount END END DESC,  

   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ContactType' THEN ContactTypeName END END ASC,  
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ContactType' THEN ContactTypeName END END DESC,  

   
    CASE WHEN @SortType = 'ASC' THEN  
      CASE         
      WHEN @SortExpression = 'Name' THEN Name  
      WHEN @SortExpression = 'Email' THEN Email  
      WHEN @SortExpression = 'Address' THEN Address
	  WHEN @SortExpression = 'City' THEN City
	  WHEN @SortExpression = 'ZipCode' THEN ZipCode  
	  WHEN @SortExpression = 'Phone1' THEN Phone1  
      END   
    END ASC,  
    CASE WHEN @SortType = 'DESC' THEN  
      CASE         
      WHEN @SortExpression = 'Name' THEN Name  
      WHEN @SortExpression = 'Email' THEN Email  
      WHEN @SortExpression = 'Address' THEN Address
	  WHEN @SortExpression = 'City' THEN City
	  WHEN @SortExpression = 'ZipCode' THEN ZipCode  
	  WHEN @SortExpression = 'Phone1' THEN Phone1  
      END  
    END DESC  
  ) AS Row,  
   
   * FROM (
   
   
   SELECT C.ContactID, C.FirstName + ' ' +C.LastName AS Name, C.Email,C.Address, C.City, C.State, C.ZipCode, C.Phone1, C.Phone2,CT.ContactTypeName,C.IsDeleted,
   ReferralCount=COUNT(R.ReferralID)
   FROM  Contacts C   
   LEFT JOIN ContactMappings CM ON CM.ContactID= C.ContactID
   LEFT JOIN ContactTypes CT ON CT.ContactTypeID=CM.ContactTypeID
   LEFT JOIN Referrals R ON R.ReferralID=CM.ReferralID AND R.IsDeleted=0
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR C.IsDeleted=@IsDeleted)  
   AND ((@Email IS NULL OR LEN(@Email)=0) OR C.Email LIKE '%' + @Email + '%')  
   AND 
    ((@Name IS NULL OR LEN(C.LastName)=0)   
   OR (  
       (C.FirstName LIKE '%'+@Name+'%' )OR    
    (C.LastName  LIKE '%'+@Name+'%') OR    
    (C.FirstName +' '+C.LastName like '%'+@Name+'%') OR    
    (C.LastName +' '+C.FirstName like '%'+@Name+'%') OR    
    (C.FirstName +', '+C.LastName like '%'+@Name+'%') OR    
    (C.LastName +', '+C.FirstName like '%'+@Name+'%'))) 
     
   AND ((@Address IS NULL OR LEN(@Address)=0) OR C.Address LIKE '%' + @Address + '%')  
   AND ((@City IS NULL OR LEN(@City)=0) OR C.City LIKE '%' + @City + '%')  
   AND ((@ZipCode IS NULL OR LEN(@ZipCode)=0) OR C.ZipCode LIKE '%' + @ZipCode + '%')  
   AND ((@Phone IS NULL OR LEN(@Phone)=0) OR C.Phone1 LIKE '%' + @Phone + '%')  

   AND ((@ContactTypeID=0) OR CT.ContactTypeID=@ContactTypeID)  


   GROUP BY C.ContactID, C.LastName , C.FirstName, C.Email,C.Phone1,C.Phone2, C.Address,C.City,C.ZipCode,C.State,C.IsDeleted,CT.ContactTypeName

  ) AS T2 ) AS T1 
    
 )  
   
 SELECT * FROM CTEParentist WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)   
END



