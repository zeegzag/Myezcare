CREATE PROCEDURE [dbo].[SetAddAssessmentQuestionPage]
@AssessmentQuestionID BIGINT,
@DDType_AssessmentQuestionCategory INT,
@DDType_AssessmentQuestionSubCategory INT
AS
BEGIN
	SELECT * FROM AssessmentQuestions WHERE AssessmentQuestionID=@AssessmentQuestionID
	SELECT * FROM AssessmentOptions WHERE AssessmentQuestionID=@AssessmentQuestionID
	SELECT Value=DDMasterID,Name=Title FROM DDMaster WHERE IsDeleted=0 and ItemType=@DDType_AssessmentQuestionCategory
	SELECT Value=DDMasterID,Name=Title FROM DDMaster WHERE IsDeleted=0 and ItemType=@DDType_AssessmentQuestionSubCategory
END
