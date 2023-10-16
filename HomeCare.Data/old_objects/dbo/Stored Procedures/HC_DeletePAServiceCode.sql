--EXEC HC_DeleteReferralBillingAuthorization @SortExpression = 'ReferralBillingAuthorizationID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50', @PayorID = '0', @AuthorizationCode = '', @ReferralID = '24345', @IsDeleted  = '0', @AuthType  = 'CMS1500', @ListOfIdsInCSV = '10061', @IsShowList = 'True'      

CREATE PROCEDURE [dbo].[HC_DeletePAServiceCode]      
@ReferralBillingAuthorizationServiceCodeID BIGINT,
@LoggedIdID BIGINT
AS                                    
BEGIN                                        


UPDATE ReferralBillingAuthorizationServiceCodes SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END,
UpdatedDate = GETDATE(),
UpdatedBy =  @LoggedIdID
WHERE ReferralBillingAuthorizationServiceCodeID=@ReferralBillingAuthorizationServiceCodeID
                                    
                                  
END
