--EXEC GetReferralTimeSlot @ReferralID = '40', @Filter = 'Active', @IsDeleted = '0', @SortExpression = 'ReferralTimeSlotMasterID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'

    
CREATE PROCEDURE [dbo].[GetReferralTimeSlot]                      
@ReferralID BIGINT =0,                      
@Filter VARCHAR(10) =null,                                                 
 @StartDate DATE = NULL,                                                  
 @EndDate DATE = NULL,                                            
 @IsDeleted int=-1,                                                  
 @SortExpression NVARCHAR(100)=NULL,                                                    
 @SortType NVARCHAR(10)=NULL,                                                  
 @FromIndex INT=0,                                                  
 @PageSize INT =0                     
  AS                   
--Select  @ReferralID = '40', @Filter = 'Active', @IsDeleted = '0', @SortExpression = 'ReferralTimeSlotMasterID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'
BEGIN                      
DECLARE @ActiveReferralTimeSlotMasterID BIGINT                              
                              
SELECT TOP 1 @ActiveReferralTimeSlotMasterID=etsdate.ReferralTimeSlotMasterID FROM ReferralTimeSlotDetails etsd                              
 left JOIN ReferralTimeSlotDates etsdate ON etsdate.ReferralTimeSlotDetailID=etsd.ReferralTimeSlotDetailID                              
 WHERE etsd.ReferralTimeSlotMasterID IN (SELECT ReferralTimeSlotMasterID FROM ReferralTimeSlotMaster WHERE ReferralID=@ReferralID) AND IsDeleted=0                              
 AND ReferralTSDate >= CONVERT(DATE,GETDATE())                              
 ORDER BY ReferralTSDate                       
                      
if @Filter='Active'                      
BEGIN                      
        SELECT   distinct    RTM.ReferralTimeSlotMasterID,rtm.ReferralID,Name=dbo.GetGeneralNameFormat(R.FirstName,R.LastName),RTDL.CareTypeId, DM.Title AS CareType,RTM.IsDeleted,                       
  RTM.StartDate, RTM.EndDate,RTM.IsEndDateAvailable,  RBA.AuthorizationCode ,RBA.ReferralBillingAuthorizationID,ServiceCode = SC.ServiceCode + CASE WHEN M.ModifierCode IS NULL THEN '' ELSE ' : '+M.ModifierCode END,RTM.IsWithPriorAuth                     
        FROM           ReferralTimeSlotMaster AS RTM                       
                   LEFT JOIN ReferralTimeSlotDates AS RTD ON RTM.ReferralTimeSlotMasterID = RTD.ReferralTimeSlotMasterID                       
       LEFT JOIN ReferralTimeSlotDetails AS RTDL ON RTM.ReferralTimeSlotMasterID = RTDL.ReferralTimeSlotMasterID -- and RTDL.IsDeleted=0                     
       LEFT JOIN  ReferralBillingAuthorizations RBA ON RTM.ReferralBillingAuthorizationID = RBA.ReferralBillingAuthorizationID                       
       INNER JOIN DDMaster DM ON RTM.CareTypeId = DM.DDMasterID                      
       INNER JOIN Referrals R ON R.ReferralID=RTM.ReferralID                
     LEFT JOIN ReferralPayorMappings RPM ON RBA.ReferralID = RPM.ReferralID AND RBA.PayorID = RPM.PayorID                 
    LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID = RBA.ServiceCodeID                      
  LEFT JOIN Modifiers M ON CONVERT(NVARCHAR(MAX), M.ModifierID) = SC.ModifierID               
where    ((rtm.EndDate IS NULL) OR (rtm.EndDate>= CAST(getdate() as date))) and rtm.IsDeleted=0 and  rtm.ReferralID=@ReferralID  and RTDL.IsDeleted=0                    
                      
END                      
ELSE IF @Filter='Expired'                      
BEGIN                      
        SELECT   distinct    RTM.ReferralTimeSlotMasterID,rtm.ReferralID,Name=dbo.GetGeneralNameFormat(R.FirstName,R.LastName),RTDL.CareTypeId, DM.Title AS CareType,RTM.IsDeleted,                       
  RTM.StartDate, RTM.EndDate,RTM.IsEndDateAvailable,  RBA.AuthorizationCode,RBA.ReferralBillingAuthorizationID ,ServiceCode = SC.ServiceCode + CASE WHEN M.ModifierCode IS NULL THEN '' ELSE ' : '+M.ModifierCode END ,RTM.IsWithPriorAuth                     
        FROM           ReferralTimeSlotMaster AS RTM                       
                   left JOIN ReferralTimeSlotDates AS RTD ON RTM.ReferralTimeSlotMasterID = RTD.ReferralTimeSlotMasterID                       
       left JOIN ReferralTimeSlotDetails AS RTDL ON RTM.ReferralTimeSlotMasterID = RTDL.ReferralTimeSlotMasterID                       
       LEFT JOIN  ReferralBillingAuthorizations RBA ON RTM.ReferralBillingAuthorizationID = RBA.ReferralBillingAuthorizationID                       
       LEFT JOIN DDMaster DM ON RTDL.CareTypeId = DM.DDMasterID                      
       INNER JOIN Referrals R ON R.ReferralID=RTM.ReferralID                  
     LEFT JOIN ReferralPayorMappings RPM ON RBA.ReferralID = RPM.ReferralID AND RBA.PayorID = RPM.PayorID                 
    LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID = RBA.ServiceCodeID                      
  LEFT JOIN Modifiers M ON CONVERT(NVARCHAR(MAX), M.ModifierID) = SC.ModifierID               
