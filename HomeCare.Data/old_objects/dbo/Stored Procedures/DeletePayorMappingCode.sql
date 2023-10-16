CREATE PROCEDURE [dbo].[DeletePayorMappingCode]        
@PayorID bigint=null,                       
@ServiceCode varchar(30)=null,                    
@ModifierID bigint=0,                                                    
@POSStartDate date=null,                            
@POSEndDate date=null,                  
@IsDeleted BIGINT = -1,      
@CareType int = -1,     
@UPCRate varchar(10),    
@NegRate varchar(10),    
@SORTEXPRESSION VARCHAR(100),                              
@SORTTYPE VARCHAR(10),                            
@FROMINDEX INT,                            
@PAGESIZE INT  ,   
@ListOfIdsInCSV varchar(300)=null,                    
@IsShowList bit            
AS                    
BEGIN                        
                    
 IF(LEN(@ListOfIdsInCSV)>0)                    
 BEGIN                  
  UPDATE PayorServiceCodeMapping SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END               
  WHERE PayorServiceCodeMappingID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))                         
 END                    
 IF(@IsShowList=1)                    
 BEGIN                    
  EXEC GetPayorServiceCodeMappingList @PayorID,@ServiceCode,@ModifierID,@POSStartDate,@POSEndDate,@IsDeleted,  
  @CareType,@UPCRate,@NegRate,@SORTEXPRESSION,@SORTTYPE,@FROMINDEX,@PAGESIZE           
 END                    
END
