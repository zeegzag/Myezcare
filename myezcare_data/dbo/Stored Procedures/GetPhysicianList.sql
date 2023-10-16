
--EXEC GetPhysicianList @PhysicianName = 'jiten', @IsDeleted = '0', @SortExpression = 'PhysicianID', @SortType = 'DESC', @FromIndex = '1', @PageSize = '10'    
CREATE PROCEDURE [dbo].[GetPhysicianList]      
@PhysicianName NVARCHAR(100) = NULL,        
@Email NVARCHAR(50) = NULL,          
@Address NVARCHAR(100) = NULL,   
@NPINumber NVARCHAR(20) = null,     
@IsDeleted BIGINT = -1,          
@SortExpression NVARCHAR(100),                      
@SortType NVARCHAR(10),                    
@FromIndex INT,                    
@PageSize INT          
AS                  
BEGIN                  
 ;WITH CTEPhysicianList AS                        
 (                         
  SELECT *,COUNT(T1.PhysicianID) OVER() AS Count FROM                         
  (                        
   SELECT ROW_NUMBER() OVER (ORDER BY               
                        
    CASE WHEN @SortType = 'ASC' THEN                        
      CASE                               
   WHEN @SortExpression = 'PhysicianName' THEN LastName          
   WHEN @SortExpression = 'Address' THEN Email             
   WHEN @SortExpression = 'Email' THEN Email      
   WHEN @SortExpression = 'NPINumber' THEN NPINumber      
      END                         
    END ASC,                        
    CASE WHEN @SortType = 'DESC' THEN                        
      CASE                               
   WHEN @SortExpression = 'PhysicianName' THEN LastName          
   WHEN @SortExpression = 'Address' THEN Address             
   WHEN @SortExpression = 'Email' THEN Email      
   WHEN @SortExpression = 'NPINumber' THEN NPINumber      
      END                        
    END DESC                        
                            
                        
  ) AS Row,PhysicianID,PhysicianName=dbo.GetGeneralNameFormat(FirstName,LastName),Address,City,ZipCode,StateCode,Email,IsDeleted,NPINumber    
   FROM  Physicians P      
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR P.IsDeleted=@IsDeleted)                        
   AND ((@PhysicianName IS NULL OR LEN(@PhysicianName)=0)             
   OR (            
       (P.FirstName LIKE '%'+@PhysicianName+'%' )OR              
    (P.LastName  LIKE '%'+@PhysicianName+'%') OR              
    (P.FirstName +' '+P.LastName like '%'+@PhysicianName+'%') OR              
    (P.LastName +' '+P.FirstName like '%'+@PhysicianName+'%') OR              
    (P.FirstName +', '+P.LastName like '%'+@PhysicianName+'%') OR              
    (P.LastName +', '+P.FirstName like '%'+@PhysicianName+'%')))       
 AND ((@Email IS NULL OR LEN(@Email)=0) OR P.Email LIKE '%' + @Email + '%')      
 AND ((@Address IS NULL OR LEN(@Address)=0) OR          
        (P.Address LIKE '%' + @Address+ '%')  OR          
  (P.City LIKE '%' + @Address+ '%')  OR          
  (P.ZipCode LIKE '%' + @Address+ '%')  OR          
  (P.StateCode LIKE '%' + @Address+ '%')) AND  
   ((@NPINumber IS NULL OR LEN(@NPINumber)=0) OR P.NPINumber LIKE '%' + @NPINumber + '%')              
  ) AS T1        
 )                        
                         
 SELECT * FROM CTEPhysicianList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                      
END

