-- EXEC SaveNote @ReferralID = '87', @NoteDetail = 'sdacv saev', @ServiceCodeTypeID = '3', @Relation = 'Parent', @KindOfNote = 'Other', @LoggedInUserID = '97',@SystemID='192.168.1.103',@Source='Email'
CREATE PROCEDURE [dbo].[SaveNote]	
@ReferralID BIGINT,
@NoteDetail varchar(MAX),
@ServiceCodeTypeID INT,
@SpokeTo VARCHAR(100)=null,
@Relation varchar(50),
@KindOfNote varchar(20),
@LoggedInUserID BIGINT,
@SystemID varchar(30),
@Source varchar(MAX),
@AttachmentURL varchar(MAX)=null,
@MonthlySummaryIds varchar(MAX)=null

AS
BEGIN

   IF(@LoggedInUserID=0)
     SET @LoggedInUserID=1


	DECLARE @CurrentDateTime datetime = GETUTCDATE()    
	DECLARE @CurrentDate date = CONVERT(DATE, getdate())	
	DECLARE @CISNumber varchar(30)
	DECLARE @AHCCCSID varchar(30)
	DECLARE @ZarephathService varchar(20) 	
	
	SELECT @CISNumber=CISNumber,@AHCCCSID = AHCCCSID,
	@ZarephathService=CASE WHEN R.RespiteService=1 THEN 'Respite' 
	                       WHEN R.LifeSkillsService=1 then 'Life Skills'
						   WHEN R.CounselingService=1 then 'Counselling'
						   WHEN R.ConnectingFamiliesService=1 then 'Connecting Families' 
						   ELSE ''  END 
    FROM Referrals R Where R.ReferralID = @ReferralID	

	INSERT INTO Notes(ReferralID,ServiceDate,StartTime,EndTime,ServiceCodeType,SpokeTo,Relation,OtherNoteType,CISNumber,AHCCCSID,NoteDetails,
	Source,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,ZarephathService,AttachmentURL,MonthlySummaryIds,MarkAsComplete,SignatureDate)
	VALUES(@ReferralID,@CurrentDate,@CurrentDate,@CurrentDate,@ServiceCodeTypeID,@SpokeTo,@Relation,@KindOfNote,@CISNumber,@AHCCCSID,@NoteDetail,
	@Source,@LoggedInUserID,@CurrentDateTime,@LoggedInUserID,@CurrentDateTime,@SystemID,@ZarephathService,@AttachmentURL,@MonthlySummaryIds,1,@CurrentDateTime)
				
   

    DECLARE @Signature varchar(MAX)
	DECLARE @SignatureID BIGINT
	DECLARE @SignatureByName varchar(MAX)
	DECLARE @MacAddress varchar(MAX)

	SELECT @MacAddress=net_address from sysprocesses where spid = @@SPID

    SELECT TOP 1 @Signature=ES.SignaturePath,@SignatureID=E.EmployeeSignatureID,@SignatureByName=E.LastName+', '+E.FirstName FROM Employees E
	LEFT JOIN EmployeeSignatures ES ON ES.EmployeeSignatureID=E.EmployeeSignatureID
    
	IF(LEN(@Signature)>0 AND @SignatureID>0 AND LEN(@SignatureByName)>0 AND LEN(@MacAddress)>0) 
	BEGIN
		INSERT INTO SignatureLogs(NoteID,Signature,EmployeeSignatureID,SignatureBy,Name,Date,MacAddress,SystemID,IsActive)
		VALUES(@@IDENTITY,@Signature,@SignatureID,@LoggedInUserID,@SignatureByName,@CurrentDateTime,@MacAddress,@SystemID,1)
    END



	EXEC UpdateFirstDos @ReferralID


END