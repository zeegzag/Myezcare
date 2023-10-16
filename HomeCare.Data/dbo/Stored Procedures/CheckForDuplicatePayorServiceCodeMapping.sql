
CREATE Procedure [dbo].[CheckForDuplicatePayorServiceCodeMapping]  
@PayorServiceCodeMappingID bigint=0,
@PayorID bigint,
@ServiceCodeID bigint,
@PosID bigint,
@StartDate date,
@EndDate date 
as    
select CountValue=Count(*) from PayorServiceCodeMapping
where PayorID=@PayorID AND ServiceCodeID=@ServiceCodeID AND PosID=@PosID AND PayorServiceCodeMappingID!=@PayorServiceCodeMappingID
and
(
    (@StartDate >= PosStartDate AND @StartDate <= PosEndDate) 
	OR (@EndDate >= PosStartDate AND @EndDate <= PosEndDate) 
	OR (@StartDate < PosStartDate AND @EndDate > PosEndDate) 
)