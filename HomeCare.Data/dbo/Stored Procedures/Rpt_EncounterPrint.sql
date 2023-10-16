-- EXEC Rpt_EncounterPrint @ReferralID = '501', @StartDate = '2017/07/31', @EndDate = '2017/07/31', @IsDeleted = '0', @InternalMessaging = 'Internal Messaging'

CREATE PROCEDURE [dbo].[Rpt_EncounterPrint]            
 @ReferralID bigint=0,                                    
 @StartDate date=null,                                                                    
 @EndDate date=null,                                                                    
 @IsDeleted bigint=0,
 @InternalMessaging varchar(150)=null
AS                                                                          
BEGIN                                    
 SELECT DISTINCT R.LastName+', ' + R.FirstName as ClientName,                                    
   N.CISNumber,N.AHCCCSID,N.BillingProviderName,                                 
   CONVERT(VARCHAR(10),CONVERT(datetime,N.ServiceDate,1),101) as ServiceDate,                              
   EC.LastName+', '+EC.FirstName as Createdby,                                    
   EP.LastName+', '+EP.FirstName as Updatedby, N.ServiceCodeType, 
   --CONVERT(VARCHAR(10), N.CreatedDate, 111) + ' ' + LTRIM(RIGHT(CONVERT(CHAR(20), N.CreatedDate, 22), 11)) as CreatedDate,  
   -- CONVERT(VARCHAR(10), N.UpdatedDate, 111) + ' ' + LTRIM(RIGHT(CONVERT(CHAR(20), N.UpdatedDate, 22), 11)) as UpdatedDate,  
     
   --STUFF(CONVERT(CHAR(20), N.CreatedDate, 22), 7, 2, YEAR(N.CreatedDate)) as CreatedDate,  
   --STUFF(CONVERT(CHAR(20), N.UpdatedDate, 22), 7, 2, YEAR(N.UpdatedDate)) as UpdatedDate   ,                                  
   
   N.CreatedDate as CreatedDate1,  
   N.UpdatedDate as UpdatedDate1,   
                                   
   (SELECT  STUFF((SELECT '~' + F.DXCodeName +' - '+ CONVERT(nvarchar(max),F.Description) +'|'+ CONVERT(varchar(10),F.Precedence)                                
   FROM NoteDXCodeMappings F where F.NoteID=N.NoteID   order by F.Precedence ASC                 
   FOR XML PATH('')),1,1,''))  DXCodeName,                                
                                   
   N.ServiceCode,N.ServiceName,PosID,  N.RandomGroupID, SC.UnitType,                 
  CONVERT(VARCHAR(5), N.StartTime, 108) + (CASE WHEN DATEPART(HOUR,  N.StartTime) > 12 THEN ' PM'ELSE ' AM'END) as StartTime,
  CONVERT(VARCHAR(5), N.EndTime, 108) +  (CASE WHEN DATEPART(HOUR, N.EndTime) > 12	THEN ' PM'        ELSE ' AM' END) as EndTime,
  datediff(DAY,N.ServiceDate,N.UpdatedDate) as LateEntry,            
                    
  N.POSDetail,N.NoteDetails,N.CalculatedUnit                                    
   ,N.Assessment,N.ActionPlan,N.StartMile as Startingodometer,N.EndMile as Endingodometer,sl.Signature as EmpSignature,sl.Name AS SignedBy,  ES.CredentialID                                
   from Notes N                                    
   inner join Referrals R on N.ReferralID=R.ReferralID          
   inner join Employees EC on N.CreatedBy=EC.EmployeeID          
   left join SignatureLogs sl on sl.NoteID=N.NoteID and sl.IsActive=1         
   LEFT join Employees ES on ES.EmployeeID=sl.SignatureBy AND sl.IsActive=1     
   left join Employees EP on N.UpdatedBy=EP.EmployeeID          
   left join ServiceCodes SC on SC.ServiceCodeID=N.ServiceCodeID AND SC.ServiceCodeType in(1,2)          
                                     
   WHERE N.ServiceCodeType!=3   AND    --N.MarkAsComplete=1 AND  
     (N.Source IS NULL OR N.Source !=@InternalMessaging ) AND   
     ((CAST(@IsDeleted AS BIGINT)=-1) OR N.IsDeleted=@IsDeleted)                                    
      AND ((@StartDate is null OR N.ServiceDate>= @StartDate) and (@EndDate is null OR N.ServiceDate<= @EndDate))                                    
      AND (( CAST(@ReferralID AS BIGINT)=0) OR N.ReferralID = CAST(@ReferralID AS BIGINT))                                                                   
END                       
--exec Rpt_EncounterPrint