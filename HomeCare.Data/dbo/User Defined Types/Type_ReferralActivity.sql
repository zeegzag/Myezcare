﻿/****** Object:  UserDefinedTableType [dbo].[Type_ReferralActivity]    Script Date: 2/7/2022 7:04:52 AM ******/
CREATE TYPE [dbo].[Type_ReferralActivity] AS TABLE (
    [ReferralActivityMasterId]   BIGINT         NULL,
    [ReferralActivityCategoryId] BIGINT         NULL,
    [Category]                   NVARCHAR (200) NULL,
    [Name]                       NVARCHAR (200) NULL,
    [Day1]                       BIT            NULL,
    [Day2]                       BIT            NULL,
    [Day3]                       BIT            NULL,
    [Day4]                       BIT            NULL,
    [Day5]                       BIT            NULL,
    [Day6]                       BIT            NULL,
    [Day7]                       BIT            NULL,
    [Day8]                       BIT            NULL,
    [Day9]                       BIT            NULL,
    [Day10]                      BIT            NULL,
    [Day11]                      BIT            NULL,
    [Day12]                      BIT            NULL,
    [Day13]                      BIT            NULL,
    [Day14]                      BIT            NULL,
    [Day15]                      BIT            NULL,
    [Day16]                      BIT            NULL,
    [Day17]                      BIT            NULL,
    [Day18]                      BIT            NULL,
    [Day19]                      BIT            NULL,
    [Day20]                      BIT            NULL,
    [Day21]                      BIT            NULL,
    [Day22]                      BIT            NULL,
    [Day23]                      BIT            NULL,
    [Day24]                      BIT            NULL,
    [Day25]                      BIT            NULL,
    [Day26]                      BIT            NULL,
    [Day27]                      BIT            NULL,
    [Day28]                      BIT            NULL,
    [Day29]                      BIT            NULL,
    [Day30]                      BIT            NULL,
    [Day31]                      BIT            NULL);
