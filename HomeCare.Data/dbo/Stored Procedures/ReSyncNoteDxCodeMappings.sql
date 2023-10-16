-- EXEC SetAddNotePage @ReferralID = '105', @NoteID = '3', @PrimaryContactTypeID = '1', @LoggedInUserID = '97'      
CREATE PROCEDURE [dbo].[ReSyncNoteDxCodeMappings]      
@ReferralID bigint,
@NoteID bigint,
@ReferralDXCodeMappingID varchar(100)
AS      
BEGIN      
	delete from NoteDXCodeMappings
	where ReferralID=@ReferralID and NoteID=@NoteID
	and ReferralDXCodeMappingID not in(SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ReferralDXCodeMappingID))

	insert into NoteDXCodeMappings(ReferralDXCodeMappingID,ReferralID,NoteID,DXCodeID,DXCodeName,Precedence,StartDate,EndDate,Description,DXCodeWithoutDot,DxCodeType,DxCodeShortName)
		select RD.ReferralDXCodeMappingID,RD.ReferralID,@NoteID,RD.DXCodeID,D.DXCodeName,RD.Precedence,RD.StartDate,RD.EndDate,D.Description,d.DXCodeWithoutDot,D.DxCodeType,DT.DxCodeShortName
		from ReferralDXCodeMappings RD
		inner join DXCodes d on d.DXCodeID=RD.DXCodeID
		inner join DxCodeTypes DT on DT.DxCodeTypeID=D.DxCodeType
		where RD.ReferralID=@ReferralID and RD.IsDeleted=0
		and RD.ReferralDXCodeMappingID in(select T.Val from (SELECT CAST(Val AS BIGINT) As Val FROM GETCSVTABLE(@ReferralDXCodeMappingID)) as T where T.Val not in( select ReferralDXCodeMappingID from NoteDXCodeMappings where NoteID=@NoteID and ReferralID=@ReferralID))      

	UPDATE ND
	SET DXCodeName=d.DXCodeName,Precedence=RD.Precedence,StartDate=RD.StartDate,EndDate=rd.EndDate,Description=d.Description,DXCodeWithoutDot=d.DXCodeWithoutDot,DxCodeType=D.DxCodeType,DxCodeShortName=DT.DxCodeShortName
	from NoteDXCodeMappings ND
	inner join ReferralDXCodeMappings RD ON RD.ReferralDXCodeMappingID=ND.ReferralDXCodeMappingID
	INNER JOIN DxCodes d on d.DXCodeID=ND.DXCodeID
	INNER JOIN DxCodeTypes DT on DT.DxCodeTypeID=D.DxCodeType
	where RD.ReferralID=@ReferralID and RD.IsDeleted=0
	and RD.ReferralDXCodeMappingID in(select ReferralDXCodeMappingID from NoteDXCodeMappings where NoteID=@NoteID and ReferralID=@ReferralID)      
	
END