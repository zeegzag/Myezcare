    

CREATE PROCEDURE [dbo].[SetTransportationGroupClientFilter]    
 -- Add the parameters for the stored procedure here    
 @ReferralID bigint,  
 @TransportationGroupClientID bigint  
 AS    
BEGIN    
 DECLARE @Age Decimal(4,2),  
 @IsPrivateRoomNeeded bit,  
 @CurrentFrquencyID int,  
 @IsCareConsentDocMissing int,
 @IsCAZPayor int,  
 @Is_Enca_Enca int,
 @IsPY int,  
 @BoosterSeat int=1,  
 @PrivateRoom int=2,  
 @CarSeat int=3,  
 @SaturdayOnly int=4,  
 @ConsentPacketServicePlan int=5,  
 @DTRFriSun int=6,  
 @DTRLifeSkills int=7  
  
 select  @Age= dbo.GetAge(Dob) , @IsPrivateRoomNeeded= isnull(NeedPrivateRoom,0),@CurrentFrquencyID=FrequencyCodeID from Referrals  where ReferralID=@ReferralID  
  
  
 --select  @IsCareConsentDocMissing=count(*) from ReferralDocuments where ReferralID=@ReferralID and DocumentTypeID=1  
 select  @IsCareConsentDocMissing=count(*) from Referrals where ReferralID=@ReferralID and CareConsent=1 -- DocumentTypeID=1  
   
 select @IsCAZPayor = count(*) from ReferralPayorMappings rpm  
 inner join Payors p on p.PayorID=rpm.PayorID  
 where rpm.ReferralID=@ReferralID and rpm.IsActive=1 and rpm.IsDeleted=0 and (p.ShortName like 'CAZ%' OR p.PayorName like '%Cenpatico of Arizona%')
 
 select @Is_Enca_Enca = COUNT(*) from TransportationGroupClients TBC
 inner join   ScheduleMasters SM on TBC.ScheduleID= SM.ScheduleID
 inner join TransportLocations PTL on PTL.TransportLocationID = SM.PickUpLocation
 inner join TransportLocations DTL on DTL.TransportLocationID = SM.DropOffLocation
 where (TBC.TransportationGroupClientID=@TransportationGroupClientID ) and
		(PTL.LocationCode ='Free' or PTL.LocationCode ='Enca' or DTL.LocationCode ='Free' or DTL.LocationCode ='Enca')
  
 select @IsPY = count(*) from ReferralPayorMappings rpm  
 inner join Payors p on p.PayorID=rpm.PayorID  
 where rpm.ReferralID=@ReferralID and rpm.IsActive=1 and rpm.IsDeleted=0 and (p.ShortName like 'PY%' OR p.PayorName like '%Pascua Yaqui%')  
  
 if(@Age between 5 and 8)  
 BEGIN  
  INSERT into TransportationGroupFilterMapping values(@TransportationGroupClientID,@BoosterSeat)  
 END   
  
 if(@Age between 0 and 5)  
 BEGIN  
  INSERT into TransportationGroupFilterMapping values(@TransportationGroupClientID,@CarSeat)  
 END   
  
 if(@IsPrivateRoomNeeded = 1)  
 BEGIN  
  INSERT into TransportationGroupFilterMapping values(@TransportationGroupClientID,@PrivateRoom)  
 END   
  
 if(@CurrentFrquencyID = 15)  
 BEGIN  
  INSERT into TransportationGroupFilterMapping values(@TransportationGroupClientID,@SaturdayOnly)  
 END   
  
 if(@IsCareConsentDocMissing = 0)  
 BEGIN  
  INSERT into TransportationGroupFilterMapping values(@TransportationGroupClientID,@ConsentPacketServicePlan)  
 END   
  
 if(@IsCAZPayor > 0)  
 BEGIN  
  INSERT into TransportationGroupFilterMapping values(@TransportationGroupClientID,@DTRFriSun)  
 END  
  
 if(@IsPY > 0 and @Is_Enca_Enca=0)  
 BEGIN  
  INSERT into TransportationGroupFilterMapping values(@TransportationGroupClientID,@DTRLifeSkills)  
 END  
  
   
END