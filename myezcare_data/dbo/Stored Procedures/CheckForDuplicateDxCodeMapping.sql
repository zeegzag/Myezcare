
CREATE Procedure [dbo].[CheckForDuplicateDxCodeMapping]  
@DXCodeID BIGINT,
@DXCodeName VARCHAR(100)
--@EffectiveFrom date,
--@EffectiveTo date 
as    
select CountValue=Count(*) from DXCodes
where DXCodeName=@DXCodeName  AND DXCodeID != @DXCodeID
--and
--(
--    (@EffectiveFrom >= EffectiveFrom AND @EffectiveFrom <= EffectiveTo) 
--	OR (@EffectiveTo >= EffectiveFrom AND @EffectiveTo <= EffectiveTo) 
--	OR (@EffectiveFrom < EffectiveFrom AND @EffectiveTo > EffectiveTo) 
--)

