CREATE FUNCTION [dbo].[GetAge](@DOB datetime)   
--Returns true if the string is a valid email address.  
RETURNS varchar(20)  
as  
BEGIN  
		DECLARE @Year varchar(3),@Month varchar(3),@asof datetime		
		SET @asof = GETDATE()
		set @Year=DATEDIFF(year,@DOB,@asof) - CASE WHEN MONTH(@asof)*100+DAY(@asof)<MONTH(@DOB)*100+DAY(@DOB) THEN 1 ELSE 0 END
        set @Month=( DATEDIFF(month,@DOB,@asof) - CASE WHEN DAY(@asof)<DAY(@DOB) THEN 1 ELSE 0 END )  % 12    
     
		return @Year  +'.' +@Month
END