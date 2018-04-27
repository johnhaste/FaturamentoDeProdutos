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

        //Retorna Clientes por Nome
        public static List<Cliente> RetornaClientesPorNome(String textoBuscado)
        {

            try
            {
                return RetornaClientes().Where(x => x.Nome.IndexOf(textoBuscado, StringComparison.OrdinalIgnoreCase) >= 0 || x.Nome.IndexOf(textoBuscado, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

    }
}
