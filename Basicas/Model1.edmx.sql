
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/28/2018 20:11:16
-- Generated from EDMX file: C:\Users\hanse\Downloads\Bocaito\Basicas\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Bocaito];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CompraIngredientes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ComprasIngredientes] DROP CONSTRAINT [FK_CompraIngredientes];
GO
IF OBJECT_ID(N'[dbo].[FK_PlatoComentario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlatosComentarios] DROP CONSTRAINT [FK_PlatoComentario];
GO
IF OBJECT_ID(N'[dbo].[FK_PlatoContorno]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlatosContornos] DROP CONSTRAINT [FK_PlatoContorno];
GO
IF OBJECT_ID(N'[dbo].[FK_PlatoIngrediente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlatosIngredientes] DROP CONSTRAINT [FK_PlatoIngrediente];
GO
IF OBJECT_ID(N'[dbo].[FK_PlatoCombo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlatosCombos] DROP CONSTRAINT [FK_PlatoCombo];
GO
IF OBJECT_ID(N'[dbo].[FK_FacturaDetalles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FacturasPlatos] DROP CONSTRAINT [FK_FacturaDetalles];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientesMovimientos_Clientes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClientesMovimientos] DROP CONSTRAINT [FK_ClientesMovimientos_Clientes];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientesMovimientosDetalles_ClientesMovimientos]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClientesMovimientosDetalles] DROP CONSTRAINT [FK_ClientesMovimientosDetalles_ClientesMovimientos];
GO
IF OBJECT_ID(N'[dbo].[FK_Ingredienteinventarios]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IngredientesInventarios] DROP CONSTRAINT [FK_Ingredienteinventarios];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Compras]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Compras];
GO
IF OBJECT_ID(N'[dbo].[ComprasIngredientes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ComprasIngredientes];
GO
IF OBJECT_ID(N'[dbo].[Contadores]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contadores];
GO
IF OBJECT_ID(N'[dbo].[FacturasPlatos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FacturasPlatos];
GO
IF OBJECT_ID(N'[dbo].[LibroCompras]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LibroCompras];
GO
IF OBJECT_ID(N'[dbo].[LibroInventarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LibroInventarios];
GO
IF OBJECT_ID(N'[dbo].[LibroVentas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LibroVentas];
GO
IF OBJECT_ID(N'[dbo].[MesasAbiertas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MesasAbiertas];
GO
IF OBJECT_ID(N'[dbo].[MesasAbiertasPlatos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MesasAbiertasPlatos];
GO
IF OBJECT_ID(N'[dbo].[MesasAbiertasPlatosAnulados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MesasAbiertasPlatosAnulados];
GO
IF OBJECT_ID(N'[dbo].[Parametros]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Parametros];
GO
IF OBJECT_ID(N'[dbo].[Platos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Platos];
GO
IF OBJECT_ID(N'[dbo].[PlatosComentarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlatosComentarios];
GO
IF OBJECT_ID(N'[dbo].[PlatosContornos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlatosContornos];
GO
IF OBJECT_ID(N'[dbo].[PlatosIngredientes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlatosIngredientes];
GO
IF OBJECT_ID(N'[dbo].[Proveedores]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Proveedores];
GO
IF OBJECT_ID(N'[dbo].[Vales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Vales];
GO
IF OBJECT_ID(N'[dbo].[Mesoneros]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Mesoneros];
GO
IF OBJECT_ID(N'[dbo].[Ingredientes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ingredientes];
GO
IF OBJECT_ID(N'[dbo].[PlatosCombos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlatosCombos];
GO
IF OBJECT_ID(N'[dbo].[Mesas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Mesas];
GO
IF OBJECT_ID(N'[dbo].[Facturas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Facturas];
GO
IF OBJECT_ID(N'[dbo].[Clientes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Clientes];
GO
IF OBJECT_ID(N'[dbo].[ClientesMovimientos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClientesMovimientos];
GO
IF OBJECT_ID(N'[dbo].[ClientesMovimientosDetalles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClientesMovimientosDetalles];
GO
IF OBJECT_ID(N'[dbo].[Usuarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Usuarios];
GO
IF OBJECT_ID(N'[dbo].[IngredientesInventariosHistorials]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IngredientesInventariosHistorials];
GO
IF OBJECT_ID(N'[dbo].[IngredientesInventarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IngredientesInventarios];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Compras'
CREATE TABLE [dbo].[Compras] (
    [IdCompra] nvarchar(6)  NOT NULL,
    [Mes] int  NULL,
    [Año] int  NULL,
    [Fecha] datetime  NULL,
    [Numero] nvarchar(100)  NULL,
    [NumeroControl] nvarchar(100)  NULL,
    [MontoExento] float  NULL,
    [MontoGravable] float  NULL,
    [MontoIva] float  NULL,
    [MontoTotal] float  NULL,
    [CedulaRif] nvarchar(20)  NULL,
    [RazonSocial] nvarchar(150)  NULL,
    [TasaIva] float  NULL,
    [MontoSinDerechoCredito] float  NULL,
    [IdProveedor] nvarchar(6)  NULL,
    [IdUsuario] nvarchar(6)  NULL,
    [Direccion] nvarchar(150)  NULL,
    [LibroInventarios] bit  NULL,
    [LibroCompras] bit  NULL,
    [ActualizacionCostos] bit  NULL,
    [IncluirLibroCompras] bit  NULL,
    [Estatus] nvarchar(20)  NULL
);
GO

-- Creating table 'ComprasIngredientes'
CREATE TABLE [dbo].[ComprasIngredientes] (
    [IdCompraIngrediente] nvarchar(6)  NOT NULL,
    [IdCompra] nvarchar(6)  NULL,
    [Ingrediente] nvarchar(100)  NULL,
    [Cantidad] float  NULL,
    [Costo] float  NULL,
    [TasaIva] float  NULL,
    [CostoIva] float  NULL,
    [CantidadNeta] float  NULL,
    [Total] float  NULL,
    [IdIngrediente] nvarchar(6)  NULL,
    [ExistenciaAnterior] float  NULL,
    [ExistenciaNueva] float  NULL,
    [UnidadMedida] nvarchar(100)  NULL,
    [Iva] float  NULL,
    [CostoNeto] float  NULL,
    [Grupo] nvarchar(100)  NULL
);
GO

-- Creating table 'Contadores'
CREATE TABLE [dbo].[Contadores] (
    [Variable] nvarchar(100)  NOT NULL,
    [Valor] int  NULL
);
GO

-- Creating table 'FacturasPlatos'
CREATE TABLE [dbo].[FacturasPlatos] (
    [IdFacturaPlato] nvarchar(6)  NOT NULL,
    [IdFactura] nvarchar(10)  NULL,
    [Idplato] nvarchar(6)  NULL,
    [Codigo] nvarchar(20)  NULL,
    [Grupo] nvarchar(20)  NULL,
    [Descripcion] nvarchar(100)  NULL,
    [Cantidad] int  NULL,
    [Precio] float  NULL,
    [TasaIva] float  NULL,
    [PrecioConIva] float  NULL,
    [Total] float  NULL,
    [Costo] float  NULL
);
GO

-- Creating table 'LibroCompras'
CREATE TABLE [dbo].[LibroCompras] (
    [IdLibroCompras] nvarchar(6)  NOT NULL,
    [Fecha] datetime  NULL,
    [Mes] int  NULL,
    [Año] int  NULL,
    [Numero] nvarchar(20)  NULL,
    [NumeroControl] nvarchar(20)  NULL,
    [MontoExento] float  NULL,
    [MontoGravable] float  NULL,
    [MontoIva] float  NULL,
    [MontoTotal] float  NULL,
    [CedulaRif] nvarchar(20)  NULL,
    [RazonSocial] nvarchar(150)  NULL,
    [TasaIva] float  NULL,
    [Direccion] nvarchar(150)  NULL,
    [IvaRetenido] float  NULL,
    [ComprobanteRetencion] nvarchar(20)  NULL
);
GO

-- Creating table 'LibroInventarios'
CREATE TABLE [dbo].[LibroInventarios] (
    [IdLibroInventarios] nvarchar(6)  NOT NULL,
    [Inicio] float  NULL,
    [Entradas] float  NULL,
    [Salidas] float  NULL,
    [Final] float  NULL,
    [InventarioFisico] float  NULL,
    [Ajustes] float  NULL,
    [Costo] float  NULL,
    [IdProducto] nvarchar(6)  NULL,
    [Producto] nvarchar(100)  NULL,
    [Mes] int  NULL,
    [Año] int  NULL,
    [Fecha] datetime  NULL
);
GO

-- Creating table 'LibroVentas'
CREATE TABLE [dbo].[LibroVentas] (
    [IdLibroVentas] nvarchar(6)  NOT NULL,
    [Fecha] datetime  NULL,
    [Factura] nvarchar(20)  NULL,
    [NumeroZ] nvarchar(6)  NULL,
    [RegistroMaquinaFiscal] nvarchar(20)  NULL,
    [RazonSocial] nvarchar(150)  NULL,
    [CedulaRif] nvarchar(20)  NULL,
    [Comprobante] nvarchar(20)  NULL,
    [FacturaAfectada] nvarchar(20)  NULL,
    [TipoOperacion] nvarchar(2)  NULL,
    [MontoTotal] float  NULL,
    [MontoGravable] float  NULL,
    [MontoExento] float  NULL,
    [MontoIva] float  NULL,
    [TasaIva] float  NULL,
    [IvaRetenido] float  NULL,
    [Año] int  NULL,
    [Mes] int  NULL,
    [Direccion] nvarchar(150)  NULL
);
GO

-- Creating table 'MesasAbiertas'
CREATE TABLE [dbo].[MesasAbiertas] (
    [IdMesaAbierta] nvarchar(6)  NOT NULL,
    [IdMesa] nvarchar(6)  NULL,
    [IdMesonero] nvarchar(6)  NULL,
    [Mesa] nvarchar(100)  NULL,
    [Mesonero] nvarchar(100)  NULL,
    [Personas] int  NULL,
    [Apertura] datetime  NULL,
    [MontoGravable] float  NULL,
    [MontoExento] float  NULL,
    [MontoIva] float  NULL,
    [MontoTotal] float  NULL,
    [MontoServicio] float  NULL,
    [Estatus] nvarchar(100)  NULL,
    [Numero] nvarchar(6)  NULL,
    [Impresa] bit  NULL
);
GO

-- Creating table 'MesasAbiertasPlatos'
CREATE TABLE [dbo].[MesasAbiertasPlatos] (
    [IdMesaAbiertaPlato] nvarchar(6)  NOT NULL,
    [IdMesaAbierta] nvarchar(6)  NULL,
    [Idplato] nvarchar(6)  NULL,
    [Codigo] nvarchar(100)  NULL,
    [Grupo] nvarchar(100)  NULL,
    [Descripcion] nvarchar(100)  NULL,
    [Cantidad] int  NULL,
    [Precio] float  NULL,
    [TasaIva] float  NULL,
    [PrecioConIva] float  NULL,
    [Total] float  NULL,
    [Costo] float  NULL,
    [EnviarComanda] nvarchar(50)  NULL
);
GO

-- Creating table 'MesasAbiertasPlatosAnulados'
CREATE TABLE [dbo].[MesasAbiertasPlatosAnulados] (
    [IdPlatoEliminado] nvarchar(6)  NOT NULL,
    [IdCajero] nvarchar(6)  NULL,
    [Cajero] nvarchar(50)  NULL,
    [Fecha] datetime  NULL,
    [IdPlato] nvarchar(6)  NULL,
    [Plato] nvarchar(100)  NULL,
    [Cantidad] float  NULL,
    [Precio] float  NULL,
    [PrecioIva] float  NULL,
    [Total] float  NULL,
    [Numero] nvarchar(6)  NULL,
    [IdUsuario] nvarchar(6)  NULL,
    [Mesa] nvarchar(50)  NULL,
    [Mesonero] nvarchar(50)  NULL
);
GO

-- Creating table 'Parametros'
CREATE TABLE [dbo].[Parametros] (
    [Empresa] nvarchar(250)  NOT NULL,
    [EmpresaRif] nvarchar(14)  NULL,
    [EmpresaDireccion] nvarchar(250)  NULL,
    [EmpresaTelefonos] nvarchar(100)  NULL,
    [MaquinaFiscal] nvarchar(14)  NULL,
    [TasaIva] float  NULL,
    [TipoIva] nvarchar(100)  NULL,
    [PuertoImpresoraFiscal] nvarchar(100)  NULL,
    [Ciudad] nvarchar(100)  NULL,
    [Licencia] nvarchar(100)  NULL,
    [ImprimirOrden] nvarchar(100)  NULL,
    [ImprimirCorteCuenta] nvarchar(100)  NULL,
    [ColaImpresionFiscalActiva] bit  NULL,
    [UsoImpresoraFiscal] nvarchar(100)  NULL,
    [SolicitarMesonero] bit  NULL
);
GO

-- Creating table 'Platos'
CREATE TABLE [dbo].[Platos] (
    [IdPlato] nvarchar(6)  NOT NULL,
    [Grupo] nvarchar(20)  NULL,
    [Codigo] nvarchar(20)  NULL,
    [Descripcion] nvarchar(100)  NULL,
    [Precio] float  NULL,
    [TasaIva] float  NULL,
    [PrecioConIva] float  NULL,
    [EnviarComanda] nvarchar(100)  NULL,
    [Activo] bit  NULL,
    [Costo] float  NULL,
    [MostrarMenu] bit  NULL
);
GO

-- Creating table 'PlatosComentarios'
CREATE TABLE [dbo].[PlatosComentarios] (
    [IdPlatoComentario] nvarchar(6)  NOT NULL,
    [IdPlato] nvarchar(6)  NULL,
    [Comentario] nvarchar(100)  NULL
);
GO

-- Creating table 'PlatosContornos'
CREATE TABLE [dbo].[PlatosContornos] (
    [IdPlatoContorno] nvarchar(6)  NOT NULL,
    [IdPlato] nvarchar(6)  NULL,
    [Contorno] nvarchar(100)  NULL
);
GO

-- Creating table 'PlatosIngredientes'
CREATE TABLE [dbo].[PlatosIngredientes] (
    [IdPlatoIngrediente] nvarchar(6)  NOT NULL,
    [IdPlato] nvarchar(6)  NULL,
    [IdIngrediente] nvarchar(6)  NULL,
    [Cantidad] float  NULL,
    [Raciones] float  NULL,
    [CostoRacion] float  NULL,
    [Ingrediente] nvarchar(100)  NULL,
    [UnidadMedida] nvarchar(20)  NULL,
    [IdPlatoPresentacion] nvarchar(6)  NULL,
    [Total] float  NULL
);
GO

-- Creating table 'Proveedores'
CREATE TABLE [dbo].[Proveedores] (
    [IdProveedor] nvarchar(6)  NOT NULL,
    [RazonSocial] nvarchar(100)  NULL,
    [CedulaRif] nvarchar(20)  NULL,
    [Direccion] nvarchar(100)  NULL,
    [Telefonos] nvarchar(100)  NULL,
    [Email] nvarchar(100)  NULL,
    [LimiteCredito] float  NULL,
    [DiasCredito] int  NULL,
    [Contacto] nvarchar(100)  NULL,
    [Activo] bit  NULL
);
GO

-- Creating table 'Vales'
CREATE TABLE [dbo].[Vales] (
    [IdVale] nvarchar(6)  NOT NULL,
    [Fecha] datetime  NULL,
    [IdCajero] nvarchar(6)  NULL,
    [Concepto] nvarchar(150)  NULL,
    [Monto] float  NULL,
    [Cajero] nvarchar(100)  NULL,
    [Numero] nvarchar(6)  NULL
);
GO

-- Creating table 'Mesoneros'
CREATE TABLE [dbo].[Mesoneros] (
    [IdMesonero] nvarchar(6)  NOT NULL,
    [Cedula] nvarchar(20)  NULL,
    [Nombre] nvarchar(100)  NULL,
    [Codigo] nvarchar(20)  NULL,
    [Puntos] float  NULL,
    [Activo] bit  NULL,
    [Direccion] nvarchar(200)  NULL,
    [Telefonos] nvarchar(100)  NULL
);
GO

-- Creating table 'Ingredientes'
CREATE TABLE [dbo].[Ingredientes] (
    [IdIngrediente] nvarchar(6)  NOT NULL,
    [Descripcion] nvarchar(100)  NULL,
    [UnidadMedida] nvarchar(20)  NULL,
    [Costo] float  NULL,
    [Raciones] float  NULL,
    [Existencia] float  NULL,
    [Grupo] nvarchar(100)  NULL,
    [Activo] bit  NULL,
    [Insumo] bit  NULL,
    [LlevaInventario] bit  NULL,
    [TasaIva] float  NULL
);
GO

-- Creating table 'PlatosCombos'
CREATE TABLE [dbo].[PlatosCombos] (
    [IdPlatoCombo] nvarchar(6)  NOT NULL,
    [IdPlato] nvarchar(6)  NULL,
    [Plato] nvarchar(100)  NULL,
    [Cantidad] float  NULL,
    [Costo] float  NULL,
    [TotalCosto] float  NULL,
    [IdCombo] nvarchar(6)  NULL
);
GO

-- Creating table 'Mesas'
CREATE TABLE [dbo].[Mesas] (
    [IdMesa] nvarchar(6)  NOT NULL,
    [Codigo] nvarchar(20)  NULL,
    [Descripcion] nvarchar(100)  NULL,
    [Ubicacion] nvarchar(100)  NULL,
    [CobraServicio] bit  NULL,
    [Descuento] float  NULL
);
GO

-- Creating table 'Facturas'
CREATE TABLE [dbo].[Facturas] (
    [IdFactura] nvarchar(10)  NOT NULL,
    [Tipo] nvarchar(20)  NULL,
    [Fecha] datetime  NULL,
    [Hora] datetime  NULL,
    [Numero] nvarchar(10)  NULL,
    [NumeroOrden] nvarchar(100)  NULL,
    [NumeroZ] nvarchar(10)  NULL,
    [MaquinaFiscal] nvarchar(14)  NULL,
    [IdCajero] nvarchar(6)  NULL,
    [Cajero] nvarchar(100)  NULL,
    [CedulaRif] nvarchar(100)  NULL,
    [RazonSocial] nvarchar(100)  NULL,
    [Direccion] nvarchar(250)  NULL,
    [Email] nvarchar(100)  NULL,
    [MontoExento] float  NULL,
    [MontoGravable] float  NULL,
    [MontoServicio] float  NULL,
    [TasaIva] float  NULL,
    [MontoIva] float  NULL,
    [MontoTotal] float  NULL,
    [Efectivo] float  NULL,
    [Cambio] float  NULL,
    [Cheque] float  NULL,
    [Tarjeta] float  NULL,
    [CestaTicket] float  NULL,
    [ConsumoInterno] float  NULL,
    [Descuento] float  NULL,
    [Credito] float  NULL,
    [Saldo] float  NULL,
    [LibroInventarios] bit  NULL,
    [LibroVentas] bit  NULL,
    [Inventarios] bit  NULL,
    [Anulado] bit  NULL,
    [Mesonero] nvarchar(100)  NULL
);
GO

-- Creating table 'Clientes'
CREATE TABLE [dbo].[Clientes] (
    [CedulaRif] nvarchar(20)  NOT NULL,
    [RazonSocial] nvarchar(100)  NULL,
    [Direccion] nvarchar(100)  NULL,
    [DiasCredito] int  NULL,
    [LimiteCredito] float  NULL,
    [Telefonos] nvarchar(100)  NULL,
    [Contacto] nvarchar(100)  NULL,
    [Email] nvarchar(100)  NULL
);
GO

-- Creating table 'ClientesMovimientos'
CREATE TABLE [dbo].[ClientesMovimientos] (
    [IdClienteMovimiento] nvarchar(6)  NOT NULL,
    [IdUsuario] nvarchar(6)  NULL,
    [Fecha] datetime  NULL,
    [Vence] datetime  NULL,
    [Mes] int  NULL,
    [Año] int  NULL,
    [CedulaRif] nvarchar(20)  NULL,
    [RazonSocial] nvarchar(100)  NULL,
    [Tipo] nvarchar(20)  NULL,
    [Concepto] nvarchar(150)  NULL,
    [Comentarios] nvarchar(150)  NULL,
    [Monto] float  NULL,
    [Saldo] float  NULL,
    [Credito] float  NULL,
    [Debito] float  NULL,
    [PagarHoy] float  NULL,
    [Efectivo] float  NULL,
    [Cheque] float  NULL,
    [ChequeBanco] nvarchar(50)  NULL,
    [ChequeNumero] nvarchar(50)  NULL,
    [Tarjeta] float  NULL,
    [Deposito] float  NULL,
    [DepositoBanco] nvarchar(50)  NULL,
    [DepositoNumero] nvarchar(50)  NULL,
    [CestaTicket] float  NULL,
    [RetencionISLR] float  NULL,
    [ComprobanteISLR] nvarchar(50)  NULL,
    [RetencionIVA] float  NULL,
    [ComprobanteIVA] nvarchar(50)  NULL
);
GO

-- Creating table 'ClientesMovimientosDetalles'
CREATE TABLE [dbo].[ClientesMovimientosDetalles] (
    [IdClientesMovimientosDetalle] nvarchar(6)  NOT NULL,
    [IdClienteMovimiento] nvarchar(6)  NULL,
    [Monto] float  NULL,
    [IdMovimientoPagado] nvarchar(6)  NULL
);
GO

-- Creating table 'Usuarios'
CREATE TABLE [dbo].[Usuarios] (
    [IdUsuario] nvarchar(6)  NOT NULL,
    [Cedula] nvarchar(20)  NULL,
    [Nombre] nvarchar(100)  NULL,
    [TipoUsuario] nvarchar(20)  NULL,
    [Clave] nvarchar(20)  NULL,
    [Direccion] nvarchar(200)  NULL,
    [Telefonos] nvarchar(100)  NULL,
    [PuedeDarConsumoInterno] bit  NULL,
    [PuedeSepararCuentas] bit  NULL,
    [PuedePedirCorteDeCuenta] bit  NULL,
    [PuedeRegistrarPago] bit  NULL,
    [PuedeCambiarMesa] bit  NULL,
    [PuedeDarCreditos] bit  NULL,
    [ReporteX] bit  NULL,
    [ReporteZ] bit  NULL,
    [Vale] bit  NULL,
    [CierreDeCaja] bit  NULL,
    [ContarDinero] bit  NULL,
    [Activo] bit  NULL,
    [Codigo] nvarchar(20)  NULL,
    [Puntos] float  NULL,
    [PuedeEliminarPlatos] bit  NULL
);
GO

-- Creating table 'IngredientesInventariosHistorials'
CREATE TABLE [dbo].[IngredientesInventariosHistorials] (
    [IdIngredienteInventario] nvarchar(6)  NOT NULL,
    [IdIngrediente] nvarchar(6)  NULL,
    [Grupo] nvarchar(50)  NULL,
    [Ingrediente] nvarchar(100)  NULL,
    [FechaInicio] datetime  NULL,
    [FechaFinal] datetime  NULL,
    [Inicio] float  NULL,
    [Entradas] float  NULL,
    [Salidas] float  NULL,
    [Final] float  NULL,
    [InventarioFisico] float  NULL,
    [Ajuste] float  NULL
);
GO

-- Creating table 'IngredientesInventarios'
CREATE TABLE [dbo].[IngredientesInventarios] (
    [IdIngredienteInventario] nvarchar(6)  NOT NULL,
    [IdIngrediente] nvarchar(6)  NULL,
    [Ingrediente] nvarchar(100)  NULL,
    [Inicio] float  NULL,
    [Entradas] float  NULL,
    [Salidas] float  NULL,
    [Final] float  NULL,
    [InventarioFisico] float  NULL,
    [Ajuste] float  NULL,
    [Deposito] nvarchar(100)  NULL,
    [FechaInicio] datetime  NULL,
    [Grupo] nvarchar(50)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdCompra] in table 'Compras'
ALTER TABLE [dbo].[Compras]
ADD CONSTRAINT [PK_Compras]
    PRIMARY KEY CLUSTERED ([IdCompra] ASC);
GO

-- Creating primary key on [IdCompraIngrediente] in table 'ComprasIngredientes'
ALTER TABLE [dbo].[ComprasIngredientes]
ADD CONSTRAINT [PK_ComprasIngredientes]
    PRIMARY KEY CLUSTERED ([IdCompraIngrediente] ASC);
GO

-- Creating primary key on [Variable] in table 'Contadores'
ALTER TABLE [dbo].[Contadores]
ADD CONSTRAINT [PK_Contadores]
    PRIMARY KEY CLUSTERED ([Variable] ASC);
GO

-- Creating primary key on [IdFacturaPlato] in table 'FacturasPlatos'
ALTER TABLE [dbo].[FacturasPlatos]
ADD CONSTRAINT [PK_FacturasPlatos]
    PRIMARY KEY CLUSTERED ([IdFacturaPlato] ASC);
GO

-- Creating primary key on [IdLibroCompras] in table 'LibroCompras'
ALTER TABLE [dbo].[LibroCompras]
ADD CONSTRAINT [PK_LibroCompras]
    PRIMARY KEY CLUSTERED ([IdLibroCompras] ASC);
GO

-- Creating primary key on [IdLibroInventarios] in table 'LibroInventarios'
ALTER TABLE [dbo].[LibroInventarios]
ADD CONSTRAINT [PK_LibroInventarios]
    PRIMARY KEY CLUSTERED ([IdLibroInventarios] ASC);
GO

-- Creating primary key on [IdLibroVentas] in table 'LibroVentas'
ALTER TABLE [dbo].[LibroVentas]
ADD CONSTRAINT [PK_LibroVentas]
    PRIMARY KEY CLUSTERED ([IdLibroVentas] ASC);
GO

-- Creating primary key on [IdMesaAbierta] in table 'MesasAbiertas'
ALTER TABLE [dbo].[MesasAbiertas]
ADD CONSTRAINT [PK_MesasAbiertas]
    PRIMARY KEY CLUSTERED ([IdMesaAbierta] ASC);
GO

-- Creating primary key on [IdMesaAbiertaPlato] in table 'MesasAbiertasPlatos'
ALTER TABLE [dbo].[MesasAbiertasPlatos]
ADD CONSTRAINT [PK_MesasAbiertasPlatos]
    PRIMARY KEY CLUSTERED ([IdMesaAbiertaPlato] ASC);
GO

-- Creating primary key on [IdPlatoEliminado] in table 'MesasAbiertasPlatosAnulados'
ALTER TABLE [dbo].[MesasAbiertasPlatosAnulados]
ADD CONSTRAINT [PK_MesasAbiertasPlatosAnulados]
    PRIMARY KEY CLUSTERED ([IdPlatoEliminado] ASC);
GO

-- Creating primary key on [Empresa] in table 'Parametros'
ALTER TABLE [dbo].[Parametros]
ADD CONSTRAINT [PK_Parametros]
    PRIMARY KEY CLUSTERED ([Empresa] ASC);
GO

-- Creating primary key on [IdPlato] in table 'Platos'
ALTER TABLE [dbo].[Platos]
ADD CONSTRAINT [PK_Platos]
    PRIMARY KEY CLUSTERED ([IdPlato] ASC);
GO

-- Creating primary key on [IdPlatoComentario] in table 'PlatosComentarios'
ALTER TABLE [dbo].[PlatosComentarios]
ADD CONSTRAINT [PK_PlatosComentarios]
    PRIMARY KEY CLUSTERED ([IdPlatoComentario] ASC);
GO

-- Creating primary key on [IdPlatoContorno] in table 'PlatosContornos'
ALTER TABLE [dbo].[PlatosContornos]
ADD CONSTRAINT [PK_PlatosContornos]
    PRIMARY KEY CLUSTERED ([IdPlatoContorno] ASC);
GO

-- Creating primary key on [IdPlatoIngrediente] in table 'PlatosIngredientes'
ALTER TABLE [dbo].[PlatosIngredientes]
ADD CONSTRAINT [PK_PlatosIngredientes]
    PRIMARY KEY CLUSTERED ([IdPlatoIngrediente] ASC);
GO

-- Creating primary key on [IdProveedor] in table 'Proveedores'
ALTER TABLE [dbo].[Proveedores]
ADD CONSTRAINT [PK_Proveedores]
    PRIMARY KEY CLUSTERED ([IdProveedor] ASC);
GO

-- Creating primary key on [IdVale] in table 'Vales'
ALTER TABLE [dbo].[Vales]
ADD CONSTRAINT [PK_Vales]
    PRIMARY KEY CLUSTERED ([IdVale] ASC);
GO

-- Creating primary key on [IdMesonero] in table 'Mesoneros'
ALTER TABLE [dbo].[Mesoneros]
ADD CONSTRAINT [PK_Mesoneros]
    PRIMARY KEY CLUSTERED ([IdMesonero] ASC);
GO

-- Creating primary key on [IdIngrediente] in table 'Ingredientes'
ALTER TABLE [dbo].[Ingredientes]
ADD CONSTRAINT [PK_Ingredientes]
    PRIMARY KEY CLUSTERED ([IdIngrediente] ASC);
GO

-- Creating primary key on [IdPlatoCombo] in table 'PlatosCombos'
ALTER TABLE [dbo].[PlatosCombos]
ADD CONSTRAINT [PK_PlatosCombos]
    PRIMARY KEY CLUSTERED ([IdPlatoCombo] ASC);
GO

-- Creating primary key on [IdMesa] in table 'Mesas'
ALTER TABLE [dbo].[Mesas]
ADD CONSTRAINT [PK_Mesas]
    PRIMARY KEY CLUSTERED ([IdMesa] ASC);
GO

-- Creating primary key on [IdFactura] in table 'Facturas'
ALTER TABLE [dbo].[Facturas]
ADD CONSTRAINT [PK_Facturas]
    PRIMARY KEY CLUSTERED ([IdFactura] ASC);
GO

-- Creating primary key on [CedulaRif] in table 'Clientes'
ALTER TABLE [dbo].[Clientes]
ADD CONSTRAINT [PK_Clientes]
    PRIMARY KEY CLUSTERED ([CedulaRif] ASC);
GO

-- Creating primary key on [IdClienteMovimiento] in table 'ClientesMovimientos'
ALTER TABLE [dbo].[ClientesMovimientos]
ADD CONSTRAINT [PK_ClientesMovimientos]
    PRIMARY KEY CLUSTERED ([IdClienteMovimiento] ASC);
GO

-- Creating primary key on [IdClientesMovimientosDetalle] in table 'ClientesMovimientosDetalles'
ALTER TABLE [dbo].[ClientesMovimientosDetalles]
ADD CONSTRAINT [PK_ClientesMovimientosDetalles]
    PRIMARY KEY CLUSTERED ([IdClientesMovimientosDetalle] ASC);
GO

-- Creating primary key on [IdUsuario] in table 'Usuarios'
ALTER TABLE [dbo].[Usuarios]
ADD CONSTRAINT [PK_Usuarios]
    PRIMARY KEY CLUSTERED ([IdUsuario] ASC);
GO

-- Creating primary key on [IdIngredienteInventario] in table 'IngredientesInventariosHistorials'
ALTER TABLE [dbo].[IngredientesInventariosHistorials]
ADD CONSTRAINT [PK_IngredientesInventariosHistorials]
    PRIMARY KEY CLUSTERED ([IdIngredienteInventario] ASC);
GO

-- Creating primary key on [IdIngredienteInventario] in table 'IngredientesInventarios'
ALTER TABLE [dbo].[IngredientesInventarios]
ADD CONSTRAINT [PK_IngredientesInventarios]
    PRIMARY KEY CLUSTERED ([IdIngredienteInventario] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IdCompra] in table 'ComprasIngredientes'
ALTER TABLE [dbo].[ComprasIngredientes]
ADD CONSTRAINT [FK_CompraIngredientes]
    FOREIGN KEY ([IdCompra])
    REFERENCES [dbo].[Compras]
        ([IdCompra])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompraIngredientes'
CREATE INDEX [IX_FK_CompraIngredientes]
ON [dbo].[ComprasIngredientes]
    ([IdCompra]);
GO

-- Creating foreign key on [IdPlato] in table 'PlatosComentarios'
ALTER TABLE [dbo].[PlatosComentarios]
ADD CONSTRAINT [FK_PlatoComentario]
    FOREIGN KEY ([IdPlato])
    REFERENCES [dbo].[Platos]
        ([IdPlato])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlatoComentario'
CREATE INDEX [IX_FK_PlatoComentario]
ON [dbo].[PlatosComentarios]
    ([IdPlato]);
GO

-- Creating foreign key on [IdPlato] in table 'PlatosContornos'
ALTER TABLE [dbo].[PlatosContornos]
ADD CONSTRAINT [FK_PlatoContorno]
    FOREIGN KEY ([IdPlato])
    REFERENCES [dbo].[Platos]
        ([IdPlato])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlatoContorno'
CREATE INDEX [IX_FK_PlatoContorno]
ON [dbo].[PlatosContornos]
    ([IdPlato]);
GO

-- Creating foreign key on [IdPlato] in table 'PlatosIngredientes'
ALTER TABLE [dbo].[PlatosIngredientes]
ADD CONSTRAINT [FK_PlatoIngrediente]
    FOREIGN KEY ([IdPlato])
    REFERENCES [dbo].[Platos]
        ([IdPlato])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlatoIngrediente'
CREATE INDEX [IX_FK_PlatoIngrediente]
ON [dbo].[PlatosIngredientes]
    ([IdPlato]);
GO

-- Creating foreign key on [IdPlato] in table 'PlatosCombos'
ALTER TABLE [dbo].[PlatosCombos]
ADD CONSTRAINT [FK_PlatoCombo]
    FOREIGN KEY ([IdPlato])
    REFERENCES [dbo].[Platos]
        ([IdPlato])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlatoCombo'
CREATE INDEX [IX_FK_PlatoCombo]
ON [dbo].[PlatosCombos]
    ([IdPlato]);
GO

-- Creating foreign key on [IdFactura] in table 'FacturasPlatos'
ALTER TABLE [dbo].[FacturasPlatos]
ADD CONSTRAINT [FK_FacturaDetalles]
    FOREIGN KEY ([IdFactura])
    REFERENCES [dbo].[Facturas]
        ([IdFactura])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FacturaDetalles'
CREATE INDEX [IX_FK_FacturaDetalles]
ON [dbo].[FacturasPlatos]
    ([IdFactura]);
GO

-- Creating foreign key on [CedulaRif] in table 'ClientesMovimientos'
ALTER TABLE [dbo].[ClientesMovimientos]
ADD CONSTRAINT [FK_ClientesMovimientos_Clientes]
    FOREIGN KEY ([CedulaRif])
    REFERENCES [dbo].[Clientes]
        ([CedulaRif])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientesMovimientos_Clientes'
CREATE INDEX [IX_FK_ClientesMovimientos_Clientes]
ON [dbo].[ClientesMovimientos]
    ([CedulaRif]);
GO

-- Creating foreign key on [IdClienteMovimiento] in table 'ClientesMovimientosDetalles'
ALTER TABLE [dbo].[ClientesMovimientosDetalles]
ADD CONSTRAINT [FK_ClientesMovimientosDetalles_ClientesMovimientos]
    FOREIGN KEY ([IdClienteMovimiento])
    REFERENCES [dbo].[ClientesMovimientos]
        ([IdClienteMovimiento])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientesMovimientosDetalles_ClientesMovimientos'
CREATE INDEX [IX_FK_ClientesMovimientosDetalles_ClientesMovimientos]
ON [dbo].[ClientesMovimientosDetalles]
    ([IdClienteMovimiento]);
GO

-- Creating foreign key on [IdIngrediente] in table 'IngredientesInventarios'
ALTER TABLE [dbo].[IngredientesInventarios]
ADD CONSTRAINT [FK_Ingredienteinventarios]
    FOREIGN KEY ([IdIngrediente])
    REFERENCES [dbo].[Ingredientes]
        ([IdIngrediente])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ingredienteinventarios'
CREATE INDEX [IX_FK_Ingredienteinventarios]
ON [dbo].[IngredientesInventarios]
    ([IdIngrediente]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------