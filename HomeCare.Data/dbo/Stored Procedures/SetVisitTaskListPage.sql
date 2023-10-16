-- EXEC [SetVisitTaskListPage] 4, 9, 1  
CREATE PROCEDURE [dbo].[SetVisitTaskListPage]          
@ServiceCodeTypeID INT  ,    
@DDType_VisitType int,                                 
@DDType_CareType int                                
AS          
BEGIN          
          
 -- Ticket#1ngp92: comment where conditoin to get list Servie Code in dropdown list of Visit Task > Clone Task  
 SELECT Name=ServiceCode+ ' - ' +ServiceName, Value=ServiceCodeID FROM ServiceCodes WHERE IsDeleted=0 --WHERE ServiceCodeType=@ServiceCodeTypeID          
           
 SELECT * FROM VisitTaskCategories Where ParentCategoryLevel is null     
      
 SELECT Name=Title,Value=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_VisitType              
 SELECT Name=Title,Value=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_CareType              
   
END