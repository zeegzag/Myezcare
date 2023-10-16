CREATE Procedure [dbo].[HC_SaveReferralPayorMapping]                  
@ReferralPayorMappingID bigint=0,                
@ReferralID bigint,                
@PayorID bigint,                
@Precedence bigint,                
@StartDate date,                
@EndDate date ,      
@loggedInUserId bigint,    
@CurrentDate DateTime,    
@SystemID varchar(MAX),
@IsPayorNotPrimaryInsured BIT,
@InsuredId varchar(1000)=NULL,
@InsuredFirstName varchar(50)=NULL,
@InsuredMiddleName varchar(50)=NULL,
@InsuredLastName varchar(50)=NULL,
@InsuredAddress varchar(1000)=NULL,
@InsuredCity varchar(100)=NULL,
@InsuredState varchar(100)=NULL,
@InsuredZipCode varchar(5)=NULL,
@InsuredPhone varchar(15)=NULL,
@InsuredPolicyGroupOrFecaNumber varchar(100)=NULL,
@InsuredDateOfBirth date=NULL,
@InsuredGender varchar(1)=NULL,
@InsuredEmployersNameOrSchoolName varchar(100)=NULL  
AS          
BEGIN    
 IF EXISTS (    
 SELECT Top 1 ReferralPayorMappingID FROM ReferralPayorMappings WHERE ReferralID=@ReferralID AND Precedence =@Precedence AND IsDeleted = 0  AND 
 ReferralPayorMappingID!=@ReferralPayorMappingID  AND    
 ((@StartDate >= PayorEffectiveDate AND @StartDate <= PayorEffectiveEndDate)                 
 OR (@EndDate >= PayorEffectiveDate AND @EndDate <= PayorEffectiveEndDate)                 
 OR (@StartDate < PayorEffectiveDate AND @EndDate > PayorEffectiveEndDate))     
 )      
 BEGIN    
  SELECT -1 AS TransactionResultId;    
  Return;    
 END    
    
 IF(@ReferralPayorMappingID = 0)    
 BEGIN    
  INSERT INTO ReferralPayorMappings (ReferralID,PayorID,PayorEffectiveDate,PayorEffectiveEndDate,IsActive,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,
									Precedence,IsPayorNotPrimaryInsured,InsuredId,InsuredFirstName,InsuredMiddleName,InsuredLastName,InsuredAddress,InsuredCity,InsuredState,InsuredZipCode,InsuredPhone,
									InsuredPolicyGroupOrFecaNumber,InsuredDateOfBirth,InsuredGender,InsuredEmployersNameOrSchoolName)    
  VALUES (@ReferralID,@PayorID,@StartDate,@EndDate,1,0,@CurrentDate,@loggedInUserId,@CurrentDate,@loggedInUserId,@SystemID,@Precedence,@IsPayorNotPrimaryInsured,@InsuredId,
			@InsuredFirstName,@InsuredMiddleName,@InsuredLastName,@InsuredAddress,@InsuredCity,@InsuredState,@InsuredZipCode,@InsuredPhone,@InsuredPolicyGroupOrFecaNumber,
			@InsuredDateOfBirth,@InsuredGender,@InsuredEmployersNameOrSchoolName)
 END    
 ELSE    
 BEGIN     
  Update ReferralPayorMappings    
  SET    
   ReferralID=@ReferralID,    
   PayorID=@PayorID,    
   PayorEffectiveDate=@StartDate,    
   PayorEffectiveEndDate=@EndDate,    
   UpdatedDate=@CurrentDate,    
   UpdatedBy=@loggedInUserId,    
   SystemID=@SystemID,    
   Precedence=@Precedence,
   IsPayorNotPrimaryInsured=@IsPayorNotPrimaryInsured,
   InsuredId=@InsuredId,
	InsuredFirstName=@InsuredFirstName,
	InsuredMiddleName=@InsuredMiddleName,
	InsuredLastName=@InsuredLastName,
	InsuredAddress=@InsuredAddress,
	InsuredCity=@InsuredCity,
	InsuredState=@InsuredState,
	InsuredZipCode=@InsuredZipCode,
	InsuredPhone=@InsuredPhone,
	InsuredPolicyGroupOrFecaNumber=@InsuredPolicyGroupOrFecaNumber,
	InsuredDateOfBirth=@InsuredDateOfBirth,
	InsuredGender=@InsuredGender,
	InsuredEmployersNameOrSchoolName=@InsuredEmployersNameOrSchoolName
  WHERE    
   ReferralPayorMappingID=@ReferralPayorMappingID    
 END    
 SELECT 1 AS TransactionResultId;    
END