where     rtm.EndDate< CAST(getdate() as date) and rtm.ReferralID=@ReferralID                      
                      
END                      
ELSE IF @Filter='Delete'                      
BEGIN                      
         SELECT   distinct    RTM.ReferralTimeSlotMasterID,rtm.ReferralID,Name=dbo.GetGeneralNameFormat(R.FirstName,R.LastName),RTDL.CareTypeId, DM.Title AS CareType,RTM.IsDeleted,                       
  RTM.StartDate, RTM.EndDate,RTM.IsEndDateAvailable,  RBA.AuthorizationCode ,RBA.ReferralBillingAuthorizationID,ServiceCode = SC.ServiceCode + CASE WHEN M.ModifierCode IS NULL THEN '' ELSE ' : '+M.ModifierCode END ,RTM.IsWithPriorAuth                     
        FROM           ReferralTimeSlotMaster AS RTM                       
                   left JOIN ReferralTimeSlotDates AS RTD ON RTM.ReferralTimeSlotMasterID = RTD.ReferralTimeSlotMasterID                       
       left JOIN ReferralTimeSlotDetails AS RTDL ON RTM.ReferralTimeSlotMasterID = RTDL.ReferralTimeSlotMasterID                       
       LEFT JOIN  ReferralBillingAuthorizations  RBA ON RTM.ReferralBillingAuthorizationID = RBA.ReferralBillingAuthorizationID                       
       LEFT JOIN DDMaster DM ON RTDL.CareTypeId = DM.DDMasterID                      
       INNER JOIN Referrals R ON R.ReferralID=RTM.ReferralID               
     LEFT JOIN ReferralPayorMappings RPM ON RBA.ReferralID = RPM.ReferralID AND RBA.PayorID = RPM.PayorID                 
    LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID = RBA.ServiceCodeID                      
  LEFT JOIN Modifiers M ON CONVERT(NVARCHAR(MAX), M.ModifierID) = SC.ModifierID               
where     rtm.IsDeleted=1 and rtm.ReferralID=@ReferralID                      
PRINT 'DEL'                      
END                      
                      
ELSE                      
BEGIN                      
    SELECT   distinct    RTM.ReferralTimeSlotMasterID,rtm.ReferralID,Name=dbo.GetGeneralNameFormat(R.FirstName,R.LastName),RTDL.CareTypeId, DM.Title AS CareType,RTM.IsDeleted,                       
  RTM.StartDate, RTM.EndDate,RTM.IsEndDateAvailable,  RBA.AuthorizationCode,RBA.ReferralBillingAuthorizationID,ServiceCode = SC.ServiceCode + CASE WHEN M.ModifierCode IS NULL THEN '' ELSE ' : '+M.ModifierCode END,RTM.IsWithPriorAuth                       
        FROM           ReferralTimeSlotMaster AS RTM                       
                   left JOIN ReferralTimeSlotDates AS RTD ON RTM.ReferralTimeSlotMasterID = RTD.ReferralTimeSlotMasterID                       
       left JOIN ReferralTimeSlotDetails AS RTDL ON RTM.ReferralTimeSlotMasterID = RTDL.ReferralTimeSlotMasterID                       
       LEFT JOIN  ReferralBillingAuthorizations RBA ON RTM.ReferralBillingAuthorizationID = RBA.ReferralBillingAuthorizationID                       
       LEFT JOIN DDMaster DM ON RTDL.CareTypeId = DM.DDMasterID                      
       INNER JOIN Referrals R ON R.ReferralID=RTM.ReferralID                
     LEFT JOIN ReferralPayorMappings RPM ON RBA.ReferralID = RPM.ReferralID AND RBA.PayorID = RPM.PayorID                 
    LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID = RBA.ServiceCodeID                      
  LEFT JOIN Modifiers M ON CONVERT(NVARCHAR(MAX), M.ModifierID) = SC.ModifierID               
where  rtm.ReferralID=@ReferralID AND ((rtm.EndDate IS NULL) OR (rtm.EndDate>= CAST(getdate() as date)))                   
PRINT 'ALL'                      
                      
                      
END                      
                      
                      
END 