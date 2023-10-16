--UpdatedBy: Akhilesh Kamal      
--UpdatedDate 02/march/2020      
--Description: For select one dx code      
 --   SELECT * FROM DDMaster WHERE ItemType=26  
--  EXEC GetSpecialist 'S', '','', ''   
 --  EXEC GetSpecialist @Specialist = 'Nurse Practitioner', @Name = 'HAMILTON, S. MARIE', @NPI = 'HAMILTON, S. MARIE', @PracticeAddress = ''       
create PROCEDURE [dbo].[GetSpecialist]         
@Specialist NVARCHAR(200)=null,          
@Name NVARCHAR(200)=null,          
@NPI NVARCHAR(200)=null,          
@PracticeAddress NVARCHAR(MAX) =null       
--@IsDeleted BIGINT        
AS          
BEGIN          
         
    -- putted top 1    
 -- kundan - 2-5-2020    
 select TOP 1 DDMasterID as PhysicianTypeID,Title  as Specialist       
    from DDMaster DM   
 inner join lu_DDMasterTypes dt on dt.DDMasterTypeID=dm.ItemType             
 WHERE              
  --DM.Title = @Specialist  
   DM.Title LIKE '%'+@Specialist+'%'       
   AND          
   IsDeleted!=1  AND dt.Name='Speciality'      
        
          
END        
--END 