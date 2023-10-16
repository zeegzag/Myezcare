CREATE PROCEDURE [dbo].[SetAddNotePage]      
@ReferralID bigint,      
@NoteID bigint=0,      
@PrimaryContactTypeID bigint,      
@LoggedInUserID bigint      
AS      
BEGIN      
-- Get client info --      
 select p.PayorID,p.PayorName,PayorShortName=p.ShortName ,r.LastName+', '+r.FirstName as ClientName,r.AHCCCSID,r.CISNumber,r.Population,r.Title,r.Gender,r.Dob,dbo.GetAge(r.Dob) as Age,       
 e.FirstName+' '+e.LastName as Empl,ro.RoleName,r.PermissionForVoiceMail,r.PermissionForEmail,r.PermissionForSMS,      
 c.Address,c.City,c.State,c.ZipCode,c.Phone1,rru.UsedRespiteHours       
 FROM Referrals r      
 LEFT join ContactMappings cm on cm.ReferralID=r.ReferralID and cm.ContactTypeID=@PrimaryContactTypeID      
 LEFT join Contacts c on c.ContactID=cm.ContactID      
 LEFT join ReferralPayorMappings rp on rp.ReferralID=r.ReferralID and rp.IsActive=1 and rp.IsDeleted=0      
 LEFT join Payors p on p.PayorID=rp.PayorID       
 LEFT join Employees e on e.EmployeeID=r.ClientID      
 LEFT join Roles ro on ro.RoleID=e.RoleID      
 LEFT join ReferralRespiteUsageLimit rru on rru.ReferralID=r.ReferralID and rru.IsActive=1 and  rru.StartDate<=GETUTCDATE()    
     
 where r.ReferralID=@ReferralID;      
      
 -- Get Note Detail      
 if(@NoteID>0)      
 BEGIN       
  SELECT * from Notes where NoteID=@NoteID;        
 END      
 ELSE      
 BEGIN      
  SELECT 0;        
 END      
      
 -- Get Payor Service codes --      
 --select sc.*,psm.PosID,pos.PosName,psm.PayorServiceCodeMappingID from PayorServiceCodeMapping psm      
 --inner join ServiceCodes sc on sc.ServiceCodeID=psm.ServiceCodeID      
 --inner join PlaceOfServices pos on pos.PosID=psm.PosID      
 --Where psm.PayorID in(select PayorID from ReferralPayorMappings where ReferralID=@ReferralID and IsActive=1 and IsDeleted=0) and psm.IsDeleted=0;      
      
 --Get dx codes   

	 --SELECT RD.ReferralDXCodeMappingID,D.DXCodeID,D.DXCodeName,D.Description,--ND.NoteDXCodeMappingID,
	 --RD.StartDate,RD.EndDate,RD.Precedence,D.DxCodeType
	 --FROM ReferralDXCodeMappings RD      
	 --INNER JOIN DxCodes D ON D.DXCodeID=RD.DXCodeID     
	 ----left join NoteDXCodeMappings nd on ND.ReferralDXCodeMappingID=RD.ReferralDXCodeMappingID and ND.ReferralID=RD.ReferralID 
	 --WHERE RD.ReferralID = @ReferralID and RD.IsDeleted=0    
	 --order by case when RD.Precedence is null then 1 else 0 end,  RD.Precedence
	 
	SELECT RD.ReferralDXCodeMappingID,D.DXCodeID,D.DXCodeName,D.Description,ND.NoteDXCodeMappingID, CASE when ND.NoteDXCodeMappingID > 0 THEN 1 ELSE 0 END AS IsChecked,
	 RD.StartDate,RD.EndDate,RD.Precedence,D.DxCodeType,DT.DxCodeShortName,D.DXCodeWithoutDot
	 FROM ReferralDXCodeMappings RD      
	 INNER JOIN DxCodes D ON D.DXCodeID=RD.DXCodeID
	 INNER JOIN DxCodeTypes DT on DT.DxCodeTypeID=D.DxCodeType
	 left join NoteDXCodeMappings nd on ND.ReferralDxCodeMappingID=RD.ReferralDxCodeMappingID AND ND.ReferralID=RD.ReferralID AND ND.NoteID=@NoteID
	 WHERE RD.ReferralID = @ReferralID AND RD.IsDeleted=0  AND RD.Precedence=1
	 order by case when RD.Precedence is null then 1 else 0 end,  RD.Precedence  


 select * from ServiceCodeTypes      



DECLARE @SignaturePath VARCHAR(MAX);
DECLARE @SignatureName VARCHAR(MAX);
DECLARE @SignatureLogID BIGINT=0;     
DECLARE @EmployeeID  BIGINT=0;     

if(@NoteID>0)    
BEGIN
	SELECT @SignaturePath=ES.SignaturePath, @SignatureName=S.Name,@SignatureLogID=S.SignatureLogID,@EmployeeID=S.SignatureBy
	FROM SignatureLogs S
	LEFT join EmployeeSignatures ES on ES.EmployeeSignatureID=S.EmployeeSignatureID 
	where NoteID=@NoteID and S.IsActive=1;        
END

