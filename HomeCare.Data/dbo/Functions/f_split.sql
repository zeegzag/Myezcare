CREATE function [dbo].[f_split]
(
@param nvarchar(max), 
@delimiter char(1)
)
returns @t table (val nvarchar(max), seq int)
as
begin
set @param += @delimiter

;with a as
(
select cast(1 as bigint) f, charindex(@delimiter, @param) t, 1 seq
union all
select t + 1, charindex(@delimiter, @param, t + 1), seq + 1
from a
where charindex(@delimiter, @param, t + 1) > 0
)
insert @t
select substring(@param, f, t - f), seq from a
option (maxrecursion 0)
return
end