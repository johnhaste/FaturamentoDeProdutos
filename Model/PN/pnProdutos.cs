using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DAO;

namespace Model.PN
{
    public class pnProdutos
    {

        public static List<Produto> RetornaProdutos()
        {
            try
            {
                Entities db = new Entities();
                return db.Produtos.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}
