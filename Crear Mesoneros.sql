USE [Restaurant]
GO

/****** Object:  Table [dbo].[Usuarios]    Script Date: 10/23/2011 08:39:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mesoneros](
	[IdMesonero] [nvarchar](6) NOT NULL,
	[Cedula] [nvarchar](20) NULL,
	[Nombre] [nvarchar](100) NULL,
	[Codigo] [nvarchar](20) NULL,
	[Puntos] [float] NULL,
	[Activo] [bit] NULL,
	[Direccion] [nvarchar](200) NULL,
	[Telefonos] [nvarchar](100) NULL
 CONSTRAINT [PK_Mesoneros] PRIMARY KEY CLUSTERED 
(
	[IdMesonero] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


