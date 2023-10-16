-- EXEC HC_GetInvoiceDetail 44          
CREATE PROCEDURE [dbo].[HC_GetInvoiceDetail]          
 @InvoiceId BIGINT          
AS          
BEGIN         
         
 SELECT
 ri.ReferralInvoiceID,ri.InvoiceDate,ri.PayAmount,ri.PaidAmount,ri.InvoiceStatus,r.ReferralID,
 dbo.GetGeneralNameFormat(r.FirstName,r.LastName) AS ReferralName,
 ReferralAddress = c.Address,ReferralCity = c.City,ReferralState = c.State,ReferralZipCode = c.ZipCode,          
 r.AHCCCSID,ri.InvoiceTaxRate,ri.InvoiceDueDate          
 FROM ReferralInvoices ri          
 INNER JOIN dbo.Referrals r ON ri.ReferralID = r.ReferralID          
 INNER JOIN dbo.ContactMappings cm ON cm.ReferralID = r.ReferralID AND cm.ContactTypeID=1      
 INNER JOIN dbo.Contacts c ON cm.ContactID = c.ContactID
 WHERE ri.ReferralInvoiceID=@InvoiceId          
          
 SELECT rit.ReferralInvoiceTransactionID,rit.EmployeeID,dbo.GetGeneralNameFormat(e.FirstName,e.LastName) AS EmployeeName,      
 rit.ScheduleID,sm.StartDate,sm.EndDate,rit.EmployeeVisitNoteID,      
 VisitTaskDetail = ISNULL(v.VisitTaskDetail,evn.Description),      
 rit.Rate,rit.PerUnitQuantity,rit.Amount,evn.ServiceTime          
 FROM dbo.ReferralInvoiceTransactions rit          
 INNER JOIN dbo.Employees e ON rit.EmployeeID = e.EmployeeID          
 INNER JOIN ScheduleMasters sm ON sm.ScheduleID=rit.ScheduleID          
 INNER JOIN EmployeeVisitNotes evn ON evn.EmployeeVisitNoteID=rit.EmployeeVisitNoteID               
 LEFT JOIN ReferralTaskMappings rtm on rtm.ReferralTaskMappingID=evn.ReferralTaskMappingID                
 LEFT JOIN VisitTasks v on v.VisitTaskID=rtm.VisitTaskID               
 WHERE rit.ReferralInvoiceID=@InvoiceId  AND rit.IsDeleted=0        
          
 SELECT ReferralPaymentHistoryId,PaymentDate,PaidAmount,IsDeleted           
 FROM ReferralPaymentHistories           
 WHERE ReferralInvoiceID=@InvoiceId          
 ORDER BY PaymentDate DESC          
END
