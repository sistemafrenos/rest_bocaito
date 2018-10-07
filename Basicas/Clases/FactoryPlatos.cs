using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HK.Clases
{
    public class FactoryPlatos
    {
        public static List<Plato> getItems(string texto)
        {
            using (RestaurantEntities db= new RestaurantEntities())
            {
                var  mplatos = (from x in db.Platos
                                      orderby x.Codigo
                                where (x.Codigo.Contains(texto) || x.Descripcion.Contains(texto) || x.Grupo.Contains(texto) || texto.Length == 0)  
                                select x).ToList();
                return mplatos;
            }
        }
        public static List<Plato> getItems(RestaurantEntities db,string texto)
        {
                var mplatos = (from x in db.Platos
                               orderby x.Codigo
                               where (x.Codigo.Contains(texto) || x.Descripcion.Contains(texto) || x.Grupo.Contains(texto) || texto.Length == 0)  
                               select x).ToList();
                return mplatos;
        }

        public static List<Plato> getItems(string grupo,string texto)
        {
            using (RestaurantEntities db= new RestaurantEntities())
            {
                var mplatos = (from x in db.Platos
                               orderby x.Codigo
                               where (x.Codigo.Contains(texto) || x.Descripcion.Contains(texto) || texto.Length == 0) && x.Grupo == grupo
                               select x).ToList();
                return mplatos;
            }
        }
        public static Plato Item(string id)
        {
            using (RestaurantEntities db= new RestaurantEntities())
            {
                var item = (from x in db.Platos
                            where (x.IdPlato == id)
                            select x).FirstOrDefault();
                return item;
            }
        }
        public static Plato Item(RestaurantEntities db,string id)
        {
                var item = (from x in db.Platos
                            where (x.IdPlato == id)
                            select x).FirstOrDefault();
                return item;
        }

        public static Plato ItemxCodgo(string codigo)
        {
            using (RestaurantEntities db= new RestaurantEntities())
            {
                var item = (from x in db.Platos
                            where x.Codigo == codigo
                            select x).FirstOrDefault();
                return item;
            }
        }
        public static Plato ItemxCodgo(RestaurantEntities db,string codigo)
        {
                var item = (from x in db.Platos
                            where x.Codigo == codigo
                            select x).FirstOrDefault();
                return item;
        }


        public static object[] getArrayGrupos()
        {
            using (RestaurantEntities db= new RestaurantEntities())
            {
                var mgrupos = (from x in db.Platos
                                        orderby x.Grupo
                                        select x.Grupo).Distinct();
                return mgrupos.ToArray();
            }
        }
        public static List<string> getListGrupos()
        {
            using (RestaurantEntities db= new RestaurantEntities())
            {
                var mgrupos = (from x in db.Platos
                               orderby x.Grupo
                               select x.Grupo).Distinct().Take(18);
                return mgrupos.ToList();
            }
        }
        public static List<string> getListContornos()
        {
            using (RestaurantEntities db= new RestaurantEntities())
            {
                var mgrupos = (from x in db.PlatosContornos
                               orderby x.Contorno
                               select x.Contorno).Distinct();
                return mgrupos.ToList();
            }
        }
        public static object[] getArrayEnviarComanda()
        {
            using (RestaurantEntities db= new RestaurantEntities())
            {
                var mgrupos = (from x in db.Platos
                               orderby x.EnviarComanda
                               where x.EnviarComanda!=null
                               select x.EnviarComanda).Distinct();
                return mgrupos.ToArray();
            }
        }
        public static object[] getArrayComentarios()
        {
            using (RestaurantEntities db= new RestaurantEntities())
            {
                var mgrupos = (from x in db.PlatosComentarios
                               orderby x.Comentario
                               where x.Comentario != null
                               select x.Comentario).Distinct();
                return mgrupos.ToArray();
            }
        }
        public static object[] getArrayComentarios(Plato plato)
        {
            using (RestaurantEntities db= new RestaurantEntities())
            {
                var mgrupos = (from x in db.PlatosComentarios
                               orderby x.Comentario
                               where x.Comentario != null && plato.IdPlato == x.IdPlato
                               select x.Comentario).Distinct();
                return mgrupos.ToArray();
            }
        }
        public static object[] getArrayContornos(Plato plato)
        {
            using (RestaurantEntities db= new RestaurantEntities())
            {
                var mgrupos = (from x in db.PlatosContornos
                               orderby x.Contorno
                               where x.Contorno != null && plato.IdPlato == x.IdPlato
                               select x.Contorno).Distinct();
                return mgrupos.ToArray();
            }
        }
        public static List<PlatosIngrediente> getIngredientes(string IdPlato)
        {
            using (RestaurantEntities db= new RestaurantEntities())
            {
                var mgrupos = from x in db.PlatosIngredientes
                              orderby x.Ingrediente
                              where x.Ingrediente != null && x.IdPlato == IdPlato
                              select x;
                return mgrupos.ToList();
            }
        }

        public static bool VerificarExistencia(Plato plato)
        {
            using (RestaurantEntities db= new RestaurantEntities())
            {
               var q =  (from x in db.Platos
                 orderby x.Codigo
                 where (x.Codigo== plato.Codigo || x.Descripcion== plato.Descripcion)
                 select x).FirstOrDefault();
               return q != null;
            }
        }

        public static void ActualizarCostos()
        {
            
            using (RestaurantEntities db = new RestaurantEntities())
            {
                var q = from x in db.Platos
                         where x.Activo==true
                         select x;
                foreach(var plato in q)
                {
                    ActualizarCosto(plato);
                }
            }
        }

        public static Plato ActualizarCosto(Plato plato)
        {
            using (RestaurantEntities db = new RestaurantEntities())
            {
                var q = from x in db.PlatosIngredientes
                        where x.IdPlato == plato.IdPlato
                        select x;
                var q2 = from x in db.PlatosCombos
                        where x.IdPlato == plato.IdPlato
                        select x;
                foreach(var item in q)
                {
                    Ingrediente i = FactoryIngredientes.Item(item.IdIngrediente);
                    if (i != null)
                    {
                        item.CostoRacion = i.Costo;
                        item.Total = i.Costo * item.Cantidad;
                    }
                        
                }
                plato = Item(db, plato.IdPlato);
                plato.Costo = q.Sum(x=> x.Total).GetValueOrDefault(0)+q2.Sum(x=> x.Costo).GetValueOrDefault(0);
                
                db.SaveChanges();
                return plato;
            }
        }
    }
}
