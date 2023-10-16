
CREATE PROCEDURE [dbo].[SaveVisitNote]                              
@EmployeeVisitID bigint,                          
@EmployeeVisitNoteID bigint,                          
@ReferralTaskMappingID bigint,                              
@ServiceTime bigint,                              
@SystemID VARCHAR(100),                  
@MacAddress VARCHAR(100),          
@SetAsIncomplete BIT,          
@LoggedInID bigint                              
AS                                            
BEGIN                                            
      --Changes by:Akhilesh kamal
   --Description:for check duplicate entry uncommente
   --Updated Date  15/01/2020                       
 -- Check For Duplicate                                        
 IF EXISTS (SELECT TOP 1 EmployeeVisitNoteID FROM EmployeeVisitNotes WHERE EmployeeVisitID=@EmployeeVisitID AND ReferralTaskMappingID=@ReferralTaskMappingID AND EmployeeVisitNoteID!=@EmployeeVisitNoteID)                          
 BEGIN                                      
 SELECT -1 RETURN;                                        
 END            
         
 Declare @invoicestatus int =null;        
 Declare @invoiceID bigint =null;        
        
 SELECT @invoicestatus=ri.InvoiceStatus,@invoiceID=ri.ReferralInvoiceID FROM dbo.ReferralInvoiceTransactions rit        
 INNER JOIN dbo.ReferralInvoices ri ON rit.ReferralInvoiceID = ri.ReferralInvoiceID        
 WHERE rit.EmployeeVisitID=@EmployeeVisitID --AND rit.EmployeeVisitNoteID=@EmployeeVisitNoteID        
        
 IF((@invoicestatus IS NOT NULL) AND @invoicestatus != 1)         
 BEGIN        
  SELECT -3; RETURN; -- Invoice generated and thier status is in paid or void so we can't change task.        
 END        
         
 DECLARE @TotalTime INT;                    
 DECLARE @TotalServiceTime INT;                    
 DECLARE @Diff INT;                    
            
        
 IF(@EmployeeVisitNoteID>0)                    
 BEGIN          
  -- Update Task                  
  SELECT @Diff=(@ServiceTime-ServiceTime) FROM EmployeeVisitNotes WHERE EmployeeVisitNoteID=@EmployeeVisitNoteID                    
  SELECT @TotalServiceTime=COALESCE((SUM(ServiceTime)+@Diff),@Diff) FROM EmployeeVisitNotes         
  WHERE EmployeeVisitID=@EmployeeVisitID AND IsDeleted=0                     
 END                    
 ELSE                    
 BEGIN        
  -- Create new Task                    
  SELECT @TotalServiceTime=COALESCE((SUM(ServiceTime)+@ServiceTime),@ServiceTime) FROM EmployeeVisitNotes         
  WHERE EmployeeVisitID=@EmployeeVisitID AND IsDeleted=0                    
 END                    
                    
 SELECT @TotalTime=DATEDIFF(MINUTE, StartDate, EndDate) FROM ScheduleMasters         
 WHERE ScheduleID=(SELECT ScheduleID FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID)                     
                        
 IF(@TotalServiceTime>@TotalTime)                    
 BEGIN                    
  SELECT -2; RETURN;                    
 END                    
            
