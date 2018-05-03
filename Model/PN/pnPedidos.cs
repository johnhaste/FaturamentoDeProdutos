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

        public static Pedido RetornaPedidoPorNumero(int nroPedido)
        {
            try
            {
                Entities db = new Entities();
                return db.Pedidos.Where(x => x.NroPedido == nroPedido).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<DetalhesPedido> RetornaDetalhesPedidos()
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

        public static DetalhesPedido RetornaDetalhesPedidosPorId(int NroPedido)
        {

            try
            {
                Entities db = new Entities();
                return db.DetalhesPedido.Where(x => x.NroPedido == NroPedido).First();
                
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
                Entities db = new Entities();
                return db.Pedidos.Where(x => x.ClienteID == clienteID).ToList();
                
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
                Entities db = new Entities();
                return db.DetalhesPedido.Where(x => x.NroPedido == numeroPedido).ToList();
                
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

        //Recebe uma lista de pedidos e retorna a soma de todos os pedidos
        public static double RetornaValorTotalDeListaDePedido(List<Pedido> pedidos)
        {

            try
            {
                double valorTotal = 0;

                for (int i = 0; i < pedidos.Count; i++)
                {

                    valorTotal += pnPedidos.RetornaValorTotalDoPedido(pedidos.ElementAt(i).NroPedido);

                }

                return valorTotal;

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

        //Retorna todos os DetalhesPedidos que envolvem um produto específico
        public static List<DetalhesPedido> RetornaDetalhesPedidosPorProduto(Guid? produtoID) {

            try
            {
                return RetornaDetalhesPedidos().Where(x => x.ProdutoID == produtoID).ToList();

            }
            catch (Exception)
            {
                throw;
            }

        }

        public static List<Pedido> RetornaPedidosPorListaDeNroPedido(List<int> nroPedidos) {

            List<Pedido> pedidos = new List<Pedido>();

            for (int i = 0; i < nroPedidos.Count; i++)
            {
                pedidos.Add(RetornaPedidoPorNumero(nroPedidos.ElementAt(i)));
            }

            return pedidos;
        }

        public static double RetornaValorDoProdutoDentroDoPedido(int nroPedido,Guid? produtoID) 
        {
            double valorDoProdutoNoPedido = 0;

            //Retorna o preco do DetalhesPedido que tem o nroPedido
            valorDoProdutoNoPedido = pnPedidos.RetornaDetalhesPedidos().Where(x => x.NroPedido == nroPedido && x.ProdutoID == produtoID).FirstOrDefault().Preco;
            
            return valorDoProdutoNoPedido;
        }
        
    }
}
