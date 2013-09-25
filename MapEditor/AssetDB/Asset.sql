﻿CREATE TABLE [dbo].[Asset]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Image] IMAGE NOT NULL, 
    [Name] NCHAR(255) NOT NULL DEFAULT 'Unnamed asset', 
    [Parent] NCHAR(255) NOT NULL DEFAULT 'Other'
	

)