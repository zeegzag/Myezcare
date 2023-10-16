-- =============================================
-- Author:		Kundan Kumar Rai
-- Create date: 2 March, 2020
-- Description:	This stored procedure insert latest downloaded ERAs from claim.md
-- =============================================
CREATE PROCEDURE [dbo].[HC_AddLatestERA]
(
	@Source NVARCHAR(50),
	@list LatestERAs READONLY
)
AS
BEGIN
	INSERT INTO [dbo].[LatestERAs]
           ([CheckNumber]
           ,[CheckType]
           ,[ClaimProviderName]
           ,[DownTime]
           ,[EraID]
		   ,[PaidAmount]
           ,[PaidDate]
           ,[PayerName]
           ,[PayerID]
           ,[ProviderName]
           ,[ProviderNPI]
           ,[ProviderTaxID]
           ,[RecievedTime]
           ,[Source]
           ,[IsDeleted]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[UpdatedDate]
           ,[UpdatedBy])
     SELECT [CheckNumber]
           ,[CheckType]
           ,[ClaimProviderName]
           ,[DownTime]
           ,[EraID]
		   ,[PaidAmount]
           ,[PaidDate]
           ,[PayerName]
           ,[PayerID]
           ,[ProviderName]
           ,[ProviderNPI]
           ,[ProviderTaxID]
           ,[RecievedTime]
           ,@Source
           ,[IsDeleted]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[UpdatedDate]
           ,[UpdatedBy] FROM @list le
	  WHERE 
		NOT EXISTS (SELECT * FROM [dbo].[LatestERAs] l
					WHERE le.[EraID] = l.[EraID] AND l.[IsDeleted]=0)
END