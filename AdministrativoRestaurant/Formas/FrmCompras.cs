using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HK;
using HK.Clases;

namespace HK.Formas
{
    public partial class FrmCompras : Form
    {
        RestaurantEntities db = new RestaurantEntities();
        List<Compra> Lista = new List<Compra>();
        public FrmCompras()
        {
            InitializeComponent();
            this.Load += new EventHandler(FrmCompras_Load);
            this.FormClosed += new FormClosedEventHandler(FrmCompras_FormClosed);
        }
        void FrmCompras_FormClosed(object sender, FormClosedEventArgs e)
        {
            Pantallas.ComprasLista = null;
        }
        void FrmCompras_Load(object sender, EventArgs e)
        {
            Busqueda();
            Buscar.Click += new EventHandler(Buscar_Click);
            gridControl1.KeyDown += new KeyEventHandler(gridControl1_KeyDown);
            btnNuevo.Click += new EventHandler(btnNuevo_Click);
            btnVer.Click += new EventHandler(btnVer_Click);
            btnEliminar.Click += new EventHandler(btnEliminar_Click);
            txtBuscar.KeyDown += new KeyEventHandler(txtBuscar_KeyDown);
            gridView1.OptionsLayout.Columns.Reset();
        }
        void btnEliminar_Click(object sender, EventArgs e)
        {
            EliminarRegistro();
        }
        void btnVer_Click(object sender, EventArgs e)
        {
            if (this.bs.Current == null)
                return;
            FrmComprasItem f = new FrmComprasItem();
            f.registro = (Compra)this.bs.Current;
            f.Ver();
        }
        void btnNuevo_Click(object sender, EventArgs e)
        {
            FrmComprasItem f = new FrmComprasItem();
            f.Incluir();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                Busqueda();
            }
        }              
        private void Busqueda()
        {
            db = new RestaurantEntities();
            switch (txtFiltro.Text)
            {
                case "TODAS":
                    Lista = (from p in db.Compras
                             where ( p.RazonSocial.Contains(txtBuscar.Text) || txtBuscar.Text.Length == 0 ) && p.Estatus=="CERRADA"
                            orderby p.Fecha
                            select p).ToList();
                    break;
                case "AYER":
                    DateTime ayer = DateTime.Today.AddDays(-1);
                    Lista = (from p in db.Compras
                             where (p.RazonSocial.Contains(txtBuscar.Text) || txtBuscar.Text.Length == 0) && p.Fecha.Value == ayer && p.Estatus == "CERRADA"
                             orderby p.Numero
                             select p).ToList();
                    break;
                case "HOY":
                    Lista = (from p in db.Compras
                             where (p.RazonSocial.Contains(txtBuscar.Text) || txtBuscar.Text.Length == 0) && p.Fecha.Value == DateTime.Today && p.Estatus == "CERRADA"
                             orderby p.Numero
                             select p).ToList();
                    break;
                case "ESTE MES":
                    Lista = (from p in db.Compras
                             where (p.RazonSocial.Contains(txtBuscar.Text) || txtBuscar.Text.Length == 0) && p.Fecha.Value.Month == DateTime.Today.Month && p.Fecha.Value.Year == DateTime.Today.Year && p.Estatus == "CERRADA"
                             orderby p.Numero
                             select p).ToList();
                    break;
            }
            this.bs.DataSource = Lista;
            this.bs.ResetBindings(true);
        }
        private void EliminarRegistro()
        {
            if (this.bs.Current == null)
                return;
            Compra documento = (Compra)this.bs.Current;
            if (MessageBox.Show("Esta seguro de eliminar esta compra", "Atencion", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                return;
            try
            {
                FactoryLibroCompras.BorrarItem(documento);
                FactoryLibroInventarios.RevertirCompra(documento);
                FactoryCompras.InventarioDevolver(documento);
                db.Compras.DeleteObject(documento);
                db.SaveChanges();
                Busqueda();
                }
                catch (Exception x)
                {
                    Basicas.ManejarError(x);
                }
        }
        #region Eventos
        private void gridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (gridView1.ActiveEditor == null)
            {
                switch (e.KeyCode)
                {
                    case Keys.Return:
                        btnVer.PerformClick();
                        break;
                    case Keys.Delete:
                        btnEliminar.PerformClick();
                        break;
                    case Keys.Subtract:
                        btnEliminar.PerformClick();
                        break;
                }
            }
        }
        private void Buscar_Click(object sender, EventArgs e)
        {
            Busqueda();
        }
        private void Eliminar_Click(object sender, EventArgs e)
        {
            EliminarRegistro();
        }
        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                Busqueda();
            }
        }
        #endregion
    }
}
