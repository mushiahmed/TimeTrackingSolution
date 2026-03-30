USE [HRDB]
GO
SET IDENTITY_INSERT [dbo].[Employees] ON 
GO
INSERT [dbo].[Employees] ([Id], [EmployeeCode], [FullName], [IsActive]) VALUES (1, N'EMP001', N'Asmin Khan', 1)
GO
INSERT [dbo].[Employees] ([Id], [EmployeeCode], [FullName], [IsActive]) VALUES (2, N'EMP002', N'Sara Kamal', 1)
GO
INSERT [dbo].[Employees] ([Id], [EmployeeCode], [FullName], [IsActive]) VALUES (3, N'EMP003', N'Rashid Ali', 1)
GO
SET IDENTITY_INSERT [dbo].[Employees] OFF
GO
SET IDENTITY_INSERT [dbo].[Projects] ON 
GO
INSERT [dbo].[Projects] ([Id], [ProjectCode], [Name], [IsActive]) VALUES (1, N'PRJ001', N'Website Development', 1)
GO
INSERT [dbo].[Projects] ([Id], [ProjectCode], [Name], [IsActive]) VALUES (2, N'PRJ002', N'Mobile App', 1)
GO
INSERT [dbo].[Projects] ([Id], [ProjectCode], [Name], [IsActive]) VALUES (3, N'PRJ003', N'Project Manager', 1)
GO
SET IDENTITY_INSERT [dbo].[Projects] OFF
GO
SET IDENTITY_INSERT [dbo].[TimeEntries] ON 
GO

