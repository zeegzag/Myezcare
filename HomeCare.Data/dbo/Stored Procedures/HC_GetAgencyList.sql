
CREATE PROCEDURE [dbo].[HC_GetAgencyList]                
@AgencyType varchar(100)=null,                
 @NickName varchar(100)=null,                
 @ShortName varchar(50)=null,                
 @RegionID bigint=0,                
 @ContactName varchar(100)=null,                
 @Email varchar(50)=null,                
 @Phone varchar(50)=null,                
 @Address varchar(50)=null,               
 @TIN VARCHAR(100)=null,         
 @EIN VARCHAR(100)=null,         
 @Mobile VARCHAR(100)=null,         
 @IsDeleted BIGINT = -1,                  
 @SortExpression NVARCHAR(100),                    
 @SortType NVARCHAR(10),                  
 @FromIndex INT,                  
 @PageSize INT                  
AS                  
-- Kundan: 17-04-2020        
-- Now binded region with DDMaster instead Regions table        
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
   WHEN @SortExpression = 'AgencyType' THEN AgencyType        
      WHEN @SortExpression = 'NickName' THEN NickName        
      WHEN @SortExpression = 'ShortName' THEN ShortName                  
      WHEN @SortExpression = 'RegionName' THEN R.Title                  
      WHEN @SortExpression = 'ContactName' THEN ContactName                
      WHEN @SortExpression = 'Email' THEN A.Email                  
      WHEN @SortExpression = 'Phone' THEN A.Email                  
      WHEN @SortExpression = 'Address' THEN A.Address          
   WHEN @SortExpression = 'TIN' THEN A.TIN            
   WHEN @SortExpression = 'EIN' THEN A.EIN            
   WHEN @SortExpression = 'Mobile' THEN A.Mobile                 
      END                   
    END ASC,                  
    CASE WHEN @SortType = 'DESC' THEN                  
      CASE                         
      WHEN @SortExpression = 'AgencyType' THEN AgencyType        
      WHEN @SortExpression = 'NickName' THEN NickName                 
      WHEN @SortExpression = 'ShortName' THEN ShortName                  
      WHEN @SortExpression = 'RegionName' THEN R.Title                  
      WHEN @SortExpression = 'ContactName' THEN ContactName                
      WHEN @SortExpression = 'Email' THEN A.Email                  
      WHEN @SortExpression = 'Phone' THEN A.Email                  
      WHEN @SortExpression = 'Address' THEN A.Address         
   WHEN @SortExpression = 'TIN' THEN A.TIN           
   WHEN @SortExpression = 'EIN' THEN A.EIN            
   WHEN @SortExpression = 'Mobile' THEN A.Mobile                              
      END                  
    END DESC                  
  ) AS Row, 
      A.AgencyID,AgencyType,TIN,EIN,Mobile,NickName,ShortName,R.Title AS RegionName, --R.RegionName  
  A.RegionID,ContactName,A.Email,Phone,Address,A.Fax,City,StateCode,ZipCode,A.IsDeleted                
   FROM   Agencies A 
     left join DDMaster R on R.DDMasterID=A.RegionID 
	               
WHERE((CAST(@IsDeleted AS BIGINT)=-1) OR A.IsDeleted=@IsDeleted)                  
AND ((@AgencyType IS NULL OR LEN(@AgencyType)=0) OR A.AgencyType LIKE '%' + @AgencyType+ '%')                  
    AND ((@NickName IS NULL OR LEN(@NickName)=0) OR A.NickName LIKE '%' + @NickName+ '%')                  
     AND ((@ShortName IS NULL OR LEN(@ShortName)=0) OR A.ShortName LIKE '%' + @ShortName+ '%')                  
     AND (( CAST(@RegionID AS BIGINT)=0) OR R.DDMasterID = CAST(@RegionID AS BIGINT))                
     AND ((@ContactName IS NULL OR LEN(@ContactName)=0) OR A.ContactName LIKE '%' + @ContactName + '%')                   
  AND ((@Email IS NULL OR LEN(@Email)=0) OR A.Email LIKE '%' + @Email + '%')                  
     AND ((@Phone IS NULL OR LEN(@Phone)=0) OR A.Phone LIKE '%' + @Phone + '%')           
  AND ((@TIN IS NULL OR LEN(@TIN)=0) OR A.TIN LIKE '%' + @TIN + '%')         
  AND ((@EIN IS NULL OR LEN(@EIN)=0) OR A.EIN LIKE '%' + @EIN + '%')                  
  AND ((@Mobile IS NULL OR LEN(@Mobile)=0) OR A.Mobile LIKE '%' + @Mobile + '%')                  
     AND ((@Address IS NULL OR LEN(@Address)=0) OR A.Address LIKE '%' + @Address + '%')          
                       
  ) AS T1  )          --  order by NickName ASC         
 SELECT * FROM CTECaseManagerList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                   
         
END