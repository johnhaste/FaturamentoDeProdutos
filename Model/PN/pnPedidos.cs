using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DAO;

/**
 * Processos de Negócio para Pedidos e DetalhesPedido, e dentro Produtos
 **/
namespace Model.PN
{
    public class pnPedidos
    {
        public static List<Pedido> RetornaPedidos()
        {
            try
            {
                Entities db = new Entities();
                return db.Pedidos.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
