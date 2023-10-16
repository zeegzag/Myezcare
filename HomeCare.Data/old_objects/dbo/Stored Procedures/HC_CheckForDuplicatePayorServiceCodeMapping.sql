-- EXEC [HC_CheckForDuplicatePayorServiceCodeMapping] 0,9,null,49,9,'2018-08-01','2019-08-01'        
-- EXEC HC_CheckForDuplicatePayorServiceCodeMapping @PayorServiceCodeMappingID = '0', @PayorID = '9', @ServiceCodeID = '3', @RevenuCode = '4', @StartDate = '2018/07/01', @EndDate = '2018/07/31'        
CREATE Procedure [dbo].[HC_CheckForDuplicatePayorServiceCodeMapping]                    
@PayorServiceCodeMappingID bigint=0,                  
@PayorID bigint,                  
@CareType bigint,                  
@ServiceCodeID bigint,                  
@RevenuCode bigint,                  
@StartDate date,                  
@EndDate date                   
as                      
 if(@RevenuCode = 0)          
 begin          
 set @RevenuCode = null          
 end          
          
 select CountValue=Count(*) from PayorServiceCodeMapping                  
 where PayorID=@PayorID AND ServiceCodeID=@ServiceCodeID AND CareType=@CareType       
 AND         
 (    
 ((@RevenuCode IS NULL OR LEN(@RevenuCode)=0) and (RevenueCode IS NULL OR LEN(RevenueCode)=0))      
 or         
 (@RevenuCode IS NOT NULL and RevenueCode=@RevenuCode)         
 )         
 AND PayorServiceCodeMappingID!=@PayorServiceCodeMappingID                  
 AND                  
 (                  
  (@StartDate >= PosStartDate AND @StartDate <= PosEndDate)                   
  OR (@EndDate >= PosStartDate AND @EndDate <= PosEndDate)                   
  OR (@StartDate < PosStartDate AND @EndDate > PosEndDate)                   
 )
