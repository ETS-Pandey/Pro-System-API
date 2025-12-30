USE [SchoolProcurement]
GO
/****** Object:  Table [dbo].[AuditLogDetails]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogDetails](
	[DetailId] [bigint] IDENTITY(1,1) NOT NULL,
	[AuditId] [bigint] NOT NULL,
	[EntityName] [nvarchar](200) NOT NULL,
	[PrimaryKey] [nvarchar](200) NULL,
	[Operation] [nvarchar](20) NOT NULL,
	[PropertyName] [nvarchar](200) NULL,
	[OldValue] [nvarchar](max) NULL,
	[NewValue] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[DetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditLogs]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogs](
	[AuditId] [bigint] IDENTITY(1,1) NOT NULL,
	[CorrelationId] [uniqueidentifier] NOT NULL,
	[EventTime] [datetime2](3) NOT NULL,
	[EventType] [nvarchar](100) NOT NULL,
	[UserId] [int] NULL,
	[UserName] [nvarchar](256) NULL,
	[TenantId] [int] NULL,
	[Source] [nvarchar](200) NULL,
	[Route] [nvarchar](400) NULL,
	[HttpMethod] [nvarchar](10) NULL,
	[ClientIp] [nvarchar](45) NULL,
	[StatusCode] [int] NULL,
	[DurationMs] [int] NULL,
	[Summary] [nvarchar](1000) NULL,
	[Extra] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[AuditId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BranchBudgetTransactions]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BranchBudgetTransactions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BranchID] [int] NOT NULL,
	[RelatedSORID] [int] NULL,
	[TransactionType] [nvarchar](20) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[BalanceAfter] [decimal](18, 2) NOT NULL,
	[Note] [nvarchar](1000) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Branches]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Branches](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[MobileNo] [nvarchar](20) NULL,
	[Website] [nvarchar](200) NULL,
	[Address] [nvarchar](500) NULL,
	[IsDelete] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedDate] [datetime2](3) NULL,
	[Budget] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactDetails]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Address] [nvarchar](1000) NULL,
	[Postcode] [nvarchar](50) NULL,
	[City] [nvarchar](200) NULL,
	[State] [nvarchar](200) NULL,
	[Country] [nvarchar](200) NULL,
	[MobileNo] [nvarchar](50) NULL,
	[EmailAddress] [nvarchar](320) NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](3) NULL,
	[BranchID] [int] NOT NULL,
	[UniqueString] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailLogs]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailLogs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BranchID] [int] NULL,
	[Name] [nvarchar](250) NULL,
	[Subject] [nvarchar](500) NULL,
	[Body] [nvarchar](max) NULL,
	[ToEmail] [nvarchar](1000) NULL,
	[FromEmail] [nvarchar](320) NULL,
	[SentDate] [datetime2](3) NULL,
	[TryCount] [int] NOT NULL,
	[IsSent] [bit] NOT NULL,
	[ErrorMessage] [nvarchar](2000) NULL,
	[IsDelete] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedAt] [datetime2](3) NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedAt] [datetime2](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailSetupDetails]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailSetupDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BranchID] [int] NOT NULL,
	[FromEmail] [nvarchar](320) NOT NULL,
	[Port] [int] NOT NULL,
	[Host] [nvarchar](500) NOT NULL,
	[EnableSsl] [bit] NOT NULL,
	[UseDefaultCredentials] [bit] NOT NULL,
	[UserName] [nvarchar](500) NULL,
	[Password] [nvarchar](1000) NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedDate] [datetime2](3) NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MasterDetails]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MasterDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[OtherName] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[ParentID] [int] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_MasterDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CategoryID] [int] NOT NULL,
	[UnitTypeID] [int] NOT NULL,
	[SalesPrice] [decimal](18, 2) NOT NULL,
	[PurchasePrice] [decimal](18, 2) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedDate] [datetime2](3) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductStocks]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductStocks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[BranchID] [int] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[ReservedQty] [decimal](18, 2) NOT NULL,
	[ReorderLevel] [decimal](18, 2) NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](3) NULL,
 CONSTRAINT [PK_ProductStocks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceOrderRequestAssignment]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceOrderRequestAssignment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SORID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Note] [nvarchar](1000) NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceOrderRequestAttachments]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceOrderRequestAttachments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SORID] [int] NOT NULL,
	[FileName] [nvarchar](500) NOT NULL,
	[FilePath] [nvarchar](2000) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceOrderRequestItems]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceOrderRequestItems](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SORID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[UnitTypeID] [int] NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[EstimatedCost] [decimal](18, 2) NULL,
	[TechnicalSpecifications] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceOrderRequests]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceOrderRequests](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UniqueString] [nvarchar](100) NOT NULL,
	[BranchID] [int] NOT NULL,
	[DepartmentID] [int] NULL,
	[PurposeDescription] [nvarchar](max) NULL,
	[AdditionalJustification] [nvarchar](max) NULL,
	[RequiredByDate] [datetime2](3) NULL,
	[UrgencyLevelID] [int] NULL,
	[Status] [nvarchar](50) NOT NULL,
	[CurrentAssignedUserID] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](3) NULL,
	[ActualCost] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SORContactMappingAttachments]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SORContactMappingAttachments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MappingID] [int] NOT NULL,
	[FileName] [nvarchar](500) NOT NULL,
	[FilePath] [nvarchar](2000) NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SORContactMappings]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SORContactMappings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BranchID] [int] NOT NULL,
	[SORID] [int] NOT NULL,
	[ContactID] [int] NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[UniqueString] [nvarchar](100) NULL,
	[Rate] [decimal](18, 2) NULL,
	[OtherDescription] [nvarchar](1000) NULL,
	[IsDelete] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedDate] [datetime2](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 16-12-2025 11:10:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[BranchID] [int] NOT NULL,
	[FirstName] [nvarchar](150) NOT NULL,
	[MiddleName] [nvarchar](150) NULL,
	[LastName] [nvarchar](150) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Password] [nvarchar](500) NOT NULL,
	[Saltkey] [nvarchar](200) NOT NULL,
	[UniqueKey] [nvarchar](200) NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedDate] [datetime2](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AuditLogDetails] ON 
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (1, 3, N'Product', N'-2147482647', N'Insert', N'ID', NULL, N'-2147482647')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (2, 3, N'Product', N'-2147482647', N'Insert', N'CategoryID', NULL, N'19')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (3, 3, N'Product', N'-2147482647', N'Insert', N'CreatedBy', NULL, N'2')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (4, 3, N'Product', N'-2147482647', N'Insert', N'CreatedDate', NULL, N'"2025-12-08T07:24:16.5019458Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (5, 3, N'Product', N'-2147482647', N'Insert', N'Description', NULL, N'"Full scape notebook"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (6, 3, N'Product', N'-2147482647', N'Insert', N'IsDeleted', NULL, N'false')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (7, 3, N'Product', N'-2147482647', N'Insert', N'Name', NULL, N'"Full scape notebook"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (8, 3, N'Product', N'-2147482647', N'Insert', N'PurchasePrice', NULL, N'20.0')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (9, 3, N'Product', N'-2147482647', N'Insert', N'SalesPrice', NULL, N'18.0')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (10, 3, N'Product', N'-2147482647', N'Insert', N'UnitTypeID', NULL, N'1')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (11, 3, N'Product', N'-2147482647', N'Insert', N'UpdatedBy', NULL, N'2')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (12, 3, N'Product', N'-2147482647', N'Insert', N'UpdatedDate', NULL, N'"2025-12-08T07:24:13.1987503Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (13, 13, N'ProductStock', N'-2147482647', N'Insert', N'ID', NULL, N'-2147482647')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (14, 13, N'ProductStock', N'-2147482647', N'Insert', N'BranchID', NULL, N'1')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (15, 13, N'ProductStock', N'-2147482647', N'Insert', N'CreatedBy', NULL, N'2')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (16, 13, N'ProductStock', N'-2147482647', N'Insert', N'CreatedDate', NULL, N'"2025-12-08T09:14:56.8285431Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (17, 13, N'ProductStock', N'-2147482647', N'Insert', N'IsDeleted', NULL, N'false')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (18, 13, N'ProductStock', N'-2147482647', N'Insert', N'ProductID', NULL, N'1')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (19, 13, N'ProductStock', N'-2147482647', N'Insert', N'Quantity', NULL, N'1000.0')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (20, 13, N'ProductStock', N'-2147482647', N'Insert', N'ReorderLevel', NULL, NULL)
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (21, 13, N'ProductStock', N'-2147482647', N'Insert', N'ReservedQty', NULL, N'0.0')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (22, 13, N'ProductStock', N'-2147482647', N'Insert', N'UpdatedBy', NULL, NULL)
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (23, 13, N'ProductStock', N'-2147482647', N'Insert', N'UpdatedDate', NULL, NULL)
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (24, 27, N'ServiceOrderRequest', N'-2147482647', N'Insert', N'ID', NULL, N'-2147482647')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (25, 27, N'ServiceOrderRequest', N'-2147482647', N'Insert', N'AdditionalJustification', NULL, N'"Need a full scape notebookNeed a full scape notebook"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (26, 27, N'ServiceOrderRequest', N'-2147482647', N'Insert', N'BranchID', NULL, N'1')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (27, 27, N'ServiceOrderRequest', N'-2147482647', N'Insert', N'CreatedBy', NULL, NULL)
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (28, 27, N'ServiceOrderRequest', N'-2147482647', N'Insert', N'CreatedDate', NULL, N'"2025-12-08T10:46:48.2572187Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (29, 27, N'ServiceOrderRequest', N'-2147482647', N'Insert', N'CurrentAssignedUserID', NULL, N'2')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (30, 27, N'ServiceOrderRequest', N'-2147482647', N'Insert', N'DepartmentID', NULL, N'13')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (31, 27, N'ServiceOrderRequest', N'-2147482647', N'Insert', N'IsDeleted', NULL, N'false')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (32, 27, N'ServiceOrderRequest', N'-2147482647', N'Insert', N'PurposeDescription', NULL, N'"Need a full scape notebook"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (33, 27, N'ServiceOrderRequest', N'-2147482647', N'Insert', N'RequiredByDate', NULL, N'"2025-12-12T10:42:59.551Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (34, 27, N'ServiceOrderRequest', N'-2147482647', N'Insert', N'Status', NULL, N'"New"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (35, 27, N'ServiceOrderRequest', N'-2147482647', N'Insert', N'UniqueString', NULL, N'"SOR-20251208-0001"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (36, 27, N'ServiceOrderRequest', N'-2147482647', N'Insert', N'UpdatedBy', NULL, NULL)
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (37, 27, N'ServiceOrderRequest', N'-2147482647', N'Insert', N'UpdatedDate', NULL, N'"2025-12-08T10:46:48.2572187Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (38, 27, N'ServiceOrderRequest', N'-2147482647', N'Insert', N'UrgencyLevelID', NULL, N'10')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (39, 28, N'ServiceOrderRequestAssignment', N'-2147482647', N'Insert', N'ID', NULL, N'-2147482647')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (40, 28, N'ServiceOrderRequestAssignment', N'-2147482647', N'Insert', N'CreatedBy', NULL, NULL)
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (41, 28, N'ServiceOrderRequestAssignment', N'-2147482647', N'Insert', N'CreatedDate', NULL, N'"2025-12-08T10:46:48.2572187Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (42, 28, N'ServiceOrderRequestAssignment', N'-2147482647', N'Insert', N'IsDeleted', NULL, N'false')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (43, 28, N'ServiceOrderRequestAssignment', N'-2147482647', N'Insert', N'Note', NULL, N'"Initial assignment"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (44, 28, N'ServiceOrderRequestAssignment', N'-2147482647', N'Insert', N'SORID', NULL, N'1')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (45, 28, N'ServiceOrderRequestAssignment', N'-2147482647', N'Insert', N'UpdatedBy', NULL, NULL)
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (46, 28, N'ServiceOrderRequestAssignment', N'-2147482647', N'Insert', N'UpdatedDate', NULL, N'"2025-12-08T10:46:48.2572187Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (47, 28, N'ServiceOrderRequestAssignment', N'-2147482647', N'Insert', N'UserID', NULL, N'2')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (48, 28, N'ServiceOrderRequestAttachment', N'-2147482647', N'Insert', N'ID', NULL, N'-2147482647')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (49, 28, N'ServiceOrderRequestAttachment', N'-2147482647', N'Insert', N'CreatedBy', NULL, NULL)
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (50, 28, N'ServiceOrderRequestAttachment', N'-2147482647', N'Insert', N'CreatedDate', NULL, N'"2025-12-08T10:46:48.2572187Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (51, 28, N'ServiceOrderRequestAttachment', N'-2147482647', N'Insert', N'FileName', NULL, N'"image.jpg"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (52, 28, N'ServiceOrderRequestAttachment', N'-2147482647', N'Insert', N'FilePath', NULL, N'"image.jpg"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (53, 28, N'ServiceOrderRequestAttachment', N'-2147482647', N'Insert', N'IsDeleted', NULL, N'false')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (54, 28, N'ServiceOrderRequestAttachment', N'-2147482647', N'Insert', N'SORID', NULL, N'1')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (55, 28, N'ServiceOrderRequestAttachment', N'-2147482647', N'Insert', N'UpdatedBy', NULL, NULL)
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (56, 28, N'ServiceOrderRequestAttachment', N'-2147482647', N'Insert', N'UpdatedDate', NULL, N'"2025-12-08T10:46:48.2572187Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (57, 28, N'ServiceOrderRequestItem', N'-2147482647', N'Insert', N'ID', NULL, N'-2147482647')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (58, 28, N'ServiceOrderRequestItem', N'-2147482647', N'Insert', N'CreatedBy', NULL, NULL)
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (59, 28, N'ServiceOrderRequestItem', N'-2147482647', N'Insert', N'CreatedDate', NULL, N'"2025-12-08T10:46:48.2572187Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (60, 28, N'ServiceOrderRequestItem', N'-2147482647', N'Insert', N'EstimatedCost', NULL, N'100.0')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (61, 28, N'ServiceOrderRequestItem', N'-2147482647', N'Insert', N'IsDeleted', NULL, N'false')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (62, 28, N'ServiceOrderRequestItem', N'-2147482647', N'Insert', N'ProductID', NULL, N'1')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (63, 28, N'ServiceOrderRequestItem', N'-2147482647', N'Insert', N'Quantity', NULL, N'5.0')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (64, 28, N'ServiceOrderRequestItem', N'-2147482647', N'Insert', N'SORID', NULL, N'1')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (65, 28, N'ServiceOrderRequestItem', N'-2147482647', N'Insert', N'TechnicalSpecifications', NULL, N'"Need for the office purpose."')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (66, 28, N'ServiceOrderRequestItem', N'-2147482647', N'Insert', N'UnitTypeID', NULL, N'1')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (67, 28, N'ServiceOrderRequestItem', N'-2147482647', N'Insert', N'UpdatedBy', NULL, NULL)
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (68, 28, N'ServiceOrderRequestItem', N'-2147482647', N'Insert', N'UpdatedDate', NULL, N'"2025-12-08T10:46:48.2572187Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (69, 28, N'ServiceOrderRequest', N'1', N'Update', N'Status', N'"New"', N'"Assigned"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (70, 39, N'Product', N'-2147482647', N'Insert', N'ID', NULL, N'-2147482647')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (71, 39, N'Product', N'-2147482647', N'Insert', N'CategoryID', NULL, N'19')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (72, 39, N'Product', N'-2147482647', N'Insert', N'CreatedBy', NULL, N'2')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (73, 39, N'Product', N'-2147482647', N'Insert', N'CreatedDate', NULL, N'"2025-12-08T12:52:44.83668Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (74, 39, N'Product', N'-2147482647', N'Insert', N'Description', NULL, N'"A4 size page bundle"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (75, 39, N'Product', N'-2147482647', N'Insert', N'IsDeleted', NULL, N'false')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (76, 39, N'Product', N'-2147482647', N'Insert', N'Name', NULL, N'"A4 size page"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (77, 39, N'Product', N'-2147482647', N'Insert', N'PurchasePrice', NULL, N'120.0')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (78, 39, N'Product', N'-2147482647', N'Insert', N'SalesPrice', NULL, N'110.0')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (79, 39, N'Product', N'-2147482647', N'Insert', N'UnitTypeID', NULL, N'2')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (80, 39, N'Product', N'-2147482647', N'Insert', N'UpdatedBy', NULL, N'2')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (81, 39, N'Product', N'-2147482647', N'Insert', N'UpdatedDate', NULL, N'"2025-12-08T12:52:44.8160246Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (82, 60, N'ContactDetail', N'-2147482647', N'Insert', N'ID', NULL, N'-2147482647')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (83, 60, N'ContactDetail', N'-2147482647', N'Insert', N'Address', NULL, N'"Sutton road"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (84, 60, N'ContactDetail', N'-2147482647', N'Insert', N'BranchID', NULL, N'1')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (85, 60, N'ContactDetail', N'-2147482647', N'Insert', N'City', NULL, N'"London"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (86, 60, N'ContactDetail', N'-2147482647', N'Insert', N'Country', NULL, N'"United Kingdom"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (87, 60, N'ContactDetail', N'-2147482647', N'Insert', N'CreatedBy', NULL, N'2')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (88, 60, N'ContactDetail', N'-2147482647', N'Insert', N'CreatedDate', NULL, N'"2025-12-08T13:33:08.4193999Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (89, 60, N'ContactDetail', N'-2147482647', N'Insert', N'EmailAddress', NULL, N'"rg@gmail.com"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (90, 60, N'ContactDetail', N'-2147482647', N'Insert', N'IsDeleted', NULL, N'false')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (91, 60, N'ContactDetail', N'-2147482647', N'Insert', N'MobileNo', NULL, N'"9876543210"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (92, 60, N'ContactDetail', N'-2147482647', N'Insert', N'Name', NULL, N'"Rajesh Gujjar"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (93, 60, N'ContactDetail', N'-2147482647', N'Insert', N'Postcode', NULL, N'"SM1 4FS"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (94, 60, N'ContactDetail', N'-2147482647', N'Insert', N'State', NULL, N'"London"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (95, 60, N'ContactDetail', N'-2147482647', N'Insert', N'UpdatedBy', NULL, NULL)
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (96, 60, N'ContactDetail', N'-2147482647', N'Insert', N'UpdatedDate', NULL, NULL)
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (97, 62, N'ContactDetail', N'-2147482647', N'Insert', N'ID', NULL, N'-2147482647')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (98, 62, N'ContactDetail', N'-2147482647', N'Insert', N'Address', NULL, N'"Sutton road"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (99, 62, N'ContactDetail', N'-2147482647', N'Insert', N'BranchID', NULL, N'1')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (100, 62, N'ContactDetail', N'-2147482647', N'Insert', N'City', NULL, N'"London"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (101, 62, N'ContactDetail', N'-2147482647', N'Insert', N'Country', NULL, N'"United Kingdom"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (102, 62, N'ContactDetail', N'-2147482647', N'Insert', N'CreatedBy', NULL, N'2')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (103, 62, N'ContactDetail', N'-2147482647', N'Insert', N'CreatedDate', NULL, N'"2025-12-08T13:37:44.4709656Z"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (104, 62, N'ContactDetail', N'-2147482647', N'Insert', N'EmailAddress', NULL, N'"rg@gmail.com"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (105, 62, N'ContactDetail', N'-2147482647', N'Insert', N'IsDeleted', NULL, N'false')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (106, 62, N'ContactDetail', N'-2147482647', N'Insert', N'MobileNo', NULL, N'"9876543210"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (107, 62, N'ContactDetail', N'-2147482647', N'Insert', N'Name', NULL, N'"Rajesh Gujjar"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (108, 62, N'ContactDetail', N'-2147482647', N'Insert', N'Postcode', NULL, N'"SM1 4FS"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (109, 62, N'ContactDetail', N'-2147482647', N'Insert', N'State', NULL, N'"London"')
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (110, 62, N'ContactDetail', N'-2147482647', N'Insert', N'UpdatedBy', NULL, NULL)
GO
INSERT [dbo].[AuditLogDetails] ([DetailId], [AuditId], [EntityName], [PrimaryKey], [Operation], [PropertyName], [OldValue], [NewValue]) VALUES (111, 62, N'ContactDetail', N'-2147482647', N'Insert', N'UpdatedDate', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[AuditLogDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[AuditLogs] ON 
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (1, N'46891761-72a2-4586-adf3-08ef17a4a029', CAST(N'2025-12-08T07:23:35.5460000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 44, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (2, N'fb0e2347-0e26-4bb2-b148-1acc01c104d1', CAST(N'2025-12-08T07:23:46.5080000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Users/login', N'POST', NULL, 200, 386, N'POST /api/Users/login', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (3, N'fd018e16-ab43-4e96-9c27-aa9ff1145135', CAST(N'2025-12-08T07:24:18.0350000' AS DateTime2), N'EntityChange', 2, N'Admin', NULL, N'EFCore', NULL, NULL, NULL, NULL, NULL, N'Entity change: 1 entries', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (4, N'ce54108f-8c7a-4720-ab5c-dd9b974d0bc0', CAST(N'2025-12-08T07:24:30.7510000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Products', N'POST', NULL, 201, 17648, N'POST /api/Products', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (5, N'a39374de-dced-4be2-8b5c-d4e4f408f998', CAST(N'2025-12-08T07:25:04.2000000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Products', N'GET', NULL, 200, 4994, N'GET /api/Products', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (6, N'58175100-ad9a-4253-9990-b98b8ce58e9d', CAST(N'2025-12-08T07:25:58.3050000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Products', N'GET', NULL, 200, 16213, N'GET /api/Products', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (7, N'907332bf-ef66-4a98-a281-61623b86577e', CAST(N'2025-12-08T09:00:43.0300000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 62, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (8, N'4b82467f-5761-4d5e-8c00-ce5e3843cc06', CAST(N'2025-12-08T09:12:28.8600000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Users/login', N'POST', NULL, 200, 1383, N'POST /api/Users/login', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (9, N'8f21323a-dfb5-48e9-818e-15611513ae3e', CAST(N'2025-12-08T09:12:56.3230000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Products', N'GET', NULL, 200, 578, N'GET /api/Products', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (10, N'c591d15f-464c-458d-ad60-cad4a874ef04', CAST(N'2025-12-08T09:13:55.1350000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 34, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (11, N'2f56c386-c7d6-4485-b1d0-f4b4c5681b83', CAST(N'2025-12-08T09:14:08.2770000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ProductStock/product/1', N'GET', NULL, 200, 504, N'GET /api/ProductStock/product/1', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (12, N'56e7403a-afb4-4ef9-bb2c-4c930640405e', CAST(N'2025-12-08T09:14:30.2130000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Branches', N'GET', NULL, 200, 86, N'GET /api/Branches', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (13, N'5598b8a5-d540-48f5-a7ea-d6087ec61281', CAST(N'2025-12-08T09:14:57.1840000' AS DateTime2), N'EntityChange', 2, N'Admin', NULL, N'EFCore', NULL, NULL, NULL, NULL, NULL, N'Entity change: 1 entries', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (14, N'f644f2ad-8691-4086-a15b-91beecca4ff7', CAST(N'2025-12-08T09:14:57.3390000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ProductStock/adjust', N'POST', NULL, 204, 651, N'POST /api/ProductStock/adjust', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (15, N'f49662c8-c3b0-476d-a4a6-51defd685843', CAST(N'2025-12-08T09:15:05.1150000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ProductStock/product/1', N'GET', NULL, 200, 75, N'GET /api/ProductStock/product/1', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (16, N'bcf787ec-da02-43e8-85d0-2fdf4d39e47f', CAST(N'2025-12-08T09:15:18.5720000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ProductStock/branch/1', N'GET', NULL, 200, 39, N'GET /api/ProductStock/branch/1', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (17, N'a271d124-d829-4896-a74a-edd5acda5cae', CAST(N'2025-12-08T09:15:31.1390000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ProductStock', N'GET', NULL, 200, 81, N'GET /api/ProductStock', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (18, N'c8923723-7515-4af1-ac39-fb894e43f367', CAST(N'2025-12-08T09:15:36.0390000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ProductStock', N'GET', NULL, 200, 38, N'GET /api/ProductStock', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (19, N'c6675b85-c081-4192-a62b-4dac056f3557', CAST(N'2025-12-08T09:15:41.3150000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ProductStock', N'GET', NULL, 200, 10, N'GET /api/ProductStock', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (20, N'b24339cf-e661-46fc-b82f-e3d81e185837', CAST(N'2025-12-08T09:15:58.6670000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ProductStock/single', N'GET', NULL, 404, 46, N'GET /api/ProductStock/single', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (21, N'b06e0710-a1be-4964-beca-1bc415216a85', CAST(N'2025-12-08T09:16:04.7080000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ProductStock/single', N'GET', NULL, 200, 23, N'GET /api/ProductStock/single', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (22, N'b1793115-10ab-4472-9a0d-ab616949df74', CAST(N'2025-12-08T09:35:28.3980000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 32, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (23, N'c5291731-80c2-4a38-8d2a-8d1498a06e0b', CAST(N'2025-12-08T10:13:33.2360000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 38, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (24, N'4cc3a162-a7b0-4141-9f7a-26a435230af6', CAST(N'2025-12-08T10:21:10.3070000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 38, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (25, N'3b259e89-704a-48e1-96ef-21bb96a26944', CAST(N'2025-12-08T10:40:36.2550000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 39, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (26, N'7d592f00-4997-4a3c-b78f-27e6cd4b4707', CAST(N'2025-12-08T10:42:51.1180000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 34, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (27, N'60176aec-3f2e-4881-a971-e5db172fb558', CAST(N'2025-12-08T10:46:50.6380000' AS DateTime2), N'EntityChange', 2, N'Admin', NULL, N'EFCore', NULL, NULL, NULL, NULL, NULL, N'Entity change: 1 entries', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (28, N'd3b3a56b-71cb-41de-b7d5-e166d5941dfe', CAST(N'2025-12-08T10:47:02.2890000' AS DateTime2), N'EntityChange', 2, N'Admin', NULL, N'EFCore', NULL, NULL, NULL, NULL, NULL, N'Entity change: 4 entries', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (29, N'539687b2-6bea-4954-b605-754ab2622c01', CAST(N'2025-12-08T10:47:07.4330000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ServiceOrderRequests', N'POST', NULL, 201, 39747, N'POST /api/ServiceOrderRequests', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (30, N'3dd2f401-741a-4692-8d52-005800fceadd', CAST(N'2025-12-08T10:49:01.0510000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ServiceOrderRequests', N'GET', NULL, 200, 82, N'GET /api/ServiceOrderRequests', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (31, N'2d6c1f0e-6a8c-47c9-ad85-207faa7861d1', CAST(N'2025-12-08T10:50:42.8370000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ServiceOrderRequests', N'GET', NULL, 200, 27836, N'GET /api/ServiceOrderRequests', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (32, N'b1da1838-153c-42d1-be65-7c0f54ebfa01', CAST(N'2025-12-08T10:50:44.0500000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/index.html', N'GET', NULL, 200, 102, N'GET /swagger/index.html', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (33, N'beb66386-6de4-4769-a965-c674dd3e09e3', CAST(N'2025-12-08T10:50:44.2570000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 41, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (34, N'2bcd93be-e186-4550-9ca0-4c8247fa0415', CAST(N'2025-12-08T10:51:05.2960000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ServiceOrderRequests', N'GET', NULL, 200, 2498, N'GET /api/ServiceOrderRequests', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (35, N'7de9e833-8e62-4e3c-916b-732dbb9d165c', CAST(N'2025-12-08T12:49:25.5340000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 56, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (36, N'978540d4-ec62-4d8b-8098-4fae3cc2ba6a', CAST(N'2025-12-08T12:49:37.4290000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Contact', N'GET', NULL, 200, 974, N'GET /api/Contact', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (37, N'3fb722c1-355e-486d-b2ca-9e7c054bedc9', CAST(N'2025-12-08T12:49:47.6970000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Products', N'GET', NULL, 200, 116, N'GET /api/Products', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (38, N'4fea48d7-a4ed-4386-83f6-d122302ded95', CAST(N'2025-12-08T12:50:51.5470000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Products', N'GET', NULL, 200, 46, N'GET /api/Products', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (39, N'92e776c6-8c82-4a87-a227-abe54d48c026', CAST(N'2025-12-08T12:52:44.8940000' AS DateTime2), N'EntityChange', 2, N'Admin', NULL, N'EFCore', NULL, NULL, NULL, NULL, NULL, N'Entity change: 1 entries', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (40, N'e430d51d-2db7-414e-8dca-e3fc2fe7e473', CAST(N'2025-12-08T12:52:45.0470000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Products', N'POST', NULL, 201, 288, N'POST /api/Products', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (41, N'6a5668b3-e17f-47ec-9d0e-601ba5abdc08', CAST(N'2025-12-08T12:53:35.9600000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Users/login', N'POST', NULL, 200, 46, N'POST /api/Users/login', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (42, N'1a09bdba-af2d-4a59-84c1-93b6a5321bff', CAST(N'2025-12-08T12:53:57.1170000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Users/login', N'POST', NULL, 200, 5871, N'POST /api/Users/login', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (43, N'c0bdd21b-f504-41a4-9183-e81a8aa21386', CAST(N'2025-12-08T13:00:28.7890000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 36, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (44, N'c9d190ef-c38b-4769-96a6-17dfc75dc9ae', CAST(N'2025-12-08T13:00:58.0230000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Users/login', N'POST', NULL, 200, 3700, N'POST /api/Users/login', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (45, N'000d38b3-a91a-43f3-9d26-ecc25e893506', CAST(N'2025-12-08T13:01:35.0960000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ProductStock/product/1', N'GET', NULL, 200, 298, N'GET /api/ProductStock/product/1', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (46, N'b4661a0b-8ed8-4784-8e84-7df18960d168', CAST(N'2025-12-08T13:01:43.6480000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ProductStock/product/1', N'GET', NULL, 200, 9, N'GET /api/ProductStock/product/1', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (47, N'07672eb6-8330-489e-92f2-572295a6adb6', CAST(N'2025-12-08T13:15:52.2540000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 33, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (48, N'e1f65991-f3c3-46f8-af83-019987f80e96', CAST(N'2025-12-08T13:16:13.3030000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Users/login', N'POST', NULL, 200, 2381, N'POST /api/Users/login', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (49, N'4343e839-ed87-4cdb-aa7a-0c71176023fb', CAST(N'2025-12-08T13:17:10.1920000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/ProductStock/product/1', N'GET', NULL, 401, 17, N'GET /api/ProductStock/product/1', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (50, N'4b65406e-dfae-4722-93f2-90bfacb6ea90', CAST(N'2025-12-08T13:26:47.3490000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 44, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (51, N'9ea2d717-0639-4be6-a22f-18e085e8e1b3', CAST(N'2025-12-08T13:27:06.9650000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Users/login', N'POST', NULL, 200, 1706, N'POST /api/Users/login', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (52, N'02e4abe6-6103-48e3-a1a3-c0cd3cb55aa0', CAST(N'2025-12-08T13:27:54.2350000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/Debug/whoami', N'GET', NULL, 200, 5103, N'GET /api/Debug/whoami', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (53, N'4ff56bc1-a6b7-4a78-8ddd-d1cf203eda2e', CAST(N'2025-12-08T13:28:08.6630000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/Products', N'GET', NULL, 200, 151, N'GET /api/Products', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (54, N'06689812-1121-4f90-b7a5-3d94cd05b011', CAST(N'2025-12-08T13:28:23.9790000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 37, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (55, N'9cec6f8c-4d2d-4566-8c8f-444bf0410a4e', CAST(N'2025-12-08T13:28:42.2340000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/Products', N'GET', NULL, 200, 345, N'GET /api/Products', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (56, N'63170731-4351-46bc-9de7-5cebb84f4890', CAST(N'2025-12-08T13:29:12.8590000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/Products', N'GET', NULL, 200, 17, N'GET /api/Products', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (57, N'2fb0bb69-4e05-4247-bbbc-2788c21fcf43', CAST(N'2025-12-08T13:30:04.0800000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 29, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (58, N'6647199f-50b8-4dbf-a8bf-7a81a2d37013', CAST(N'2025-12-08T13:30:37.0070000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Users/login', N'POST', NULL, 200, 350, N'POST /api/Users/login', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (59, N'46536ae0-938f-4afd-89df-b0f73a68f4be', CAST(N'2025-12-08T13:32:37.7690000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Contact', N'POST', NULL, 401, 23, N'POST /api/Contact', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (60, N'0b740883-2544-4509-8891-e8a78767dc6e', CAST(N'2025-12-08T13:33:08.7840000' AS DateTime2), N'EntityChange', 2, N'Admin', NULL, N'EFCore', NULL, NULL, NULL, NULL, NULL, N'Entity change: 1 entries', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (61, N'32531b4e-7cdc-434a-ab38-1b10940c37b3', CAST(N'2025-12-08T13:37:08.0100000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 32, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (62, N'08a38d54-ad47-4f8a-94a8-442207e2d3c6', CAST(N'2025-12-08T13:37:44.5360000' AS DateTime2), N'EntityChange', 2, N'Admin', NULL, N'EFCore', NULL, NULL, NULL, NULL, NULL, N'Entity change: 1 entries', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (63, N'f6aedb80-7787-4d99-ada7-d9ed0d55ed21', CAST(N'2025-12-08T13:37:50.0680000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/Contact', N'POST', NULL, 201, 5809, N'POST /api/Contact', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (64, N'06fd25ec-7cca-48e9-8775-008a14feb2a4', CAST(N'2025-12-08T13:38:25.4560000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/Contact/2', N'GET', NULL, 200, 99, N'GET /api/Contact/2', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (65, N'782895f4-e2cd-4128-93df-efc58fc21b17', CAST(N'2025-12-08T13:38:50.9000000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/Contact/2', N'GET', NULL, 200, 10589, N'GET /api/Contact/2', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (66, N'86c61c6c-7181-4686-95d1-b644c2426d32', CAST(N'2025-12-08T13:39:15.5580000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/Contact/2', N'GET', NULL, 200, 19810, N'GET /api/Contact/2', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (67, N'd89bb67e-2a05-487d-bb5f-97247078263a', CAST(N'2025-12-08T13:39:16.6470000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/index.html', N'GET', NULL, 200, 132, N'GET /swagger/index.html', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (68, N'561d81ad-b3b5-4d2b-ae83-082fca399176', CAST(N'2025-12-08T13:39:16.8830000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 68, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (69, N'0195e305-7e08-40ea-901b-1751d0b924ae', CAST(N'2025-12-08T13:39:37.4850000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Users/login', N'POST', NULL, 200, 97, N'POST /api/Users/login', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (70, N'c10a854f-6d11-46e0-86e9-7cd99424a84d', CAST(N'2025-12-08T13:40:01.5720000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/Contact', N'GET', NULL, 200, 95, N'GET /api/Contact', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (71, N'95e5884a-fbea-4605-92e3-d1e240482163', CAST(N'2025-12-15T12:14:17.2360000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 60, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (72, N'17cb0b28-13cf-435a-84da-e159362ce7e1', CAST(N'2025-12-15T12:16:01.2830000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Users/login', N'POST', NULL, 200, 11195, N'POST /api/Users/login', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (73, N'fdfc2e2d-c7c5-4520-b5a6-c02a5dc942e9', CAST(N'2025-12-15T12:16:21.7500000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/SorContactMapping', N'GET', NULL, 401, 24, N'GET /api/SorContactMapping', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (74, N'c5ee7237-ae0e-4e66-a62c-2cdaabaa9ab3', CAST(N'2025-12-15T12:16:36.9050000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/SorContactMapping', N'GET', NULL, 200, 127, N'GET /api/SorContactMapping', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (75, N'af5de7db-2f76-4193-bb05-ea180213a648', CAST(N'2025-12-15T12:16:51.6900000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/ServiceOrderRequests', N'GET', NULL, 200, 183, N'GET /api/ServiceOrderRequests', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (76, N'69e1ac86-6a8f-47b1-90ac-ce104bdb9ae6', CAST(N'2025-12-15T12:16:58.6640000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/Roles', N'GET', NULL, 200, 30, N'GET /api/Roles', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (77, N'4b043f92-1973-4b0c-ae91-d7dd5bdbcde8', CAST(N'2025-12-15T12:17:04.8030000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/Branches', N'GET', NULL, 200, 38, N'GET /api/Branches', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (78, N'4abfb5fb-cb99-45e2-be5c-a8645bc5bff0', CAST(N'2025-12-15T12:17:15.1640000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/Contact', N'GET', NULL, 200, 140, N'GET /api/Contact', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (79, N'0b1a6345-ba30-49d1-bd0f-101e610f4d98', CAST(N'2025-12-15T12:17:36.8070000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/MasterDetails/category/UnitType', N'GET', NULL, 200, 55, N'GET /api/MasterDetails/category/UnitType', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (80, N'8c45eebd-f152-48c4-9190-7117ec533ead', CAST(N'2025-12-15T12:17:48.6900000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/MasterDetails/category/Urgency%20Level', N'GET', NULL, 200, 5, N'GET /api/MasterDetails/category/Urgency%20Level', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (81, N'12d81076-d929-4bed-b4b6-95377dc0f555', CAST(N'2025-12-15T12:17:58.2710000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/MasterDetails/category/Department', N'GET', NULL, 200, 4, N'GET /api/MasterDetails/category/Department', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (82, N'b2b5bfa7-bdfe-486b-89f1-37d8ab72b397', CAST(N'2025-12-15T12:18:07.5400000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/MasterDetails/category/ProductCategory', N'GET', NULL, 200, 4, N'GET /api/MasterDetails/category/ProductCategory', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (83, N'75c7f5c8-2255-40d2-81a1-1a2a939e76c0', CAST(N'2025-12-15T12:37:13.7220000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 48, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (84, N'c23e1550-4cfb-4361-97b4-e652f5e83372', CAST(N'2025-12-15T12:38:08.8130000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Users/login', N'POST', NULL, 200, 555, N'POST /api/Users/login', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (85, N'2f93820e-ba1c-44ff-8d23-4c9e572c1213', CAST(N'2025-12-15T12:38:24.7450000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Contact', N'GET', NULL, 401, 27, N'GET /api/Contact', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (86, N'3d8bcaf7-36fc-4523-9514-889172d50726', CAST(N'2025-12-15T12:38:33.9710000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/Contact', N'GET', NULL, 200, 234, N'GET /api/Contact', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (87, N'eae1e21e-7b74-4ad9-9e23-8c734bf05ef2', CAST(N'2025-12-15T13:31:39.1180000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 99, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (88, N'5fd59b20-2716-41f5-b462-cf77aa26dabd', CAST(N'2025-12-15T13:32:04.0110000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Users/login', N'POST', NULL, 200, 562, N'POST /api/Users/login', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (89, N'f30f2959-061f-4367-85fe-da7ab1b44646', CAST(N'2025-12-15T13:32:20.3910000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Products', N'GET', NULL, 401, 25, N'GET /api/Products', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (90, N'1cc895b2-d733-4d71-867c-b0e9c67d4fca', CAST(N'2025-12-15T13:32:31.0580000' AS DateTime2), N'ApiRequest', 2, N'Admin', NULL, N'API', N'/api/Products', N'GET', NULL, 200, 312, N'GET /api/Products', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (91, N'e005aa8a-4988-439d-964f-815e7b1bc758', CAST(N'2025-12-16T04:39:17.8040000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/index.html', N'GET', NULL, 200, 208, N'GET /swagger/index.html', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (92, N'91dd4f07-af45-4d9d-a77b-83df0e54ef47', CAST(N'2025-12-16T04:39:19.4120000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 98, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (93, N'286a8d72-fc51-425b-81c6-fb85d765c8ca', CAST(N'2025-12-16T04:39:19.4080000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/favicon-32x32.png', N'GET', NULL, 200, 38, N'GET /swagger/favicon-32x32.png', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (94, N'6e4374b4-f1f1-4c63-a8e0-1234f92a74b8', CAST(N'2025-12-16T04:40:28.0650000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 44, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (95, N'087f0312-2e6f-4ba1-add4-6940b1c5cb8a', CAST(N'2025-12-16T05:35:03.4390000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/swagger/v1/swagger.json', N'GET', NULL, 200, 67, N'GET /swagger/v1/swagger.json', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (96, N'4017e0dc-30a9-4e62-b89b-66aa025385e7', CAST(N'2025-12-16T05:35:28.7740000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Users/login', N'POST', NULL, 200, 757, N'POST /api/Users/login', NULL)
GO
INSERT [dbo].[AuditLogs] ([AuditId], [CorrelationId], [EventTime], [EventType], [UserId], [UserName], [TenantId], [Source], [Route], [HttpMethod], [ClientIp], [StatusCode], [DurationMs], [Summary], [Extra]) VALUES (97, N'5f257d83-456f-408a-ab17-aba60036150a', CAST(N'2025-12-16T05:35:42.9320000' AS DateTime2), N'ApiRequest', NULL, NULL, NULL, N'API', N'/api/Users/login', N'POST', NULL, 200, 142, N'POST /api/Users/login', NULL)
GO
SET IDENTITY_INSERT [dbo].[AuditLogs] OFF
GO
SET IDENTITY_INSERT [dbo].[Branches] ON 
GO
INSERT [dbo].[Branches] ([ID], [Name], [MobileNo], [Website], [Address], [IsDelete], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate], [Budget]) VALUES (1, N'Sutton Branch', N'9879879870', N'www.google.com', N'London', 0, 1, 1, CAST(N'2025-11-28T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-28T00:00:00.0000000' AS DateTime2), CAST(0.00 AS Decimal(18, 2)))
GO
SET IDENTITY_INSERT [dbo].[Branches] OFF
GO
SET IDENTITY_INSERT [dbo].[ContactDetails] ON 
GO
INSERT [dbo].[ContactDetails] ([ID], [Name], [Address], [Postcode], [City], [State], [Country], [MobileNo], [EmailAddress], [IsDeleted], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [BranchID], [UniqueString]) VALUES (1, N'Rajesh Gujjar', N'Sutton road', N'SM1 4FS', N'London', N'London', N'United Kingdom', N'9876543210', N'rg@gmail.com', 0, 2, CAST(N'2025-12-08T13:33:08.4190000' AS DateTime2), NULL, NULL, 1, NULL)
GO
INSERT [dbo].[ContactDetails] ([ID], [Name], [Address], [Postcode], [City], [State], [Country], [MobileNo], [EmailAddress], [IsDeleted], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [BranchID], [UniqueString]) VALUES (2, N'Rajesh Gujjar', N'Sutton road', N'SM1 4FS', N'London', N'London', N'United Kingdom', N'9876543210', N'rg@gmail.com', 0, 2, CAST(N'2025-12-08T13:37:44.4710000' AS DateTime2), NULL, NULL, 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[ContactDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[MasterDetails] ON 
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (1, N'UnitType', N'Pieces', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.227' AS DateTime), CAST(N'2025-12-05T13:02:21.227' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (2, N'UnitType', N'Boxes', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.227' AS DateTime), CAST(N'2025-12-05T13:02:21.227' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (3, N'UnitType', N'Units', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.227' AS DateTime), CAST(N'2025-12-05T13:02:21.227' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (4, N'UnitType', N'Sets', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.227' AS DateTime), CAST(N'2025-12-05T13:02:21.227' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (5, N'UnitType', N'Kilograms', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.227' AS DateTime), CAST(N'2025-12-05T13:02:21.227' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (6, N'UnitType', N'Liters', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.227' AS DateTime), CAST(N'2025-12-05T13:02:21.227' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (7, N'UnitType', N'Meters', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.227' AS DateTime), CAST(N'2025-12-05T13:02:21.227' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (8, N'UnitType', N'Hours', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.227' AS DateTime), CAST(N'2025-12-05T13:02:21.227' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (9, N'Urgency Level', N'Low - Within 30 days', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.470' AS DateTime), CAST(N'2025-12-05T13:02:21.470' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (10, N'Urgency Level', N'Medium - Within 15 days', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.470' AS DateTime), CAST(N'2025-12-05T13:02:21.470' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (11, N'Urgency Level', N'High - Within 7 days', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.470' AS DateTime), CAST(N'2025-12-05T13:02:21.470' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (12, N'Urgency Level', N'Urgent - Within 3 days', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.470' AS DateTime), CAST(N'2025-12-05T13:02:21.470' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (13, N'Department', N'Information Technology', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.507' AS DateTime), CAST(N'2025-12-05T13:02:21.507' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (14, N'Department', N'Finance & Accounting', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.507' AS DateTime), CAST(N'2025-12-05T13:02:21.507' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (15, N'Department', N'Human Resources', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.507' AS DateTime), CAST(N'2025-12-05T13:02:21.507' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (16, N'Department', N'Operations', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.507' AS DateTime), CAST(N'2025-12-05T13:02:21.507' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (17, N'Department', N'Maintenance', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.507' AS DateTime), CAST(N'2025-12-05T13:02:21.507' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (18, N'Department', N'Administration', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.507' AS DateTime), CAST(N'2025-12-05T13:02:21.507' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (19, N'ProductCategory', N'Stationary', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.507' AS DateTime), CAST(N'2025-12-05T13:02:21.507' AS DateTime))
GO
INSERT [dbo].[MasterDetails] ([ID], [Category], [Name], [OtherName], [Description], [ParentID], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (20, N'ProductCategory', N'Electrical', N'', N'', 0, 0, 1, 1, CAST(N'2025-12-05T13:02:21.507' AS DateTime), CAST(N'2025-12-05T13:02:21.507' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[MasterDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 
GO
INSERT [dbo].[Products] ([ID], [Name], [Description], [CategoryID], [UnitTypeID], [SalesPrice], [PurchasePrice], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (1, N'Full scape notebook', N'Full scape notebook', 19, 1, CAST(18.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 0, 2, 2, CAST(N'2025-12-08T07:24:16.5020000' AS DateTime2), CAST(N'2025-12-08T07:24:13.1990000' AS DateTime2))
GO
INSERT [dbo].[Products] ([ID], [Name], [Description], [CategoryID], [UnitTypeID], [SalesPrice], [PurchasePrice], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (2, N'A4 size page', N'A4 size page bundle', 19, 2, CAST(110.00 AS Decimal(18, 2)), CAST(120.00 AS Decimal(18, 2)), 0, 2, 2, CAST(N'2025-12-08T12:52:44.8370000' AS DateTime2), CAST(N'2025-12-08T12:52:44.8160000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductStocks] ON 
GO
INSERT [dbo].[ProductStocks] ([ID], [ProductID], [BranchID], [Quantity], [ReservedQty], [ReorderLevel], [IsDeleted], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, 1, 1, CAST(1000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, 0, 2, CAST(N'2025-12-08T09:14:56.8290000' AS DateTime2), 2, CAST(N'2025-12-08T09:14:56.8290000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[ProductStocks] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([ID], [Name], [CreatedDate]) VALUES (1, N'SuperAdmin', CAST(N'2025-11-28T12:21:28.5590000' AS DateTime2))
GO
INSERT [dbo].[Roles] ([ID], [Name], [CreatedDate]) VALUES (2, N'BranchAdmin', CAST(N'2025-11-28T12:21:28.5590000' AS DateTime2))
GO
INSERT [dbo].[Roles] ([ID], [Name], [CreatedDate]) VALUES (3, N'AdminHRAdmin', CAST(N'2025-11-28T12:21:28.5590000' AS DateTime2))
GO
INSERT [dbo].[Roles] ([ID], [Name], [CreatedDate]) VALUES (4, N'Department', CAST(N'2025-11-28T12:21:28.5590000' AS DateTime2))
GO
INSERT [dbo].[Roles] ([ID], [Name], [CreatedDate]) VALUES (5, N'Principal', CAST(N'2025-11-28T12:21:28.5590000' AS DateTime2))
GO
INSERT [dbo].[Roles] ([ID], [Name], [CreatedDate]) VALUES (6, N'Teacher', CAST(N'2025-11-28T12:21:28.5590000' AS DateTime2))
GO
INSERT [dbo].[Roles] ([ID], [Name], [CreatedDate]) VALUES (7, N'Clinic', CAST(N'2025-11-28T12:21:28.5590000' AS DateTime2))
GO
INSERT [dbo].[Roles] ([ID], [Name], [CreatedDate]) VALUES (8, N'Staff', CAST(N'2025-11-28T12:21:28.5590000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[ServiceOrderRequestAssignment] ON 
GO
INSERT [dbo].[ServiceOrderRequestAssignment] ([ID], [SORID], [UserID], [Note], [IsDeleted], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, 1, 2, N'Initial assignment', 0, NULL, CAST(N'2025-12-08T10:46:48.2570000' AS DateTime2), NULL, CAST(N'2025-12-08T10:46:48.2570000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[ServiceOrderRequestAssignment] OFF
GO
SET IDENTITY_INSERT [dbo].[ServiceOrderRequestAttachments] ON 
GO
INSERT [dbo].[ServiceOrderRequestAttachments] ([ID], [SORID], [FileName], [FilePath], [IsDeleted], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, 1, N'image.jpg', N'image.jpg', 0, NULL, CAST(N'2025-12-08T10:46:48.2570000' AS DateTime2), NULL, CAST(N'2025-12-08T10:46:48.2570000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[ServiceOrderRequestAttachments] OFF
GO
SET IDENTITY_INSERT [dbo].[ServiceOrderRequestItems] ON 
GO
INSERT [dbo].[ServiceOrderRequestItems] ([ID], [SORID], [ProductID], [UnitTypeID], [Quantity], [EstimatedCost], [TechnicalSpecifications], [IsDeleted], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (1, 1, 1, 1, CAST(5.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), N'Need for the office purpose.', 0, NULL, CAST(N'2025-12-08T10:46:48.2570000' AS DateTime2), NULL, CAST(N'2025-12-08T10:46:48.2570000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[ServiceOrderRequestItems] OFF
GO
SET IDENTITY_INSERT [dbo].[ServiceOrderRequests] ON 
GO
INSERT [dbo].[ServiceOrderRequests] ([ID], [UniqueString], [BranchID], [DepartmentID], [PurposeDescription], [AdditionalJustification], [RequiredByDate], [UrgencyLevelID], [Status], [CurrentAssignedUserID], [IsDeleted], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [ActualCost]) VALUES (1, N'SOR-20251208-0001', 1, 13, N'Need a full scape notebook', N'Need a full scape notebookNeed a full scape notebook', CAST(N'2025-12-12T10:42:59.5510000' AS DateTime2), 10, N'Assigned', 2, 0, 1, CAST(N'2025-12-08T10:46:48.2570000' AS DateTime2), 1, CAST(N'2025-12-08T10:46:48.2570000' AS DateTime2), CAST(100.00 AS Decimal(18, 2)))
GO
SET IDENTITY_INSERT [dbo].[ServiceOrderRequests] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([ID], [RoleID], [BranchID], [FirstName], [MiddleName], [LastName], [Email], [Password], [Saltkey], [UniqueKey], [IsDelete], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate]) VALUES (2, 1, 1, N'Admin', N'User', N'', N'admin@gmail.com', N'AOVgqzVEsXDHE6orQ5kqwBkZi9DoHXnsidv3Nqb8dXQ=', N'gb9WwbmRYPOylUV2Ln0djzAVeqReFKyVKzcJidEbngou7WFBWcMk47p0Fs1VP7W2cKHHu55BlyzixbkpxLoMag==', N'111', 0, 1, 1, CAST(N'2025-11-28T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-28T00:00:00.0000000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [UQ_ProductStocks]    Script Date: 16-12-2025 11:10:29 ******/
ALTER TABLE [dbo].[ProductStocks] ADD  CONSTRAINT [UQ_ProductStocks] UNIQUE NONCLUSTERED 
(
	[ProductID] ASC,
	[BranchID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AuditLogs] ADD  DEFAULT (sysutcdatetime()) FOR [EventTime]
GO
ALTER TABLE [dbo].[BranchBudgetTransactions] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Branches] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Branches] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Branches] ADD  CONSTRAINT [DF_Branches_Budget]  DEFAULT ((0.00)) FOR [Budget]
GO
ALTER TABLE [dbo].[ContactDetails] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ContactDetails] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ContactDetails] ADD  DEFAULT ((1)) FOR [BranchID]
GO
ALTER TABLE [dbo].[EmailLogs] ADD  DEFAULT ((0)) FOR [TryCount]
GO
ALTER TABLE [dbo].[EmailLogs] ADD  DEFAULT ((0)) FOR [IsSent]
GO
ALTER TABLE [dbo].[EmailLogs] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[EmailLogs] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[EmailSetupDetails] ADD  DEFAULT ((1)) FOR [EnableSsl]
GO
ALTER TABLE [dbo].[EmailSetupDetails] ADD  DEFAULT ((0)) FOR [UseDefaultCredentials]
GO
ALTER TABLE [dbo].[EmailSetupDetails] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0.00)) FOR [SalesPrice]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0.00)) FOR [PurchasePrice]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (sysutcdatetime()) FOR [UpdatedDate]
GO
ALTER TABLE [dbo].[ProductStocks] ADD  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[ProductStocks] ADD  DEFAULT ((0)) FOR [ReservedQty]
GO
ALTER TABLE [dbo].[ProductStocks] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ProductStocks] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Roles] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ServiceOrderRequestAssignment] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ServiceOrderRequestAssignment] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ServiceOrderRequestAttachments] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ServiceOrderRequestAttachments] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ServiceOrderRequestItems] ADD  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[ServiceOrderRequestItems] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ServiceOrderRequestItems] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ServiceOrderRequests] ADD  DEFAULT ('New') FOR [Status]
GO
ALTER TABLE [dbo].[ServiceOrderRequests] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ServiceOrderRequests] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SORContactMappingAttachments] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[SORContactMappingAttachments] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SORContactMappings] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[SORContactMappings] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[AuditLogDetails]  WITH CHECK ADD  CONSTRAINT [FK_AuditLogDetails_AuditLogs] FOREIGN KEY([AuditId])
REFERENCES [dbo].[AuditLogs] ([AuditId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuditLogDetails] CHECK CONSTRAINT [FK_AuditLogDetails_AuditLogs]
GO
ALTER TABLE [dbo].[BranchBudgetTransactions]  WITH CHECK ADD  CONSTRAINT [FK_BranchBudgetTransactions_Branch] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branches] ([ID])
GO
ALTER TABLE [dbo].[BranchBudgetTransactions] CHECK CONSTRAINT [FK_BranchBudgetTransactions_Branch]
GO
ALTER TABLE [dbo].[BranchBudgetTransactions]  WITH CHECK ADD  CONSTRAINT [FK_BranchBudgetTransactions_SOR] FOREIGN KEY([RelatedSORID])
REFERENCES [dbo].[ServiceOrderRequests] ([ID])
GO
ALTER TABLE [dbo].[BranchBudgetTransactions] CHECK CONSTRAINT [FK_BranchBudgetTransactions_SOR]
GO
ALTER TABLE [dbo].[ContactDetails]  WITH CHECK ADD  CONSTRAINT [FK_ContactDetails_Branch] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branches] ([ID])
GO
ALTER TABLE [dbo].[ContactDetails] CHECK CONSTRAINT [FK_ContactDetails_Branch]
GO
ALTER TABLE [dbo].[EmailLogs]  WITH CHECK ADD  CONSTRAINT [FK_EmailLogs_Branch] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branches] ([ID])
GO
ALTER TABLE [dbo].[EmailLogs] CHECK CONSTRAINT [FK_EmailLogs_Branch]
GO
ALTER TABLE [dbo].[EmailSetupDetails]  WITH CHECK ADD  CONSTRAINT [FK_EmailSetupDetails_Branch] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branches] ([ID])
GO
ALTER TABLE [dbo].[EmailSetupDetails] CHECK CONSTRAINT [FK_EmailSetupDetails_Branch]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_MasterDetails_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[MasterDetails] ([ID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_MasterDetails_Category]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_MasterDetails_UnitType] FOREIGN KEY([UnitTypeID])
REFERENCES [dbo].[MasterDetails] ([ID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_MasterDetails_UnitType]
GO
ALTER TABLE [dbo].[ProductStocks]  WITH CHECK ADD  CONSTRAINT [FK_ProductStocks_Branch] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branches] ([ID])
GO
ALTER TABLE [dbo].[ProductStocks] CHECK CONSTRAINT [FK_ProductStocks_Branch]
GO
ALTER TABLE [dbo].[ProductStocks]  WITH CHECK ADD  CONSTRAINT [FK_ProductStocks_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ID])
GO
ALTER TABLE [dbo].[ProductStocks] CHECK CONSTRAINT [FK_ProductStocks_Product]
GO
ALTER TABLE [dbo].[ServiceOrderRequestAssignment]  WITH CHECK ADD  CONSTRAINT [FK_SORAssign_SOR] FOREIGN KEY([SORID])
REFERENCES [dbo].[ServiceOrderRequests] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServiceOrderRequestAssignment] CHECK CONSTRAINT [FK_SORAssign_SOR]
GO
ALTER TABLE [dbo].[ServiceOrderRequestAssignment]  WITH CHECK ADD  CONSTRAINT [FK_SORAssign_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[ServiceOrderRequestAssignment] CHECK CONSTRAINT [FK_SORAssign_User]
GO
ALTER TABLE [dbo].[ServiceOrderRequestAttachments]  WITH CHECK ADD  CONSTRAINT [FK_SORAttach_SOR] FOREIGN KEY([SORID])
REFERENCES [dbo].[ServiceOrderRequests] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServiceOrderRequestAttachments] CHECK CONSTRAINT [FK_SORAttach_SOR]
GO
ALTER TABLE [dbo].[ServiceOrderRequestItems]  WITH CHECK ADD  CONSTRAINT [FK_SORItem_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ID])
GO
ALTER TABLE [dbo].[ServiceOrderRequestItems] CHECK CONSTRAINT [FK_SORItem_Product]
GO
ALTER TABLE [dbo].[ServiceOrderRequestItems]  WITH CHECK ADD  CONSTRAINT [FK_SORItem_SOR] FOREIGN KEY([SORID])
REFERENCES [dbo].[ServiceOrderRequests] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServiceOrderRequestItems] CHECK CONSTRAINT [FK_SORItem_SOR]
GO
ALTER TABLE [dbo].[ServiceOrderRequestItems]  WITH CHECK ADD  CONSTRAINT [FK_SORItem_UnitType_MasterDetails] FOREIGN KEY([UnitTypeID])
REFERENCES [dbo].[MasterDetails] ([ID])
GO
ALTER TABLE [dbo].[ServiceOrderRequestItems] CHECK CONSTRAINT [FK_SORItem_UnitType_MasterDetails]
GO
ALTER TABLE [dbo].[ServiceOrderRequests]  WITH CHECK ADD  CONSTRAINT [FK_SOR_Branch] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branches] ([ID])
GO
ALTER TABLE [dbo].[ServiceOrderRequests] CHECK CONSTRAINT [FK_SOR_Branch]
GO
ALTER TABLE [dbo].[ServiceOrderRequests]  WITH CHECK ADD  CONSTRAINT [FK_SOR_CurrentAssignedUser] FOREIGN KEY([CurrentAssignedUserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[ServiceOrderRequests] CHECK CONSTRAINT [FK_SOR_CurrentAssignedUser]
GO
ALTER TABLE [dbo].[ServiceOrderRequests]  WITH CHECK ADD  CONSTRAINT [FK_SOR_Department_MasterDetails] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[MasterDetails] ([ID])
GO
ALTER TABLE [dbo].[ServiceOrderRequests] CHECK CONSTRAINT [FK_SOR_Department_MasterDetails]
GO
ALTER TABLE [dbo].[ServiceOrderRequests]  WITH CHECK ADD  CONSTRAINT [FK_SOR_Urgency_MasterDetails] FOREIGN KEY([UrgencyLevelID])
REFERENCES [dbo].[MasterDetails] ([ID])
GO
ALTER TABLE [dbo].[ServiceOrderRequests] CHECK CONSTRAINT [FK_SOR_Urgency_MasterDetails]
GO
ALTER TABLE [dbo].[SORContactMappingAttachments]  WITH CHECK ADD  CONSTRAINT [FK_SORContactMappingAttachments_Mapping] FOREIGN KEY([MappingID])
REFERENCES [dbo].[SORContactMappings] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SORContactMappingAttachments] CHECK CONSTRAINT [FK_SORContactMappingAttachments_Mapping]
GO
ALTER TABLE [dbo].[SORContactMappings]  WITH CHECK ADD  CONSTRAINT [FK_SORContactMappings_Branch] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branches] ([ID])
GO
ALTER TABLE [dbo].[SORContactMappings] CHECK CONSTRAINT [FK_SORContactMappings_Branch]
GO
ALTER TABLE [dbo].[SORContactMappings]  WITH CHECK ADD  CONSTRAINT [FK_SORContactMappings_Contact] FOREIGN KEY([ContactID])
REFERENCES [dbo].[ContactDetails] ([ID])
GO
ALTER TABLE [dbo].[SORContactMappings] CHECK CONSTRAINT [FK_SORContactMappings_Contact]
GO
ALTER TABLE [dbo].[SORContactMappings]  WITH CHECK ADD  CONSTRAINT [FK_SORContactMappings_SOR] FOREIGN KEY([SORID])
REFERENCES [dbo].[ServiceOrderRequests] ([ID])
GO
ALTER TABLE [dbo].[SORContactMappings] CHECK CONSTRAINT [FK_SORContactMappings_SOR]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Branch] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branches] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Branch]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Role]
GO
