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


namespace HK
{
    public partial class FrmCaja : Form
    {
        int cantidad = 1;
        List<Button> grupos = new List<Button>();
        List<Button> platos = new List<Button>();
        List<Button> cantidades = new List<Button>();
        List<Plato> mplatos = new List<Plato>();
        Mesa mesa = null;
        Factura factura = new Factura();
        Cliente cliente = new Cliente();
        private bool esNuevo = true;
        RestaurantEntities db = new RestaurantEntities();
        private bool ImpresoraEnUso = false;
        public FrmCaja()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load);            
        }
        void Form1_Load(object sender, EventArgs e)
        {
            if (!Basicas.ImpresoraActiva)
            {
                toolReporteX.Visible = false;
                toolReporteZ.Visible = false;
            }
            esNuevo = true;
            cantidades.AddRange(new Button[] { cantidad0, cantidad1, cantidad2, cantidad3, cantidad4, cantidad5, cantidad6, cantidad7, cantidad8 });
            grupos.AddRange(new Button[] { grupo0, grupo1, grupo2, grupo3, grupo4, grupo5, grupo6, grupo7, grupo8, grupo9 });
            platos.AddRange(new Button[] { plato0, plato1, plato2, plato3, plato4, plato5, plato6, plato7, plato8, plato9, plato10, plato11, plato12, plato13, plato14, plato15, plato16, plato17, plato18, plato19, plato20, plato21, plato22, plato23, plato24, plato25, plato26, plato27, plato28, plato29 });
            foreach (Button b in grupos)
            {
                b.Visible = false;
                b.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);
            }
            OcultarPlatos();
            foreach (Button b in grupos)
            {
                b.Click += new EventHandler(grupo_Click);                
            }
            foreach (Button b in platos)
            {
                b.Click += new EventHandler(plato_Click);
            }
            foreach (Button b in cantidades)
            {
                b.Click += new EventHandler(cantidad_Click);
            }
            CargarGrupos();
            factura = new Factura();
            cliente = new Cliente();
            cliente.CedulaRif = "V000000000";
            cliente.RazonSocial = "CONTADO";
            cliente.Direccion = Basicas.parametros().Ciudad;
            LeerCliente();
            factura.TasaIva = Basicas.parametros().TasaIva;
            factura.Anulado = false;
            facturaBindingSource.DataSource = factura;
            facturaBindingSource.ResetBindings(true);
            facturaProductoBindingSource.DataSource = factura.FacturasPlatos;
            facturaProductoBindingSource.ResetBindings(true);
            this.gridControl1.KeyDown += new KeyEventHandler(gridControl1_KeyDown);
            this.Pagos.Click += new EventHandler(Pagos_Click);
            this.toolSalir.Click += new EventHandler(Salir_Click);
            this.txtEmpresa.Text = Basicas.parametros().Empresa;
            this.txtUsuario.Text = FactoryUsuarios.UsuarioActivo.Nombre;
            this.toolCierreCaja.Click += new EventHandler(toolCierreCaja_Click);
            this.toolCortexHoras.Click += new EventHandler(toolCortexHoras_Click);
            this.toolReporteX.Click += new EventHandler(toolReporteX_Click);
            this.toolReporteZ.Click += new EventHandler(toolReporteZ_Click);
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(FrmCaja_KeyDown);
            this.txtCedulaRif.Validating += new CancelEventHandler(txtCedulaRif_Validating);
            this.txtCedulaRif.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtCedulaRif_ButtonClick);
            this.txtPlato.Validating += new CancelEventHandler(txtPlato_Validating);
            this.txtPlato.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPlato_ButtonClick);
            this.Guardar.Click += new EventHandler(Guardar_Click);
            this.Cargar.Click += new EventHandler(Cargar_Click);
            this.toolContarDinero.Click += new EventHandler(toolContarDinero_Click);
            this.timer1.Tick += new EventHandler(timer1_Tick);
            this.toolVale.Click += new EventHandler(toolVale_Click);
            if (!Basicas.ImpresoraActiva)
            {
                ImpresoraEnUso = true;
                this.toolReporteX.Visible = false;
                this.toolReporteZ.Visible = false;
                this.toolVale.Visible = false;
            }
            txtPlato.Focus();
        }

        void toolVale_Click(object sender, EventArgs e)
        {
            FrmVale f = new FrmVale();
            f.Incluir();
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            ImpresoraEnUso = true;
            this.timer1.Enabled = false;
            ImprimirFacturasPendientes();
            ImpresoraEnUso = false;
            this.timer1.Enabled = true;
        }

        private void ImprimirFacturasPendientes()
        {
            using (var data = new RestaurantEntities())
            {
                var q = from p in data.Facturas
                        where p.Tipo=="POR IMPRIMIR"
                        select p;
                foreach (Factura item in q)
                {
                    try
                    {
                        Imprimir(item);
                        item.Tipo = "FACTURA";
                        item.Hora = DateTime.Now;
                        item.Fecha = DateTime.Today;
                        data.SaveChanges();
                    }
                    catch (Exception x)
                    {
                        Basicas.ManejarError(x);
                    }
                }
            }
        }

        void toolContarDinero_Click(object sender, EventArgs e)
        {
            FrmContarDinero f = new FrmContarDinero();
            f.ShowDialog();
        }
        void Cargar_Click(object sender, EventArgs e)
        {
            FrmBuscarEntidades f = new FrmBuscarEntidades();
            f.BuscarFacturas("");
            if (f.DialogResult == System.Windows.Forms.DialogResult.Abort)
            {
                using (var db = new RestaurantEntities())
                {
                    var x = (from item in db.Facturas
                            where item.IdFactura == ((Factura)(f.registro)).IdFactura
                            select item).FirstOrDefault();
                    if (x != null)
                    {
                        db.Facturas.DeleteObject(x);
                    db.SaveChanges();
                        return;
                    }
                }
            }
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (f.registro != null)
                {
                    factura = FactoryFacturas.Item(db, ((Factura)f.registro).IdFactura);
                    facturaBindingSource.DataSource = factura;
                    facturaBindingSource.ResetBindings(true);
                    facturaProductoBindingSource.DataSource = factura.FacturasPlatos;
                    facturaProductoBindingSource.ResetBindings(true);
                    esNuevo = false;
                    factura.Tipo = "FACTURA";
                }
            }
        }

        void Guardar_Click(object sender, EventArgs e)
        {
            try
            {
                factura.Fecha = DateTime.Today;
                factura.Hora = DateTime.Now;
                Validar();
                factura.Tipo = "PENDIENTE";
                Guadar();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception x)
            {
                Basicas.ManejarError(x);
            }
        }

        void toolCierreCajero_Click(object sender, EventArgs e)
        {
            FrmReportes f = new FrmReportes();
            f.CierreDeCajaHoras();
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
        void txtCedulaRif_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FrmBuscarEntidades F = new FrmBuscarEntidades();
            F.BuscarClientes("");
            if (F.registro != null)
            {
                cliente = (Cliente)F.registro;
                cliente = FactoryClientes.Item(cliente.CedulaRif);
                factura.CedulaRif = cliente.CedulaRif;
                LeerCliente();
            }
            else
            {
                cliente = new Cliente();
                cliente.CedulaRif = "";
                LeerCliente();
            }
        }
        void txtCedulaRif_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.TextEdit Editor = (DevExpress.XtraEditors.TextEdit)sender;
            if (!Editor.IsModified)
                return;
            string Texto = Editor.Text;
            this.facturaBindingSource.EndEdit();
            List<Cliente> T = FactoryClientes.getItems(Texto);
            switch (T.Count)
            {
                case 0:
                    cliente = new Cliente();
                    cliente.CedulaRif = Basicas.CedulaRif(Texto);
                    break;
                case 1:
                    cliente = T[0];
                    break;
                default:
                    FrmBuscarEntidades F = new FrmBuscarEntidades();
                    F.BuscarClientes(Texto);
                    if (F.registro != null)
                    {
                        cliente = (Cliente)F.registro;
                        cliente = FactoryClientes.Item(cliente.CedulaRif);
                    }
                    else
                    {
                        cliente = null;
                    }
                    break;
            }
            LeerCliente();
        }
        private void LeerCliente()
        {
            factura.CedulaRif = cliente.CedulaRif;
            factura.RazonSocial = cliente.RazonSocial;
            factura.Direccion = cliente.Direccion;
            this.facturaBindingSource.ResetCurrentItem();
        }
        void toolCortexHoras_Click(object sender, EventArgs e)
        {
            FrmReportes f = new FrmReportes();
            f.CierreDeCajaHoras();
        }
        void FrmCaja_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    FacturasPlato ultimo = factura.FacturasPlatos.LastOrDefault();
                    if (ultimo != null)
                    {
                        factura.FacturasPlatos.Remove(ultimo);
                    }
                    e.Handled = true;
                    break;
                case Keys.F2:
                    this.Guardar.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.F3:
                    this.Cargar.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.F4:
                    this.Pagos.PerformClick();
                    e.Handled = true;
                    break;
            } 
        }
        void toolReporteZ_Click(object sender, EventArgs e)
        {
            this.ImpresoraEnUso = true;
            FiscalBixolon f = new FiscalBixolon();
            f.ReporteZ();
            f = null;
            this.ImpresoraEnUso = false;
        }
        void toolReporteX_Click(object sender, EventArgs e)
        {
            this.ImpresoraEnUso = true;
            FiscalBixolon f = new FiscalBixolon();
            f.ReporteX();
            f = null;
            this.ImpresoraEnUso = false;
        }
        void toolCierreCaja_Click(object sender, EventArgs e)
        {
            FrmReportes f = new FrmReportes();
            f.CierreDeCaja();
        }
        void CedulaRifTextEdit_Validating(object sender, CancelEventArgs e)
        {
        }
        private void CedulaRifButtonEdit_Click(object sender, EventArgs e)
         {
             FrmBuscarEntidades F = new FrmBuscarEntidades();
             F.BuscarClientes("");
             if (F.registro != null)
             {
                 cliente = (Cliente)F.registro;
                 cliente = FactoryClientes.Item(cliente.CedulaRif);
                 LeerCliente();
             }
             else
             {
                 cliente = new Cliente();                 
             }
         }
        void Salir_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        void Pagos_Click(object sender, EventArgs e)
        {
            this.facturaBindingSource.EndEdit();
            try
            {
                Validar();
                FrmPagar pago = new FrmPagar();
                pago.factura = factura;
                pago.ShowDialog();
                if (pago.DialogResult != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                factura.Totalizar(false, 0,12);
                if (factura.Cambio.GetValueOrDefault(0) > 0)
                {
                    Application.DoEvents();
                }                
                if (decimal.Round( (decimal)factura.Saldo.GetValueOrDefault(0),0)==0)
                {
                    if (factura.ConsumoInterno.GetValueOrDefault(0) == 0)
                    {
                        if (ImpresoraEnUso)
                        {
                            factura.Tipo = "POR IMPRIMIR";
                        }
                        else
                        {
                            factura.Tipo = "FACTURA";
                            ImpresoraEnUso = true;
                            Imprimir();
                            ImpresoraEnUso = false;
                        }
                    }
                    else
                    {
                        factura.Tipo = "CONSUMO";
                        factura.Hora = DateTime.Now;
                        factura.Fecha = DateTime.Today;                        
                        factura.Numero = FactoryContadores.GetMax("Consumo");
                    }
                    FrmPedirNumeroOrden pOrden = new FrmPedirNumeroOrden();
                    pOrden.cambio = factura.Cambio.GetValueOrDefault(0);
                    pOrden.ShowDialog();
                    if (pOrden.numeroOrden != null)
                    {
                        factura.NumeroOrden = pOrden.numeroOrden;
                    }
                    else
                    {
                        factura.NumeroOrden = FactoryContadores.GetMax("NumeroOrden");
                    }
                    if (Basicas.parametros().ImprimirOrden == "FISCAL")
                    {
                        if(!ImpresoraEnUso)
                        {
                            ImpresoraEnUso = true;
                            FiscalBixolon f = new FiscalBixolon();
                            f.ImprimeOrden(factura);
                            f = null;
                            ImpresoraEnUso = false;
                        }

                    }
                    else
                    {
                        Basicas.ImprimirOrden(factura);
                    }
                    Guadar();
                }
                else
                {
                    return;
                }
            }
            catch (Exception x)
            {
                Basicas.ManejarError(x);
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        private void Validar()
        {
            factura.Totalizar(false,0, 12);
            if (factura.FacturasPlatos.Count == 0)
                throw new Exception("Factura sin platos");
            if (string.IsNullOrEmpty(factura.CedulaRif))
            {
                throw new Exception("Debe registrar la cedula o el rif");
            }
            if (string.IsNullOrEmpty(factura.RazonSocial))
            {
                throw new Exception("Debe registrar la razon social");
            }

        }
        private void Guadar()
        {
            {
                facturaBindingSource.EndEdit();
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
                if (!Basicas.ImpresoraActiva)
                {
                    factura.Tipo = "POR IMPRIMIR";
                }
                db.SaveChanges();
                factura = new Factura();
            }
        }
        private void Imprimir()
        {
            factura.Hora = DateTime.Now;
            factura.Fecha = DateTime.Today;
            FiscalBixolon f = new FiscalBixolon();
            try
            {
                f.ImprimeFactura(factura);
              //  FactoryFacturas.FinalizarFactura(factura);
            }
            catch (Exception x)
            {
                Basicas.ManejarError(x);
            }
        }
        private void Imprimir(Factura Item)
        {
            FiscalBixolon f = new FiscalBixolon();
            try
            {
                f.ImprimeFactura(factura);
            }
            catch (Exception x)
            {
                Basicas.ManejarError(x);
            }
        }
        void gridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Subtract)
            {
                if (this.gridView1.IsFocusedView)
                {
                    try
                    {
                        factura.FacturasPlatos.Remove((FacturasPlato)facturaProductoBindingSource.Current);
                    }
                    catch { }
                }
                e.Handled = true;
            }
        }
        private void OcultarPlatos()
        {
            foreach (Button b in platos)
            {
                b.Visible = false;
                b.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);
            }
        }
        void cantidad_Click(object sender, EventArgs e)
        {
            Button item = (Button)sender;
            cantidad = Convert.ToInt16(item.Text);
        }
        void grupo_Click(object sender, EventArgs e)
        {
            Button item = (Button)sender;
            CargarPlatos(item.Text);
        }
        void plato_Click(object sender, EventArgs e)
        {
            Button item = (Button)sender;
            AgregarItem((Plato)item.Tag);
        }
        void AgregarItem(Plato plato)
        {
            FacturasPlato item = new FacturasPlato();
            item.Descripcion = plato.Descripcion;
            item.Precio = plato.Precio;
            item.PrecioConIva = plato.PrecioConIva;
            if ( FactoryPlatos.getArrayComentarios(plato).Count() > 0 || FactoryPlatos.getArrayContornos(plato).Count() > 0)
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
            item.Costo = item.Costo.GetValueOrDefault(0) * cantidad;
            factura.FacturasPlatos.Add(item);
        }
        Image LeerImagen(string codigo)
        {
                string archivo = Application.StartupPath + "\\Imagenes\\" + codigo + ".jpg";
                System.Drawing.Bitmap imagen = new System.Drawing.Bitmap((Image)Image.FromFile(
                                                    Application.StartupPath + "\\Imagenes\\" + codigo + ".jpg"));
                return imagen.GetThumbnailImage(40, 40, null, IntPtr.Zero);
        }
        void CargarPlatos(string grupo)
        {

            using (RestaurantEntities db = new RestaurantEntities())
            {
                mplatos = (from x in db.Platos
                            orderby x.Descripcion
                            where x.Grupo == grupo
                            select x).Take(30).ToList();
                int i = 0;
                OcultarPlatos();
                foreach (Plato s in mplatos)
                {
                    try
                    {
                        platos[i].Image = LeerImagen(s.Codigo);
                    }
                    catch { platos[i].Image = null; } 
                    platos[i].Visible = true;
                    platos[i].Text = s.Descripcion;
                    platos[i].Tag = s;
                    i++;
                }
            }
        }
        void CargarGrupos()
        {
            using(RestaurantEntities db = new RestaurantEntities())
            {
                List<string> mgrupos = FactoryPlatos.getListGrupos();
                int i = 0;
                foreach( string s in mgrupos)
                {
                    Plato p = (from y in db.Platos
                                                where y.Grupo == s
                                                orderby y.Descripcion
                                                select y).FirstOrDefault();
                    try
                    {
                        grupos[i].Image = LeerImagen(p.Codigo);
                    }
                    catch {  }
                    finally { }
                    grupos[i].Visible = true;
                    grupos[i].Text = s;
                    i++;                    
                }
            }
        }
    }
}
