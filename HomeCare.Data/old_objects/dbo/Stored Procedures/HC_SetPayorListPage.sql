
CREATE PROCEDURE  [dbo].[HC_SetPayorListPage]     
@DDType_PayerGroup int                                
AS                                    
BEGIN                                    
 SELECT Name=Title,Value=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_PayerGroup                      
 END
 
