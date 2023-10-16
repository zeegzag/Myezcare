CREATE PROCEDURE [dbo].[GetSetAddPayorPage]                        
@PayorID BIGINT                
AS                          
BEGIN                          
 select *  from States;                          
 select *  from PayorTypes order by PayorTypeName ASC;                         
 select *  from Payors WHERE   PayorID= @PayorID;                            
 select * from Modifiers where IsDeleted=0 order by ModifierName ASC;                
 select * from PlaceOfServices where IsDeleted=0 order by PosName ASC;                         
 select *  from PayorEdi837Settings WHERE PayorID= @PayorID;        
 select Name=FacilityName, Value=FacilityID from Facilities WHERE IsDeleted=0 AND ParentFacilityID=0 ORDER BY FacilityName ASC      
 --SELECT 0;                    
 --SELECT 0;               
 --SELECT 0;               
       
      
 SELECT FacilityID FROM FacilityApprovedPayors WHERE PayorID= @PayorID --AND IsActive=1;             
 END