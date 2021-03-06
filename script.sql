USE [STUDYONLINE]
GO
/****** Object:  Table [dbo].[AnswerOption]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnswerOption](
	[answer_option_id] [int] NOT NULL,
	[question_id] [int] NOT NULL,
	[answer_text] [nvarchar](2000) NOT NULL,
	[answer_corect] [bit] NOT NULL,
 CONSTRAINT [PK_AnswerOption] PRIMARY KEY CLUSTERED 
(
	[answer_option_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[course_id] [int] IDENTITY(1,1) NOT NULL,
	[subject_id] [int] NOT NULL,
	[teacher_id] [int] NOT NULL,
	[course_is_default] [bit] NOT NULL,
	[course_start_date] [varchar](50) NOT NULL,
	[course_end_date] [varchar](50) NOT NULL,
	[course_name] [varchar](50) NOT NULL,
	[course_status] [bit] NOT NULL,
	[course_note] [nvarchar](2000) NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[course_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coursework]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coursework](
	[coursework_id] [int] IDENTITY(1,1) NOT NULL,
	[course_id] [int] NOT NULL,
	[due_date] [varchar](50) NOT NULL,
	[coursework_status] [bit] NOT NULL,
	[coursework_name] [varchar](100) NULL,
	[usercreate_id] [int] NULL,
	[coursework_weight] [int] NULL,
	[coursework_link] [varchar](200) NULL,
	[test_id] [int] NOT NULL,
	[coursework_note] [nvarchar](2000) NULL,
 CONSTRAINT [PK_Coursework] PRIMARY KEY CLUSTERED 
(
	[coursework_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Domain]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Domain](
	[domain_id] [int] IDENTITY(1,1) NOT NULL,
	[subject_id] [int] NULL,
	[domain_name] [varchar](50) NULL,
	[domain_description] [varchar](200) NULL,
	[domain_status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[domain_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Exam]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exam](
	[exam_id] [int] IDENTITY(1,1) NOT NULL,
	[exam_level] [varchar](50) NOT NULL,
	[exam_name] [varchar](50) NOT NULL,
	[exam_is_practice] [bit] NOT NULL,
	[subject_id] [int] NULL,
	[exam_duration] [int] NOT NULL,
	[exam_description] [varchar](2000) NULL,
	[test_type] [varchar](200) NULL,
	[pass_rate] [float] NULL,
 CONSTRAINT [PK_Exam] PRIMARY KEY CLUSTERED 
(
	[exam_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExamConfig]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExamConfig](
	[exam_config_id] [int] IDENTITY(1,1) NOT NULL,
	[exam_id] [int] NOT NULL,
	[domain_id] [int] NOT NULL,
	[domain_size] [varchar](50) NOT NULL,
	[lesson_id] [int] NOT NULL,
	[lesson_size] [varchar](50) NOT NULL,
	[keywords] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ExamConfig] PRIMARY KEY CLUSTERED 
(
	[exam_config_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExamTest]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExamTest](
	[test_id] [int] IDENTITY(1,1) NOT NULL,
	[exam_id] [int] NOT NULL,
	[course_id] [int] NOT NULL,
	[test_name] [varchar](50) NOT NULL,
	[exam_note] [varchar](2000) NULL,
	[test_code] [varchar](50) NULL,
	[note] [varchar](200) NULL,
 CONSTRAINT [PK_ExamTest] PRIMARY KEY CLUSTERED 
(
	[test_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Grade]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Grade](
	[grade_id] [int] IDENTITY(1,1) NOT NULL,
	[registration_id] [int] NOT NULL,
	[course_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[coursework_id] [int] NOT NULL,
	[grade] [int] NOT NULL,
	[grade_comment] [varchar](50) NULL,
 CONSTRAINT [PK_Grade] PRIMARY KEY CLUSTERED 
(
	[grade_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lesson]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lesson](
	[lesson_id] [int] IDENTITY(1,1) NOT NULL,
	[subject_id] [int] NOT NULL,
	[lesson_name] [varchar](50) NULL,
	[lesson_order] [int] NULL,
	[lesson_type] [varchar](50) NULL,
	[lesson_status] [bit] NULL,
	[lesson_link] [varchar](200) NULL,
	[lesson_content] [varchar](2000) NULL,
	[parent_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[lesson_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menu]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[menu_id] [int] IDENTITY(1,1) NOT NULL,
	[parent_id] [int] NULL,
	[menu_name] [varchar](50) NOT NULL,
	[menu_link] [varchar](50) NULL,
	[menu_order] [int] NULL,
	[menu_status] [bit] NULL,
	[menu_description] [varchar](2000) NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[menu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permission]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permission](
	[permission_id] [int] IDENTITY(1,1) NOT NULL,
	[permission_name] [varchar](50) NULL,
	[permission_link] [varchar](50) NULL,
	[permission_status] [bit] NULL,
	[permission_describe] [varchar](max) NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[permission_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Post]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Post](
	[post_type] [varchar](50) NULL,
	[post_category] [varchar](50) NULL,
	[post_brief_info] [varchar](2000) NOT NULL,
	[post_embeb] [varchar](200) NULL,
	[post_name] [varchar](200) NOT NULL,
	[post_id] [int] IDENTITY(1,1) NOT NULL,
	[post_thumbnail] [varchar](500) NULL,
	[post_detail_info] [varchar](max) NULL,
	[post_document_link] [varchar](200) NULL,
	[post_status] [varchar](10) NULL,
	[post_date] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[post_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PostTag]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostTag](
	[post_id] [int] NOT NULL,
	[tag_id] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Question]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Question](
	[question_id] [int] NOT NULL,
	[subject_id] [int] NOT NULL,
	[domain_id] [int] NOT NULL,
	[lesson_id] [int] NOT NULL,
	[level] [varchar](50) NOT NULL,
	[question_level] [varchar](50) NULL,
	[question_status] [varchar](50) NULL,
	[question_name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Question] PRIMARY KEY CLUSTERED 
(
	[question_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Registration]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Registration](
	[registration_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[course_id] [int] NOT NULL,
	[registration_time] [varchar](50) NOT NULL,
	[registration_status] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Registration] PRIMARY KEY CLUSTERED 
(
	[registration_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleMenu]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleMenu](
	[role_menu_id] [int] IDENTITY(1,1) NOT NULL,
	[role_id] [int] NOT NULL,
	[menu_id] [int] NOT NULL,
 CONSTRAINT [PK_RoleMenu] PRIMARY KEY CLUSTERED 
(
	[role_menu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RolePermission]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolePermission](
	[role_permission_id] [int] IDENTITY(1,1) NOT NULL,
	[role_id] [int] NOT NULL,
	[permission_id] [int] NOT NULL,
	[role_name] [varchar](255) NULL,
 CONSTRAINT [PK_RolePermission] PRIMARY KEY CLUSTERED 
(
	[role_permission_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[role_id] [int] IDENTITY(1,1) NOT NULL,
	[role_name] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Setting]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Setting](
	[setting_id] [int] IDENTITY(1,1) NOT NULL,
	[setting_group_value] [varchar](50) NOT NULL,
	[setting_name] [varchar](50) NOT NULL,
	[setting_description] [varchar](2000) NOT NULL,
	[setting_order] [int] NOT NULL,
	[setting_status] [bit] NOT NULL,
 CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED 
(
	[setting_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Slider]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Slider](
	[slider_id] [int] IDENTITY(1,1) NOT NULL,
	[slider_title] [varchar](50) NOT NULL,
	[slider_caption] [varchar](200) NOT NULL,
	[slider_back_link] [varchar](500) NOT NULL,
	[slider_picture_url] [varchar](500) NULL,
	[slider_status] [varchar](50) NULL,
 CONSTRAINT [PK_Slider] PRIMARY KEY CLUSTERED 
(
	[slider_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subject]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subject](
	[subject_category] [varchar](50) NULL,
	[subject_name] [varchar](50) NOT NULL,
	[subject_brief_info] [varchar](2000) NULL,
	[subject_type] [varchar](50) NULL,
	[subject_status] [varchar](50) NULL,
	[subject_tag_line] [varchar](50) NULL,
	[subject_id] [int] IDENTITY(1,1) NOT NULL,
	[picture] [nvarchar](250) NULL,
	[ObjectiveCourse] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[subject_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubjectReference]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubjectReference](
	[subject_id] [int] NOT NULL,
	[post_id] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tag]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tag](
	[tag_id] [int] NOT NULL,
	[tag_name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[tag_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestAnswer]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestAnswer](
	[test_answer_id] [int] IDENTITY(1,1) NOT NULL,
	[test_user_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[question_id] [int] NOT NULL,
	[user_answer] [varchar](10) NOT NULL,
	[test_id] [int] NOT NULL,
 CONSTRAINT [PK_TestAnswer] PRIMARY KEY CLUSTERED 
(
	[test_answer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestBatch]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestBatch](
	[batch_id] [int] IDENTITY(1,1) NOT NULL,
	[batch_name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TestBatch] PRIMARY KEY CLUSTERED 
(
	[batch_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestQuestion]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestQuestion](
	[test_id] [int] NOT NULL,
	[question_id] [int] NOT NULL,
	[test_question_keys] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestResult]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestResult](
	[test_user_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[test_id] [int] NOT NULL,
	[exam_id] [int] NOT NULL,
	[test_type] [varchar](50) NOT NULL,
	[batch_id] [int] NOT NULL,
	[tested] [int] NULL,
	[average] [varchar](10) NULL,
	[pass_rate] [varchar](10) NULL,
	[tested_at] [varchar](50) NULL,
 CONSTRAINT [PK_TestResult] PRIMARY KEY CLUSTERED 
(
	[test_user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[user_fullname] [nvarchar](50) NOT NULL,
	[user_email] [varchar](50) NOT NULL,
	[use_mobile] [varchar](50) NULL,
	[user_status] [bit] NOT NULL,
	[user_image] [nvarchar](250) NULL,
	[user_description] [nvarchar](max) NULL,
	[user_position] [nvarchar](150) NULL,
	[check_recieveInformation] [bit] NULL,
	[user_gender] [bit] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 11/12/2019 10:46:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[user_role_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[role_id] [int] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[user_role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Course] ON 

INSERT [dbo].[Course] ([course_id], [subject_id], [teacher_id], [course_is_default], [course_start_date], [course_end_date], [course_name], [course_status], [course_note]) VALUES (4, 1, 3, 1, N'12/08/2019', N'12/12/2019', N'Course name 1', 1, NULL)
INSERT [dbo].[Course] ([course_id], [subject_id], [teacher_id], [course_is_default], [course_start_date], [course_end_date], [course_name], [course_status], [course_note]) VALUES (5, 2, 4, 0, N'12/01/2019', N'23/09/2019', N'Course name 2', 0, NULL)
SET IDENTITY_INSERT [dbo].[Course] OFF
SET IDENTITY_INSERT [dbo].[Coursework] ON 

INSERT [dbo].[Coursework] ([coursework_id], [course_id], [due_date], [coursework_status], [coursework_name], [usercreate_id], [coursework_weight], [coursework_link], [test_id], [coursework_note]) VALUES (9, 4, N'20/10/2019', 1, N'Coursework 1', 3, 30, N'abc', 1, NULL)
INSERT [dbo].[Coursework] ([coursework_id], [course_id], [due_date], [coursework_status], [coursework_name], [usercreate_id], [coursework_weight], [coursework_link], [test_id], [coursework_note]) VALUES (10, 5, N'15/02/2019', 0, N'Coursework 2', 3, 30, N'abc', 2, NULL)
SET IDENTITY_INSERT [dbo].[Coursework] OFF
SET IDENTITY_INSERT [dbo].[Domain] ON 

INSERT [dbo].[Domain] ([domain_id], [subject_id], [domain_name], [domain_description], [domain_status]) VALUES (1, 1, N'Listen Skill', N'Listening', 1)
INSERT [dbo].[Domain] ([domain_id], [subject_id], [domain_name], [domain_description], [domain_status]) VALUES (2, 2, N'Speak Skill', N'Speaking', 0)
INSERT [dbo].[Domain] ([domain_id], [subject_id], [domain_name], [domain_description], [domain_status]) VALUES (3, 1, N'Old Speak Skill', N'Old Speaking', 1)
INSERT [dbo].[Domain] ([domain_id], [subject_id], [domain_name], [domain_description], [domain_status]) VALUES (4, 1, N'Read Skill', N'Reading', 1)
INSERT [dbo].[Domain] ([domain_id], [subject_id], [domain_name], [domain_description], [domain_status]) VALUES (10, 2, N'gsdf', N'hgfhfg', 1)
INSERT [dbo].[Domain] ([domain_id], [subject_id], [domain_name], [domain_description], [domain_status]) VALUES (11, 10, N'sdgdf', N'fdsfg', 0)
INSERT [dbo].[Domain] ([domain_id], [subject_id], [domain_name], [domain_description], [domain_status]) VALUES (12, 10, N'sdfsdf', N'hfshg', 0)
SET IDENTITY_INSERT [dbo].[Domain] OFF
SET IDENTITY_INSERT [dbo].[Exam] ON 

INSERT [dbo].[Exam] ([exam_id], [exam_level], [exam_name], [exam_is_practice], [subject_id], [exam_duration], [exam_description], [test_type], [pass_rate]) VALUES (1, N'Hard', N'Exam 1', 1, 1, 60, NULL, NULL, NULL)
INSERT [dbo].[Exam] ([exam_id], [exam_level], [exam_name], [exam_is_practice], [subject_id], [exam_duration], [exam_description], [test_type], [pass_rate]) VALUES (2, N'Easy', N'Exam 2', 0, 2, 45, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Exam] OFF
SET IDENTITY_INSERT [dbo].[ExamTest] ON 

INSERT [dbo].[ExamTest] ([test_id], [exam_id], [course_id], [test_name], [exam_note], [test_code], [note]) VALUES (1, 1, 4, N'Course Test 1', N'Topic: Fundemental', NULL, NULL)
INSERT [dbo].[ExamTest] ([test_id], [exam_id], [course_id], [test_name], [exam_note], [test_code], [note]) VALUES (2, 2, 5, N'Assignment 2', N'Topic: Profestional ', NULL, NULL)
SET IDENTITY_INSERT [dbo].[ExamTest] OFF
SET IDENTITY_INSERT [dbo].[Grade] ON 

INSERT [dbo].[Grade] ([grade_id], [registration_id], [course_id], [user_id], [coursework_id], [grade], [grade_comment]) VALUES (3, 1, 4, 1, 9, 90, N'good')
INSERT [dbo].[Grade] ([grade_id], [registration_id], [course_id], [user_id], [coursework_id], [grade], [grade_comment]) VALUES (4, 2, 5, 2, 10, 80, N'good')
SET IDENTITY_INSERT [dbo].[Grade] OFF
SET IDENTITY_INSERT [dbo].[Lesson] ON 

INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (7, 1, N'Android basic', 1, N'Subject Topic', 1, N'', N'', 7)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (8, 1, N'Introduction to Android', 2, N'HTML Lesson', 1, N'https://youtu.be/EknEIzswvC0', N'Conten 2', 7)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (9, 1, N'Installing Android Studio', 3, N'HTML Lesson', 1, N'https://youtu.be/EknEIzswvC0', N'Content 3', 7)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (10, 1, N'Introduction to Android Studio', 4, N'HTML Lesson', 1, N'https://youtu.be/EknEIzswvC0', N'Content 4', 7)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (11, 1, N'Activity Lifecycle', 5, N'HTML Lesson', 1, N'https://youtu.be/EknEIzswvC0', N'Content 5', 7)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (23, 1, N'Quiz', 6, N'HTML Lesson', 1, N'https://youtu.be/EknEIzswvC0', N'Content 6', 7)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (44, 1, N'Android UI', 7, N'Subject Topic', 1, N'', N'Content 7', 44)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (45, 1, N'Adding Two Numbers App', 8, N'HTML Lesson', 1, N'https://youtu.be/EknEIzswvC0', N'Content 8', 44)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (60, 1, N'Password Field and Toast in Android', 9, N'HTML Lesson', 1, N'https://youtu.be/EknEIzswvC0', N'Content 9', 44)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (63, 1, N'Android Checkbox Basics and Example', 10, N'HTML Lesson', 1, N'https://youtu.be/EknEIzswvC0', N'Content 10', 44)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (65, 1, N'Android Advance', 11, N'Subject Topic', 1, N'', N'Content 11', 65)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (69, 1, N'Introduction to Services', 12, N'HTML Lesson', 1, N'https://youtu.be/EknEIzswvC0', N'Content 12', 65)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1027, 1, N' Android RadioButton Basics With Example', 13, N'HTML Lesson', 1, N'https://youtu.be/cTlWwuAvRpE', N'Content 13', 44)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1029, 1, N'Android RatingBar Basics', 14, N'HTML Lesson', 1, N'https://youtu.be/0MrPs4yk9pU', N'Content 14', 44)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1030, 1, N'Android Alert Dialog Example', 15, N'HTML Lesson', 1, N'https://youtu.be/oqEXYBepqus', N'Content 15', 44)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1034, 1, N'How to Start New Activity On Button', 16, N'HTML Lesson', 1, N'https://youtu.be/3f0NAn5xFy4', N'Content 16', 44)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1036, 1, N'Android Analogclock And Digitalclock Example', 17, N'HTML Lesson', 1, N'https://youtu.be/xRXg7RaXG64', N'Content 17', 44)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1037, 1, N'Android Login Screen Example', 18, N'HTML Lesson', 1, N'https://youtu.be/PQqEKrr8KSQ', N'Content 18', 44)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1038, 1, N'Android Login Screen Example Part 2', 19, N'HTML Lesson', 1, N'https://youtu.be/ihHdHra7zF8', N'Content 19', 44)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1040, 1, N'Android ImageView example', 20, N'HTML Lesson', 1, N'https://youtu.be/IgbGeOIPu8w', N'Content 20', 44)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1043, 1, N'Service and Thread in Android', 21, N'HTML Lesson', 1, N'https://youtu.be/c7kjSs2l7iM', N'Content 21', 65)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1045, 1, N'Creating Service Using IntentService', 22, N'HTML Lesson', 1, N'https://youtu.be/p3RHxwA2-8A', N'Content 22', 65)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1046, 1, N'Bound Services', 23, N'HTML Lesson', 1, N'https://youtu.be/cJsqMisTaa8', N'Content 23', 65)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1048, 1, N'Applying Styles on Components', 24, N'HTML Lesson', 1, N'https://youtu.be/x0UkbmhxT6c', N'Content 24', 65)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1050, 1, N'Style inheritance in Android', 25, N'HTML Lesson', 1, N'https://youtu.be/E-wJJQFlQBs', N'Content 25', 65)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1051, 1, N'How to Save a File on Internal Storage', 26, N'HTML Lesson', 1, N'https://youtu.be/_15mKw--RG0', N'Content 26', 65)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1052, 1, N'Creating Database And Writing PHP Script', 27, N'HTML Lesson', 1, N'https://youtu.be/HK515-8-Q_w', N'Content 27', 65)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1053, 1, N'Android Login with PHP MySQL', 28, N'HTML Lesson', 1, N'https://youtu.be/eldh8l8yPew', N'Content 28', 65)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1054, 1, N'Connecting Android App to Online MySQL Database', 29, N'HTML Lesson', 1, N'https://youtu.be/UqY4DY2rHOs', N'Content 29', 65)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1057, 1, N'Insert Data in Mysql Database', 30, N'HTML Lesson', 1, N'https://youtu.be/age2l7Rrwtc', N'Content 30', 65)
INSERT [dbo].[Lesson] ([lesson_id], [subject_id], [lesson_name], [lesson_order], [lesson_type], [lesson_status], [lesson_link], [lesson_content], [parent_id]) VALUES (1060, 1, N'How to Convert a Website into Android Application', 31, N'HTML Lesson', 1, N'https://youtu.be/a5dlmqM9Oo8', N'Content 31', 65)
SET IDENTITY_INSERT [dbo].[Lesson] OFF
SET IDENTITY_INSERT [dbo].[Menu] ON 

INSERT [dbo].[Menu] ([menu_id], [parent_id], [menu_name], [menu_link], [menu_order], [menu_status], [menu_description]) VALUES (10, NULL, N'Menu 1', N'/menu1', 2, 0, N'444444')
INSERT [dbo].[Menu] ([menu_id], [parent_id], [menu_name], [menu_link], [menu_order], [menu_status], [menu_description]) VALUES (11, NULL, N'Menu 2', N'/menu2', 6, 0, NULL)
INSERT [dbo].[Menu] ([menu_id], [parent_id], [menu_name], [menu_link], [menu_order], [menu_status], [menu_description]) VALUES (12, NULL, N'Menu 3', N'/Menu3', 4, 1, NULL)
SET IDENTITY_INSERT [dbo].[Menu] OFF
SET IDENTITY_INSERT [dbo].[Permission] ON 

INSERT [dbo].[Permission] ([permission_id], [permission_name], [permission_link], [permission_status], [permission_describe]) VALUES (1, N'Permission 1', N'/menu/add', 1, N'Admin all permission')
INSERT [dbo].[Permission] ([permission_id], [permission_name], [permission_link], [permission_status], [permission_describe]) VALUES (2, N'Permission 2 ', N'/menu/dalete', 1, NULL)
INSERT [dbo].[Permission] ([permission_id], [permission_name], [permission_link], [permission_status], [permission_describe]) VALUES (3, N'Permission 3', N'demo', 1, N'demo')
INSERT [dbo].[Permission] ([permission_id], [permission_name], [permission_link], [permission_status], [permission_describe]) VALUES (1004, N'Permission 4', N'demo', 0, N'demo')
SET IDENTITY_INSERT [dbo].[Permission] OFF
SET IDENTITY_INSERT [dbo].[Post] ON 

INSERT [dbo].[Post] ([post_type], [post_category], [post_brief_info], [post_embeb], [post_name], [post_id], [post_thumbnail], [post_detail_info], [post_document_link], [post_status], [post_date]) VALUES (N'Guide', N'Login', N'An effective and well-designed project requires a sufficient amount of detailed information, avoiding too much information. Normally, the more details the plan looks good, but the slower the project implementation ...', NULL, N'So easy to project manager', 1, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/tintuc1.jpg?alt=media&token=f4f80835-0f38-48a2-a7a1-07c3a71a02b4', NULL, NULL, N'Submitted', N'June 6, 2019')
INSERT [dbo].[Post] ([post_type], [post_category], [post_brief_info], [post_embeb], [post_name], [post_id], [post_thumbnail], [post_detail_info], [post_document_link], [post_status], [post_date]) VALUES (N'Guide', N'Test for newbee', N'The offline course focuses on projects, hands-on projects and interviews geared toward work. Online course as an offline key document for the first 4 sessions.', NULL, N'Code is happy', 2, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/tintuc1.jpg?alt=media&token=f4f80835-0f38-48a2-a7a1-07c3a71a02b4', NULL, NULL, N'Submitted', N'June 6, 2019')
INSERT [dbo].[Post] ([post_type], [post_category], [post_brief_info], [post_embeb], [post_name], [post_id], [post_thumbnail], [post_detail_info], [post_document_link], [post_status], [post_date]) VALUES (N'Guide', N'Test for newbee', N'An effective and well-designed project requires a sufficient amount of detailed information, avoiding too much information. Normally, the more details', NULL, N'Code is happy', 4, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/tintuc1.jpg?alt=media&token=f4f80835-0f38-48a2-a7a1-07c3a71a02b4', NULL, NULL, N'Submitted', N'June 6, 2019')
INSERT [dbo].[Post] ([post_type], [post_category], [post_brief_info], [post_embeb], [post_name], [post_id], [post_thumbnail], [post_detail_info], [post_document_link], [post_status], [post_date]) VALUES (N'Guide', N'Test for newbee', N'An effective and well-designed project requires a sufficient amount of detailed information, avoiding too much information. Normally, the more details the plan looks good,', NULL, N'Code is happy', 6, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/tintuc1.jpg?alt=media&token=f4f80835-0f38-48a2-a7a1-07c3a71a02b4', NULL, NULL, N'Submitted', N'June 6, 2019')
INSERT [dbo].[Post] ([post_type], [post_category], [post_brief_info], [post_embeb], [post_name], [post_id], [post_thumbnail], [post_detail_info], [post_document_link], [post_status], [post_date]) VALUES (N'Guide', N'Test for newbee', N'An effective and well-designed project requires a sufficient amount of detailed information, avoiding too much information. Normally, the more details the plan looks good,', NULL, N'Code is happy', 8, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/tintuc1.jpg?alt=media&token=f4f80835-0f38-48a2-a7a1-07c3a71a02b4', NULL, NULL, N'Submitted', N'June 6, 2019')
INSERT [dbo].[Post] ([post_type], [post_category], [post_brief_info], [post_embeb], [post_name], [post_id], [post_thumbnail], [post_detail_info], [post_document_link], [post_status], [post_date]) VALUES (N'Guide', N'Test for new bee', N'An effective and well-designed project requires a sufficient amount of detailed information, avoiding too much information. Normally, the more details the plan looks good,', NULL, N'Code is happy', 1005, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/tintuc1.jpg?alt=media&token=f4f80835-0f38-48a2-a7a1-07c3a71a02b4', NULL, NULL, N'Submitted', N'June 6, 2019')
INSERT [dbo].[Post] ([post_type], [post_category], [post_brief_info], [post_embeb], [post_name], [post_id], [post_thumbnail], [post_detail_info], [post_document_link], [post_status], [post_date]) VALUES (N'Guide', N'Test for new bee', N'An effective and well-designed project requires a sufficient amount of detailed information, avoiding too much information. Normally, the more details the plan looks good,', NULL, N'Code is happy', 1006, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/tintuc1.jpg?alt=media&token=f4f80835-0f38-48a2-a7a1-07c3a71a02b4', NULL, NULL, N'Draft', N'June 6, 2019')
SET IDENTITY_INSERT [dbo].[Post] OFF
SET IDENTITY_INSERT [dbo].[Registration] ON 

INSERT [dbo].[Registration] ([registration_id], [user_id], [course_id], [registration_time], [registration_status]) VALUES (1, 1, 4, N'06/08/2019 13:45', N'Blocked')
INSERT [dbo].[Registration] ([registration_id], [user_id], [course_id], [registration_time], [registration_status]) VALUES (2, 2, 5, N'09/08/2018 04:20', N'Submitted')
INSERT [dbo].[Registration] ([registration_id], [user_id], [course_id], [registration_time], [registration_status]) VALUES (3, 2, 4, N'07/06/2011 07:56', N'Approved')
INSERT [dbo].[Registration] ([registration_id], [user_id], [course_id], [registration_time], [registration_status]) VALUES (9, 6, 4, N'07/06/2011 07:56', N'Approved')
SET IDENTITY_INSERT [dbo].[Registration] OFF
SET IDENTITY_INSERT [dbo].[RolePermission] ON 

INSERT [dbo].[RolePermission] ([role_permission_id], [role_id], [permission_id], [role_name]) VALUES (1, 1, 1, N'Admin')
INSERT [dbo].[RolePermission] ([role_permission_id], [role_id], [permission_id], [role_name]) VALUES (2, 2, 2, N'Teacher')
INSERT [dbo].[RolePermission] ([role_permission_id], [role_id], [permission_id], [role_name]) VALUES (3, 3, 3, N'Student')
SET IDENTITY_INSERT [dbo].[RolePermission] OFF
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([role_id], [role_name]) VALUES (1, N'Admin')
INSERT [dbo].[Roles] ([role_id], [role_name]) VALUES (2, N'Teacher')
INSERT [dbo].[Roles] ([role_id], [role_name]) VALUES (3, N'Student')
SET IDENTITY_INSERT [dbo].[Roles] OFF
SET IDENTITY_INSERT [dbo].[Setting] ON 

INSERT [dbo].[Setting] ([setting_id], [setting_group_value], [setting_name], [setting_description], [setting_order], [setting_status]) VALUES (1, N'Subject Category', N'Project Managerment', N'Select to set suject category to project managerment ', 2, 1)
INSERT [dbo].[Setting] ([setting_id], [setting_group_value], [setting_name], [setting_description], [setting_order], [setting_status]) VALUES (2, N'Lesson Type', N'Subject Topic', N'Select to set lesson type to subject topic', 1, 1)
INSERT [dbo].[Setting] ([setting_id], [setting_group_value], [setting_name], [setting_description], [setting_order], [setting_status]) VALUES (3, N'Subject Category', N'Sort Skills', N'Select to set suject category to sort skills', 1, 1)
INSERT [dbo].[Setting] ([setting_id], [setting_group_value], [setting_name], [setting_description], [setting_order], [setting_status]) VALUES (4, N'Post Type', N'Guide', N'Select to set post type to guide', 1, 1)
INSERT [dbo].[Setting] ([setting_id], [setting_group_value], [setting_name], [setting_description], [setting_order], [setting_status]) VALUES (5, N'Post Type', N'Resourse', N'Select to set post type to resourse', 2, 1)
INSERT [dbo].[Setting] ([setting_id], [setting_group_value], [setting_name], [setting_description], [setting_order], [setting_status]) VALUES (6, N'Guide Category', N'Login', N'Select to set guide category to login', 1, 1)
INSERT [dbo].[Setting] ([setting_id], [setting_group_value], [setting_name], [setting_description], [setting_order], [setting_status]) VALUES (7, N'Guide Category', N'Test for newbee', N'Select to set guide category to test for newbee', 2, 1)
INSERT [dbo].[Setting] ([setting_id], [setting_group_value], [setting_name], [setting_description], [setting_order], [setting_status]) VALUES (8, N'Subject Type', N'Exam', N'Select to set subject type to exam ', 1, 1)
INSERT [dbo].[Setting] ([setting_id], [setting_group_value], [setting_name], [setting_description], [setting_order], [setting_status]) VALUES (9, N'Subject Type', N'Online', N'Select to set subject type to online', 2, 1)
INSERT [dbo].[Setting] ([setting_id], [setting_group_value], [setting_name], [setting_description], [setting_order], [setting_status]) VALUES (10, N'Subject Type', N'Offline', N'Select to set subject type to offline', 3, 1)
INSERT [dbo].[Setting] ([setting_id], [setting_group_value], [setting_name], [setting_description], [setting_order], [setting_status]) VALUES (11, N'Subject Type', N'Blended', N'Select to set subject type to blended', 4, 1)
INSERT [dbo].[Setting] ([setting_id], [setting_group_value], [setting_name], [setting_description], [setting_order], [setting_status]) VALUES (12, N'Subject Type', N'External', N'Select to set subject type to external', 5, 1)
INSERT [dbo].[Setting] ([setting_id], [setting_group_value], [setting_name], [setting_description], [setting_order], [setting_status]) VALUES (13, N'Lesson Type', N'Video Lesson', N'Select to set lesson type to video lesson', 2, 1)
INSERT [dbo].[Setting] ([setting_id], [setting_group_value], [setting_name], [setting_description], [setting_order], [setting_status]) VALUES (14, N'Lesson Type', N'HTML Lesson', N'Select to set lesson type to html lesson', 3, 1)
INSERT [dbo].[Setting] ([setting_id], [setting_group_value], [setting_name], [setting_description], [setting_order], [setting_status]) VALUES (15, N'Lesson Type', N'Quiz', N'Select to set lesson type to quiz', 4, 1)
SET IDENTITY_INSERT [dbo].[Setting] OFF
SET IDENTITY_INSERT [dbo].[Slider] ON 

INSERT [dbo].[Slider] ([slider_id], [slider_title], [slider_caption], [slider_back_link], [slider_picture_url], [slider_status]) VALUES (1, N'Maths ', N'Maths is very easy', N'http://image/link.here', N'/Assets/customimg/ronaldo.jpg', N'Draft')
INSERT [dbo].[Slider] ([slider_id], [slider_title], [slider_caption], [slider_back_link], [slider_picture_url], [slider_status]) VALUES (2, N'Physics', N'Physics too difficult', N'http://image/link.here', N'/Assets/customimg/ronaldo.jpg', N'Submitted')
INSERT [dbo].[Slider] ([slider_id], [slider_title], [slider_caption], [slider_back_link], [slider_picture_url], [slider_status]) VALUES (3, N'History', N'History is too long', N'http://image/link.here', N'/Assets/customimg/ronaldo.jpg', N'Hiden')
SET IDENTITY_INSERT [dbo].[Slider] OFF
SET IDENTITY_INSERT [dbo].[Subject] ON 

INSERT [dbo].[Subject] ([subject_category], [subject_name], [subject_brief_info], [subject_type], [subject_status], [subject_tag_line], [subject_id], [picture], [ObjectiveCourse]) VALUES (N'Software Engineer', N'Mobile Application', N'Learn Android Programming and how to develop android mobile phone and ipad applications starting from Environment setup, application components', N'Online', N'Submitted', N'Software Engineer', 1, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/AndroidCourse.jpg?alt=media&token=ba15b2a6-1fbb-482a-831b-c01284ef12ca', N'Set up a working environment with the Java programming language.
Understand and apply properties in object-oriented programming.
Building a number of simple business programs.
Good programming orientation and towards high technology apply object-oriented programming.
')
INSERT [dbo].[Subject] ([subject_category], [subject_name], [subject_brief_info], [subject_type], [subject_status], [subject_tag_line], [subject_id], [picture], [ObjectiveCourse]) VALUES (N'Science', N'Physics', N'Physics, science that deals with the structure of matter and the interactions between the fundamental constituents of the observable universe.Explore more about the world', N'Online', N'Submitted', N'physical', 2, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/physicscourse.jpg?alt=media&token=403b6e8b-d2c9-4e10-9d3e-f2136d3cc01c', NULL)
INSERT [dbo].[Subject] ([subject_category], [subject_name], [subject_brief_info], [subject_type], [subject_status], [subject_tag_line], [subject_id], [picture], [ObjectiveCourse]) VALUES (N'Software Engineer', N'JAVA', N'Java is among the most popular programming languages out there, mainly because of how versatile and compatible it is,used for a large number of things', N'Online', N'Submitted', N'Software Engineer', 6, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/JavaCourse.jpg?alt=media&token=afd4136b-b7c6-4f62-9ad7-54dfe4eab11b', NULL)
INSERT [dbo].[Subject] ([subject_category], [subject_name], [subject_brief_info], [subject_type], [subject_status], [subject_tag_line], [subject_id], [picture], [ObjectiveCourse]) VALUES (N'Software Engineer', N'C#', N'C# is one of the most popular programming languages and can be used for a variety of things, including mobile applications, game development, and enterprise software.', N'Online', N'Submitted', N'Software Engineer', 10, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/C%23Course.jpg?alt=media&token=5cf8bbae-c53a-4da3-9e7c-d230b4befb31', NULL)
INSERT [dbo].[Subject] ([subject_category], [subject_name], [subject_brief_info], [subject_type], [subject_status], [subject_tag_line], [subject_id], [picture], [ObjectiveCourse]) VALUES (N'Software Engineer', N'Angular 7', N'Angular 7 is an open source JavaScript framework for building web applications and apps in JavaScript, html, and Typescript which is a superset of JavaScript. ', N'Online', N'Submitted', N'Software Engineer', 16, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/Angular7.jpg?alt=media&token=cf9ebac9-4aa7-442c-a045-aad9f54b6997', NULL)
INSERT [dbo].[Subject] ([subject_category], [subject_name], [subject_brief_info], [subject_type], [subject_status], [subject_tag_line], [subject_id], [picture], [ObjectiveCourse]) VALUES (N'Software Engineer', N'C Program', N'C is often considered to be the mother of all languages because so many other languages have been based on it. Most widely programming languages in the world', N'Online', N'Draft', N'Software Engineer', 17, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/CCourse.jpg?alt=media&token=0b955bfd-1208-4fb1-8319-ce6988bee8b6', NULL)
INSERT [dbo].[Subject] ([subject_category], [subject_name], [subject_brief_info], [subject_type], [subject_status], [subject_tag_line], [subject_id], [picture], [ObjectiveCourse]) VALUES (N'Software Engineer', N'Data Structures and Algorithms', N'This course provides learners with much knowledge of data structures and algorithms and allows them to design and install relevant algorithmic data structures to a specific problem. This is a compulsory subject for the degree of informatics in general ', N'Online', N'Submitted', N'Software Engineer', 18, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/CSDCourse.jpg?alt=media&token=2f1da8bb-c3ee-4efc-9595-a942a23bb32f', NULL)
INSERT [dbo].[Subject] ([subject_category], [subject_name], [subject_brief_info], [subject_type], [subject_status], [subject_tag_line], [subject_id], [picture], [ObjectiveCourse]) VALUES (N'Software Engineer', N'Front-End Web Development', N'If you would like to get started as a front-end web developer, you are going to LOVE this course! Work on projects ranging from a simple HTML page to a complete JavaScript based Google Chrome extension.Get started as a front-end web developer using HTML, CSS, JavaScript, jQuery, and Bootstrap!', N'Online', N'Submitted', N'Software Engineer', 19, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/fontendCourse.jpg?alt=media&token=ff9d2adc-74d1-460a-b0bf-86fe86910b86', NULL)
INSERT [dbo].[Subject] ([subject_category], [subject_name], [subject_brief_info], [subject_type], [subject_status], [subject_tag_line], [subject_id], [picture], [ObjectiveCourse]) VALUES (N'Software Engineer', N'Node.js Developer Course', N'Learning Node.js is a great way to get into backend web development, or expand your fullstack development practice. With FPT''''S hands-on Node.js courses, you can learn the concepts and applications of developing', N'Online', N'Submitted', N'Software Engineer', 20, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/nodejsCourse.jpg?alt=media&token=56b07287-9420-4ab4-b38a-aaf3a14734a1', NULL)
INSERT [dbo].[Subject] ([subject_category], [subject_name], [subject_brief_info], [subject_type], [subject_status], [subject_tag_line], [subject_id], [picture], [ObjectiveCourse]) VALUES (N'Software Engineer', N'Software Project Management', N'Become IT Project Management & SDLC expert-gain confidence in Waterfall, Agile and Kanban methodologies with MS projects', N'Online', N'Submitted', N'Software Engineer', 21, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/nodejsCourse.jpg?alt=media&token=56b07287-9420-4ab4-b38a-aaf3a14734a1', NULL)
SET IDENTITY_INSERT [dbo].[Subject] OFF
SET IDENTITY_INSERT [dbo].[TestBatch] ON 

INSERT [dbo].[TestBatch] ([batch_id], [batch_name]) VALUES (1, N'No')
INSERT [dbo].[TestBatch] ([batch_id], [batch_name]) VALUES (2, N'Yes')
SET IDENTITY_INSERT [dbo].[TestBatch] OFF
SET IDENTITY_INSERT [dbo].[TestResult] ON 

INSERT [dbo].[TestResult] ([test_user_id], [user_id], [test_id], [exam_id], [test_type], [batch_id], [tested], [average], [pass_rate], [tested_at]) VALUES (1, 1, 1, 1, N'Test', 1, 15, N'30%', N'40%', NULL)
INSERT [dbo].[TestResult] ([test_user_id], [user_id], [test_id], [exam_id], [test_type], [batch_id], [tested], [average], [pass_rate], [tested_at]) VALUES (2, 2, 2, 2, N'Test ', 2, 30, N'20%', N'50%', NULL)
SET IDENTITY_INSERT [dbo].[TestResult] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([user_id], [user_fullname], [user_email], [use_mobile], [user_status], [user_image], [user_description], [user_position], [check_recieveInformation], [user_gender]) VALUES (1, N'Nguyen Quang Nhat', N'nhat@gmail.com', N'0987675467', 0, NULL, N'Excellent Student', N'Student', 1, 1)
INSERT [dbo].[User] ([user_id], [user_fullname], [user_email], [use_mobile], [user_status], [user_image], [user_description], [user_position], [check_recieveInformation], [user_gender]) VALUES (2, N'Do Tung Duong', N'duong@gmail.com', N'0987654321', 0, NULL, NULL, N'Student', NULL, 1)
INSERT [dbo].[User] ([user_id], [user_fullname], [user_email], [use_mobile], [user_status], [user_image], [user_description], [user_position], [check_recieveInformation], [user_gender]) VALUES (3, N'Ngô Tùng Sơn', N'son@gmail.com', N'0123456789', 1, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/thaySon.jpg?alt=media&token=1ab94bfe-2dda-4eb9-9201-874c5e022a21', N'Master Computer Science from Lorraine University (France),a lecturer of FPT- Greenwich international bachelor program of FPT University.', N'Teacher', NULL, 1)
INSERT [dbo].[User] ([user_id], [user_fullname], [user_email], [use_mobile], [user_status], [user_image], [user_description], [user_position], [check_recieveInformation], [user_gender]) VALUES (4, N'	
Quách Ngọc Xuân', N'quan@gmail.com', N'0123456789', 1, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/thayXuan.jpg?alt=media&token=ae15bba2-68c7-4567-8d1f-b65222cac896', N'Manager education of Funix university', N'Mentor', NULL, 1)
INSERT [dbo].[User] ([user_id], [user_fullname], [user_email], [use_mobile], [user_status], [user_image], [user_description], [user_position], [check_recieveInformation], [user_gender]) VALUES (5, N'Do Tung Duong', N'duongdtse04940@fpt.edu.vn', N'0986714270', 1, NULL, NULL, N'Student', NULL, 1)
INSERT [dbo].[User] ([user_id], [user_fullname], [user_email], [use_mobile], [user_status], [user_image], [user_description], [user_position], [check_recieveInformation], [user_gender]) VALUES (6, N'NguyenAnhDuc-K11FUGHN', N'ducnase04962@fpt.edu.vn', N'0942624944', 0, NULL, N'Developer Fosft', N'Admin', NULL, 0)
INSERT [dbo].[User] ([user_id], [user_fullname], [user_email], [use_mobile], [user_status], [user_image], [user_description], [user_position], [check_recieveInformation], [user_gender]) VALUES (7, N'Nguyễn Trung Kiên', N'kien@gmail.com', N'0123456789', 1, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/thaykien.PNG?alt=media&token=8bd7404b-f06b-4d61-9130-bfa43498fa45', N'Project Manager and CTO of CMC Software', N'Manager', NULL, 1)
INSERT [dbo].[User] ([user_id], [user_fullname], [user_email], [use_mobile], [user_status], [user_image], [user_description], [user_position], [check_recieveInformation], [user_gender]) VALUES (8, N'Nguyễn Quyết', N'quyet@gmail.com', N'0123456789', 1, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/thayQuyet.jpg?alt=media&token=2a1c33f9-aa15-467c-8170-f7f2945fbbc1', N'Manager of center education CMC Software', N'Mentor', NULL, 1)
INSERT [dbo].[User] ([user_id], [user_fullname], [user_email], [use_mobile], [user_status], [user_image], [user_description], [user_position], [check_recieveInformation], [user_gender]) VALUES (12, N'Bùi Ngọc Anh', N'anh@gmail.com', N'0123456789', 1, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/thayAnh.jpg?alt=media&token=680effed-4883-4a8e-a551-53f1101d3eda', N'Master Computer Science from Lorraine University (France),a lecturer of FPT- Greenwich international bachelor program of FPT University.', N'Teacher', NULL, 1)
INSERT [dbo].[User] ([user_id], [user_fullname], [user_email], [use_mobile], [user_status], [user_image], [user_description], [user_position], [check_recieveInformation], [user_gender]) VALUES (15, N'Nguyễn Khắc Thành', N'thanh@gmail.com', N'0123456789', 1, N'https://firebasestorage.googleapis.com/v0/b/doan-872be.appspot.com/o/ThayThanh.jpg?alt=media&token=cafa43a3-de1a-43e8-96c0-bc90bce46ff5', N'Associate Doctor of Mathematics - Physics - Lomonosov University - Russia', N'HeadMaster ', NULL, 1)
INSERT [dbo].[User] ([user_id], [user_fullname], [user_email], [use_mobile], [user_status], [user_image], [user_description], [user_position], [check_recieveInformation], [user_gender]) VALUES (1013, N'DucAnh', N'ducnguyen@gmail.com', N'', 0, NULL, NULL, N'Student', NULL, 1)
INSERT [dbo].[User] ([user_id], [user_fullname], [user_email], [use_mobile], [user_status], [user_image], [user_description], [user_position], [check_recieveInformation], [user_gender]) VALUES (1014, N'AnhĐứcNguyễn', N'ducnasc00138x@funix.edu.vn', N'', 0, NULL, NULL, N'Student', NULL, 1)
INSERT [dbo].[User] ([user_id], [user_fullname], [user_email], [use_mobile], [user_status], [user_image], [user_description], [user_position], [check_recieveInformation], [user_gender]) VALUES (1016, N'bach', N'bachhtFX00045@funix.edu.vn', N'0123456789', 1, NULL, N'demo', N'Admin', 0, 0)
INSERT [dbo].[User] ([user_id], [user_fullname], [user_email], [use_mobile], [user_status], [user_image], [user_description], [user_position], [check_recieveInformation], [user_gender]) VALUES (1019, N'Đức Nguyễn', N'soitrangtn123@gmail.com', N'', 0, N'https://lh3.googleusercontent.com/a-/AAuE7mDx3nyxuRlqGEaqrM3B5XtnGL2BK1fIXJKLW_h2BQ', NULL, N'', NULL, 0)
SET IDENTITY_INSERT [dbo].[User] OFF
SET IDENTITY_INSERT [dbo].[UserRole] ON 

INSERT [dbo].[UserRole] ([user_role_id], [user_id], [role_id]) VALUES (1, 1, 3)
INSERT [dbo].[UserRole] ([user_role_id], [user_id], [role_id]) VALUES (2, 2, 3)
INSERT [dbo].[UserRole] ([user_role_id], [user_id], [role_id]) VALUES (8, 5, 3)
INSERT [dbo].[UserRole] ([user_role_id], [user_id], [role_id]) VALUES (9, 6, 1)
INSERT [dbo].[UserRole] ([user_role_id], [user_id], [role_id]) VALUES (10, 3, 2)
INSERT [dbo].[UserRole] ([user_role_id], [user_id], [role_id]) VALUES (11, 4, 2)
INSERT [dbo].[UserRole] ([user_role_id], [user_id], [role_id]) VALUES (12, 7, 2)
INSERT [dbo].[UserRole] ([user_role_id], [user_id], [role_id]) VALUES (13, 8, 2)
INSERT [dbo].[UserRole] ([user_role_id], [user_id], [role_id]) VALUES (14, 12, 2)
INSERT [dbo].[UserRole] ([user_role_id], [user_id], [role_id]) VALUES (1001, 1013, 1)
INSERT [dbo].[UserRole] ([user_role_id], [user_id], [role_id]) VALUES (1002, 1016, 1)
SET IDENTITY_INSERT [dbo].[UserRole] OFF
ALTER TABLE [dbo].[Lesson] ADD  DEFAULT ((1)) FOR [parent_id]
GO
ALTER TABLE [dbo].[AnswerOption]  WITH CHECK ADD FOREIGN KEY([question_id])
REFERENCES [dbo].[Question] ([question_id])
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD FOREIGN KEY([subject_id])
REFERENCES [dbo].[Subject] ([subject_id])
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD FOREIGN KEY([teacher_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Coursework]  WITH CHECK ADD FOREIGN KEY([course_id])
REFERENCES [dbo].[Course] ([course_id])
GO
ALTER TABLE [dbo].[Coursework]  WITH CHECK ADD FOREIGN KEY([test_id])
REFERENCES [dbo].[ExamTest] ([test_id])
GO
ALTER TABLE [dbo].[Coursework]  WITH CHECK ADD FOREIGN KEY([usercreate_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Domain]  WITH CHECK ADD FOREIGN KEY([subject_id])
REFERENCES [dbo].[Subject] ([subject_id])
GO
ALTER TABLE [dbo].[Exam]  WITH CHECK ADD FOREIGN KEY([subject_id])
REFERENCES [dbo].[Subject] ([subject_id])
GO
ALTER TABLE [dbo].[ExamConfig]  WITH CHECK ADD FOREIGN KEY([exam_id])
REFERENCES [dbo].[Exam] ([exam_id])
GO
ALTER TABLE [dbo].[ExamTest]  WITH CHECK ADD FOREIGN KEY([exam_id])
REFERENCES [dbo].[Exam] ([exam_id])
GO
ALTER TABLE [dbo].[Grade]  WITH CHECK ADD FOREIGN KEY([course_id])
REFERENCES [dbo].[Course] ([course_id])
GO
ALTER TABLE [dbo].[Grade]  WITH CHECK ADD FOREIGN KEY([coursework_id])
REFERENCES [dbo].[Coursework] ([coursework_id])
GO
ALTER TABLE [dbo].[Grade]  WITH CHECK ADD FOREIGN KEY([registration_id])
REFERENCES [dbo].[Registration] ([registration_id])
GO
ALTER TABLE [dbo].[Grade]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Lesson]  WITH NOCHECK ADD FOREIGN KEY([parent_id])
REFERENCES [dbo].[Lesson] ([lesson_id])
GO
ALTER TABLE [dbo].[Lesson]  WITH CHECK ADD FOREIGN KEY([subject_id])
REFERENCES [dbo].[Subject] ([subject_id])
GO
ALTER TABLE [dbo].[Menu]  WITH CHECK ADD FOREIGN KEY([parent_id])
REFERENCES [dbo].[Menu] ([menu_id])
GO
ALTER TABLE [dbo].[PostTag]  WITH CHECK ADD FOREIGN KEY([post_id])
REFERENCES [dbo].[Post] ([post_id])
GO
ALTER TABLE [dbo].[PostTag]  WITH CHECK ADD FOREIGN KEY([tag_id])
REFERENCES [dbo].[Tag] ([tag_id])
GO
ALTER TABLE [dbo].[Question]  WITH CHECK ADD FOREIGN KEY([domain_id])
REFERENCES [dbo].[Domain] ([domain_id])
GO
ALTER TABLE [dbo].[Question]  WITH CHECK ADD FOREIGN KEY([lesson_id])
REFERENCES [dbo].[Lesson] ([lesson_id])
GO
ALTER TABLE [dbo].[Question]  WITH CHECK ADD FOREIGN KEY([subject_id])
REFERENCES [dbo].[Subject] ([subject_id])
GO
ALTER TABLE [dbo].[Registration]  WITH CHECK ADD FOREIGN KEY([course_id])
REFERENCES [dbo].[Course] ([course_id])
GO
ALTER TABLE [dbo].[Registration]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[RoleMenu]  WITH CHECK ADD FOREIGN KEY([menu_id])
REFERENCES [dbo].[Menu] ([menu_id])
GO
ALTER TABLE [dbo].[RolePermission]  WITH CHECK ADD FOREIGN KEY([permission_id])
REFERENCES [dbo].[Permission] ([permission_id])
GO
ALTER TABLE [dbo].[RolePermission]  WITH CHECK ADD FOREIGN KEY([role_id])
REFERENCES [dbo].[Roles] ([role_id])
GO
ALTER TABLE [dbo].[SubjectReference]  WITH CHECK ADD FOREIGN KEY([post_id])
REFERENCES [dbo].[Post] ([post_id])
GO
ALTER TABLE [dbo].[SubjectReference]  WITH CHECK ADD FOREIGN KEY([subject_id])
REFERENCES [dbo].[Subject] ([subject_id])
GO
ALTER TABLE [dbo].[TestAnswer]  WITH CHECK ADD FOREIGN KEY([question_id])
REFERENCES [dbo].[Question] ([question_id])
GO
ALTER TABLE [dbo].[TestAnswer]  WITH CHECK ADD FOREIGN KEY([test_id])
REFERENCES [dbo].[ExamTest] ([test_id])
GO
ALTER TABLE [dbo].[TestAnswer]  WITH CHECK ADD FOREIGN KEY([test_user_id])
REFERENCES [dbo].[TestResult] ([test_user_id])
GO
ALTER TABLE [dbo].[TestQuestion]  WITH CHECK ADD FOREIGN KEY([question_id])
REFERENCES [dbo].[Question] ([question_id])
GO
ALTER TABLE [dbo].[TestQuestion]  WITH CHECK ADD FOREIGN KEY([test_id])
REFERENCES [dbo].[ExamTest] ([test_id])
GO
ALTER TABLE [dbo].[TestResult]  WITH CHECK ADD FOREIGN KEY([batch_id])
REFERENCES [dbo].[TestBatch] ([batch_id])
GO
ALTER TABLE [dbo].[TestResult]  WITH CHECK ADD FOREIGN KEY([exam_id])
REFERENCES [dbo].[Exam] ([exam_id])
GO
ALTER TABLE [dbo].[TestResult]  WITH CHECK ADD FOREIGN KEY([test_id])
REFERENCES [dbo].[ExamTest] ([test_id])
GO
ALTER TABLE [dbo].[TestResult]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD FOREIGN KEY([role_id])
REFERENCES [dbo].[Roles] ([role_id])
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[User] ([user_id])
GO
