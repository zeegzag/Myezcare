CREATE PROCEDURE [dbo].[HC_DeleteTransportContact]              
@FirstName VARCHAR(100) = NULL,               
@Email VARCHAR(100) = NULL,               
@MobileNumber VARCHAR(100) = NULL,              
@Address VARCHAR(100) = NULL,               
@ContactType VARCHAR(100) = NULL,             
@IsDeleted BIGINT=-1,                
@SortExpression VARCHAR(100)=NULL,                  
@SortType VARCHAR(10)=NULL,                
@FromIndex INT,                
@PageSize INT,                
@ListOfIdsInCsv varchar(300),                
@IsShowList bit,                
@loggedInID BIGINT                
AS                
BEGIN                    
                 
 IF(LEN(@ListOfIdsInCsv)>0)                
 BEGIN                
                 
  UPDATE TransportContacts SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE() WHERE ContactID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv))                 
                    
 END                
                
 IF(@IsShowList=1)                
 BEGIN                
  EXEC HC_GetTransportContactList @FirstName, @Email, @MobileNumber, @Address, @ContactType, @IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize                
 END                
END 