using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using HK.Formas;
using HK.Clases;
using System.Globalization;

namespace HK
{
    public partial class FrmEditarMesa : Form
    {
        int cantidad = 1;
        string grupoActivo = null;
        List<Button> grupos = new List<Button>();
        List<Button> platos = new List<Button>();
        List<Button> cantidades = new List<Button>();
        System.Linq.IQueryable<HK.Plato> mplatos = null;
        Factura factura = new Factura();
        Cliente cliente = new Cliente();
        private bool esNuevo = true;
        RestaurantEntities db = new RestaurantEntities();
        public Mesa mesa = null;
        public MesasAbierta mesaAbierta = null;
        private List<MesasAbiertasPlato> mesaAbiertaPlatos = new List<MesasAbiertasPlato>();
        private Usuario mesonero = null;
        int pagina = 0;
        public FrmEditarMesa()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load);            
        }
        void Form1_Load(object sender, EventArgs e)
        {
            #region IniciarPantalla
            esNuevo = true;
            this.txtEmpresa.Text = Basicas.parametros().Empresa;
            this.txtUsuario.Text = FactoryUsuarios.UsuarioActivo.Nombre;
            cantidades.AddRange(new Button[] { cantidad0, cantidad1, cantidad2, cantidad3, cantidad4, cantidad5, cantidad6, cantidad7, cantidad8 });
            grupos.AddRange(new Button[] { grupo0, grupo1, grupo2, grupo3, grupo4, grupo5, grupo6, grupo7, grupo8, grupo9, grupo10,grupo11,grupo12,grupo13,grupo14,grupo15,grupo16,grupo17});
            platos.AddRange(new Button[] { plato1, plato2, plato3, plato4, plato5, plato6, plato7, plato8, plato9, plato10, plato11, plato12, plato13, plato14, plato15, plato16, plato17, plato18, plato19, plato20, plato21, plato22, plato23, plato24, plato25, plato26, plato27, plato28, plato29, plato30, plato31, plato32, plato33, plato34, plato35, plato36, plato37, plato38, plato39, plato40, plato41, plato42 });
            #endregion
            #region Eventos
            foreach (Button b in cantidades)
            {
                b.Click += new EventHandler(cantidad_Click);
            }
            foreach (Button b in grupos)
            {
                b.Visible = false;
                b.Click += new EventHandler(grupo_Click);
            }
            foreach (Button b in platos)
            {
                b.Visible = false;
                b.Click += new EventHandler(plato_Click);
            }
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(FrmCaja_KeyDown);
            this.gridControl1.KeyDown += new KeyEventHandler(gridControl1_KeyDown);
            this.btnImprimir.Visible = FactoryUsuarios.UsuarioActivo.PuedePedirCorteDeCuenta.GetValueOrDefault(false);
            this.btnImprimir.Click += new EventHandler(btnImprimir_Click);
            this.btnPagos.Visible = FactoryUsuarios.UsuarioActivo.PuedeRegistrarPago.GetValueOrDefault(false);
            this.btnPagos.Click += new EventHandler(Pagos_Click);
            this.btnGuardar.Click += new EventHandler(btnGuardar_Click); 
            this.btnCancelar.Click += new EventHandler(btnCancelar_Click);
            this.txtPlato.Validating += new CancelEventHandler(txtPlato_Validating);
            this.txtPlato.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPlato_ButtonClick);
            if (FactoryUsuarios.UsuarioActivo.PuedeCambiarMesa.GetValueOrDefault(false) == true)
            {
                this.MesaTextEdit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(MesaTextEdit_ButtonClick);
            }
            #endregion
            #region Enlazar
            if (mesaAbierta == null)
            {
                esNuevo = true;
                mesaAbierta = new MesasAbierta();
                mesaAbierta.IdMesa = mesa.IdMesa;
                mesaAbierta.Mesa = mesa.Descripcion;
                mesaAbierta.Apertura = DateTime.Now;
                mesaAbierta.Estatus = "ABIERTA";
                mesaAbierta.Personas = 1;
            }
            else
            {
                mesonero = FactoryUsuarios.Item(mesaAbierta.IdMesonero);
                mesaAbierta = FactoryMesas.MesaAbiertaItem(db, mesaAbierta);
                if (mesaAbierta.IdMesa != null)
                {
                    mesaAbiertaPlatos = (from x in db.MesasAbiertasPlatos
                                         where x.IdMesaAbierta == mesaAbierta.IdMesaAbierta
                                         select x).ToList();
                }
            }
            mesasAbiertaBindingSource.DataSource = mesaAbierta;
            mesasAbiertaBindingSource.ResetBindings(true);
            mesasAbiertasPlatoBindingSource.DataSource = mesaAbiertaPlatos;
            mesasAbiertasPlatoBindingSource.ResetBindings(true);
            CargarGrupos();
            #endregion

            mplatos = from x in db.Platos
                      orderby x.Descripcion
                      where x.Grupo == grupoActivo && x.MostrarMenu == true
                      select x;
            CargarPlatos();
            btnMas.Click += new EventHandler(btnMas_Click);
            this.btnSeparar.Visible = FactoryUsuarios.UsuarioActivo.PuedeSepararCuentas.GetValueOrDefault(false);
            this.btnSeparar.Click += new EventHandler(btnSeparar_Click);
            txtPlato.Focus();
        }

        void btnSeparar_Click(object sender, EventArgs e)
        {
            Guardar();
            FrmSepararMesas f = new FrmSepararMesas();
            f.mesaAbierta = this.mesaAbierta;
            f.ShowDialog();
            this.Close();
        }

        void btnMas_Click(object sender, EventArgs e)
        {
            CargarPlatos();
        }

        void MesaTextEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FrmBuscarEntidades f = new FrmBuscarEntidades();
            f.BuscarMesasDisponibles("");
            if (f.registro != null)
            {
                Mesa item = (Mesa)f.registro;
                mesaAbierta.IdMesa = item.IdMesa;
                mesaAbierta.Mesa = item.Descripcion;
            }
        }
        void btnImprimir_Click(object sender, EventArgs e)
        {
            if (Basicas.parametros().ImprimirOrden == "FISCAL")
            {
                ImprimirCorte();
            }
            else
            {
                ImprimirCorte();
            }
        }
        void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void btnGuardar_Click(object sender, EventArgs e)
        {            
            Guardar();
        }
        private void Guardar()
        {
            if (Basicas.parametros().SolicitarMesonero.GetValueOrDefault(false) == true)
            {
                FrmSolicitarMesonero f = new FrmSolicitarMesonero();
                f.ShowDialog();
                if (f.DialogResult != System.Windows.Forms.DialogResult.OK)
                    return;
                mesaAbierta.IdMesonero = f.mesonero.IdMesonero;
                mesaAbierta.Mesonero = f.mesonero.Nombre;
            }
            try
            {
                esNuevo = false;
                this.Validar();
                if (mesaAbierta.Numero == null)
                {
                    mesaAbierta.Numero = FactoryContadores.GetMaxDiario("CuentaAbierta");
                }
                Basicas.ImprimirComanda(mesaAbierta,mesaAbiertaPlatos);
                if (mesaAbierta.IdMesaAbierta == null)
                {
                    esNuevo = true;                    
                    mesaAbierta.IdMesaAbierta = FactoryContadores.GetMax("IdMesaAbierta");
                }
                foreach (MesasAbiertasPlato p in mesaAbiertaPlatos)
                {
                    if (p.IdMesaAbiertaPlato == null)
                    {
                        p.IdMesaAbiertaPlato = FactoryContadores.GetMax("IdMesaAbiertaPlato");
                        p.IdMesaAbierta = mesaAbierta.IdMesaAbierta;
                        db.MesasAbiertasPlatos.AddObject(p);
                    }
                }
                if (esNuevo)
                {
                    db.MesasAbiertas.AddObject(mesaAbierta);
                }
                db.SaveChanges();
                this.Close();
            }
            catch (Exception x)
            {
                Basicas.ManejarError(x);
            }
        }
        void txtPlato_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DevExpress.XtraEditors.TextEdit Editor = (DevExpress.XtraEditors.TextEdit)sender;
            string Texto = Editor.Text;
            Editor.Text = "";
            Plato plato = new Plato();
            FrmBuscarEntidades F = new FrmBuscarEntidades();
            F.BuscarPlatos(Texto);
            if (F.registro != null)
            {
                plato = (Plato)F.registro;
            }
            else
            {
                return;
            }
        }
        void txtPlato_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.TextEdit Editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (!Editor.IsModified)
                return;
            Plato plato = new Plato();
            string Texto = Editor.Text;
            Editor.Text = "";
            List<Plato> T = FactoryPlatos.getItems(Texto);
            switch (T.Count)
            {
                case 0:
                    return;
                case 1:
                    plato = T[0];
                    break;
                default:
                    FrmBuscarEntidades F = new FrmBuscarEntidades();
                    F.BuscarPlatos(Texto);
                    if (F.registro != null)
                    {
                        plato = (Plato)F.registro;
                    }
                    else
                    {
                        return;
                    }
                    break;
            }
            AgregarItem(plato);            
        }
        void FrmCaja_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    MesasAbiertasPlato ultimo = mesaAbiertaPlatos.LastOrDefault();
                    if (ultimo != null)
                    {
                        if (ultimo.IdMesaAbiertaPlato == null)
                        {
                            mesaAbiertaPlatos.Remove(ultimo);
                        }
                    }
                    e.Handled = true;
                    break;
                case Keys.F11:
                    ImprimirCorte();
                    break;
                case Keys.F10:
                    if (Basicas.parametros().ImprimirOrden == "FISCAL")
                    {
                        ImprimirCorte();
                    }
                    else
                    {
                        ImprimirCorte();
                    }
                    e.Handled = true;
                    break;
                case Keys.F12:
                    this.btnGuardar.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.F4:
                    this.btnPagos.PerformClick();
                    e.Handled = true;
                    break;
            }
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Subtract)
            {
                EliminarItem((MesasAbiertasPlato)this.mesasAbiertasPlatoBindingSource.Current);
                e.Handled = true;
            }
        }
        private void ImprimirCorte()
        {
            try
            {
                this.Guardar();
                Basicas.ImprimirCorteCuenta(mesaAbierta);
                mesaAbierta.Estatus = "IMPRESA";
                db.SaveChanges();
            }
            catch (Exception x)
            {
                Basicas.ManejarError(x);
            }
        }

        void Pagos_Click(object sender, EventArgs e)
        {
            this.mesasAbiertasPlatoBindingSource.EndEdit();
            try
            {
                Validar();
                CargarFactura();
                FrmPagar pago = new FrmPagar();
                pago.factura = factura;
                pago.descuento = mesa.Descuento.GetValueOrDefault();
                pago.ShowDialog();
                if (pago.DialogResult != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                factura.calcularSaldo();
                // factura.Totalizar(mesa.CobraServicio.GetValueOrDefault(false),mesa.Descuento.GetValueOrDefault(0), 12);
                if (decimal.Round((decimal)factura.Saldo.GetValueOrDefault(0), 0) == 0)
                {
                    if (factura.ConsumoInterno.GetValueOrDefault(0) == 0)
                    {
                        factura.Tipo = "FACTURA";
                        ImprimirFactura();
                    }
                    else
                    {
                        factura.Tipo = "CONSUMO";
                        factura.Hora = DateTime.Now;
                        factura.Fecha = DateTime.Today;
                        factura.Numero = FactoryContadores.GetMax("Consumo");
                    }
                    GuardarFactura();
                    EliminarMesaAbierta(mesaAbierta);
                }
                else
                {
                    return;
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        private void GuardarFactura()
        {
            using (var db = new RestaurantEntities())
            {
                cliente = FactoryClientes.Item(db, factura.CedulaRif);
                if (cliente == null)
                {
                    cliente = new Cliente();
                    cliente.CedulaRif = factura.CedulaRif;
                    cliente.RazonSocial = factura.RazonSocial;
                    cliente.Direccion = factura.Direccion;
                    db.Clientes.AddObject(cliente);
                }
                else
                {
                    cliente.CedulaRif = factura.CedulaRif;
                    cliente.RazonSocial = factura.RazonSocial;
                    cliente.Direccion = factura.Direccion;
                }
                if (factura.IdFactura == null)
                {
                    esNuevo = true;
                    factura.IdFactura = FactoryContadores.GetMax("IdFactura");
                }
                foreach (FacturasPlato p in factura.FacturasPlatos)
                {
                    if (p.IdFacturaPlato == null)
                    {
                        p.IdFacturaPlato = FactoryContadores.GetMax("IdFacturaPlato");
                    }
                }
                if (esNuevo)
                {
                    factura.Anulado = false;
                    factura.Inventarios=false;
                    factura.LibroVentas = false;
                    factura.LibroInventarios = false;
                    db.Facturas.AddObject(factura);
                }
                try
                {
                    db.SaveChanges();
                    FactoryFacturas.DescontarInventario(factura);
                    if (factura.Credito.GetValueOrDefault(0) > 0)
                    {
                        FactoryCxC.CrearCxC(factura);
                    }

                }
                catch (Exception x)
                {
                    if (x.InnerException != null)
                    {
                        MessageBox.Show(x.InnerException.Message);
                    }
                    else
                    {
                        MessageBox.Show(x.Message);
                    }
                }
            }
        }
        private void EliminarMesaAbierta(MesasAbierta mesaAbierta)
        {
            if (mesaAbierta.IdMesaAbierta == null)
                return;
            using (var db = new RestaurantEntities())
            {
                try
                {
                    MesasAbierta m = (from x in db.MesasAbiertas
                                      where x.IdMesaAbierta == mesaAbierta.IdMesaAbierta
                                      select x).FirstOrDefault();
                    db.MesasAbiertas.DeleteObject(m);
                    db.SaveChanges();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
            }
        }
        private void Validar()
        {
            mesaAbierta.Totalizar(mesa.CobraServicio.GetValueOrDefault(false),mesaAbiertaPlatos,mesa.Descuento);
            // if (mesaAbiertaPlatos.Count == 0)
            //    throw new Exception("Cuenta sin platos");
        }
        private void ValidarFactura()
        {
            factura.Totalizar(mesa.CobraServicio.GetValueOrDefault(false),mesa.Descuento.GetValueOrDefault(0),12);
            if (factura.FacturasPlatos.Count == 0)
                throw new Exception("Cuenta sin platos");
        }
        private void GuadarFactura()
        {
            mesasAbiertaBindingSource.EndEdit();
            ValidarFactura();
            factura.Cajero = FactoryUsuarios.UsuarioActivo.Nombre;
            factura.IdCajero = FactoryUsuarios.UsuarioActivo.IdUsuario;
            if (esNuevo && factura.IdFactura==null)
            {
                factura.IdFactura = FactoryContadores.GetMax("IdFactura");
            }
            factura.Anulado = false;
            factura.Saldo = (double)decimal.Round((decimal)factura.Saldo.GetValueOrDefault(0), 0);
            foreach (HK.FacturasPlato p in factura.FacturasPlatos)
            {
                if (p.IdFacturaPlato == null)
                {
                    p.IdFacturaPlato = FactoryContadores.GetMax("IdFacturaPlato");
                }
            }
            cliente = FactoryClientes.Item(db, factura.CedulaRif);
            if (cliente== null)
            {
                cliente = new Cliente();
                cliente.CedulaRif = factura.CedulaRif;
                cliente.RazonSocial = factura.RazonSocial;
                cliente.Direccion = factura.Direccion;
                db.Clientes.AddObject(cliente);
            }
            else
            {
                cliente.CedulaRif = factura.CedulaRif;
                cliente.RazonSocial = factura.RazonSocial;
                cliente.Direccion = factura.Direccion;
            }
            if (esNuevo )
            {
                db.Facturas.AddObject(factura);
            }
            factura.Fecha = DateTime.Today;
            factura.Hora = DateTime.Now;
            factura.Totalizar(mesa.CobraServicio.GetValueOrDefault(false), mesa.Descuento.GetValueOrDefault(0),12);
            if (!Basicas.ImpresoraActiva)
            {
                factura.Tipo = "POR IMPRIMIR";
            }
            db.SaveChanges();
            factura = new Factura();
        }
        private void CargarFactura()
        {
            factura = new Factura();
            factura.Cajero = FactoryUsuarios.UsuarioActivo.Nombre;
            factura.IdCajero = FactoryUsuarios.UsuarioActivo.IdUsuario;
            cliente = new Cliente();
            cliente.CedulaRif = "V000000000";
            cliente.RazonSocial = "CONTADO";
            cliente.Direccion = Basicas.parametros().Ciudad;
            factura.CedulaRif = cliente.CedulaRif;
            factura.Direccion = cliente.Direccion;
            factura.RazonSocial = cliente.RazonSocial;
            factura.Tipo = "FACTURA";
            factura.Mesonero = mesaAbierta.Mesonero;
            factura.NumeroOrden = mesaAbierta.Numero;
            foreach (MesasAbiertasPlato item in mesaAbiertaPlatos)
            {
                FacturasPlato nuevo = new FacturasPlato();
                nuevo.Cantidad = item.Cantidad;
                nuevo.Codigo = item.Codigo;
                nuevo.Comentarios = item.Comentarios;
                nuevo.Contornos = item.Contornos;
                nuevo.Descripcion = item.Descripcion;
                nuevo.Grupo = item.Grupo;
                nuevo.Idplato = item.Idplato;
                nuevo.Precio = item.Precio;
                nuevo.PrecioConIva = item.PrecioConIva;
                nuevo.TasaIva = item.TasaIva;
                nuevo.Total = item.Total;
                nuevo.Costo = item.Costo;
                factura.FacturasPlatos.Add(nuevo);
            }
            factura.Totalizar(mesa.CobraServicio.GetValueOrDefault(false),mesa.Descuento.GetValueOrDefault(0),12);
        }
        private void Imprimir()
        {
            try
            {
                this.Guardar();
                Basicas.ImprimirCorteCuenta(mesaAbierta);
                mesaAbierta.Estatus = "IMPRESA";
                db.SaveChanges();
            }
            catch (Exception x)
            {
                Basicas.ManejarError(x);
            }
        }
        private void ImprimirFactura()
        {
            Basicas.ImprimirFactura(factura);
            if (factura.Cambio.GetValueOrDefault(0) > 0)
            {
                MessageBox.Show(string.Format("Cambio {0}", factura.Cambio.Value.ToString("n2")), "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void EliminarItem(MesasAbiertasPlato ultimo)
        {
            if (ultimo == null)
                return;
            if (ultimo.IdMesaAbiertaPlato == null)
            {
                mesaAbiertaPlatos.Remove(ultimo);
                this.mesasAbiertasPlatoBindingSource.DataSource = mesaAbiertaPlatos;
                this.mesasAbiertasPlatoBindingSource.ResetBindings(true);
                return;
            }
            // FrmAnulacionDePlato f = new FrmAnulacionDePlato();
            MesasAbiertasPlatosAnulado item = new MesasAbiertasPlatosAnulado();
            item.Cajero = FactoryUsuarios.UsuarioActivo.Nombre;
            item.IdCajero = FactoryUsuarios.UsuarioActivo.IdUsuario;
            item.Cantidad = ultimo.Cantidad;
            item.Fecha = DateTime.Today;
            item.IdPlato = ultimo.Idplato;
            item.Numero = mesaAbierta.Numero;
            item.Plato = ultimo.Descripcion;
            item.Precio = ultimo.Precio;
            item.PrecioIva = ultimo.PrecioConIva;
            item.Total = ultimo.Total;
            // f.platoAnulado = item;
            // f.ShowDialog();
            // if(f.DialogResult!= System.Windows.Forms.DialogResult.OK)
            // {
            //     return;
            // }
            using (var db =  new RestaurantEntities())
            {
                item.Mesa = mesaAbierta.Mesa;
                item.Mesonero = mesaAbierta.Mesonero;

            //    Basicas.ImprimirAnulacion(item);
                item.IdPlatoEliminado = FactoryContadores.GetMax("IdPlatoEliminado");
                db.MesasAbiertasPlatosAnulados.AddObject(item);
                db.SaveChanges();
                MesasAbiertasPlato ItemaElminar = (from x in db.MesasAbiertasPlatos
                                                    where x.IdMesaAbiertaPlato == ultimo.IdMesaAbiertaPlato
                                                    select x).FirstOrDefault();
                if(ItemaElminar!=null)
                    db.MesasAbiertasPlatos.DeleteObject(ItemaElminar);
                db.SaveChanges();
            }
            mesaAbiertaPlatos.Remove(ultimo);
            mesaAbierta.Totalizar(mesa.CobraServicio.GetValueOrDefault(false), mesaAbiertaPlatos, mesa.Descuento);
            Guardar();
        }
        void gridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Subtract)
            {
                EliminarItem((MesasAbiertasPlato)this.mesasAbiertasPlatoBindingSource.Current);
                e.Handled = true;
            }
        }
        private void OcultarPlatos()
        {
            foreach (Button b in platos)
            {
                b.Visible = false;
            }
        }
        void cantidad_Click(object sender, EventArgs e)
        {
            Button item = (Button)sender;
            foreach (Button c in cantidades)
            {
               item.Font = new System.Drawing.Font("Verdana", 9,c != item? FontStyle.Regular:FontStyle.Bold);
            }
            cantidad = Convert.ToInt16(item.Text);
        }
        void grupo_Click(object sender, EventArgs e)
        {
            Button item = (Button)sender;
            grupoActivo = item.Text;
            pagina = 0;
            CargarPlatos();
        }
        void plato_Click(object sender, EventArgs e)
        {
            Button item = (Button)sender;
            AgregarItem((Plato)item.Tag);
        }
        void AgregarItem(Plato plato)
        {
            MesasAbiertasPlato item = new MesasAbiertasPlato();
            item.Descripcion = plato.Descripcion;
            item.Precio = plato.Precio;
            item.TasaIva = plato.TasaIva;
            item.PrecioConIva = item.Precio+(item.Precio * item.TasaIva / 100);
            item.EnviarComanda = plato.EnviarComanda;
            if (FactoryPlatos.getArrayComentarios(plato).Count() > 0 || FactoryPlatos.getArrayContornos(plato).Count() > 0)
            {
                FrmPedirContornos f = new FrmPedirContornos();
                f.codigoPlato = plato.Codigo;
                f.ShowDialog();
                if (f.presentacion != null)
                {
                    item.Descripcion = plato.Descripcion + "-" + f.presentacion;
                    item.Precio = f.precio;
                    item.PrecioConIva = item.Precio + (item.Precio * plato.TasaIva / 100);
                }
                item.Comentarios = f.Comentarios;
                item.Contornos = f.Contornos;
            }
            item.Cantidad = cantidad;
            item.Codigo = plato.Codigo;
            item.Grupo = plato.Grupo;
            item.Idplato = plato.IdPlato;
            item.TasaIva = plato.TasaIva;
            item.Total = item.PrecioConIva.GetValueOrDefault(0) * cantidad;
            item.Costo = plato.Costo.GetValueOrDefault(0) * cantidad;
            mesaAbiertaPlatos.Add(item);
            this.mesasAbiertasPlatoBindingSource.DataSource = mesaAbiertaPlatos;
            this.mesasAbiertasPlatoBindingSource.ResetBindings(true);
            mesaAbierta.Totalizar(mesa.CobraServicio.GetValueOrDefault(false),mesaAbiertaPlatos,mesa.Descuento);
            var montoBsS = "BsS." + String.Format(CultureInfo.CreateSpecificCulture("es-VE"), "{0:0,0.00}", this.mesaAbierta.MontoTotal / 1000);
            this.toolStripTextBox1.Text = montoBsS;
        }
        Image LeerImagen(string codigo)
        {
            string archivo = Application.StartupPath + "\\Imagenes\\" + codigo + ".jpg";
            System.Drawing.Bitmap imagen = new System.Drawing.Bitmap((Image)Image.FromFile(
                                                Application.StartupPath + "\\Imagenes\\" + codigo + ".jpg"));
            return imagen.GetThumbnailImage(40, 40, null, IntPtr.Zero);
        }
        void CargarPlatos()
        {
            int i=0;
            OcultarPlatos();
            foreach (Plato s in mplatos.Skip(pagina*42).Take(42))
            {
                //try
                //{
                //    platos[i].Image = LeerImagen(s.Codigo);
                //}
                //catch { platos[i].Image = null; }
                platos[i].Visible = true;
                platos[i].Text = s.Descripcion;
                platos[i].Tag = s;
                i++;
            }
            pagina++;
        }
        void CargarGrupos()
        {
            List<string> mgrupos = (from x in db.Platos
                        orderby x.Grupo
                        select x.Grupo).Distinct().Take(18).ToList();
            int i = 0;
            foreach (string s in mgrupos)
            {
                //Plato p = (from y in db.Platos
                //           where y.Grupo == s
                //           orderby y.Descripcion
                //           select y).FirstOrDefault();
                //try
                //{
                //    grupos[i].Image = LeerImagen(p.Codigo);
                //}
                //catch { }
                //finally { }
                if (i == 0)
                {
                    grupoActivo = s;
                }
                    grupos[i].Visible = true;
                    grupos[i].Text = s;
                i++;
            }
        }

        private void btnImprimir_Click_1(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {

        }
    }
}
