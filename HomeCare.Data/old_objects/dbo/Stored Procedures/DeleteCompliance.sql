CREATE PROCEDURE [dbo].[DeleteCompliance]        
@DocumentName NVARCHAR(MAX) = NULL,      
@UserType INT = -1,          
@DocumentationType INT = -1,
@IsTimeBased INT = -1,
@Type NVARCHAR(50)=NULL,
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
 UPDATE Compliances SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,      
 UpdatedDate=GETUTCDATE()       
 WHERE ComplianceID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))      
END       
IF(@IsShowList=1)                      
BEGIN                      
 EXEC GetComplianceList @DocumentName,@UserType,@DocumentationType,@IsTimeBased,@Type,@IsDeleted,@SortExpression,@SortType,@FromIndex,@PageSize          
END                      
END
