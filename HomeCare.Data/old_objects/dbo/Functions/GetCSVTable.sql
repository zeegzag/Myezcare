--SELECT * FROM getCSVTable('asdsasd,asda',DEFAULT)  
-- SELECT val FROM getCSVTable('asdsasd,asda')  
CREATE FUNCTION [dbo].[GetCSVTable]  
(   
 @CSVlist varchar(max)  
)  
RETURNS @Temp Table  
(  
 id int identity(1,1),  
 val nvarchar(255)  
)  
AS  
begin  
--set @CSVlist = '1,22,333'  
declare @i int  
--declare @paramValue int  
declare @paramValue nvarchar(max)  
  
while (len(@CSVlist) > 0)  
 begin  
  set @i = CHARINDEX(',',@CSVlist)  
  if (@i != 0)  
   begin  
    SET @paramValue = SUBSTRING(@CSVlist,0,@i)   
   end  
  else   
   begin  
    SET @paramValue = @CSVlist  
   end  
  --select @paramValue  
  insert into @Temp values(@paramValue)     
  SET @CSVlist = SUBSTRING(@CSVlist, @i+1, len(@CSVlist))  
 if ( @i = 0)  
  BEGIN     
   SET @CSVlist = '''';    
   break;  
  END   
  
 END  
return   
end
