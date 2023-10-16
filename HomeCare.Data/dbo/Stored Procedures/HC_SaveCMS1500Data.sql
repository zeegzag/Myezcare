
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

--exec HC_SaveCMS1500Data 17,1,'123',atens,5,'uuuuu',5,'123',12,'19'

CREATE PROCEDURE [dbo].[HC_SaveCMS1500Data] 
@BatchID as bigint,

---- 1
@PayorID as bigint,
@PayorIdentificationNumber as nvarchar(100),
@PayorName as nvarchar(100),
@ReferralID as bigint,
@AHCCCSID as nvarchar(100),
-----3
@PatientDOB as nvarchar(100),
-----5
@ContactID as bigint,
@PatientAddress as nvarchar(max),
@PatientCity as nvarchar(100),
@PatientState as nvarchar(100),
@PatientZipCode as nvarchar(100),
---- 17a
@PhysicianID as bigint,
@Ref_NPI as nvarchar(100),
-----23
@ReferralBillingAuthorizationID as bigint,
@AuthorizationCode as nvarchar(100),
-----24
@ServiceDate as nvarchar(100),
@PlaceOfServiceID as int,
@PlaceOfService as nvarchar(100),
@ModifierID1 as int,
@ModifierCode1 as nvarchar(100),
@ModifierID2 as int,
@ModifierCode2 as nvarchar(100),
@ModifierID3 as int,
@ModifierCode3 as nvarchar(100),
@ModifierID4 as int,
@ModifierCode4 as nvarchar(100),

--- 33a
@NoteID as bigint,
@BillingProviderNPI as nvarchar(100),
--- 25
@BillingProviderEIN	as nvarchar(100)
AS
BEGIN
	---------1
	IF(dbo.IsNullOrEmpty(@PayorName) = 0 and @PayorID > 0)
	BEGIN 
	UPDATE [Payors] set PayorName=@PayorName WHERE PayorID=@PayorName
	END
	IF(dbo.IsNullOrEmpty(@PayorIdentificationNumber) = 0 and @PayorID > 0)
	BEGIN 
	UPDATE [Payors] set PayorIdentificationNumber=@PayorIdentificationNumber WHERE PayorID=@PayorID
	UPDATE [Notes] set PayorIdentificationNumber=@PayorIdentificationNumber WHERE NoteID=@NoteID
	END
	---------1a
	IF(dbo.IsNullOrEmpty(@AHCCCSID) = 0 and @ReferralID > 0)
	BEGIN 
	UPDATE Referrals set AHCCCSID=@AHCCCSID WHERE ReferralID=@ReferralID
	UPDATE [Notes] set [AHCCCSID]=@AHCCCSID WHERE NoteID=@NoteID
	END
	---------3
	IF(dbo.IsNullOrEmpty(@PatientDOB) = 0 and @ReferralID > 0)
	BEGIN 
	UPDATE Referrals set Dob=@PatientDOB WHERE ReferralID=@ReferralID
	END
	---------5
	IF(@ContactID > 0)
	BEGIN 
	UPDATE [Contacts] set [Address]=@PatientAddress,[City]=@PatientCity,[State]=@PatientState,[ZipCode]=@PatientZipCode WHERE ContactID=@ContactID
	END
	---------17a
	IF(dbo.IsNullOrEmpty(@Ref_NPI) = 0 and @PhysicianID > 0)
	BEGIN 
	UPDATE [Physicians] set NPINumber=@Ref_NPI WHERE PhysicianID=@PhysicianID
	END
	--------23
	IF(dbo.IsNullOrEmpty(@AuthorizationCode) = 0 and @ReferralBillingAuthorizationID > 0)
	BEGIN 
	UPDATE ReferralBillingAuthorizations set AuthorizationCode=@AuthorizationCode WHERE ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID
	END	

	-------24
	IF(dbo.IsNullOrEmpty(@ServiceDate) = 0 and @NoteID > 0)
	BEGIN 
	UPDATE Notes set ServiceDate=@ServiceDate WHERE NoteID=@NoteID
	END

	IF(dbo.IsNullOrEmpty(@PlaceOfService) = 0 and @PlaceOfServiceID > 0)
	BEGIN 
	UPDATE [DDMaster] set [Value]=@PlaceOfService WHERE DDMasterID=@PlaceOfServiceID
	END

	IF(dbo.IsNullOrEmpty(@ModifierCode1) = 0 and @ModifierID1 > 0)
	BEGIN 
	UPDATE [Modifiers] set ModifierCode=@ModifierCode1 WHERE [ModifierID]=@ModifierID1
	END
	IF(dbo.IsNullOrEmpty(@ModifierCode2) = 0 and @ModifierID2 > 0)
	BEGIN 
	UPDATE [Modifiers] set ModifierCode=@ModifierCode2 WHERE [ModifierID]=@ModifierID2
	END
	IF(dbo.IsNullOrEmpty(@ModifierCode3) = 0 and @ModifierID3 > 0)
	BEGIN 
	UPDATE [Modifiers] set ModifierCode=@ModifierCode3 WHERE [ModifierID]=@ModifierID3
	END
	IF(dbo.IsNullOrEmpty(@ModifierCode4) = 0 and @ModifierID4 > 0)
	BEGIN 
	UPDATE [Modifiers] set ModifierCode=@ModifierCode4 WHERE [ModifierID]=@ModifierID4
	END
	------25
	IF(dbo.IsNullOrEmpty(@BillingProviderEIN) = 0 and @NoteID > 0)
	BEGIN 
	UPDATE [Notes] set BillingProviderEIN=@BillingProviderEIN WHERE NoteID=@NoteID
	END
	------33a
	IF(dbo.IsNullOrEmpty(@BillingProviderNPI) = 0 and @NoteID > 0)
	BEGIN 
	UPDATE [Notes] set BillingProviderNPI=@BillingProviderNPI WHERE NoteID=@NoteID
	END

END