--select  dbo.[GetNextScheduleRequestDate]('sdf') as [er]  
CREATE FUNCTION [dbo].[GetNextScheduleRequestDate](@Value varchar(500))     
--Returns true if the string is a valid email address.    
RETURNS varchar(50)    
as    
BEGIN    
 DECLARE @DataSource TABLE  
 (  
  FullDate varchar(100),  
  StartDate date,  
  EndDate date,  
  [ID] TINYINT IDENTITY(1,1)     
 )     
  
 DECLARE @ReturnValue varchar(50)=null  
   
 --DECLARE @Value VARCHAR(MAX) = '11/18/16-11/19/16,11/10/16-11/11/16,11/20/16-11/21/16,11/15/16-11/16/16'  
  
 IF(LEN(@Value)> 0)  
 BEGIN   
  DECLARE @XML xml = N'<r><![CDATA[' + REPLACE(@Value, ',', ']]></r><r><![CDATA[') + ']]></r>'  
  
  INSERT INTO @DataSource (FullDate,StartDate,EndDate)  
  SELECT RTRIM(LTRIM(T.c.value('.', 'NVARCHAR(128)'))),null,null  
  FROM @xml.nodes('//r') T(c)  
  
  --UPDATE @DataSource set StartDate=convert(datetime, left(FullDate, charindex('-', FullDate) - 1), 1),  
  --      EndDate=convert(datetime, right(FullDate, charindex('-', FullDate) - 1), 1)   
  
  UPDATE @DataSource set StartDate=convert(datetime, left(FullDate, charindex('-', FullDate) - 1)),  
        EndDate=convert(datetime, right(FullDate, charindex('-', FullDate) - 1)) 

  --SELECT [ID],FullDate,StartDate,EndDate  
  --FROM @DataSource where StartDate >= CAST(GETDATE() as DATE)  
  
  SELECT TOP 1 @ReturnValue = CONVERT(varchar(10),DATEPART(MM, StartDate)) + '/' + CONVERT(varchar(10),DATEPART(D, StartDate)) + ' - ' + CONVERT(VARCHAR(10),DATEPART(MM, EndDate)) + '/' + CONVERT(varchar(10),DATEPART(D, EndDate))  
  FROM @DataSource where StartDate >= CAST(GETDATE() as DATE) order by StartDate  
  
  return @ReturnValue  
 END  
 ELSE  
 BEGIN  
  return @ReturnValue  
 END  
  
 return @ReturnValue  
       
   
END  
