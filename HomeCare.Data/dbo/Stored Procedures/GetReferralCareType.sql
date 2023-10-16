 --  EXEC GetReferralCareType         
CREATE procedure [dbo].[GetReferralCareType]                                                                                       
AS                                                  
BEGIN           
          
WITH CTEList AS         
(          
SELECT *,COUNT(T1.CareType) OVER() AS Count FROM                                                   
  (       
SELECT D.DDMasterID AS CareTypeID, D.Title AS CareType, COUNT(1) AS CareTypeCount FROM Referrals R   
INNER JOIN DDMaster D ON D.DDMasterID IN (select val from GetCSVTable(R.CareTypeIds))          
WHERE R.IsDeleted=0 AND D.IsDeleted=0 AND D.ItemType=1  GROUP By  D.DDMasterID, D.Title) AS T1  )        
  
SELECT * FROM CTEList              
 outer apply(select sum(ap.CareTypeCount) as TotalCareType from CTEList ap) as TotalCareType                                                                
          
END 