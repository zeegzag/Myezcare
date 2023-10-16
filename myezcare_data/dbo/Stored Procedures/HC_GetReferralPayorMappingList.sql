-- exec [HC_GetReferralPayorMappingList] 14232,null,0,-1,null,null,null,'ASC',1,10                    
CREATE PROCEDURE [dbo].[HC_GetReferralPayorMappingList]                                      
@ReferralID bigint=null,                                       
@PayorName nvarchar(50)=null,                                    
@Precedence int=0,
@IsDeleted int = -1,                      
@PayorEffectiveDate date=null,                                            
@PayorEffectiveEndDate date=null,                                  
                    
@SORTEXPRESSION VARCHAR(100),                                              
@SORTTYPE VARCHAR(10),                                            
@FROMINDEX INT,                                            
@PAGESIZE INT                                             
AS                                                      
BEGIN                                                        
;WITH CTEReferralPayorMapping AS                                                  
 (                                                       
	SELECT *,COUNT(ReferralPayorMappingID) OVER() AS COUNT FROM                                               
	(                                                      
		SELECT ROW_NUMBER() OVER (ORDER BY                                                  
                                                  
	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralPayorMappingID' THEN ReferralPayorMappingID END END ASC,                                                  
	CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralPayorMappingID' THEN ReferralPayorMappingID END END DESC,                                                  
                                  
	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PayorName' THEN PayorName END END ASC,                                    
	CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PayorName' THEN PayorName END END DESC,                             
                          
	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Precedence' THEN Precedence END END ASC,                                    
	CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Precedence' THEN Precedence END END DESC,                                                   
                              
	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'StarDate' THEN  CONVERT(date, RPM.PayorEffectiveDate, 105)     END END ASC,                                                  
	CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'StarDate' THEN  CONVERT(date, RPM.PayorEffectiveDate, 105)     END END DESC,                                                  
                              
	CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EndDate' THEN  CONVERT(date, RPM.PayorEffectiveEndDate, 105)     END END ASC,                                                  
	CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EndDate' THEN  CONVERT(date, RPM.PayorEffectiveEndDate, 105)     END END DESC                                                 
                       
    ) AS ROW,
	RPM.ReferralPayorMappingID,RPM.PayorID,P.PayorName,RPM.PayorEffectiveDate,RPM.PayorEffectiveEndDate,RPM.Precedence,RPM.IsDeleted,
	RPM.IsPayorNotPrimaryInsured,RPM.InsuredId,RPM.InsuredFirstName,RPM.InsuredMiddleName,RPM.InsuredLastName,RPM.InsuredAddress,RPM.InsuredCity,RPM.InsuredState,
	RPM.InsuredZipCode,RPM.InsuredPhone,RPM.InsuredPolicyGroupOrFecaNumber,RPM.InsuredDateOfBirth,RPM.InsuredGender,RPM.InsuredEmployersNameOrSchoolName
	FROM  ReferralPayorMappings RPM  
	left Join Referrals R on R.ReferralID= RPM.ReferralID
	left Join Payors P on P.PayorID=RPM.PayorID                                     
	WHERE                    
	(@ReferralID =(case when @ReferralID=0 then @ReferralID else RPM.ReferralID  end )) AND                               
	((@IsDeleted=-1) OR (RPM.IsDeleted=@IsDeleted)) AND                     
	((@PayorName IS NULL) OR (P.PayorName LIKE '%' + @PayorName+ '%')) AND                    
	((@Precedence =0) or RPM.Precedence= @Precedence ) AND                    
	((@PayorEffectiveDate is null OR RPM.PayorEffectiveDate >= @PayorEffectiveDate) and (@PayorEffectiveEndDate is null OR RPM.PayorEffectiveEndDate<= @PayorEffectiveEndDate))
  
   ) AS P1                              
 )                                
 SELECT * FROM CTEReferralPayorMapping WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)                                    
END
