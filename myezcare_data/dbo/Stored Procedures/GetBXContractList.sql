--select * from ReferralBehaviorContracts

CREATE PROCEDURE [dbo].[GetBXContractList]                            
@ReferralID bigint=0,
@SORTEXPRESSION NVARCHAR(100),             
@SORTTYPE NVARCHAR(10),            
@FROMINDEX INT,                            
@PAGESIZE INT                              
AS                            
BEGIN                              
;WITH CTEBXContract AS                        
 (                             
  SELECT *,COUNT(T1.ReferralBehaviorContractID) OVER() AS COUNT FROM                        
  (                            
   SELECT ROW_NUMBER() OVER (ORDER BY   
   
                        
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ReferralBehaviorContractID' THEN CONVERT(bigint,RBC.ReferralBehaviorContractID)  END END ASC,                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ReferralBehaviorContractID' THEN CONVERT(bigint,RBC.ReferralBehaviorContractID)  END END DESC,  
  
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CreatedByName' THEN CE.LastName END END ASC,                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CreatedByName' THEN CE.LastName  END END DESC,  

  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'WarningReason' THEN RBC.WarningReason END END ASC,                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'WarningReason' THEN RBC.WarningReason  END END DESC,  
  
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'WarningDate' THEN CONVERT(datetime, RBC.WarningDate, 103)  END END ASC,                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'WarningDate' THEN CONVERT(datetime, RBC.WarningDate, 103)  END END DESC,

    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CaseManagerNotifyDate' THEN CONVERT(datetime,RBC.CaseManagerNotifyDate,103)  END END ASC,                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CaseManagerNotifyDate' THEN CONVERT(datetime,RBC.CaseManagerNotifyDate,103)  END END DESC,

 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'IsActive' THEN CONVERT(bit,RBC.IsActive)  END END ASC,                        
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'IsActive' THEN CONVERT(bit,RBC.IsActive)  END END DESC   
               
    ) AS ROW,        
   
	
		RBC.ReferralBehaviorContractID,RBC.WarningDate,RBC.WarningReason,RBC.CaseManagerNotifyDate,RBC.ReferralID,RBC.IsActive,RBC.IsDeleted,RBC.CreatedDate,RBC.CreatedBy,
		RBC.UpdatedDate,RBC.UpdatedBy,CreatedByName=CE.LastName+', '+CE.FirstName
		From ReferralBehaviorContracts RBC
		INNER JOIN Employees CE ON CE.EmployeeID=RBC.CreatedBy
		WHERE RBC.ReferralID=@ReferralID 
           
   ) AS T1        
 )        
 SELECT * FROM CTEBXContract WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)END             


 --select * from ReferralBehaviorContracts
