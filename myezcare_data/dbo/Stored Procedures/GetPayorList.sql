-- exec [GetPayorList] '','','','',-1,-1,'PayorID','ASC',1,50   
CREATE PROCEDURE [dbo].[GetPayorList]        
@PayorName VARCHAR(255)=null,                  
@ShortName VARCHAR(255)=null,  
@Address NVARCHAR(100)=null,          
@AgencyNPID NVARCHAR(20)=null,                  
@PayerGroup bigint=-1,   
@IsDeleted BIGINT = -1,                 
@SORTEXPRESSION NVARCHAR(100),          
@SORTTYPE NVARCHAR(10),        
@FROMINDEX INT,        
@PAGESIZE INT         
AS                  
BEGIN                    
;WITH CTEPayor AS              
 (                   
  SELECT *,COUNT(PayorID) OVER() AS COUNT FROM              
  (                  
   SELECT ROW_NUMBER() OVER (ORDER BY              
              
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PayorID' THEN PayorID END END ASC,              
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PayorID' THEN PayorID END END DESC,              
          
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PayorName' THEN PayorName END END ASC,              
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PayorName' THEN PayorName END END DESC,       
      
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ShortName' THEN ShortName END END ASC,              
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ShortName' THEN ShortName END END DESC,       
      
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Address' THEN Address END END ASC,              
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Address' THEN Address END END DESC,       
      
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AgencyNPID' THEN AgencyNPID END END ASC,              
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AgencyNPID' THEN AgencyNPID END END DESC,       
      
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PayerGroup' THEN PayerGroup END END ASC,              
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PayerGroup' THEN PayerGroup END END DESC      
      
 ) AS ROW,                  
 PayorID,PayorName,ShortName,AgencyNPID,PayerGroup,  
 CONCAT(           
   case when (Address is not null and Address != '') then LTRIM(RTRIM(Address))+' ' else '' end,          
   case when (City is not null and City != '') then LTRIM(RTRIM(City))+', ' else '' end,          
   case when (StateCode is not null and StateCode != '') then LTRIM(RTRIM(StateCode))+' ' else '' end,          
   case when (ZipCode is not null and ZipCode != '') then LTRIM(RTRIM(ZipCode)) else '' end          
 ) As Address ,  
 --Address,City,StateCode,ZipCode,  
 IsDeleted                    
 FROM  Payors           
 --LEFT JOIN PayorTypes PT on PT.PayorTypeID=P.PayorTypeID        
   WHERE ((@IsDeleted = -1) OR IsDeleted=@IsDeleted)       
   AND ((@PayorName IS NULL OR LEN(@PayorName)=0) OR PayorName LIKE '%' + @PayorName+ '%')                  
   AND ((@ShortName  IS NULL OR LEN(@ShortName )=0) OR (ShortName LIKE '%' + @ShortName + '%'))                      
   AND ((@AgencyNPID IS NULL OR LEN(@AgencyNPID)=0) OR (AgencyNPID LIKE '%' + @AgencyNPID+ '%'))  
   AND   
   (  
   ((@Address  IS NULL OR LEN(@Address)=0) OR (Address LIKE '%' + @Address+ '%'))                      
   OR  ((@Address  IS NULL OR LEN(@Address)=0) OR (City LIKE '%' + @Address+ '%'))                         
   OR  ((@Address  IS NULL OR LEN(@Address)=0) OR (StateCode LIKE '%' + @Address+ '%'))                    
   OR  ((@Address  IS NULL OR LEN(@Address)=0) OR (ZipCode LIKE '%' + @Address+ '%'))  
   )   
   AND ((@PayerGroup IS NULL OR @PayerGroup=-1) OR (PayerGroup =@PayerGroup))                      
  ) AS P1        
 )                  
 SELECT * FROM CTEPayor WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)                 
                
END
