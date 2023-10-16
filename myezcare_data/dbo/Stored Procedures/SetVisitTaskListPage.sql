CREATE PROCEDURE [dbo].[SetVisitTaskListPage]        
@ServiceCodeTypeID INT  ,  
@DDType_VisitType int,                               
@DDType_CareType int                              
AS        
BEGIN        
        
 SELECT Name=ServiceCode+ ' - ' +ServiceName, Value=ServiceCodeID FROM ServiceCodes WHERE ServiceCodeType=@ServiceCodeTypeID        
        
 SELECT * FROM VisitTaskCategories Where ParentCategoryLevel is null   
    
 SELECT Name=Title,Value=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_VisitType            
 SELECT Name=Title,Value=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_CareType            
END
