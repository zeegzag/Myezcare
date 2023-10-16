CREATE PROCEDURE [dbo].[DeleteServiceCode]      
 @ServiceCode NVARCHAR(100) = NULL,          
 @Email NVARCHAR(50) = NULL,            
 @Address NVARCHAR(100) = NULL,    
 @NPINumber NVARCHAR(20) = null,        
 @IsDeleted BIGINT = -1,            
 @SortExpression NVARCHAR(100),                        
 @SortType NVARCHAR(10),                      
 @FromIndex INT,                      
 @PageSize INT,      
 @ListOfIdsInCsv varchar(300),                  
 @IsShowList bit,                  
 @loggedInID BIGINT                  
AS                  
BEGIN               
                  
 IF(LEN(@ListOfIdsInCsv)>0)                  
	 BEGIN                    
		DECLARE @CanDelete AS INT
		SET @CanDelete = (SELECT COUNT(*) FROM PayorServiceCodeMapping WHERE IsDeleted = 0 and ServiceCodeId in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv)))
	
		-- kunal patel: Hotfix bugs service code payor mapping #2frxen
		IF(@CanDelete = 0)                  
			BEGIN
				UPDATE ServiceCodes SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END, ServiceCode = '_' + ServiceCode WHERE ServiceCodeId in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))                       
				--UPDATE ServiceCodes SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() WHERE ServiceCodeId in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))                       
				
				SELECT * FROM ServiceCodes  WHERE ServiceCodeId in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))
			
			END                  
	END 
END 
