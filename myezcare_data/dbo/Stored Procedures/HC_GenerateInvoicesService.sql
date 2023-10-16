-- exec [HC_GenerateInvoicesService]                
CREATE PROCEDURE [dbo].[HC_GenerateInvoicesService]                                                                                        
AS                                                                                                      
BEGIN       
 DECLARE @CurrentDateTime DATETIME;    
 SET @CurrentDateTime=dbo.GetOrgCurrentDateTime();    
    
 DECLARE @InvoiceGenerationFrequency INT,@InvoiceDueDays INT;                  
 SELECT @InvoiceGenerationFrequency=InvoiceGenerationFrequency,@InvoiceDueDays=InvoiceDueDays FROM OrganizationSettings               
              
 DECLARE @InvoiceTaxRate Decimal(5,2);        
 SELECT @InvoiceTaxRate = InvoiceTaxRate FROM OrganizationSettings           
         
 --SELECT * FROM dbo.OrganizationSettings os          
              
 DECLARE @WeekEndDate DATE,@MonthEndDate DATE;        
              
 SELECT @WeekEndDate=DATEADD(DAY, 7 - DATEPART(WEEKDAY, @CurrentDateTime), CAST(@CurrentDateTime AS DATE));              
 SELECT @MonthEndDate=EOMONTH(@CurrentDateTime);             
            
 IF((@InvoiceGenerationFrequency = 2 AND Convert(DATE,@CurrentDateTime) != @WeekEndDate) OR             
  (@InvoiceGenerationFrequency = 3 AND Convert(DATE,@CurrentDateTime) != @MonthEndDate))            
 BEGIN            
  SELECT 1 AS TransactionResultId;             
  RETURN;            
 END            
            
            
             
 DECLARE @TmpTable TABLE             
 (ScheduleID BIGINT,StartDate DATETIME,PayorID BIGINT,Rate DECIMAL(18,2), PerUnitQuantity DECIMAL(18,2),            
 UnitType INT,ServiceCodeID BIGINT,PayorInvoiceType INT,ReferralID INT,EmployeeID INT,ServiceTime BIGINT,            
 EmployeeVisitID BIGINT,EmployeeVisitNoteID BIGINT,WeekNumber INT,MonthNumber INT,DateYear INT)                
              
 INSERT INTO @TmpTable                
 Select SM.ScheduleID,SM.StartDate,P.PayorID,PSM.Rate,PSM.PerUnitQuantity,PSM.UnitType,PSM.ServiceCodeID,            
 P.PayorInvoiceType,SM.ReferralID,SM.EmployeeID,EVN.ServiceTime,EV.EmployeeVisitID,EVN.EmployeeVisitNoteID,                
 WeekNumber= datepart(week, SM.StartDate),MonthNumber= datepart(mm, SM.StartDate),DateYear =datepart(yyyy,SM.StartDate)                
 FROM EmployeeVisits EV                
 INNER JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID           
 --AND EVN.Description IS NULL           
 AND (EVN.Description IS NULL OR (EVN.Description IS NOT NULL AND EVN.ServiceTime>0))          
 AND EVN.IsDeleted=0                
 INNER JOIN ScheduleMasters SM ON SM.ScheduleID=EV.ScheduleID AND SM.PayorID IS NOT NULL AND SM.IsDeleted=0                
 INNER JOIN Payors P ON P.PayorID=SM.PayorID AND P.PayorInvoiceType=2 AND P.IsDeleted=0                
 LEFT JOIN PayorServiceCodeMapping PSM ON PSM.PayorID=SM.PayorID AND PSM.ServiceCodeID = EVN.ServiceCodeID AND PSM.IsDeleted=0                
 WHERE EV.IsDeleted=0 AND PSM.ServiceCodeID IS NOT NULL AND EVN.IsInvoiceGenerated=0                
                
 --SELECT * FROM @TmpTable tt                 
                
 DECLARE @TmpInvoiceTable TABLE             
 (TempInvoiceID UniqueIdentifier default NewId(),InvoiceDate DATETIME,InvoiceType INT,InvoiceStatus NVARCHAR(100),PayAmount DECIMAL(18,2),            
 PaidAmount DECIMAL(18,2),ReferralID BIGINT,ScheduleID BIGINT,WeekNumber INT,MonthNumber INT,DateYear INT)                
                
 DECLARE @TmpInvoiceTransactionTable             
 TABLE (ReferralInvoiceTransactionID BIGINT IDENTITY(1,1),InvoiceID BIGINT,ScheduleID BIGINT,EmployeeID BIGINT,EmployeeVisitID BIGINT,              
 EmployeeVisitNoteID BIGINT, Rate DECIMAL(18,2),PerUnitQuantity DECIMAL(18,2),Amount DECIMAL(18,2),ServiceTime BIGINT)                
                
 DECLARE @UnitTypeVisit INT= 2                
                
