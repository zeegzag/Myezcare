-- EXEC GetContactListForAutoCompleter 'A'  
CREATE PROCEDURE [dbo].[GetContactListForAutoCompleter]  
 -- Add the parameters for the stored procedure here  
 @SearchText VARCHAR(MAX)   ,
 @PazeSize int 
 AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 select TOP (@PazeSize) * from Contacts   
 WHERE   IsDeleted=0 AND
   (  
    (
		FirstName LIKE '%'+@SearchText+'%' OR
		LastName  LIKE '%'+@SearchText+'%' OR
		FirstName +' '+LastName like '%'+@SearchText+'%' OR
		LastName +' '+FirstName like '%'+@SearchText+'%' OR
		FirstName +', '+LastName like '%'+@SearchText+'%' OR
		LastName +', '+FirstName like '%'+@SearchText+'%'
	) OR  
    Email LIKE '%'+@SearchText+'%' OR   
    Address LIKE '%'+@SearchText+'%' OR   
    Phone1 LIKE '%'+@SearchText+'%'    
   );  
END  

