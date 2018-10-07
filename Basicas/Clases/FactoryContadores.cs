using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HK.Clases
{
    public class FactoryContadores
    {
        public static int GetContador(string Variable)
        {
            try
            {
                using (var oEntidades = new RestaurantEntities())
                {
                    Contadore Contador = oEntidades.Contadores.FirstOrDefault(x => x.Variable == Variable);
                    if (Contador == null)
                    {
                        Contador = new Contadore();
                        Contador.Variable = Variable;
                        Contador.Valor = 1;
                        oEntidades.Contadores.AddObject(Contador);
                    }
                    else
                    {
                        Contador.Valor++;
                        if (Contador.Valor > 99)
                        {
                            Contador.Valor = 1;
                        }
                    }
                    oEntidades.SaveChanges();
                    return Contador.Valor.GetValueOrDefault(0);
                }
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
            return 1;
        }
        public static string GetMax(string Variable)
        {
            try
            {
                using (var oEntidades = new RestaurantEntities())
                {
                    Contadore Contador = oEntidades.Contadores.FirstOrDefault(x => x.Variable == Variable);
                    if (Contador == null)
                    {
                        Contador = new Contadore();
                        Contador.Variable = Variable;
                        Contador.Valor = 1;
                        oEntidades.Contadores.AddObject(Contador);
                    }
                    else
                    {                        
                        Contador.Valor++;

                    }
                    oEntidades.SaveChanges();
                    return ((int)Contador.Valor).ToString("000000");
                }
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
            return "";
        }
        public static string GetMaxDiario(string Variable)
        {
            try
            {
                string Fecha = DateTime.Today.ToShortDateString();
                using (var oEntidades = new RestaurantEntities())
                {
                    Contadore Contador = oEntidades.Contadores.FirstOrDefault(x => x.Variable == Variable+Fecha);
                    if (Contador == null)
                    {
                        Contador = new Contadore();
                        Contador.Variable = Variable+Fecha;
                        Contador.Valor = 1;
                        oEntidades.Contadores.AddObject(Contador);
                    }
                    else
                    {
                        Contador.Valor++;

                    }
                    oEntidades.SaveChanges();
                    return ((int)Contador.Valor).ToString("000000");
                }
            }
            catch (Exception ex)
            {
                Basicas.ManejarError(ex);
            }
            return "";
        }
    }
}