IF(@SignaturePath IS  NULL OR LEN(@SignaturePath)=0)
BEGIN
	 SELECT @SignaturePath=es.SignaturePath,@SignatureName=E.LastName+', ' +E.FirstName,@EmployeeID =e.EmployeeID
	 FROM Employees e  
	 inner join EmployeeSignatures es on es.EmployeeSignatureID=e.EmployeeSignatureID  
	 where e.EmployeeID=@LoggedInUserID 
END


SELECT Signature=@SignaturePath,SignatureName=@SignatureName,SignatureLogID=@SignatureLogID,EmployeeID=@EmployeeID;




     
 DECLARE @BillingProviderID bigint=0
 select @BillingProviderID=BillingProviderID from Notes where NoteID=@NoteID      
 DECLARE @RenderingProviderID bigint=0
 select @RenderingProviderID=RenderingProviderID from Notes where NoteID=@NoteID      
 

 Select DefaultBillingProviderID=BillingProviderID,DefaultRenderingProviderID=RenderingProviderID FROM Payors P
 LEFT join ReferralPayorMappings RP on RP.PayorID=P.PayorID
 WHERE rp.ReferralID=@ReferralID AND  RP.IsActive=1 AND RP.IsDeleted=0


 select Value=FacilityID,Name=FacilityName from Facilities WHERE IsDeleted=0 AND ParentFacilityID=0 ORDER BY FacilityName ASC
 --select Name=FacilityName, Value=FacilityID from Facilities WHERE IsDeleted=0 AND ParentFacilityID=0 ORDER BY FacilityName ASC

    
 --select F.FacilityID Value,F.FacilityName Name from FacilityApprovedPayors FAP      
 --inner join Facilities F on F.FacilityID=FAP.FacilityID and (F.IsDeleted=0 OR F.FacilityID=@BillingProviderID OR F.FacilityID=@RenderingProviderID) and ParentFacilityID=0
 --where FAP.PayorID=(select p.PayorID from Referrals R LEFT join ReferralPayorMappings rp on rp.ReferralID=r.ReferralID and rp.IsActive=1 and rp.IsDeleted=0      
 --LEFT join Payors p on p.PayorID=rp.PayorID where r.ReferralID=@ReferralID)      
 --Order BY F.FacilityName ASC


 
 if(@NoteID>0)      
 BEGIN       
  SELECT * from SignatureLogs where NoteID=@NoteID and IsActive=1;        
 END      
 ELSE      
 BEGIN      
  SELECT 0;        
 END      
      
 IF(@NoteID > 0 AND (select ServiceCodeID from Notes where NoteID=@NoteID)>0)      
	 BEGIN       
	  DECLARE @ServiceDate date ;
	  DECLARE @PayorID BIGINT;
	  
	  select @ServiceDate=ServiceDate,@PayorID=PayorID from Notes where NoteID=@NoteID      
	  DECLARE @ServiceCodeID int      
	  select @ServiceCodeID=ServiceCodeID from Notes where NoteID=@NoteID      
	  EXEC GetNotePagePlaceOfService @ReferralID=@ReferralID, @ServiceDate=@ServiceDate, @ServiceCodeID=@ServiceCodeID, @PayorID=@PayorID , @NoteID=@NoteID      
	 END      
 ELSE      
	 BEGIN      
	 select * from ServiceCodes where ServiceCodeID=0   
	 END      
      

select EmployeeID Value,LastName +', '+FirstName Name from Employees where IsActive=1 and IsDeleted=0 Order By LastName ASC



DECLARE @RandomGroupID varchar(100)      
IF(@NoteID>0)      
  SELECT @RandomGroupID=RandomGroupID from Notes where NoteID=@NoteID       

IF(@RandomGroupID IS NULL OR len(@RandomGroupID)=0)
BEGIN
	 IF(@NoteID>0)      
	     SELECT * from Notes where NoteID=@NoteID        
	 ELSE      
		 SELECT 1 where 1=2;        
END
ELSE
BEGIN
	SELECT * from Notes where RandomGroupID=(select RandomGroupID from Notes where NoteID=@NoteID AND RandomGroupID IS NOT NULL);        
END



SELECT * FROM NoteSentences WHERE IsDeleted=0 ORDER BY NoteSentenceTitle ASC






DECLARE @NotePayorID BIGINT=0;

IF(@NoteID > 0)
	BEGIN
	 SELECT @NotePayorID=ISNULL(P.PayorID,0) FROM Notes N LEFT JOIN Payors P ON P.PayorID=N.PayorID WHERE N.NoteID=@NoteID;
	
	 IF(@NotePayorID>0)
	  SELECT NotePayorID=P.PayorID,NotePayorName=P.ShortName FROM Notes N LEFT JOIN Payors P ON P.PayorID=N.PayorID WHERE N.NoteID=@NoteID;
	
	END


IF(@NoteID<=0 OR @NotePayorID<=0)
	BEGIN
	 SELECT NotePayorID=P.PayorID,NotePayorName=P.ShortName FROM Referrals R
	 INNER JOIN ReferralPayorMappings RP on RP.ReferralID=R.ReferralID and RP.IsActive=1 and RP.IsDeleted=0      
	 INNER JOIN Payors P ON P.PayorID=RP.PayorID WHERE R.ReferralID=@ReferralID;
	END











END