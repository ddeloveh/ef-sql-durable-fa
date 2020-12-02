SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[EmployeeID] [uniqueidentifier] NOT NULL,
	[EmployeeNumber] [nvarchar](60) NOT NULL,
	[EmployeeEmails] [nvarchar](max) NULL,
	[EmployeeUpn] [nvarchar](200) NULL,
	[EmployeeDisplayName] [nvarchar](200) NULL,
	[DateCreated] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
