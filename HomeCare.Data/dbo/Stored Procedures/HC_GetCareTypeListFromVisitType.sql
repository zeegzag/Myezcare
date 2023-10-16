-- HC_GetCareTypeListFromVisitType 29,1    
CREATE PROCEDURE [dbo].[HC_GetCareTypeListFromVisitType]    
@VisitTaskID BIGINT,                    
@DDType_CareType INT                    
AS                    
BEGIN                    
     
SELECT Name=Title, Value=DDMasterID FROM DDMaster WHERE ItemType=@DDType_CareType AND (ISNULL(@VisitTaskID,0) = 0 OR ParentID=@VisitTaskID) AND IsDeleted=0      
                    
END