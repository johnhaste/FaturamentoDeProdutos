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

        public static List<DetalhesPedido> RetornaDetalhesPedidos(int numeroPedido)
        {

            try
            {
                Entities db = new Entities();
                return db.DetalhesPedido.ToList();

            }
            catch (Exception)
            {
                throw;
            }

        }

        public static List<Pedido> RetornaPedidosDoCliente(Guid clienteID)
        {

            try
            {
                return RetornaPedidos().Where(x => x.ClienteID == clienteID ).ToList();
           
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        //Retorna os detalhes do pedido através do número
        public static List<DetalhesPedido> RetornaDetalhesPedidosPorNumero(int numeroPedido)
        {

            try
            {
                return RetornaDetalhesPedidos(numeroPedido).Where(x => x.NroPedido == numeroPedido).ToList();

            }
            catch (Exception)
            {
                throw;
            }

        }

        //Recebe um número de pedido e retorna o valor da soma dos preços dos produtos
        public static double RetornaValorTotalDoPedido(int numeroPedido)
        {

            try
            {
                return RetornaDetalhesPedidosPorNumero(numeroPedido).Sum(x => x.Preco);

            }
            catch (Exception)
            {
                throw;
            }

        }

        //Retorna os pedidos por mês
        public static List<Pedido> RetornaPedidosPorPeriodo(int ano, int mes)
        {

            try
            {
                return RetornaPedidos().Where(x => x.Data.Year == ano && x.Data.Month == mes).ToList();

            }
            catch (Exception)
            {
                throw;
            }

        }
        

    }
}
