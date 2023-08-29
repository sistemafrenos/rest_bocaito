using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HK;
using TfhkaNet.IF.VE;
using TfhkaNet.IF;

namespace HK
{
    public class Pagos 
    {
        static public string Efectivo="201";
        static public string Tarjeta="213";
        static public string PagoMobil = "207"; // Internamente se almacena como cestaticket
        static public string Divisa="220";
        static public string Resto="102";
    }
    
    public class FiscalBixolon2020
    {
        Tfhka fiscal;
        string Puerto="";
        double TasaIva=0;
        double TasaIvaA=16;
        double TasaIvaB=12;
        double TasaIvaC=10;
        string TipoDeIva="Incluido";
        public FiscalBixolon2020(string _puerto)
        {
            Puerto = _puerto;
            fiscal = new Tfhka();
            DetectarImpresora();
        }
        ~FiscalBixolon2020()
        {
            fiscal.CloseFpCtrl();
            fiscal = null;
        }
        private double strToDouble(string p)
        {
            double Base = Convert.ToDouble(p.Substring(0, 10));
            double Decimales = Convert.ToDouble(p.Substring(10, 2));
            return Base + (Decimales / 100);
        }
        private void DetectarImpresora()
        {
            try
            {
                bool test1 = fiscal.CheckFPrinter();
                if (fiscal.OpenFpCtrl(Puerto))
                {
                    if (!fiscal.ReadFpStatus())
                    {
                        throw (new Exception(string.Format("Error de conexión, Estatus {0} verifique el puerto por favor...", fiscal.Status_Error)));
                    }
                }
                else
                {
                    var texto = fiscal.ComPort;
                    bool test = fiscal.CheckFPrinter();
                    var x = fiscal.Estado;
                    throw (new Exception(string.Format("Error al abrir el puerto {0}", Puerto)));
                }
            }
            catch (TfhkaNet.IF.PrinterException x)
            {
                throw new Exception(string.Format("Error de impresora: {0}, estatus {1}", x.Message, fiscal.Estado));

            }
        }
        private S2PrinterData CargarS2()
        {
            try
            {
                return fiscal.GetS2PrinterData();
            }
            catch (TfhkaNet.IF.PrinterException x)
            {
                throw new Exception(string.Format("Error de impresora: {0}, estatus {1}", x.Message, fiscal.Estado));

            }
            catch (Exception x)
            {
                throw new Exception(string.Format("Error de conexión\n{0}\nEstatus {1}", x.Message, fiscal.Status_Error));
            }
        }
        private S1PrinterData CargarS1()
        {
            try
            {
                return fiscal.GetS1PrinterData();
            }
            catch (TfhkaNet.IF.PrinterException x)
            {
                throw new Exception(string.Format("Error de impresora: {0}, estatus {1}", x.Message, fiscal.Estado));

            }
            catch (Exception x)
            {
                throw new Exception(string.Format("Error de conexión\n{0}\nEstatus {1}", x.Message, fiscal.Status_Error));
            }
        }
        public void ImprimeFactura(Factura documento)
        {
            #region Validaciones
            double SubTotal = 0;
            double MontoIva = 0;
            if (documento == null)
            {
                throw new Exception("Documento en blanco no se puede imprimir");
            }
            if (documento.MontoTotal <= 0)
            {
                throw new Exception("Esta factura no tiene productos");
            }

            #endregion
            #region Encabezado
            fiscal.SendCmd("i01Cedula/Rif:" + documento.CedulaRif);
                fiscal.SendCmd("i02Razon Social:");
                if (documento.RazonSocial.Length <= 37)
                {
                    fiscal.SendCmd("i03" + documento.RazonSocial);
                }
                else
                {
                    fiscal.SendCmd("i03" + documento.RazonSocial.Substring(0, 36));
                    fiscal.SendCmd("i04" + documento.RazonSocial.Substring(36, (documento.RazonSocial.Length - 36)));
                }
                if (documento.CedulaRif == "V000000000")
                {
                    fiscal.SendCmd("i04 SIN DERECHO A CREDITO FISCAL");
                }
                if (documento.Direccion != null)
                {
                    if (documento.Direccion.Length > 40)
                    {
                        fiscal.SendCmd("i05" + documento.Direccion);
                        fiscal.SendCmd("i06" + documento.Direccion.Substring(40, documento.Direccion.Length - 40));
                    }
                    else
                    {
                        fiscal.SendCmd("i06" + documento.Direccion);
                    }

                }
            System.Threading.Thread.Sleep(500);
#endregion
            #region Productos
                var Acumulado = from p in documento.FacturasPlatos.Where(x => x.Cantidad.GetValueOrDefault(0) >= 0.5)
                        group p by new { p.Descripcion, p.TasaIva, p.Precio, p.PrecioConIva }
                        into itemResumido
                        select new
                        {
                            Descripcion = itemResumido.Key.Descripcion,
                            TasaIva = itemResumido.Key.TasaIva,
                            Cantidad = itemResumido.Sum(x => x.Cantidad),
                            Precio = itemResumido.Key.Precio,
                            PrecioConIva = itemResumido.Key.PrecioConIva
                        };
                foreach (var d in Acumulado.Where(x => x.Cantidad.GetValueOrDefault() > 0 && x.Precio.GetValueOrDefault() > 0))
                {
                    var sCmd = "!";
                    if (d.TasaIva == 0)
                    {
                        sCmd = " ";
                    }
                    else if (d.TasaIva == TasaIvaB)
                    {
                        sCmd = '"'.ToString();
                    }
                    else if (d.TasaIva == TasaIvaC)
                    {
                        sCmd = '#'.ToString();
                    }
                    SubTotal = ((double)d.Cantidad.GetValueOrDefault(0) * (double)d.PrecioConIva.GetValueOrDefault(0));
                    MontoIva += ((double)d.Cantidad.GetValueOrDefault(0) * ((double)d.PrecioConIva.GetValueOrDefault(0)) - (double)d.Precio.GetValueOrDefault(0));
                    string Precio = "0000000000";
                    if (TipoDeIva == "INCLUIDO")
                    {
                        Precio = (d.PrecioConIva.GetValueOrDefault(0) * 100).ToString("0000000000");
                    }
                    else
                    {
                        Precio = (d.Precio.GetValueOrDefault(0) * 100).ToString("0000000000");
                    }
                    string Cantidad = (d.Cantidad.GetValueOrDefault(0) * 1000).ToString("00000000");
                    string Descripcion = d.Descripcion.PadRight(37);
                    if (d.Descripcion.Length <= 37)
                    {
                        var bRet = fiscal.SendCmd(sCmd + Precio + Cantidad + d.Descripcion);
                        if (!bRet)
                        {
                            TfhkaNet.IF.PrinterStatus e = fiscal.GetPrinterStatus();
                            throw new Exception(string.Format("Estatus:{0},Error:{1}", e.PrinterStatusDescription, e.PrinterErrorDescription));
                        }
                    }
                    else
                    {
                        var bRet = fiscal.SendCmd(sCmd + Precio + Cantidad + Descripcion.Substring(0, 36));
                        string Descripcion2 = d.Descripcion.Substring(36, (d.Descripcion.Length - 36));
                        bRet = fiscal.SendCmd("@" + Descripcion2);
                        if (!bRet)
                        {
                            TfhkaNet.IF.PrinterStatus e = fiscal.GetPrinterStatus();
                            throw new Exception(string.Format("Estatus:{0},Error:{1}", e.PrinterStatusDescription, e.PrinterErrorDescription));
                        }
                    }
                }
            System.Threading.Thread.Sleep(500);
#endregion
            fiscal.SendCmd("3"); // subtotal
            #region Pagos     
                double TotalPagos = 0;
                if (documento.Efectivo.GetValueOrDefault(0) != 0)
                {
                    double x = documento.Efectivo.GetValueOrDefault(0) + documento.Cambio.GetValueOrDefault(0);
                    fiscal.SendCmd(Pagos.Efectivo + (x * 100).ToString("000000000000"));
                    TotalPagos += documento.Efectivo.Value;
                }
                if (documento.Tarjeta.GetValueOrDefault(0) != 0)
                {
                        fiscal.SendCmd(Pagos.Tarjeta + ((double)documento.Tarjeta * 100).ToString("000000000000"));
                        TotalPagos += documento.Tarjeta.Value;
                }
                if (documento.Dolares.GetValueOrDefault(0) != 0)
                {
                        fiscal.SendCmd(Pagos.Divisa + ((double)documento.Dolares * 100).ToString("000000000000"));
                        TotalPagos += documento.Dolares.Value;
                }
                if (documento.CestaTicket.GetValueOrDefault(0) != 0) 
                {
                    fiscal.SendCmd(Pagos.PagoMobil + ((double)documento.CestaTicket * 100).ToString("000000000000"));
                    TotalPagos += documento.CestaTicket.Value;
                }
             
                System.Threading.Thread.Sleep(500);
                S2PrinterData s2 = CargarS2();
                System.Threading.Thread.Sleep(500);
                if (s2.AmountPayable > 0)
                {
                    fiscal.SendCmd(Pagos.Resto + (s2.AmountPayable * 100).ToString("000000000000"));
                }
                // cerrar factura
                fiscal.SendCmd("199");
                System.Threading.Thread.Sleep(500);
#endregion
            #region CargarFactura
                        S1PrinterData s1 = CargarS1();
                        System.Threading.Thread.Sleep(500);
                        documento.Fecha = DateTime.Now;
                        documento.Hora = DateTime.Now;
                        documento.Numero = s1.LastInvoiceNumber.ToString("00000000");
                        documento.NumeroZ  = (s1.DailyClosureCounter+1).ToString("0000");
                        documento.MontoGravable = s2.SubTotalBases;
                        documento.MontoIva = s2.SubTotalTax;
                        documento.MontoTotal = s2.SubTotalBases + s2.SubTotalTax;
                        System.Threading.Thread.Sleep(500);
            #endregion
                        fiscal.CheckFPrinter();
                        fiscal.CloseFpCtrl();
            
        }
        public void ReporteX()
        {
            string sCmd;
            bool bRet;
            try
            {
                //************ Imprimir Reporte X *******************
                sCmd = "I0X";
                bRet = fiscal.SendCmd(sCmd);
            }
            catch (TfhkaNet.IF.PrinterException x)
            {
                throw new Exception(string.Format("Error de impresora: {0}, estatus {1}", x.Message, fiscal.Estado));

            }
            catch (Exception x)
            {
                throw new Exception(string.Format("Error de conexión\n{0}\nEstatus {1}", x.Message, fiscal.Status_Error));
            }
        }
        public void ReporteZ()
        {
            try
            {
                S1PrinterData d = fiscal.GetS1PrinterData();
                if (d.QuantityOfInvoicesToday < 1)
                {
                    throw new Exception("No hay facturas aún hoy");
                }
                //************ Imprimir Reporte Z *******************
                fiscal.SendCmd("I0Z");
            }
            catch (TfhkaNet.IF.PrinterException x)
            {
                throw new Exception(string.Format("Error de impresora: {0}, estatus {1}", x.Message, fiscal.Estado));

            }
            catch (Exception x)
            {
                throw new Exception(string.Format("Error de conexión\n{0}\nEstatus {1}", x.Message, fiscal.Status_Error));
            }
        }
        public void ImprimeCorteConMontos(MesasAbierta documento)
        {
            unsafe
            {
                try
                {
                    fiscal.SendCmd("800" + "                        CORTE DE CUENTA");
                    fiscal.SendCmd("800" + "MESA:" + documento.Mesa);
                    fiscal.SendCmd("800" + "MESONERO:" + documento.Mesonero);
                    fiscal.SendCmd("800" + "CUENTA:" + documento.Numero);
                    fiscal.SendCmd("800" + " ");
                    fiscal.SendCmd("800" + "CANT  DESCRIPCION                          MONTO");
                    fiscal.SendCmd("800" + "================================================");
                    using (var db = new RestaurantEntities())
                    {
                        var mesaPlatos = from p in db.MesasAbiertasPlatos
                                         where p.IdMesaAbierta == documento.IdMesaAbierta
                                         select p;
                        foreach (var item in mesaPlatos)
                        {
                            fiscal.SendCmd(string.Format("800" + "{0} {1} {2}", item.Cantidad.Value.ToString("000"), item.Descripcion.PadRight(30).Substring(0, 30), item.Total.GetValueOrDefault(0).ToString("n2").PadLeft(8)));
                        }
                    }
                    fiscal.SendCmd("800" + "================================================");
                    fiscal.SendCmd("800" + string.Format("MONTO SERVICIO:{0}".PadLeft(40), documento.MontoServicio.GetValueOrDefault(0).ToString("N2").PadLeft(8)));
                    fiscal.SendCmd("800" + string.Format("  MONTO EXENTO:{0}".PadLeft(40), documento.MontoExento.GetValueOrDefault(0).ToString("N2").PadLeft(8)));
                    fiscal.SendCmd("800" + string.Format("MONTO GRAVABLE:{0}".PadLeft(40), documento.MontoGravable.GetValueOrDefault(0).ToString("N2").PadLeft(8)));
                    fiscal.SendCmd("800" + string.Format("     MONTO IVA:{0}".PadLeft(40), documento.MontoIva.GetValueOrDefault(0).ToString("N2").PadLeft(8)));
                    fiscal.SendCmd("800" + string.Format("   MONTO TOTAL:{0}".PadLeft(40), documento.MontoTotal.GetValueOrDefault(0).ToString("N2").PadLeft(8)));
                    fiscal.SendCmd("800" + "================================================");
                    fiscal.SendCmd("810" + "  ");
                }
                catch (Exception x)
                {
                    fiscal.SendCmd("810  ");
                    throw x;
                }
            }

        }
      
    }
}
