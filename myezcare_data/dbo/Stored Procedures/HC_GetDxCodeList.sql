
    
CREATE PROCEDURE [dbo].[HC_GetDxCodeList]    
 @DXCodeName VARCHAR(100) = NULL,    
 @DXCodeWithoutDot VARCHAR(100) = NULL,    
 @Description VARCHAR(500)= NULL,    
 @DXCodeShortName VARCHAR(20)= NULL,    
 @EffectiveFrom DATE= NULL,    
 @EffectiveTo DATE= NULL,    
 @IsDeleted BIGINT = -1,    
 @SortExpression NVARCHAR(100),      
 @SortType NVARCHAR(10),    
 @FromIndex INT,    
 @PageSize INT    
AS    
BEGIN    
 ;WITH CTEDxCodeList AS    
 (     
  SELECT *,COUNT(T1.DXCodeID) OVER() AS Count FROM     
  (    
   SELECT ROW_NUMBER() OVER (ORDER BY     
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'DXCodeID' THEN DXCodeID END END ASC,    
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'DXCodeID' THEN DXCodeID END END DESC,    
    CASE WHEN @SortType = 'ASC' THEN    
      CASE           
      WHEN @SortExpression = 'DXCodeName' THEN DXCodeName     
      WHEN @SortExpression = 'Description' THEN DC.Description     
      WHEN @SortExpression = 'DXCodeWithoutDot' THEN DC.DXCodeWithoutDot    
      WHEN @SortExpression = 'DXCodeShortName' THEN DT.DXCodeShortName                
      END     
    END ASC,    
    CASE WHEN @SortType = 'DESC' THEN    
      CASE           
      WHEN @SortExpression = 'DXCodeName' THEN DXCodeName      
      WHEN @SortExpression = 'Description' THEN DC.Description     
      WHEN @SortExpression = 'DXCodeWithoutDot' THEN DC.DXCodeWithoutDot      
      WHEN @SortExpression = 'DXCodeShortName' THEN DT.DXCodeShortName      
      END    
    END DESC,    
    CASE WHEN @SortType = 'ASC' THEN    
      CASE           
      WHEN @SortExpression = 'EffectiveFrom' THEN EffectiveFrom     
      WHEN @SortExpression = 'EffectiveTo' THEN DC.EffectiveTo     
      WHEN @SortExpression = 'UpdatedDate' THEN DC.UpdatedDate          
      END     
    END ASC,    
    CASE WHEN @SortType = 'DESC' THEN    
      CASE           
      WHEN @SortExpression = 'EffectiveFrom' THEN EffectiveFrom      
      WHEN @SortExpression = 'EffectiveTo' THEN DC.EffectiveTo    
      WHEN @SortExpression = 'UpdatedDate' THEN DC.UpdatedDate         
      END    
    END DESC    
        
    
  ) AS Row,    
   DC.DXCodeID,DC.DXCodeName,DC.Description,DC.EffectiveFrom,DC.EffectiveTo,DC.IsDeleted,DC.DXCodeWithoutDot,    
   CASE    
    WHEN  DC.EffectiveFrom < CAST(GETUTCDATE() AS DATE) AND DC.EffectiveTo < CAST(GETUTCDATE() AS DATE) THEN 1    
    ELSE  0    
   END AS IsDxCodeExpired,DT.DxCodeShortName    
    
   FROM  DXCodes DC     
   inner join DxCodeTypes DT on DT.DxCodeTypeID=DC.DxCodeType    
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR DC.IsDeleted=@IsDeleted)    
   AND ((@DXCodeName IS NULL OR LEN(@DXCodeName)=0) OR ((DC.DXCodeName LIKE '%' + @DXCodeName + '%') OR (DC.DXCodeWithoutDot LIKE '%' + @DXCodeName + '%')))     
   AND ((@DXCodeWithoutDot IS NULL OR LEN(@DXCodeWithoutDot)=0) OR (DC.DXCodeWithoutDot LIKE '%' + @DXCodeWithoutDot + '%'))     
   AND ((@DXCodeShortName IS NULL OR LEN(@DXCodeShortName)=0) OR (DT.DXCodeShortName LIKE '%' + @DXCodeShortName + '%'))     
   AND ((@Description IS NULL OR LEN(@Description)=0) OR (DC.Description LIKE '%' + @Description + '%'))          
   AND    
   ((     
     (@EffectiveFrom IS NULL AND @EffectiveTo IS NULL)    
     OR     
    (    
     @EffectiveTo IS NULL AND (@EffectiveFrom <= DC.EffectiveFrom )    
    )    
    OR     
    (    
     @EffectiveFrom IS NULL AND @EffectiveTo >= DC.EffectiveTo    
    )    
    OR    
    (    
      @EffectiveFrom <= DC.EffectiveFrom AND @EffectiveTo >= DC.EffectiveTo        
    )     
   ))    
  ) AS T1      
 )    
     
 SELECT * FROM CTEDxCodeList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)     
END    

