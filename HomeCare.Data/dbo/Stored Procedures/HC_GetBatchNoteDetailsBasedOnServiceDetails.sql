
CREATE  PROCEDURE [dbo].[HC_GetBatchNoteDetailsBasedOnServiceDetails]                
@ServiceCode VARCHAR(MAX),                
@ServiceStartDate DATE,              
@ServiceEndDate DATE,              
@ServiceCode_Mod_01 VARCHAR(100),            
@ServiceCode_Mod_02 VARCHAR(100),              
@ServiceCode_Mod_03 VARCHAR(100),              
@ServiceCode_Mod_04 VARCHAR(100),              
--@PatientAHCCCSID VARCHAR(100),              
@ClientHIC NVARCHAR(MAX),              
@ClientFirstName NVARCHAR(MAX),              
@ClientLastName NVARCHAR(MAX),  
  
  
@ClientReferralID BIGINT  = 0   ,  
@BatchID BIGINT  = 0            
AS                
BEGIN                
                
 DECLARE @ServiceCode_Mod_01_ID BIGINT;              
 SELECT @ServiceCode_Mod_01_ID=ModifierID FROM Modifiers WHERE ModifierCode IN (@ServiceCode_Mod_01)              
              
 DECLARE @ServiceCode_Mod_02_ID BIGINT;              
 SELECT @ServiceCode_Mod_02_ID=ModifierID FROM Modifiers WHERE ModifierCode IN (@ServiceCode_Mod_02)              
              
 DECLARE @ServiceCode_Mod_03_ID BIGINT;              
 SELECT @ServiceCode_Mod_03_ID=ModifierID FROM Modifiers WHERE ModifierCode IN (@ServiceCode_Mod_03)              
              
 DECLARE @ServiceCode_Mod_04_ID BIGINT;              
 SELECT @ServiceCode_Mod_04_ID=ModifierID FROM Modifiers WHERE ModifierCode IN (@ServiceCode_Mod_04)              
               
 DECLARE @BatchNoteID BIGINT;              
  
  
PRINT @BatchID  
PRINT @ClientReferralID  
PRINT @ServiceCode_Mod_01_ID
PRINT @ServiceCode_Mod_02_ID

-- EXEC HC_GetBatchNoteDetailsBasedOnServiceDetails 'T1019','2023-03-22','2023-03-22', '76','','','','','','','248','100658'
  
IF(@BatchID!=0 AND @ClientReferralID!=0)                       
BEGIN   
  
PRINT 'By Batch  & ReferalID '  
 SELECT DISTINCT @BatchNoteID=BN.BatchNoteID              
 FROM Notes N        
 INNER JOIN BatchNotes BN ON BN.NoteID=N.NoteID   
 INNER JOIN ServiceCodes S ON S.ServiceCodeID = N.ServiceCodeID   
 AND ((ISNULL(@ServiceCode_Mod_01_ID,0)=0  AND LEN(ISNULL(S.ModifierID,''))=0 ) OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) )   
 AND ((ISNULL(@ServiceCode_Mod_02_ID,0)=0  AND LEN(ISNULL(S.ModifierID,''))=0 ) OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) ) 
 AND ((ISNULL(@ServiceCode_Mod_03_ID,0)=0  AND LEN(ISNULL(S.ModifierID,''))=0 ) OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) ) 
 AND ((ISNULL(@ServiceCode_Mod_04_ID,0)=0  AND LEN(ISNULL(S.ModifierID,''))=0 ) OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) )  
 
 WHERE   
 BN.BatchID=@BatchID AND N.ReferralID=@ClientReferralID   
 AND N.ServiceCode=@ServiceCode AND N.ServiceDate BETWEEN @ServiceStartDate AND @ServiceEndDate   
  
END  
ELSE  
BEGIN  
    
 PRINT 'By @ClientHIC'  
 SELECT DISTINCT @BatchNoteID=BN.BatchNoteID              
 FROM Notes N              
 INNER JOIN ServiceCodes S ON S.ServiceCodeID = N.ServiceCodeID 
 --AND               
 --(  ISNULL(@ServiceCode_Mod_01_ID,0)=0 OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) ) AND              
 --(  ISNULL(@ServiceCode_Mod_02_ID,0)=0 OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) ) AND              
 --(  ISNULL(@ServiceCode_Mod_03_ID,0)=0 OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) ) AND              
 --(  ISNULL(@ServiceCode_Mod_04_ID,0)=0 OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) )      
 

 AND ((ISNULL(@ServiceCode_Mod_01_ID,0)=0  AND LEN(ISNULL(S.ModifierID,''))=0 ) OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) )   
 AND ((ISNULL(@ServiceCode_Mod_02_ID,0)=0  AND LEN(ISNULL(S.ModifierID,''))=0 ) OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) ) 
 AND ((ISNULL(@ServiceCode_Mod_03_ID,0)=0  AND LEN(ISNULL(S.ModifierID,''))=0 ) OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) ) 
 AND ((ISNULL(@ServiceCode_Mod_04_ID,0)=0  AND LEN(ISNULL(S.ModifierID,''))=0 ) OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) )  
 
   
 INNER JOIN Referrals R ON R.ReferralID = N.ReferralID  
 INNER JOIN ReferralPayorMappings RPM  ON RPM.ReferralID = R.ReferralID AND RPM.IsActive=1  
 INNER JOIN BatchNotes BN ON BN.NoteID=N.NoteID  
 WHERE N.ServiceCode=@ServiceCode AND N.ServiceDate BETWEEN @ServiceStartDate AND @ServiceEndDate AND   
 ( N.AHCCCSID=@ClientHIC  OR RPM.MemberID=@ClientHIC OR RPM.BeneficiaryNumber=@ClientHIC  OR (R.FirstName=@ClientFirstName AND R.LastName= @ClientLastName))  
END            
   
   
   
 SELECT ISNULL(@BatchNoteID,0);   
   
  
 -- EXEC HC_GetBatchNoteDetailsBasedOnServiceDetails @ServiceCode = 'T1019', @ServiceStartDate = '02/10/2023', @ServiceEndDate = '02/10/2023', @ServiceCode_Mod_01 = '', @ServiceCode_Mod_02 = '', @ServiceCode_Mod_03 = '', @ServiceCode_Mod_04 = '', @ClientHIC = '2000899301', @ClientFirstName = 'PATRICIA', @ClientLastName = 'BRADLEY'  
  
                 
END 