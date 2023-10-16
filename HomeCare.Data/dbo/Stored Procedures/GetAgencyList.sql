CREATE PROCEDURE [dbo].[GetAgencyList]        
 @NickName varchar(100)=null,        
 @ShortName varchar(50)=null,        
 @RegionID bigint=0,        
 @ContactName varchar(100)=null,        
 @Email varchar(50)=null,        
 @Phone varchar(50)=null,        
 @Address varchar(50)=null,        
 @IsDeleted BIGINT = -1,          
 @SortExpression NVARCHAR(100),            
 @SortType NVARCHAR(10),          
 @FromIndex INT,          
 @PageSize INT          
AS          
BEGIN          
 ;WITH CTECaseManagerList AS          
 (           
  SELECT *,COUNT(T1.AgencyID) OVER() AS Count FROM           
  (          
   SELECT ROW_NUMBER() OVER (ORDER BY           
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AgencyID' THEN AgencyID END END ASC,          
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AgencyID' THEN AgencyID END END DESC,          
             
    CASE WHEN @SortType = 'ASC' THEN          
      CASE                 
      WHEN @SortExpression = 'NickName' THEN NickName         
      WHEN @SortExpression = 'ShortName' THEN ShortName          
      WHEN @SortExpression = 'RegionName' THEN R.RegionName          
      WHEN @SortExpression = 'ContactName' THEN ContactName        
      WHEN @SortExpression = 'Email' THEN A.Email          
      WHEN @SortExpression = 'Phone' THEN A.Email          
      WHEN @SortExpression = 'Address' THEN A.Address          
      END           
    END ASC,          
    CASE WHEN @SortType = 'DESC' THEN          
      CASE                 
      WHEN @SortExpression = 'NickName' THEN NickName         
      WHEN @SortExpression = 'ShortName' THEN ShortName          
      WHEN @SortExpression = 'RegionName' THEN R.RegionName          
      WHEN @SortExpression = 'ContactName' THEN ContactName        
      WHEN @SortExpression = 'Email' THEN A.Email          
      WHEN @SortExpression = 'Phone' THEN A.Email          
      WHEN @SortExpression = 'Address' THEN A.Address        
      END          
    END DESC          
  ) AS Row,          
    A.AgencyID,NickName,ShortName,R.RegionName,A.RegionID,ContactName,A.Email,Phone,Address,A.Fax,City,StateCode,ZipCode,A.IsDeleted            
   FROM   Agencies A         
   left join Regions R on R.RegionID=A.RegionID           
WHERE((CAST(@IsDeleted AS BIGINT)=-1) OR A.IsDeleted=@IsDeleted)          
     AND ((@NickName IS NULL OR LEN(@NickName)=0) OR A.NickName LIKE '%' + @NickName+ '%')          
     AND ((@ShortName IS NULL OR LEN(@ShortName)=0) OR A.ShortName LIKE '%' + @ShortName+ '%')          
     AND (( CAST(@RegionID AS BIGINT)=0) OR R.RegionID = CAST(@RegionID AS BIGINT))        
     AND ((@ContactName IS NULL OR LEN(@ContactName)=0) OR A.ContactName LIKE '%' + @ContactName + '%')           
     AND ((@Email IS NULL OR LEN(@Email)=0) OR A.Email LIKE '%' + @Email + '%')          
     AND ((@Phone IS NULL OR LEN(@Phone)=0) OR A.Phone LIKE '%' + @Phone + '%')          
     AND ((@Address IS NULL OR LEN(@Address)=0) OR A.Address LIKE '%' + @Address + '%')  
               
  ) AS T1  )          --  order by NickName ASC 
 SELECT * FROM CTECaseManagerList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)           
 
END