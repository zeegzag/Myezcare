--EXEC HC_GetPayorServiceCodeMappingList @ServiceCode = '', @PayorID = '1', @IsDeleted  = '0', @ModifierID = '2,3,4', @CareType = '-1', @RevenueCode = '0', @Rate = '', @SortExpression = 'PayorServiceCodeMappingID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'            
CREATE PROCEDURE [dbo].[HC_GetPayorServiceCodeMappingList]          
 @PayorID bigint=null,                                                   
 @ServiceCode varchar(30)=null,                                                
 @ModifierID VARCHAR(100)=NULL,            
 @POSStartDate date=null,                                                        
 @POSEndDate date=null,                                              
 @IsDeleted BIGINT = -1,                                  
 @CareType int = -1,                                 
 @RevenueCode BIGINT=-1,                                
 @Rate varchar(10),              
 @SORTEXPRESSION VARCHAR(100),                                                          
 @SORTTYPE VARCHAR(10),                                                        
 @FROMINDEX INT,                                                        
 @PAGESIZE INT,          
 @UnitType INT = NULL,          
 @PerUnitQuantity DECIMAL(18,0) = NULL,          
 @RoundUpUnit INT = NULL,          
 @MaxUnit INT = NULL,          
 @DailyUnitLimit INT = NULL          
