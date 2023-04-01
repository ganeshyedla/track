/****** Object:  UserDefinedTableType [dbo].[CustomerFilingMasterDraft]    Script Date: 31-03-2023 22:09:26 ******/
CREATE TYPE [dbo].[CustomerFilingMasterDraft] AS TABLE(
	[CustomerId] [numeric](18, 0) NOT NULL,
	[FilingID] [numeric](18, 0) NOT NULL,
	[Notes] [varchar](2000) NULL,
	[CreateDate] [datetime] NULL,
	[CreateUser] [varchar](100) NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [varchar](100) NULL,
	[status] [varchar](100) NULL,
	[BusinessOperation] [varchar](100) NULL,
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[DraftID] [numeric](18, 0) NULL,
	PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO

