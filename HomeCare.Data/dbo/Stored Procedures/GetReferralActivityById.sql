CREATE PROC [dbo].[GetReferralActivityById]
@ReferralId INT,@Month nvarchar(200), @Year int
AS BEGIN

	--SELECT * FROM ReferralActivityCategory

	select 
	 Category,b.Name,[ReferralId], [Month], [Year], a.[ReferralActivityCategoryId], [Day1], [Day2], [Day3], [Day4], [Day5], [Day6], [Day7], [Day8], [Day9], [Day10], [Day11], [Day12], [Day13], [Day14], [Day15], [Day16], [Day17], [Day18], [Day19], [Day20], 
	 [Day22], [Day21], [Day23], [Day24], [Day25], [Day26], [Day27], [Day28], [Day29], [Day30], [Day31]
	from ReferralActivityList a join ReferralActivityCategory b 
		on a.ReferralActivityCategoryId = b.ReferralActivityCategoryId
		JOIN ReferralActivityMaster c on c.ReferralActivityMasterId=a.ReferralActivityMasterId
	where [Month]=@Month and [Year]= @Year and ReferralId=@ReferralId
END
