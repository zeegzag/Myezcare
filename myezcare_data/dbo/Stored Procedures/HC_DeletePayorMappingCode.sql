CREATE PROCEDURE [dbo].[HC_DeletePayorMappingCode]                    
@PayorID bigint=null,                                   
@ServiceCode varchar(30)=null,                                
@ModifierID VARCHAR(100)=NULL,                                                            
@POSStartDate date=null,                                        
@POSEndDate date=null,                              
@IsDeleted BIGINT = -1,                  
@CareType int = -1,                 
@RevenueCode BIGINT=-1,                    
@Rate varchar(10),              
@SORTEXPRESSION VARCHAR(100),                                          
@SORTTYPE VARCHAR(10),                                        
@FROMINDEX INT,                                        
@PAGESIZE INT  ,     
@ListOfIdsInCSV varchar(300)=null,                                
@IsShowList bit,    
@UnitType INT = NULL,    
@PerUnitQuantity DECIMAL(18,0) = NULL,    
@RoundUpUnit INT = NULL,    
@MaxUnit INT = NULL,    
@DailyUnitLimit INT = NULL    
AS                                
BEGIN                                    
                                
 IF(LEN(@ListOfIdsInCSV)>0)                                
 BEGIN                              
  UPDATE PayorServiceCodeMapping SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END                           
  WHERE PayorServiceCodeMappingID IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))                                     
 END                                
 IF(@IsShowList=1)                                
 BEGIN                                
  EXEC HC_GetPayorServiceCodeMappingList @PayorID,@ServiceCode,@ModifierID,@POSStartDate,@POSEndDate,@IsDeleted,              
  @CareType,@RevenueCode,@Rate,@SORTEXPRESSION,@SORTTYPE,@FROMINDEX,@PAGESIZE,@UnitType,@PerUnitQuantity,@RoundUpUnit,@MaxUnit,@DailyUnitLimit    
 END       
                          
END
