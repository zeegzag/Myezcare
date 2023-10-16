CREATE PROCEDURE [dbo].[DeleteAgency]
@NickName varchar(100)=null,  
@ShortName varchar(50)=null,  
@RegionID bigint=0,  
@ContactName varchar(100)=null,  
@Email varchar(50)=null,  
@Phone varchar(50)=null,  
@Address varchar(50)=null,  
@IsDeleted BIGINT = -1,           
@SortExpression nvarchar(100),      
@SortType nvarchar(10),            
@FromIndex int,            
@PageSize int,      
@ListOfIdsInCSV  varchar(300),        
@IsShowList bit,  
@loggedInID BIGINT       
AS        
BEGIN            
       
 IF(LEN(@ListOfIdsInCSV)>0)        
 BEGIN       
   UPDATE Agencies SET IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as BIGINT) ,UpdatedDate=GETUTCDATE()  
   WHERE AgencyID  IN (SELECT CAST(VAL AS BIGINT) FROM GETCSVTABLE(@ListOfIdsInCSV))             
  --END        
 END        
 IF(@IsShowList=1)        
 BEGIN        
  EXEC GetAgencyList  @NickName,@ShortName,@RegionID,@ContactName,@Email,@Phone ,@Address,@IsDeleted,@SortExpression,@SortType ,@FromIndex,@PageSize      
 END        
END