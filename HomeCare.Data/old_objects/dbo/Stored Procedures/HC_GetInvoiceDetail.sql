
-- EXEC HC_GetInvoiceDetail 44          
CREATE PROCEDURE [dbo].[HC_GetInvoiceDetail]          
 @InvoiceId BIGINT          
AS          
BEGIN         
         
 SELECT
 ri.ReferralInvoiceID,ri.InvoiceDate,ri.PayAmount,
 isnull(ri.PaidAmount, 0) as PaidAmount,
 ri.InvoiceStatus,r.ReferralID,
 dbo.GetGeneralNameFormat(r.FirstName,r.LastName) AS ReferralName,
 ReferralAddress = c.Address,ReferralCity = c.City,ReferralState = c.State,ReferralZipCode = c.ZipCode,          
 r.AHCCCSID,ri.InvoiceTaxRate,ri.InvoiceDueDate          
 FROM ReferralInvoices ri          
 LEFT JOIN dbo.Referrals r ON ri.ReferralID = r.ReferralID          
 LEFT JOIN dbo.ContactMappings cm ON cm.ReferralID = r.ReferralID AND cm.ContactTypeID=1      
 LEFT JOIN dbo.Contacts c ON cm.ContactID = c.ContactID
 WHERE ri.ReferralInvoiceID=@InvoiceId          
          
 SELECT rit.ReferralInvoiceTransactionID,rit.EmployeeID,dbo.GetGeneralNameFormat(e.FirstName,e.LastName) AS EmployeeName,      
 rit.ScheduleID,sm.StartDate,sm.EndDate,     
 CareTypeName = DD.Title,      
 rit.Rate,rit.PerUnitQuantity,rit.Amount,rit.ServiceDate,rit.ServiceTime          
 FROM dbo.ReferralInvoiceTransactions rit          
 LEFT JOIN dbo.Employees e ON rit.EmployeeID = e.EmployeeID          
 LEFT JOIN ScheduleMasters sm ON sm.ScheduleID=rit.ScheduleID     
 LEFT JOIN DDMaster DD ON DD.DDMasterID=sm.CareTypeId 
 WHERE rit.ReferralInvoiceID=@InvoiceId  AND rit.IsDeleted=0   
 ORDER BY rit.ServiceDate, rit.EmployeeID
          
 SELECT ReferralPaymentHistoryId,PaymentDate,PaidAmount,IsDeleted           
 FROM ReferralPaymentHistories           
 WHERE ReferralInvoiceID=@InvoiceId          
 ORDER BY PaymentDate DESC          
END
