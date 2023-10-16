-- EXEC [HC_GetAlreadyExistFacility] '074118','1194196337','412111326',0
CREATE procedure [dbo].[HC_GetAlreadyExistFacility]                      
@AHCCCSID varchar(50),                      
@NPI varchar(50),                      
@EIN  varchar(50),                      
@FacilityID bigint--,                      
--@ParentFacilityID bigint                   
as                      
BEGIN  
  
SELECT top 1 *
FROM Facilities   
WHERE   
((AHCCCSID IS NOT NULL AND AHCCCSID=@AHCCCSID )  
OR   
(NPI IS NOT NULL AND NPI=@NPI ))   
AND    
FacilityID != @FacilityID   
                      
--if @ParentFacilityID=0                      
-- BEGIN                      
--  if @FacilityID>0       
--    BEGIN                      
--     SELECT top 1 * FROM Facilities WHERE (AHCCCSID=@AHCCCSID OR NPI=@NPI /*OR EIN=@EIN*/) AND  FacilityID!=@FacilityID  AND ParentFacilityID=0                
--    END                      
--  ELSE                      
--    BEGIN                      
--    SELECT top 1 * FROM Facilities WHERE (AHCCCSID=@AHCCCSID OR NPI=@NPI /*OR EIN=@EIN*/) AND ParentFacilityID=0   --ParentFacilityID!=@ParentFacilityID --AND  FacilityID!=@FacilityID                
--    END                      
-- END                       
-- ELSE      
-- if @FacilityID>0                      
--    BEGIN                      
--     SELECT top 1 * FROM Facilities WHERE FacilityID=@ParentFacilityID  --(AHCCCSID=@AHCCCSID OR NPI=@NPI OR EIN=@EIN)   AND    --(ParentFacilityID!=@ParentFacilityID AND ParentFacilityID!=0)      
--   END          
--   ELSE      
--   BEGIN      
--     SELECT top 1 * FROM Facilities WHERE  FacilityID=@ParentFacilityID --(AHCCCSID=@AHCCCSID OR NPI=@NPI OR EIN=@EIN)  AND       
--   END      
END