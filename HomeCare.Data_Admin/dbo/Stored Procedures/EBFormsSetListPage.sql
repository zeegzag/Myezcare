/*  
Created by : Neeraj Sharma  
Created Date: 14 August 2020  
Updated by :  
Updated Date :  
  
Purpose: This stored procedure is used to get EBForms Data  
  
*/  
CREATE PROCEDURE [dbo].[EBFormsSetListPage]    
AS    
BEGIN    
    
SELECT * FROM EbMarkets    
SELECT * FROM EbCategories    
    
END