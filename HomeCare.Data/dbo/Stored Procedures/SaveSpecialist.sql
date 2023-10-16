--UpdatedBy: Akhilesh Kamal      
--UpdatedDate 02/march/2020      
--Description: For select one dx code      
      
--  EXEC [GetDxCode] 'a00', 'A00','', '',''        
        
CREATE PROCEDURE [dbo].[SaveSpecialist]         
--@DXCodeID BIGINT=0,         
@Specialist NVARCHAR(200),          
@Name NVARCHAR(200),          
@NPI NVARCHAR(200),          
@PracticeAddress NVARCHAR(MAX)        
--@IsDeleted BIGINT        
AS          
BEGIN      
 IF EXISTS (SELECT * FROM DDMaster WHERE  Title = @Specialist)     
  BEGIN                        
 SELECT -1 RETURN;     
END    
    
--IF(@DXCodeID=0)                             
-- BEGIN                              
  INSERT INTO DDMaster                              
  (ItemType,Title,Value,ParentID,SortOrder,Remark,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)      
  VALUES                              
  (26,@Specialist,1,0,'','',0,GETUTCDATE(),1,GETUTCDATE(),1,1);            
    
 IF(@Specialist !=NULL)    
 BEGIN    
 select  TOP 1 DM.DDMasterID,DM.ItemType,DM.Title    
    from DDMaster DM      
 inner join lu_DDMasterTypes dt on dt.DDMasterTypeID=dm.ItemType     
 WHERE         
    DM.Title LIKE '%'+@Specialist+'%'     
   AND      
   IsDeleted!=1  AND dt.Name='Specialist'    
   END      
      
END