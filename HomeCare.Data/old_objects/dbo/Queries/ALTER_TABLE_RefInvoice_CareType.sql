ALTER TABLE ReferralInvoices
ADD CareTypeID bigint

ALTER TABLE ReferralInvoiceTransactions
ADD ServiceDate date

ALTER TABLE ReferralInvoices
ADD ServiceStartDate date

ALTER TABLE ReferralInvoices
ADD ServiceEndDate date

ALTER TABLE ReferralInvoiceTransactions
DROP COLUMN EmployeeVisitNoteID