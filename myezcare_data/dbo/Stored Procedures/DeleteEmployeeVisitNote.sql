CREATE PROCEDURE [dbo].[DeleteEmployeeVisitNote]            
 @EmployeeVisitNoteID BIGINT = 0,        
 @EmployeeVisitID BIGINT = 0,          
 @Name NVARCHAR(100) = NULL,              
 @PatientName NVARCHAR(100) = NULL,              
 @VisitTaskDetail NVARCHAR(100) = NULL,          
 @Description NVARCHAR(100) = NULL,       
 @VisitTaskType NVARCHAR(30)=NULL,         
 @ServiceTime BIGINT=0,         
 @StartDate DATE = NULL,                                  
 @EndDate DATE = NULL,                     
 @IsDeleted int=-1,                    
 @SortExpression NVARCHAR(100),                      
 @SortType NVARCHAR(10),                    
 @FromIndex INT,                    
 @PageSize INT,                    
 @ListOfIdsInCsv varchar(300),                    
 @IsShowList bit,                    
 @loggedInID BIGINT                    
AS                    
BEGIN                    
     
Declare @invoicestatus int =null;      
Declare @invoiceID bigint =null;      
      
SELECT @invoicestatus=ri.InvoiceStatus,@invoiceID=ri.ReferralInvoiceID FROM dbo.ReferralInvoiceTransactions rit      
INNER JOIN dbo.ReferralInvoices ri ON rit.ReferralInvoiceID = ri.ReferralInvoiceID      
WHERE rit.EmployeeVisitID=@EmployeeVisitID      
      
IF((@invoicestatus IS NOT NULL) AND @invoicestatus != 1)       
BEGIN      
 -- Invoice generated and thier status is in paid or void so we can't change task.      
    
 SELECT -1;      
 IF(@IsShowList=1)                    
 BEGIN      
  EXEC GetEmployeeVisitNoteList @EmployeeVisitNoteID,@EmployeeVisitID,@Name,@PatientName,@VisitTaskDetail,@Description,    
  @VisitTaskType,@ServiceTime,@StartDate,@EndDate,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize                    
 END       
     
 RETURN;    
END      
                    
IF(LEN(@ListOfIdsInCsv)>0 )                    
BEGIN                      
 UPDATE EmployeeVisitNotes     
 SET     
 IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,    
 UpdatedDate=GETUTCDATE() WHERE EmployeeVisitNoteID in (SELECT CAST(Val AS VARCHAR(100))     
 FROM GetCSVTable(@ListOfIdsInCsv))             
END     

 DECLARE @TotalTime INT;                    
 DECLARE @TotalServiceTime INT;                    
 DECLARE @Diff INT;  

 SELECT @TotalTime=DATEDIFF(MINUTE, StartDate, EndDate) FROM ScheduleMasters         
 WHERE ScheduleID=(SELECT ScheduleID FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID)
 
 SELECT @TotalServiceTime=SUM(ServiceTime) FROM EmployeeVisitNotes         
  WHERE EmployeeVisitID=@EmployeeVisitID AND IsDeleted=0 

 IF(@TotalServiceTime>@TotalTime)                    
 BEGIN
	UPDATE EmployeeVisitNotes     
	SET     
	IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,    
	UpdatedDate=GETUTCDATE() WHERE EmployeeVisitNoteID in (SELECT CAST(Val AS VARCHAR(100))     
	FROM GetCSVTable(@ListOfIdsInCsv))
                     
	SELECT -2;
 END
 ELSE
 BEGIN
	SELECT 1;
 END
              
IF(@IsShowList=1)                    
BEGIN      
 EXEC GetEmployeeVisitNoteList @EmployeeVisitNoteID,@EmployeeVisitID,@Name,@PatientName,@VisitTaskDetail,@Description,    
 @VisitTaskType,@ServiceTime,@StartDate,@EndDate,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize                    
END     
    
    
       
IF((@invoicestatus IS NOT NULL) AND @invoicestatus = 1)       
BEGIN      
   -- Add Invoice transaction and update invoice details      
     
 UPDATE rit    
 SET     
 rit.IsDeleted=evn.IsDeleted    
 FROM    
 ReferralInvoiceTransactions rit    
 INNER JOIN EmployeeVisitNotes evn on evn.EmployeeVisitNoteID = rit.EmployeeVisitNoteID AND evn.IsInvoiceGenerated = 1     
 WHERE rit.EmployeeVisitID=@EmployeeVisitID    
     
 DECLARE @TotalAmount decimal(18,2) = null;       
 DECLARE @InvoiceTaxRate decimal(18,2) = null;    
  
 SELECT @InvoiceTaxRate=InvoiceTaxRate   
 FROM dbo.ReferralInvoices ri     
 WHERE ReferralInvoiceID=@invoiceID AND IsDeleted=0    
  
  
 SELECT @TotalAmount=sum(Amount)  
    FROM ReferralInvoiceTransactions      
    WHERE ReferralInvoiceID=@invoiceID AND IsDeleted=0    
      
    UPDATE dbo.ReferralInvoices      
    SET PayAmount = @TotalAmount+((@TotalAmount * @InvoiceTaxRate)/100)    
 WHERE ReferralInvoiceID=@invoiceID    
          
END      
    
                   
END
