--CreatedBy: Abhishek Gautam
--CreatedDate: 10 sept 2020
--Description: Delete DMAS99 form

CREATE PROC [dbo].[DeleteDMAS99]  
@Dmas99ID BIGINT=0  
AS  
BEGIN  
  
UPDATE DMAS99 SET IsDeleted=1 WHERE Dmas99ID=@Dmas99ID  
SELECT 1 RETURN;  

END