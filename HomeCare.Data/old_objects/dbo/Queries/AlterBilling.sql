/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
/*
Created by : Neeraj Sharma
Created Date: 12 August 2020
Updated by :
Updated Date :

Purpose: This is used for saving result from Autorized.net in Billing table

*/
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
ALTER TABLE dbo.BillingInformation ADD
    UpdatedDate datetime NULL, 
	Statuscode nvarchar(MAX) NULL,
	ErrorCode nvarchar(MAX) NULL,
	ErrorText nvarchar(MAX) NULL
GO
ALTER TABLE dbo.BillingInformation SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