--Set as Incomplete          
 IF(@SetAsIncomplete=1)          
  UPDATE EmployeeVisits SET IsPCACompleted=0,IsSigned=0 WHERE EmployeeVisitID=@EmployeeVisitID          
                
 IF(@EmployeeVisitNoteID = 0)                          
 BEGIN                          
  INSERT INTO EmployeeVisitNotes         
  (EmployeeVisitID,ServiceTime,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,ReferralTaskMappingID)                              
  VALUES         
  (@EmployeeVisitID,@ServiceTime,GETDATE(),@LoggedInID,GETDATE(),@LoggedInID,@SystemID,@ReferralTaskMappingID)          
          
  SET @EmployeeVisitNoteID = scope_identity();        
 END                          
 ELSE                          
 BEGIN                          
  UPDATE EmployeeVisitNotes         
  SET         
  ReferralTaskMappingID=@ReferralTaskMappingID,ServiceTime=@ServiceTime,        
  UpdatedBy=@LoggedInID,UpdatedDate=GETDATE(),SystemID=@SystemID                          
  WHERE         
  EmployeeVisitNoteID=@EmployeeVisitNoteID                          
 END         
         
 DECLARE @ScheduleID bigint = null;         
 DECLARE @EmployeeID bigint = null;         
 DECLARE @Rate decimal(18,2) = null;         
 DECLARE @PerUnitQuantity decimal(18,2) = null;         
 DECLARE @Amount decimal(18,2) = null;         
 DECLARE @TotalAmount decimal(18,2) = null;         
 DECLARE @ServiceCodeID bigint = null;         
 DECLARE @InvoiceTaxRate decimal(18,2) = null;       
     
 IF((@invoicestatus IS NOT NULL) AND @invoicestatus = 1)         
  BEGIN        
   -- Add Invoice transaction and update invoice details        
           
   -- get ScheduleID from EmployeeVisits        
   SELECT @ScheduleID=ScheduleID FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID         
   --  get EmployeeID from ScheduleMasters        
   SELECT @EmployeeID=EmployeeID FROM ScheduleMasters WHERE ScheduleID=@ScheduleID         
        
   -- get ServiceCodeID from VisitTask        
   SELECT @ServiceCodeID=vt.ServiceCodeID FROM ReferralTaskMappings rtm         
   INNER JOIN VisitTasks vt ON rtm.VisitTaskID = vt.VisitTaskID        
   WHERE rtm.ReferralTaskMappingID=@ReferralTaskMappingID        
        
   -- get rate, perUnitQuantity, amount from Payor Service code Mapping        
   SELECT @Rate=PSM.Rate,@PerUnitQuantity=PSM.PerUnitQuantity,        
     @Amount=        
       CASE WHEN PSM.UnitType=2 THEN PSM.Rate             
       ELSE CONVERT(DECIMAL(18,2),CONVERT(DECIMAL(18,2),ROUND(@ServiceTime/PSM.PerUnitQuantity,0,0))*PSM.Rate)           
       END        
   FROM ScheduleMasters SM         
   INNER JOIN Payors P ON P.PayorID=SM.PayorID AND P.IsDeleted=0              
   LEFT JOIN PayorServiceCodeMapping PSM ON PSM.PayorID=SM.PayorID AND PSM.IsDeleted=0  AND PSM.ServiceCodeID = @ServiceCodeID           
   WHERE SM.ScheduleID=@ScheduleID        
        
        
   -- Add All details in Referral Transaction Table when task in payor mapping        
   IF((@Rate IS NOT NULL) OR (@PerUnitQuantity IS NOT NULL))        
   BEGIN      
         
       
    
    
    IF EXISTS (SELECT TOP 1 * FROM ReferralInvoiceTransactions WHERE EmployeeVisitNoteID=@EmployeeVisitNoteID)        
    BEGIN        
     --update ReferralInvoiceTransactions     
     UPDATE ReferralInvoiceTransactions        
     SET        
         Rate = @Rate,         
         PerUnitQuantity = @PerUnitQuantity,         
         Amount = @Amount,         
         ServiceTime = @ServiceTime         
     WHERE        
      EmployeeVisitNoteID=@EmployeeVisitNoteID        
    END        
    ELSE        
    BEGIN        
     -- insert ReferralInvoiceTransactions        
     INSERT INTO dbo.ReferralInvoiceTransactions        
     (ReferralInvoiceID,ScheduleID,EmployeeID,EmployeeVisitID,EmployeeVisitNoteID,Rate,PerUnitQuantity,Amount,ServiceTime,IsDeleted)        
     VALUES        
     (@invoiceID,@ScheduleID,@EmployeeID,@EmployeeVisitID,@EmployeeVisitNoteID,@Rate,@PerUnitQuantity,@Amount,@ServiceTime,0)       
       
    END        
       
    SELECT @InvoiceTaxRate=InvoiceTaxRate   
 FROM dbo.ReferralInvoices      
 WHERE ReferralInvoiceID=@invoiceID AND IsDeleted=0      
    
 UPDATE dbo.EmployeeVisitNotes      
 SET  IsInvoiceGenerated = 1      
 WHERE       
 EmployeeVisitNoteID = @EmployeeVisitNoteID       
      
    SELECT @TotalAmount=sum(Amount)         
    FROM ReferralInvoiceTransactions        
    WHERE ReferralInvoiceID=@invoiceID        
        
    UPDATE dbo.ReferralInvoices        
    SET PayAmount = @TotalAmount+((@TotalAmount * @InvoiceTaxRate)/100)    
 WHERE ReferralInvoiceID=@invoiceID    
         
   END        
           
           
  END        
         
                                
 SELECT 1; RETURN;                                        
                                        
END  
GO
