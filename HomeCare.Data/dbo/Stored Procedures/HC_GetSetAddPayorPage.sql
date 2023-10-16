CREATE PROCEDURE [dbo].[HC_GetSetAddPayorPage]
  @PayorID bigint,
  @DDType_NPIOptions int,
  @DDType_PayerGroup int,
  @DDType_BussinessLine int,
  @DDType_RevenueCode int,
  @DDType_CareType int,
  @DDType_NPINumber int
AS
BEGIN
  SELECT
    *
  FROM States;

  SELECT
    Name = CONCAT(ModifierCode, ' - ', ModifierName),
    Value = ModifierID
  FROM Modifiers;
  SELECT
    *
  FROM Payors
  WHERE
    PayorID = @PayorID;

  --SELECT PayorEdi837SettingId,PayorID,      
  --ISA06_InterchangeSenderId,ISA06_InterchangeSenderId,ISA08_InterchangeReceiverId,ISA11_RepetitionSeparator,ISA16_ComponentElementSeparator,        
  --Submitter_NM103_NameLastOrOrganizationName,Submitter_NM109_IdCodeQualifierEnum,Submitter_EDIContact1_PER02_Name,Submitter_EDIContact1_PER04_CommunicationNumber,        
  --Submitter_EDIContact1_PER08_CommunicationNumber,CMS1500HourRounding,UB04HourRounding      
  --FROM PayorEdi837Settings WHERE PayorID=@PayorID      

  --SELECT Name=Title,Value=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_NPIOptions            
  --SELECT Name=Title,Value=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_PayerGroup                               
  --SELECT Name=Title,Value=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_BussinessLine             
  --SELECT Name=Title,Value=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_RevenueCode            
  --SELECT Name=Title,Value=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_CareType            

  EXEC GetGeneralMasterList @DDType_NPIOptions
  EXEC GetGeneralMasterList @DDType_PayerGroup
  EXEC GetGeneralMasterList @DDType_BussinessLine

  EXEC GetGeneralMasterList @DDType_RevenueCode
  EXEC GetGeneralMasterList @DDType_CareType
  --EXEC GetGeneralMasterList @DDType_NPINumber

  SELECT
    DD.Title AS Name,
    DD.Title AS Value
  FROM DDMaster DD
  INNER JOIN lu_DDMasterTypes T
    ON DD.ItemType = T.DDMasterTypeID
  WHERE
    T.Name = 'NPINumber'
    AND DD.IsDeleted = 0
END