using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.AUXILIAR;
using Model.VM;
using Model.DAO;
using Model.PN;
using PagedList;

namespace Web.Controllers
{
    public class DetalhesPedidoController : Controller
    {
        private Entities db = new Entities();

        /*
        // GET: DetalhesPedido Com Paginação
        public ActionResult Index(int? page, int numeroPedido)
        {
            ViewBag.Total = pnPedidos.RetornaDetalhesPedidosPorNumero(numeroPedido).Sum(x => x.Preco);

            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            IPagedList<DetalhesPedido> detalhesPedido;

            detalhesPedido = pnPedidos.RetornaDetalhesPedidosPorNumero(numeroPedido).ToPagedList(pageIndex, pageSize);
           
            return View(detalhesPedido);
        }
        */

        public ActionResult DetalhesPedidoPorNumero(int numeroPedido)
        {

            ViewBag.Total = pnPedidos.RetornaDetalhesPedidosPorNumero(numeroPedido).Sum(x => x.Preco);

            //Com Um Pedido funciona
            List<DetalhesPedido> detalhesPedido = pnPedidos.RetornaDetalhesPedidosPorNumero(numeroPedido);
            
          
            return View(detalhesPedido);
        }

        //Faturamentos Total por Mês e ano
        public ActionResult Faturamento(int? anoEscolhido, Guid? produtoID)
        {
            
            //Ajustando o ano atual
            if (anoEscolhido == null) {
                anoEscolhido = DateTime.Now.Year;
            }

            //DropDownList de Produtos
            ViewBag.ProdutosDropDown = new SelectList(pnProdutos.RetornaProdutos(), "ProdutoID", "Descricao");

            //Cria a ViewModel
            vmFaturamentoTotal FaturamentoGeral = new vmFaturamentoTotal();

            //Faturamentos que aparecerão na ViewModel
            List<Faturamento> faturamentos = new List<Faturamento>();

            //Se for uma busca geral (não de um produto específico)
            if (produtoID == null)
            {

                //12 meses
                for (int i = 1; i < 2; i++)
                {

                    //Pegar todos os pedidos de janeiro
                    List<Pedido> pedidosDoMes = pnPedidos.RetornaPedidosPorPeriodo((int) anoEscolhido, i);

                    faturamentos.Add(new Faturamento(i)
                    {
                        ano = (int) anoEscolhido,
                        mes = i,
                        valorTotal = pnPedidos.RetornaValorTotalDeListaDePedido(pedidosDoMes)
                    });
                    
                }

                //Passa os faturamentos para a ViewModel
                FaturamentoGeral.Faturamentos = faturamentos;

                return View(FaturamentoGeral);

            }//Se for a busca por um produto específico
            else {

                //Pegar todos os DetalhesPedido que possuem o produtoID
                List<DetalhesPedido> detalhesPedidoDoProduto = pnPedidos.RetornaDetalhesPedidosPorProduto(produtoID);
              
                 //Números dos pedidos que possuem o produto
                List<int> nroPedidos = new List<int>();

                //Pegar todos os pedidos a partir dos DetalhesPedido
                for (int i = 0; i < detalhesPedidoDoProduto.Count; i++){
                    
                    //Verifica se o pedido já foi adicionado
                    if (!nroPedidos.Contains(detalhesPedidoDoProduto.ElementAt(i).NroPedido)) {

                        nroPedidos.Add(detalhesPedidoDoProduto.ElementAt(i).NroPedido);

                    }
                    
                }

                //Lista com os pedidos que possuem o produto
                List<Pedido> pedidosComOProduto = pnPedidos.RetornaPedidosPorListaDeNroPedido(nroPedidos);
                
                //12 meses
                for (int i = 1; i < 2; i++)
                {

                    //Valor total por mês
                    double valorTotalDoMes = 0;

                    //Pedidos do mês do loop
                    List<Pedido> pedidosDoMes = pedidosComOProduto.Where(x => x.Data.Month == i && x.Data.Year == anoEscolhido).ToList();

                    //Recebe o valor total do mês DO PRODUTO ESPECÍFICO
                    if (pedidosDoMes.Count > 0) {

                        //Soma de cada pedido
                        for (int j = 0; j < pedidosDoMes.Count; j++) {

                            valorTotalDoMes += pnPedidos.RetornaValorDoProdutoDentroDoPedido(pedidosDoMes.ElementAt(j).NroPedido, produtoID);

                        }
                        
                    }
                    
                    faturamentos.Add(new Faturamento(i)
                    {
                        ano = (int) anoEscolhido,
                        mes = i,
                        valorTotal = valorTotalDoMes
                    });


                }

                FaturamentoGeral.Faturamentos = faturamentos;

                return View(FaturamentoGeral);
                
            }
            
        }

        // GET: DetalhesPedido/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalhesPedido detalhesPedido = db.DetalhesPedido.Find(id);
            if (detalhesPedido == null)
            {
                return HttpNotFound();
            }
            return View(detalhesPedido);
        }

        // GET: DetalhesPedido/Create
        public ActionResult Create()
        {
            ViewBag.NroPedido = new SelectList(db.Pedidos, "NroPedido", "NroPedido");
            ViewBag.ProdutoID = new SelectList(db.Produtos, "ProdutoID", "Descricao");
            return View();
        }

        // POST: DetalhesPedido/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NroPedido,ProdutoID,Qtde,Preco")] DetalhesPedido detalhesPedido)
        {
            if (ModelState.IsValid)
            {
                db.DetalhesPedido.Add(detalhesPedido);
                db.SaveChanges();
                return RedirectToAction("Index", new { numeroPedido = detalhesPedido.NroPedido });
            }

            ViewBag.NroPedido = new SelectList(db.Pedidos, "NroPedido", "NroPedido", detalhesPedido.NroPedido);
            ViewBag.ProdutoID = new SelectList(db.Produtos, "ProdutoID", "Descricao", detalhesPedido.ProdutoID);
            return View(detalhesPedido);
        }

        // GET: DetalhesPedido/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalhesPedido detalhesPedido = db.DetalhesPedido.Find(id);
            if (detalhesPedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.NroPedido = new SelectList(db.Pedidos, "NroPedido", "NroPedido", detalhesPedido.NroPedido);
            ViewBag.ProdutoID = new SelectList(db.Produtos, "ProdutoID", "Descricao", detalhesPedido.ProdutoID);
            return View(detalhesPedido);
        }

        // POST: DetalhesPedido/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NroPedido,ProdutoID,Qtde,Preco")] DetalhesPedido detalhesPedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detalhesPedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NroPedido = new SelectList(db.Pedidos, "NroPedido", "NroPedido", detalhesPedido.NroPedido);
            ViewBag.ProdutoID = new SelectList(db.Produtos, "ProdutoID", "Descricao", detalhesPedido.ProdutoID);
            return View(detalhesPedido);
        }

        // GET: DetalhesPedido/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalhesPedido detalhesPedido = db.DetalhesPedido.Find(id);
            if (detalhesPedido == null)
            {
                return HttpNotFound();
            }
            return View(detalhesPedido);
        }

        // POST: DetalhesPedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetalhesPedido detalhesPedido = db.DetalhesPedido.Find(id);
            db.DetalhesPedido.Remove(detalhesPedido);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
