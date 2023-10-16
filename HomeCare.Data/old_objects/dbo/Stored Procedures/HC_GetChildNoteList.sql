

-- exec [HC_GetChildNoteList] 0,1,18,'','2018-08-15','2018-09-15',24234                       
-- exec [HC_GetChildNoteList] 134,1,13,'','2017-12-01','2018-12-31',24233                       
CREATE PROCEDURE [dbo].[HC_GetChildNoteList]                              
@BatchID bigint=0,                                                                   
@BatchTypeID bigint=1,                                                                   
@PayorId BIGINT=18,                                          
@ServiceCodeIDs VARCHAR(MAX)=null,                                              
@StartDate Date=null,                                
@EndDate Date=null,                         
@ReferralID bigint                                      
AS                                                            
BEGIN                                 
                    
if(@BatchID = 0)                    
begin                                                          
 if(@BatchTypeID = 1) -- 1: Initial Submission                              
begin               
 select N.NoteID,dbo.GetGeneralNameFormat(N.RenderingProviderFirstName,N.RenderingProviderName) as EmployeeName,N.ServiceDate,N.Rate,          
 ROUND(CN.CalculatedAmount,2) as CalculatedAmount,CN.CalculatedUnit,N.CalculatedServiceTime,    
 --DM.Title as CareType,                      
 ServiceCode = N.ServiceCode +                        
 case                        
 WHEN (SC.ModifierID IS NULL OR SC.ModifierID ='')                    
 then ''                        
 else                       
 ' -'+                        
  STUFF(                            
  (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                            
  FROM Modifiers M                            
  where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))                            
  FOR XML PATH (''))                            
  , 1, 1, '')                        
 end                          
 from Notes N          
 INNER JOIN ChildNotes CN ON CN.NoteID=N.NoteID          
 inner join Payors P  On P.PayorID = N.PayorID                            
 inner join Referrals R  On R.ReferralID = N.ReferralID                         
 inner join ServiceCodes SC on SC.ServiceCodeID = N.ServiceCodeID                       
 LEFT JOIN Modifiers M ON M.ModifierID=SC.ModifierID                          
 --left join DDMaster DM on DM.DDMasterID =  SC.CareType                        
 where                         
 N.IsBillable=1 AND N.IsDeleted=0 AND N.MarkAsComplete=1  AND N.GroupID IS NOT NULL                        
 AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))                             
 AND N.PayorID=@PayorID                         
 AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))                         
 AND N.ReferralID=@ReferralID                        
    AND                         
 N.NoteID NOT IN (                              
     SELECT DISTINCT NoteID FROM (                              
     SELECT BatchNoteID,BN.NoteID,CLP02_ClaimStatusCode, ROW_NUMBER() OVER ( PARTITION BY BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber                              
     FROM BatchNotes BN                              
     ) AS A WHERE RowNumber=1  AND ( CLP02_ClaimStatusCode IS NULL OR ( CLP02_ClaimStatusCode IN (1,2,3,4) ) )                              
     -- SELECT NoteID FROM BatchNotes WHERE NoteID NOT IN ( SELECT NoteID FROM BatchNotes  WHERE CLP02_ClaimStatusCode IN (22) AND Submitted_ClaimAdjustmentTypeID IN ('Void','Replacement') )                              
    )                               
 order by N.ServiceDate desc                          
end                            
                                                   
 if(@BatchTypeID = 2) -- 2: Denial Re-Submission                              
begin                            
                        
 select N.NoteID,dbo.GetGeneralNameFormat(N.RenderingProviderFirstName ,N.RenderingProviderName) as EmployeeName,N.ServiceDate,N.Rate,ROUND(N.CalculatedAmount,2) as CalculatedAmount,N.CalculatedUnit,N.CalculatedServiceTime,    
 --DM.Title as CareType,               
 ServiceCode = N.ServiceCode +                        
 case                        
 when (SC.ModifierID IS NULL OR SC.ModifierID ='')              
 then ''                        
 else                       
 ' -'+                        
  STUFF(                            
  (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                            
  FROM Modifiers M                            
  where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))                            
  FOR XML PATH (''))                            
  , 1, 1, '')                        
 end                          
 from Notes N                         
 INNER JOIN (                              
  SELECT t.NoteID                        
  FROM                              
  (SELECT DISTINCT ROW_NUMBER() OVER                               
  -- ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber,                              
  ( PARTITION BY BN.NoteID Order BY BN.BatchID DESC, MarkAsLatest DESC,  BN.BatchNoteID Desc)  AS RowNumber,                               
  NoteID,CLP02_ClaimStatusCode,CLP01_ClaimSubmitterIdentifier,                              
  CLP07_PayerClaimControlNumber, ClaimAdjustmentTypeID,ClaimAdjustmentReason,CLP04_TotalClaimPaymentAmount FROM BatchNotes BN                               
  ) AS t --WHERE RowNumber=1 AND CLP02_ClaimStatusCode IS NOT NULL AND CLP02_ClaimStatusCode IN (4,22) AND ClaimAdjustmentTypeID IS NULL                              
  LEFT JOIN  BatchNotes BN1 ON   t.CLP07_PayerClaimControlNumber=BN1.Original_PayerClaimControlNumber                              
  WHERE RowNumber=1 AND                               
  (t.ClaimAdjustmentTypeID IS NULL OR t.ClaimAdjustmentTypeID NOT IN ('Replacement','Void','Write-Off','Denial') ) AND                              
  ( CONVERT(DECIMAL(18,2),ISNULL(t.CLP04_TotalClaimPaymentAmount,0))= 0 AND t.CLP02_ClaimStatusCode IS NOT NULL) AND BN1.Original_PayerClaimControlNumber IS NULL                              
  --t.CLP02_ClaimStatusCode IS NOT NULL AND t.CLP02_ClaimStatusCode IN (4,22) AND t.ClaimAdjustmentTypeID IS NULL AND BN1.Original_PayerClaimControlNumber IS NULL                              
                              
 ) BND ON BND.NoteID=N.NoteID                            
 inner join Payors P  On P.PayorID = N.PayorID                            
 inner join Referrals R  On R.ReferralID = N.ReferralID                         
 inner join ServiceCodes SC on SC.ServiceCodeID = N.ServiceCodeID                      
 LEFT JOIN Modifiers M ON M.ModifierID=SC.ModifierID                              
 --left join DDMaster DM on DM.DDMasterID =  SC.CareType                        
 where                         
 N.IsBillable=1 AND N.IsDeleted=0 AND N.MarkAsComplete=1  AND N.GroupID IS NOT NULL                        
 AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))                           
 AND N.PayorID=@PayorID                         
 AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))                        
 AND N.ReferralID=@ReferralID                        
    order by N.ServiceDate desc                             
