/*
Created by : Neeraj Sharma
Created Date: 31 July 2020
Updated by :
Updated Date :

Purpose: This table invoide is altered for keeping detail of trnsaction once payment has done against invoice
 from authorized .net

*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Invoice ADD
	TransactionIdAuthNet nvarchar(MAX) NULL,
	ResponseCodeAuthNet nvarchar(MAX) NULL,
	MessageCodeAuthNet nvarchar(MAX) NULL,
	DescriptionAuthNet nvarchar(MAX) NULL,
	AuthCodeAuthNet nvarchar(MAX) NULL,
    NinjaInvoiceNumber bigint NULL,
	Statuscode nvarchar(MAX) NULL,
	ErrorCode nvarchar(MAX) NULL,
	ErrorText nvarchar(MAX) NULL
GO
ALTER TABLE dbo.Invoice SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