AS                                                                  
BEGIN                                                                    
;WITH CTEPayor AS                                                              
 (                                                                   
  SELECT ROW_NUMBER() OVER (ORDER BY                                                              
                                                              
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CareType' THEN CareType END END ASC,                                                              
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CareType' THEN CareType END END DESC,                                                              
                                              
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ServiceCode' THEN ServiceCode END END ASC,                                                
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ServiceCode' THEN ServiceCode END END DESC,                                         
                                      
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ModifierName' THEN ModifierName END END ASC,                                                
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ModifierName' THEN ModifierName END END DESC,                                                               
                                          
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'RevenueCode' THEN RevenueCode  END END ASC,                                                              
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'RevenueCode' THEN RevenueCode  END END DESC,                                                              
                                  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Rate' THEN CAST(Rate AS decimal)  END END ASC,                                                              
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Rate' THEN CAST(Rate AS decimal)  END END DESC,                                                              
                                 
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'POSStartDate' THEN  CONVERT(date, POSStartDate, 105)     END END ASC,                                                              
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'POSStartDate' THEN  CONVERT(date, POSStartDate, 105)     END END DESC,      
                                          
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'UnitType' THEN UnitType END END ASC,                                 
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'UnitType' THEN UnitType END END DESC,          
          
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PerUnitQuantity' THEN PerUnitQuantity END END ASC,                                                              
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PerUnitQuantity' THEN PerUnitQuantity END END DESC,                
           
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'RoundUpMinutes' THEN RoundUpUnit END END ASC,                                                              
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'RoundUpMinutes' THEN RoundUpUnit END END DESC,                
           
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'MaxUnit' THEN MaxUnit END END ASC,                                                              
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'MaxUnit' THEN MaxUnit END END DESC,                
           
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'DailyUnitLimit' THEN DailyUnitLimit END END ASC,                                                              
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'DailyUnitLimit' THEN DailyUnitLimit END END DESC                                                             
                                   
    ) AS ROW,*,COUNT(P1.PayorServiceCodeMappingID) OVER() AS COUNT FROM                                                           
  (                                                                  
 SELECT PSM.PayorServiceCodeMappingID,SC.ServiceCodeID,P.PayorID,P.PayorName,                                
 --case SC.CareType when 1 then 'PCA' else 'Respite' end as CareType,                                
 DM.Title as CareType,DM.DDMasterID AS CareTypeID,                                
 SC.ServiceCode,SC.ServiceName,SC.Description,                                
 PSM.MaxUnit,PSM.DailyUnitLimit,PSM.UnitType,                                
 PSM.PerUnitQuantity,SC.IsBillable,PSM.RoundUpUnit,                                
 SC.ModifierID,--M.ModifierCode,                          
 STUFF(              
         (SELECT ', ' + convert(varchar(100), M.ModifierCode, 120)              
          FROM Modifiers M              
          where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID)) AND M.IsDeleted=0        
          FOR XML PATH (''))              
          , 1, 1, '')  AS ModifierName,                             
 --PSM.UPCRate,PSM.NegRate,                            
 PSM.Rate,                            
 PSM.POSStartDate,PSM.POSEndDate,PSM.IsDeleted,DM1.Title as RevenueCode,DM1.DDMasterID as RevenueCodeID,                                
 ServiceCodeMappingExpired=Case When (GetDate()-1  > PSM.POSEndDate) then 1 else 0 end                                           
                                
 FROM  PayorServiceCodeMapping PSM                                                        
 left Join ServiceCodes SC on PSM.ServiceCodeID=SC.ServiceCodeID                                                  
 left Join Payors P on P.PayorID=PSM.PayorID                                                         
 --left Join Modifiers M on m.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))            
 LEFT JOIN DDmaster DM on DM.DDMasterID = PSM.CareType                       
 LEFT JOIN DDmaster DM1 on DM1.DDMasterID = PSM.RevenueCode                                            
 WHERE                                
 (@PayorID =(case when @PayorID=0 then @PayorID else PSM.PayorID  end )) AND                                           
 ((@IsDeleted=-1) OR (PSM.IsDeleted=@IsDeleted)) AND                                 
 ((@CareType = -1) OR PSM.CareType = @CareType)  AND                                 
 ((@ServiceCode IS NULL) OR (SC.ServiceCode LIKE '%' + @ServiceCode+ '%')) AND                                
 ((@ModifierID IS NULL OR LEN(@ModifierID)=0) OR SC.ModifierID = @ModifierID) AND            
 ((@RevenueCode =0) or PSM.RevenueCode= @RevenueCode ) AND                                
 ((@Rate IS NULL OR LEN(@Rate)=0 or @Rate = 0) OR PSM.Rate LIKE '%' + @Rate+ '%')  AND                                   
 --((@NegRate IS NULL OR LEN(@NegRate)=0 or @NegRate = 0) OR PSM.NegRate LIKE '%' + @NegRate+ '%')  AND           
 --((@POSStartDate is null OR PSM.POSStartDate >= @POSStartDate) and (@POSEndDate is null OR PSM.POSEndDate<= @POSEndDate))              
  ((@POSStartDate is null and @POSEndDate is null) or ((@POSStartDate is null and @POSEndDate is not null) and @POSEndDate=POSEndDate) or ((@POSEndDate is null and @POSStartDate is not null) and @POSStartDate=POSStartDate)              
or((@POSEndDate is not null and @POSStartDate is not null) and PSM.POSStartDate >= @POSStartDate and PSM.POSEndDate<= @POSEndDate))              
    AND ((@UnitType  IS NULL OR @UnitType=0 OR LEN(@UnitType )=0) OR PSM.UnitType = @UnitType)          
 AND ((@PerUnitQuantity  IS NULL OR @PerUnitQuantity=0 OR LEN(@PerUnitQuantity )=0) OR PSM.PerUnitQuantity = @PerUnitQuantity)          
 AND ((@RoundUpUnit  IS NULL OR @RoundUpUnit=0 OR LEN(@RoundUpUnit )=0) OR PSM.RoundUpUnit = @RoundUpUnit)          
 AND ((@MaxUnit  IS NULL OR @UnitType=0 OR LEN(@MaxUnit )=0) OR PSM.MaxUnit = @MaxUnit)          
 AND ((@DailyUnitLimit  IS NULL OR @DailyUnitLimit=0 OR LEN(@DailyUnitLimit )=0) OR PSM.DailyUnitLimit = @DailyUnitLimit)          
   ) AS P1                                          
 )                                            
 SELECT * FROM CTEPayor WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)                
END
