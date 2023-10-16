USE [Live_AHSAPPO]
GO

/****** Object:  StoredProcedure [dbo].[GetPayorDetailsByReferralID]    Script Date: 12/8/2020 11:05:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetPayorDetailsByReferralID]
	@ReferralID bigint
AS
BEGIN

 SELECT ReferralPayorMappings.PayorID, PayorName     
 FROM ReferralPayorMappings            
 LEFT JOIN Payors ON ReferralPayorMappings.PayorID = Payors.PayorID          

 WHERE ReferralID = @ReferralID AND ReferralPayorMappings.IsDeleted = 0
       
 ORDER BY PayorName ASC 

END
GO

