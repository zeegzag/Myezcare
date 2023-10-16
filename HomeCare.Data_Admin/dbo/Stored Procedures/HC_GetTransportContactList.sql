--   EXEC HC_GetTransportContactList @FirstName = '', @Email = '', @MobileNumber = '', @Address = '', @ContactType = '2', @IsDeleted = '0', @SortExpression = 'Address', @SortType = 'DESC', @FromIndex = '1', @PageSize = '50'           
CREATE PROCEDURE [dbo].[HC_GetTransportContactList]                    
@FirstName VARCHAR(100) = NULL,                     
@Email VARCHAR(100) = NULL,                     
@MobileNumber VARCHAR(100) = NULL,                    
@Address VARCHAR(100) = NULL,                     
@ContactType VARCHAR(100) = NULL,                     
@IsDeleted BIGINT = -1,                      
@SortExpression VARCHAR(100),                        
@SortType VARCHAR(10),                      
@FromIndex INT,                      
@PageSize INT                                
                                
AS                                            
BEGIN                                            
                                            
;WITH List AS                                            
 (                                             
  SELECT *,COUNT(T1.ContactID) OVER() AS Count FROM                                             
  (                                            
   SELECT ROW_NUMBER() OVER (ORDER BY                                             
                                        
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'FirstName' THEN t.FirstName END END ASC,                                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'FirstName' THEN t.FirstName END END DESC,                  
                   
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Email' THEN t.Email END END ASC,                                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Email' THEN t.Email END END DESC,                  
                  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'MobileNumber' THEN t.MobileNumber END END ASC,                                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'MobileNumber' THEN t.MobileNumber END END DESC,                  
                  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Address' THEN t.Address END END ASC,                                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Address' THEN t.Address END END DESC,                  
                   
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ContactType' THEN t.TransportMasterID END END ASC,                                      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ContactType' THEN t.TransportMasterID END END DESC                   
                                    
   ) AS ROW,                                            
   t.*  FROM     (                                  
                        
SELECT tc.ContactID, tc.FirstName, tc.LastName, tc.Email, tc.Phone, tc.MobileNumber, tc.Fax, tc.ApartmentNo,                  
tc.Address, tc.City, tc.State, tc.ZipCode, tm.TransportMasterID, tm.Title AS ContactType, tc.IsDeleted, tc.CreatedDate                       
   FROM TransportContacts tc                 
   LEFT JOIN TransportMaster tm ON tm.TransportMasterID = tc.ContactType                
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR tc.IsDeleted=@IsDeleted)    
   AND ((@ContactType IS NULL OR LEN(@ContactType)=0) OR (tm.TransportMasterID in (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@ContactType))))   
   AND ((@Email IS NULL OR LEN(tc.Email)=0) OR ((tc.Email LIKE '%'+@Email+'%' )))  
   AND ((@MobileNumber IS NULL OR LEN(tc.MobileNumber)=0) OR ((tc.MobileNumber LIKE '%'+@MobileNumber+'%' )))  
   AND ((@Address IS NULL OR LEN(tc.Address)=0) OR ((tc.Address LIKE '%'+@Address+'%' )))  
--   AND ((@FirstName IS NULL OR LEN(tc.FirstName)=0) OR ((tc.FirstName LIKE '%'+@FirstName+'%' )))        
--   OR ((@FirstName IS NULL OR LEN(tc.LastName)=0) OR ((tc.LastName LIKE '%'+@FirstName+'%' )))   
   AND ((@FirstName IS NULL OR LEN(tc.LastName)=0)                                 
   OR (                                
    (tc.FirstName LIKE '%'+@FirstName+'%' )OR                                  
    (tc.LastName  LIKE '%'+@FirstName+'%') OR                                  
    (tc.FirstName +' '+tc.LastName like '%'+@FirstName+'%') OR                                  
    (tc.LastName +' '+tc.FirstName like '%'+@FirstName+'%')))  
  ) t                      
                      
)  AS T1 )                                
                                
SELECT * FROM List WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                                             
END 