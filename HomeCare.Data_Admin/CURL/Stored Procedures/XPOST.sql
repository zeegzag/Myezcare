﻿CREATE PROCEDURE [CURL].[XPOST]
@H NVARCHAR (MAX) NULL, @d NVARCHAR (MAX) NULL, @url NVARCHAR (4000) NULL
AS EXTERNAL NAME [SqlClrCurl].[Curl].[Post]
