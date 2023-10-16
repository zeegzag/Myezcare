
-- =============================================  
-- Author:  Pallav Saxena  
-- Create date: 06/07/2021  
-- Description: Function to get the local Date format based on the organization preference settings  
-- =============================================  
Create FUNCTION [dbo].[fn_getDateFormat]   
(  
 -- Add the parameters for the function here  
 @dbname nvarchar(max)  
)  
RETURNS nvarchar(max)  
AS  
BEGIN  
 -- Declare the return variable here  
 DECLARE @Result nvarchar(max)  
  
Declare @dateformat nvarchar(max)  
  
select @dateformat=DateFormat from OrganizationPreference op inner join Organizations o on o.OrganizationID=op.OrganizationID where o.DBName=@dbname  
  set @dateformat=
  case when @dateformat='MM/DD/YYYY' then  'MM/dd/yyyy'
   when @dateformat='DD/MM/YYYY' then  'dd/MM/yyyy'
   when @dateformat='YYYYMMDD' then  'yyyyMMdd'
   when @dateformat='DD.MM.YYYY' then  'dd.MM.yyyy'
   when @dateformat='MM.DD.YYYY' then  'MM.dd.yyyy'
   when @dateformat='MMM/DD/YYYY' then  'MMM/dd/yyyy'
   when @dateformat='DD/MMM/YYYY' then  'dd/MMM/yyyy'
   when @dateformat='DD.MMM.YYYY' then  'dd.MMM.yyyy'
   when @dateformat='MMM.DD.YYYY' then  'MMM.dd.yyyy'
  end 
  
 -- Add the T-SQL statements to compute the return value here  
   
  
 -- Return the result of the function  
 RETURN @dateformat  
  
END