BEGIN TRANSACTION trans                                                                                              
BEGIN TRY                                  
                     
 IF(@InvoiceGenerationFrequency = 1)  -- Schedule Wise              
 BEGIN               
                
  DECLARE @outputForSchedulewise TABLE (TempInvoiceID UniqueIdentifier,ScheduleID BIGINT)              
                  
  Insert Into @TmpInvoiceTable             
  (InvoiceDate,InvoiceType,InvoiceStatus,PayAmount,PaidAmount,ReferralID,ScheduleID)              
  OUTPUT inserted.TempInvoiceID,inserted.ScheduleID INTO @outputForSchedulewise                
  SELECT InvoiceDate = @CurrentDateTime,InvoiceType = @InvoiceGenerationFrequency,InvoiceStatus = 'NEW',              
  PayAmount = SUM(            
     CASE WHEN tt.UnitType=@UnitTypeVisit THEN tt.Rate               
     ELSE CONVERT(DECIMAL(18,2),CONVERT(DECIMAL(18,2),ROUND(tt.ServiceTime/tt.PerUnitQuantity,0,0))*tt.Rate)             
     END),                
  PaidAmount=0,ReferralID = tt.ReferralID,ScheduleID = tt.ScheduleID                
  FROM @TmpTable tt                
  GROUP BY tt.ReferralID,tt.ScheduleID,tt.PayorInvoiceType,tt.ScheduleID              
              
            
  DECLARE @outputReferralInvoices TABLE (ReferralInvoiceID BIGINT,TempInvoiceID UniqueIdentifier)              
            
  Insert Into ReferralInvoices     
  (InvoiceDate,InvoiceType,InvoiceStatus,PayAmount,ReferralID,IsDeleted,TempInvoiceID,InvoiceTaxRate,InvoiceDueDate)            
  OUTPUT inserted.ReferralInvoiceID,inserted.TempInvoiceID INTO @outputReferralInvoices               
  SELECT InvoiceDate,InvoiceType,1,PayAmount=PayAmount + ((PayAmount * @InvoiceTaxRate) / 100),ReferralID,0,TempInvoiceID,@InvoiceTaxRate,    
  dateadd(dd,@InvoiceDueDays,@CurrentDateTime)             
  FROM @TmpInvoiceTable   
    
              
            
  Insert Into @TmpInvoiceTransactionTable             
  (InvoiceID,ScheduleID,EmployeeID,EmployeeVisitID,EmployeeVisitNoteID,Rate,PerUnitQuantity,Amount,ServiceTime)                
  SELECT ori.ReferralInvoiceID,tt.ScheduleID,EmployeeID,EmployeeVisitID,EmployeeVisitNoteID,Rate,PerUnitQuantity,              
  Amount= CASE WHEN tt.UnitType=2 THEN tt.Rate                
    ELSE CONVERT(DECIMAL(18,2),CONVERT(DECIMAL(18,2),ROUND(tt.ServiceTime/tt.PerUnitQuantity,0,0))*tt.Rate)             
    END,            
  ServiceTime                
  FROM @TmpTable tt              
  Inner JOIN @outputForSchedulewise O On O.ScheduleID=tt.ScheduleID            
  INNER JOIN @outputReferralInvoices ori ON ori.TempInvoiceID=o.TempInvoiceID            
            
  INSERT INTO ReferralInvoiceTransactions            
  (ReferralInvoiceID,ScheduleID,EmployeeID,EmployeeVisitID,EmployeeVisitNoteID,Rate,PerUnitQuantity,Amount,ServiceTime,IsDeleted)              
  SELECT InvoiceID,ScheduleID,EmployeeID,EmployeeVisitID,EmployeeVisitNoteID,Rate,PerUnitQuantity,Amount,ServiceTime,0             
  FROM            
  @TmpInvoiceTransactionTable                
            
  UPDATE evn                 
  SET evn.IsInvoiceGenerated=1                
  From @TmpInvoiceTransactionTable T                
  INNER JOIN EmployeeVisitNotes evn ON evn.EmployeeVisitNoteID= T.EmployeeVisitNoteID                
            
  --UPDATE ReferralInvoices            
  --SET TempInvoiceID=NULL             
  --WHERE TempInvoiceID IN (SELECT TempInvoiceID FROM @outputReferralInvoices)            
 END                
                
 IF(@InvoiceGenerationFrequency = 2 AND Convert(DATE,@CurrentDateTime)=@WeekEndDate)  -- Weekly              
 --IF(@InvoiceGenerationFrequency = 2)  -- Weekly              
 BEGIN                
               
  DECLARE @outputFowWeekly TABLE (TempInvoiceID UniqueIdentifier,WeekNumber INT,DateYear INT,ReferralID bigint)            
                  
  Insert Into @TmpInvoiceTable                 
  (InvoiceDate,InvoiceType,InvoiceStatus,PayAmount,PaidAmount,ReferralID,WeekNumber,DateYear)                
  OUTPUT inserted.TempInvoiceID,inserted.WeekNumber,inserted.DateYear,inserted.ReferralID INTO @outputFowWeekly                
  SELECT             
  InvoiceDate = @CurrentDateTime,InvoiceType = @InvoiceGenerationFrequency,InvoiceStatus = 'NEW',                
  PayAmount = SUM(            
     CASE WHEN tt.UnitType=@UnitTypeVisit THEN tt.Rate                 
     ELSE CONVERT(DECIMAL(18,2),CONVERT(DECIMAL(18,2),ROUND(tt.ServiceTime/tt.PerUnitQuantity,0,0))*tt.Rate)             
     END),                
  PaidAmount=0,ReferralID = tt.ReferralID,--ScheduleID = tt.ScheduleID,            
  WeekNumber= datepart(week, tt.StartDate),DateYear =datepart(yyyy,tt.StartDate)                
  FROM @TmpTable tt                
  GROUP BY datepart(week, tt.StartDate),datepart(yyyy,tt.StartDate),tt.ReferralID,tt.PayorInvoiceType--,tt.ScheduleID                
                
  DECLARE @outputForWeeklyInvoice TABLE (ReferralInvoiceID BIGINT,TempInvoiceID UniqueIdentifier)              
            
  Insert Into ReferralInvoices     
  (InvoiceDate,InvoiceType,InvoiceStatus,PayAmount,ReferralID,IsDeleted,TempInvoiceID,InvoiceTaxRate,InvoiceDueDate)            
  OUTPUT inserted.ReferralInvoiceID,inserted.TempInvoiceID INTO @outputForWeeklyInvoice               
  SELECT InvoiceDate,InvoiceType,1,PayAmount=PayAmount + ((PayAmount * @InvoiceTaxRate) / 100),ReferralID,0,TempInvoiceID,@InvoiceTaxRate,    
  dateadd(dd,@InvoiceDueDays,@CurrentDateTime)               
  FROM @TmpInvoiceTable            
            
            
  Insert Into @TmpInvoiceTransactionTable                
  (InvoiceID,ScheduleID,EmployeeID,EmployeeVisitID,EmployeeVisitNoteID,Rate,PerUnitQuantity,Amount,ServiceTime)                
  SELECT                 
  ofwi.ReferralInvoiceID,tt.ScheduleID,EmployeeID,                
  EmployeeVisitID,EmployeeVisitNoteID,                
  Rate,PerUnitQuantity,                
  Amount= CASE WHEN tt.UnitType=@UnitTypeVisit THEN tt.Rate                 
  ELSE CONVERT(DECIMAL(18,2),CONVERT(DECIMAL(18,2),ROUND(tt.ServiceTime/tt.PerUnitQuantity,0,0))*tt.Rate)             
    END,                
  ServiceTime                
  FROM @TmpTable tt                
  INNER JOIN @outputFowWeekly O On O.WeekNumber=tt.WeekNumber AND O.DateYear=tt.DateYear AND O.ReferralID=tt.ReferralID            
  INNER JOIN @outputForWeeklyInvoice ofwi ON ofwi.TempInvoiceID=o.TempInvoiceID              
              
  INSERT INTO ReferralInvoiceTransactions            
  (ReferralInvoiceID,ScheduleID,EmployeeID,EmployeeVisitID,EmployeeVisitNoteID,Rate,PerUnitQuantity,Amount,ServiceTime,IsDeleted)              
  SELECT InvoiceID,ScheduleID,EmployeeID,EmployeeVisitID,EmployeeVisitNoteID,Rate,PerUnitQuantity,Amount,ServiceTime,0             
  FROM            
  @TmpInvoiceTransactionTable                
            
  UPDATE evn                 
  SET evn.IsInvoiceGenerated=1                
  From @TmpInvoiceTransactionTable T                
  INNER JOIN EmployeeVisitNotes evn ON evn.EmployeeVisitNoteID= T.EmployeeVisitNoteID                
               
 END                
                
 IF(@InvoiceGenerationFrequency = 3 AND Convert(DATE,@CurrentDateTime)=@MonthEndDate) -- Monthly               
 --IF(@InvoiceGenerationFrequency = 3) -- Monthly               
 BEGIN                
            
  DECLARE @outputForMonthly TABLE (TempInvoiceID UniqueIdentifier,MonthNumber INT,DateYear INT,ReferralID bigint)                
                
  Insert Into @TmpInvoiceTable                 
  (InvoiceDate,InvoiceType,InvoiceStatus,PayAmount,PaidAmount,ReferralID,MonthNumber,DateYear)                
  OUTPUT inserted.TempInvoiceID,inserted.MonthNumber,inserted.DateYear,inserted.ReferralID INTO @outputForMonthly                
  SELECT                 
  InvoiceDate = @CurrentDateTime,InvoiceType = @InvoiceGenerationFrequency,InvoiceStatus = 'NEW',                
  PayAmount = SUM(            
     CASE WHEN tt.UnitType=@UnitTypeVisit THEN tt.Rate                 
     ELSE CONVERT(DECIMAL(18,2),CONVERT(DECIMAL(18,2),ROUND(tt.ServiceTime/tt.PerUnitQuantity,0,0))*tt.Rate)             
     END),                
  PaidAmount=0,ReferralID = tt.ReferralID,--ScheduleID = tt.ScheduleID,                
  MonthNumber= datepart(mm, tt.StartDate),DateYear =datepart(yyyy,tt.StartDate)                
  FROM @TmpTable tt                
  GROUP BY datepart(mm, tt.StartDate),datepart(yyyy,tt.StartDate),tt.ReferralID,tt.PayorInvoiceType--,tt.ScheduleID                
              
  DECLARE @outputForMonthlyInvoice TABLE (ReferralInvoiceID BIGINT,TempInvoiceID UniqueIdentifier)              
            
  Insert Into ReferralInvoices     
  (InvoiceDate,InvoiceType,InvoiceStatus,PayAmount,ReferralID,IsDeleted,TempInvoiceID,InvoiceTaxRate,InvoiceDueDate)            
  OUTPUT inserted.ReferralInvoiceID,inserted.TempInvoiceID INTO @outputForMonthlyInvoice               
  SELECT InvoiceDate,InvoiceType,1,PayAmount=PayAmount + ((PayAmount * @InvoiceTaxRate) / 100),ReferralID,0,TempInvoiceID,@InvoiceTaxRate,    
  dateadd(dd,@InvoiceDueDays,@CurrentDateTime)               
  FROM @TmpInvoiceTable            
            
  Insert Into @TmpInvoiceTransactionTable                
  (InvoiceID,ScheduleID,EmployeeID,EmployeeVisitID,EmployeeVisitNoteID,Rate,PerUnitQuantity,Amount,ServiceTime)                
  SELECT                 
  ofmi.ReferralInvoiceID,tt.ScheduleID,EmployeeID,EmployeeVisitID,EmployeeVisitNoteID,Rate,PerUnitQuantity,                
  Amount= CASE WHEN tt.UnitType=@UnitTypeVisit THEN tt.Rate                 
    ELSE CONVERT(DECIMAL(18,2),CONVERT(DECIMAL(18,2),ROUND(tt.ServiceTime/tt.PerUnitQuantity,0,0))*tt.Rate)             
    END,                
  ServiceTime                
  FROM @TmpTable tt                
  Inner JOIN @outputForMonthly O On O.MonthNumber=tt.MonthNumber AND O.DateYear=tt.DateYear AND O.ReferralID=tt.ReferralID               
  INNER JOIN @outputForMonthlyInvoice ofmi ON ofmi.TempInvoiceID=O.TempInvoiceID                
              
  INSERT INTO ReferralInvoiceTransactions            
  (ReferralInvoiceID,ScheduleID,EmployeeID,EmployeeVisitID,EmployeeVisitNoteID,Rate,PerUnitQuantity,Amount,ServiceTime,IsDeleted)              
  SELECT InvoiceID,ScheduleID,EmployeeID,EmployeeVisitID,EmployeeVisitNoteID,Rate,PerUnitQuantity,Amount,ServiceTime,0             
  FROM            
  @TmpInvoiceTransactionTable                
            
  UPDATE evn                 
  SET evn.IsInvoiceGenerated=1                
  From @TmpInvoiceTransactionTable T                
  INNER JOIN EmployeeVisitNotes evn ON evn.EmployeeVisitNoteID= T.EmployeeVisitNoteID               
 END                
            
  SELECT 1 AS TransactionResultId;             
                                                                             
  IF @@TRANCOUNT > 0                                                                                              
  BEGIN                                                                                               
   COMMIT TRANSACTION trans                                                                                      
  END                                           
END TRY                                                                        
BEGIN CATCH                                                                
  SELECT -1 AS TransactionResultId,ERROR_MESSAGE() AS ErrorMessage;                                                                                              
  IF @@TRANCOUNT > 0                                                                                              
  BEGIN                                                                                              
   ROLLBACK TRANSACTION trans                                                                                               
  END                                                                                
END CATCH                                                                            
            
END
