using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HK;

namespace HK.Clases
{
    public class FactoryCompras
    {
        public static Compra Item(string id)
        {
            using (RestaurantEntities db= new RestaurantEntities())
            {
                var item = (from x in db.Compras
                            where (x.IdCompra == id)
                            select x).FirstOrDefault();
                return item;
            }
        }
        public static Compra Item(RestaurantEntities db, string id)
        {
            var item = (from x in db.Compras
                        where (x.IdCompra == id)
                        select x).FirstOrDefault();
            return item;
        }
        public static List<Compra> getComprasEspera(RestaurantEntities db, string texto)
        {
            var mFacturas = (from x in db.Compras
                             orderby x.IdCompra
                             where (x.Estatus == "ESPERA")
                             select x).ToList();
            return mFacturas;
        }
        public static void Inventario(Compra factura)
        {
            foreach (ComprasIngrediente item in factura.ComprasIngredientes)
            {
                FactoryIngredientes.RegistrarInventario(item);
            }
        }            
        public static void InventarioDevolver(Compra factura)
        {
            foreach (ComprasIngrediente item in factura.ComprasIngredientes)
            {
                FactoryIngredientes.DevolverInventario(item);
            }
        }
        public static void PasarComprasLibro()
        {
            using (RestaurantEntities db = new RestaurantEntities())
            {
                var x = (from p in db.Compras
                        where p.IncluirLibroCompras==true && p.LibroCompras==false
                        select p).ToList();
                foreach (var item in x)
                { 
                    if(!FactoryLibroCompras.Existe(item))
                    {
                        FactoryLibroCompras.EscribirItem(item);
                    }
                }
            }
        }
    }
}

