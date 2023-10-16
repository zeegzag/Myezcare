CREATE PROCEDURE [dbo].[GetTransportaLocationList]        
@TRANSPORTLOCATIONID BIGINT=0,        
@LOCATION VARCHAR(255)=null,        
@LOCATIONCODE VARCHAR(255)=null,        
@STATE NVARCHAR(100)=null,        
@ADDRESS NVARCHAR(255)=null,        
@PHONE NVARCHAR(100)=null,
@IsDeleted BIGINT = -1,       
@RegionID bigint=0,
@SORTEXPRESSION NVARCHAR(100),        
@SORTTYPE NVARCHAR(10),        
@FROMINDEX INT,        
@PAGESIZE INT          
AS        
BEGIN          
;WITH CTETRANSPORTLOCATION AS    
 (         
  SELECT *,COUNT(T1.TRANSPORTLOCATIONID) OVER() AS COUNT FROM    
  (        
   SELECT ROW_NUMBER() OVER (ORDER BY    
    
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'TransportLocationID' THEN TransportLocationID END END ASC,    
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'TransportLocationID' THEN TransportLocationID END END DESC,    
    
    CASE WHEN @SORTTYPE = 'ASC' THEN        
      CASE         
      WHEN @SORTEXPRESSION = 'LOCATION' THEN LOCATION       
      WHEN @SORTEXPRESSION = 'LOCATIONCODE' THEN LOCATIONCODE        
      WHEN @SORTEXPRESSION = 'STATE' THEN STATE        
      WHEN @SORTEXPRESSION = 'ADDRESS' THEN ADDRESS        
      WHEN @SORTEXPRESSION = 'PHONE' THEN PHONE      
	  WHEN @SORTEXPRESSION = 'RegionName' THEN RegionName      
    END         
    END ASC,        
    CASE WHEN @SORTTYPE = 'DESC' THEN        
      CASE         
      WHEN @SORTEXPRESSION = 'LOCATION' THEN LOCATION       
      WHEN @SORTEXPRESSION = 'LOCATIONCODE' THEN LOCATIONCODE        
      WHEN @SORTEXPRESSION = 'STATE' THEN STATE        
      WHEN @SORTEXPRESSION = 'ADDRESS' THEN ADDRESS        
      WHEN @SORTEXPRESSION = 'PHONE' THEN PHONE      
	  WHEN @SORTEXPRESSION = 'RegionName' THEN RegionName      
      END        
    END DESC      
    ) AS ROW,        
   T.TransportLocationID, T.LOCATION ,T.LOCATIONCODE ,T.STATE ,T.ADDRESS ,T.PHONE,t.IsDeleted,R.RegionName
   FROM  TRANSPORTLOCATIONS T        
   Left Join  Regions R ON R.RegionID=T.RegionID
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR T.ISDELETED=@IsDeleted)   and T.TransportLocationID!=110      
   AND ((@LOCATION IS NULL OR LEN(@LOCATION)=0) OR T.LOCATION LIKE '%' + @LOCATION + '%')        
   AND ((@LOCATIONCODE  IS NULL OR LEN(@LOCATIONCODE )=0) OR (T.LOCATIONCODE LIKE '%' + @LOCATIONCODE  + '%'))            
   AND  ((@STATE  IS NULL OR LEN(@STATE )=0) OR (T.STATE LIKE '%' + @STATE  + '%'))            
   AND  ((@ADDRESS  IS NULL OR LEN(@ADDRESS)=0) OR (T.STATE LIKE '%' + @ADDRESS  + '%'))            
   AND  ((@PHONE  IS NULL OR LEN(@PHONE)=0) OR (T.PHONE LIKE '%' + @PHONE  + '%'))            
   AND ((@RegionID =0) OR (T.RegionID =@RegionID))            

  ) AS T1            
 )        
 SELECT * FROM CTETRANSPORTLOCATION WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)       
      
END