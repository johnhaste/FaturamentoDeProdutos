using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DAO;

/**
 * Processos de Negócio para Clientes e seus Endereços
 **/
namespace Model.PN
{
    public class pnClientes
    {
        public static List<Cliente> RetornaClientes()
        {
            try
            {
                Entities db = new Entities();
                return db.Clientes.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
