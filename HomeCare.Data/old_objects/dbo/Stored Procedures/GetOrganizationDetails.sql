  
-- exec [GetOrganizationDetails] @DomainName='ASAPCARESTAGE',@CompanyName=null              
-- exec [GetOrganizationDetails] @DomainName=null,@CompanyName='192'              
CREATE PROCEDURE [dbo].[GetOrganizationDetails]                          
@DomainName NVARCHAR(MAX)=null,      
@CompanyName NVARCHAR(MAX)=null      
AS                                                        
BEGIN          
        
DECLARE @CurrentDate DATE=GETUTCDATE();                           
   
IF (@DomainName IS NOT NULL)   
BEGIN   
 SELECT *         
 FROM Organizations WHERE DomainName=@DomainName AND IsActive=1           
 AND @CurrentDate >= Convert(Date,StartDate)          
 --AND (@CurrentDate <= Convert(Date,EndDate) OR EndDate IS NULL)          
      
--Akhilesh 
-- 20 may 2020
-- for get releaseNotes if IsActive=1 and endDate>=current date.	      
 SELECT *         
 FROM ReleaseNotes WHERE IsDeleted=0      
 AND @CurrentDate <= Convert(Date,EndDate)  
 AND IsActive=1        
 --AND (@CurrentDate <= Convert(Date,EndDate) OR EndDate IS NULL)          
END  
ELSE  
BEGIN  
 SELECT *         
 FROM Organizations WHERE CompanyName=@CompanyName AND IsActive=1           
 AND @CurrentDate >= Convert(Date,StartDate)          
 --AND (@CurrentDate <= Convert(Date,EndDate) OR EndDate IS NULL)    
  
 select NULL;  
END  
                                   
END  