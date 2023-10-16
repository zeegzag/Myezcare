      
CREATE Procedure [dbo].[CheckForDuplicateVisitQuestionMapping]        
@VisitQuestionID BIGINT,      
@VisitQuestionText NVARCHAR(1000),  
@VisitQuestionType NVARCHAR(100)      
--@EffectiveFrom date,      
--@EffectiveTo date       
as          
select CountValue=Count(*) from VisitQuestions      
where VisitQuestionText=@VisitQuestionText AND VisitQuestionType=@VisitQuestionType AND VisitQuestionID != @VisitQuestionID      
--and      
--(      
--    (@EffectiveFrom >= EffectiveFrom AND @EffectiveFrom <= EffectiveTo)       
-- OR (@EffectiveTo >= EffectiveFrom AND @EffectiveTo <= EffectiveTo)       
-- OR (@EffectiveFrom < EffectiveFrom AND @EffectiveTo > EffectiveTo)       
--) 