end                               
                            
 if(@BatchTypeID = 3) -- 3: Adjustment(Void/Replace) Submission                             
begin                            
 select N.NoteID,dbo.GetGeneralNameFormat(N.RenderingProviderFirstName ,N.RenderingProviderName) as EmployeeName,N.ServiceDate ,N.Rate,ROUND(N.CalculatedAmount,2) as CalculatedAmount,N.CalculatedUnit,N.CalculatedServiceTime,    
 --DM.Title as CareType ,             
 ServiceCode = N.ServiceCode +                        
 case                        
 when (SC.ModifierID IS NULL OR SC.ModifierID ='')                        
 then ''                        
 else                       
 ' -'+                        
  STUFF(                            
  (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                            
  FROM Modifiers M                            
  where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))                            
  FOR XML PATH (''))                            
  , 1, 1, '')                        
 end                        
 from Notes N                            
 INNER JOIN (                              
  SELECT t.NoteID           
  FROM                              
  (SELECT DISTINCT ROW_NUMBER() OVER                               
  ( PARTITION BY BN.NoteID Order BY BN.BatchID DESC,MarkAsLatest DESC,  BN.BatchNoteID Desc)  AS RowNumber,                               
  -- ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber,                               
  NoteID,CLP02_ClaimStatusCode,CLP01_ClaimSubmitterIdentifier,                               
  CLP07_PayerClaimControlNumber, ClaimAdjustmentTypeID , ClaimAdjustmentReason FROM BatchNotes BN                               
  ) AS t -- WHERE RowNumber=1 AND ClaimAdjustmentTypeID IS NOT NULL                              
  LEFT JOIN  BatchNotes BN1 ON   t.CLP07_PayerClaimControlNumber=BN1.Original_PayerClaimControlNumber                              
  WHERE RowNumber=1 AND (t.ClaimAdjustmentTypeID IS NOT NULL AND                               
  --t.ClaimAdjustmentTypeID !='Write-Off'                              
  t.ClaimAdjustmentTypeID NOT IN ('Write-Off','Denial')                              
  )                               
  AND BN1.Original_PayerClaimControlNumber IS NULL                              
                              
 ) BND ON BND.NoteID=N.NoteID                              
 inner join Payors P  On P.PayorID = N.PayorID                            
 inner join Referrals R  On R.ReferralID = N.ReferralID                         
 inner join ServiceCodes SC on SC.ServiceCodeID = N.ServiceCodeID                        
 LEFT JOIN Modifiers M ON M.ModifierID=SC.ModifierID                            
 --left join DDMaster DM on DM.DDMasterID =  SC.CareType                           
 where                         
 N.IsBillable=1 AND N.IsDeleted=0 AND N.MarkAsComplete=1  AND N.GroupID IS NOT NULL                        
 AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))                           
 AND N.PayorID=@PayorID                         
 AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))                        
 AND N.ReferralID=@ReferralID                        
    order by N.ServiceDate desc                           
end                            
end                    
else                    
begin                  
                    
 select N.NoteID,dbo.GetGeneralNameFormat(N.RenderingProviderFirstName,N.RenderingProviderName) as EmployeeName,N.ServiceDate,N.Rate,      
 ROUND(N.CalculatedAmount,2) as CalculatedAmount,      
 N.CalculatedUnit,      
 N.CalculatedServiceTime,    
 --DM.Title as CareType,                 
 ServiceCode = N.ServiceCode +                        
 case                        
 when (SC.ModifierID IS NULL OR SC.ModifierID ='')                        
 then ''                        
 else                       
 ' -'+                        
 STUFF(                            
 (SELECT ', ' + convert(varchar(100),M.ModifierCode, 120)                            
 FROM Modifiers M                            
 where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))                            
 FOR XML PATH (''))                            
 , 1, 1, '')                        
 end                    
 from BatchNotes BN                    
 inner join Notes N on N.NoteID = BN.NoteID and BN.BatchID=@BatchID                    
 inner join Referrals R on R.referralid =N.referralid and N.ReferralID=@ReferralID                     
 inner join ServiceCodes SC on SC.ServiceCodeID = N.ServiceCodeID                       
 LEFT JOIN Modifiers M ON M.ModifierID=SC.ModifierID                          
 --left join DDMaster DM on DM.DDMasterID =  SC.CareType                      
 order by N.ServiceDate                      
end          
                                                   
END    
  


