﻿using System;
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
using Rotativa;

namespace Web.Controllers
{
    public class DetalhesPedidoController : Controller
    {
        private Entities db = new Entities();

        public ActionResult ExportPDF(int? anoEscolhidoEmTela, Guid? produtoIdNaTela)
        {

            ActionAsPdf resultado = new ActionAsPdf("Faturamento", new { anoEscolhido = anoEscolhidoEmTela , produtoId = produtoIdNaTela })
            {
                FileName = Server.MapPath("~/Content/Faturamento.pdf")
            };

            return resultado;
        }

        public ActionResult ExportPDFDetalhesPedidoPorNumero(int? numeroPedidoEmTela)
        {

            ActionAsPdf resultado = new ActionAsPdf("DetalhesPedidoPorNumero", new { numeroPedido = numeroPedidoEmTela})
            {
                FileName = Server.MapPath("~/Content/DetalhesPedidoPorNumero.pdf")
            };

            return resultado;
        }

        public ActionResult DetalhesPedidoPorNumero(int numeroPedido)
        {

            //Número do pedido em questão
            ViewBag.NumeroDoPedido = numeroPedido;

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

            //ViewBags

            //Ano
            ViewBag.AnoEscolhido = ""+anoEscolhido;
    
            //Nome Produto e seu ID
            if (!String.IsNullOrEmpty(produtoID.ToString())) {
                ViewBag.NomeDoProduto = "" + db.Produtos.Where(x => x.ProdutoID == produtoID).First().Descricao;
                ViewBag.ProdutoId = produtoID.ToString();
            }

            
            //DropDownList de Produtos
            Produto escolhaUmProduto = new Produto();
            escolhaUmProduto.Descricao = "Escolha um produto";
            List<Produto> produtos = pnProdutos.RetornaProdutos();
            produtos.Insert(0, escolhaUmProduto);  
            ViewBag.ProdutosDropDown = new SelectList(produtos, "ProdutoID", "Descricao");

            //Cria a ViewModel
            vmFaturamentoTotal FaturamentoGeral = new vmFaturamentoTotal();

            //Faturamentos que aparecerão na ViewModel
            List<Faturamento> faturamentos = new List<Faturamento>();

            //Se for uma busca geral (não de um produto específico)
            if (produtoID == null)
            {
                Entities db = new Entities();
                int size = db.Pedidos.Include("DetalhesPedido").Where(p => p.Data.Year == (int)anoEscolhido).Count();
                //12 meses
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                //List<Pedido> pedidosDoAno = pnPedidos.RetornaPedidosPorPeriodo((int)anoEscolhido, 0);

                for (int i = 1; i < 13; i++)
                {

                    //Pegar todos os pedidos de janeiro
                    /*
                    double valorTotal = 0;

                    foreach(Pedido pedido in pedidosDoMes)
                    {
                        foreach(DetalhesPedido item in pedido.DetalhesPedido)
                        {
                            valorTotal += item.Preco;
                        }
                    }
                    */
                    Faturamento mes_corrente = new Faturamento(i);
                    mes_corrente.ano = (int)anoEscolhido;
                    mes_corrente.mes = i;
                    mes_corrente.valorTotal = 0;
                    faturamentos.Add(mes_corrente);
                }

                int skip = 0;
                int chunk = 25000;

                while (size > 0)
                {
                    var pedidosChunk = (from p in db.Pedidos
                                        join dp in db.DetalhesPedido
                                        on p.NroPedido equals dp.NroPedido
                                        where p.Data.Year == (int)anoEscolhido
                                        orderby p.NroPedido ascending 
                                        select new
                                        {
                                            mes = p.Data.Month,
                                            nroPedido = p.NroPedido,
                                            vlr_item = dp.Preco
                                        }).Skip(skip).Take(chunk).ToList();
                    //List<IEnumerable> nrosPedidos = db.Pedidos.Where(x => x.Data.Year == (int)anoEscolhido).OrderBy(x => x.NroPedido).Skip(skip).Take(chunk).Select(x => new { x.NroPedido, x.Data.Month});
                    //List<Pedido> pedidosChunk = db.Pedidos.Include("DetalhesPedido").Where(x => x.Data.Year == (int)anoEscolhido).OrderBy(x => x.NroPedido).Skip(skip).Take(chunk).ToList();
                    foreach(var pedido in pedidosChunk)
                    {
                        faturamentos[pedido.mes - 1].valorTotal += pedido.vlr_item;
                    }                    
                    size -= chunk;
                    skip += pedidosChunk.Count();
                    //skip += pedidosChunk.Count();
                }
                /*
                List<Pedido> pedidosDoAno = db.Pedidos.Include("DetalhesPedido").Where(x => x.Data.Year == (int)anoEscolhido).ToList();

                foreach (Pedido pedido in pedidosDoAno)
                {
                    int mes_corrente = pedido.Data.Month;
                    foreach(DetalhesPedido item in pedido.DetalhesPedido)
                    {
                        faturamentos[mes_corrente - 1].valorTotal += item.Preco;
                    }
                }
                */

                //Passa os faturamentos para a ViewModel
                FaturamentoGeral.Faturamentos = faturamentos;

                return View(FaturamentoGeral);

            }//Se for a busca por um produto específico
            else {


                /*
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


                }*/

               
                //12 meses
                for (int i = 1; i < 12; i++)
                {

                    //Valor total por mês
                    double valorTotalDoMes = 0;

                    //Pegar todos os Pedidos por Mês e Ano
                    List<Pedido> pedidosDoPeriodo = pnPedidos.RetornaPedidosPorPeriodo((int)anoEscolhido, i).ToList();

                    //Pegar todos os Pedidos que tem um DetalhesPedidos com o produtoID
                    List<DetalhesPedido> pedidosDoPeriodoComOProduto = pnPedidos.RetornaDetalhesPedidoDeListaDePedidosPorProduto(pedidosDoPeriodo, Guid.Parse(produtoID.ToString()));

                    //Soma os lucros daquele produto específico
                    valorTotalDoMes = pedidosDoPeriodoComOProduto.Sum(x => x.Preco);

                    faturamentos.Add(new Faturamento(i)
                    {
                        ano = (int)anoEscolhido,
                        mes = i,
                        valorTotal = valorTotalDoMes
                    });


                }


                //ViewBag.PedidosEnvolvendoOProduto += "|Pedidos do Período" + pedidosDoPeriodo.Count() + "|Com o produto" + pedidosDoPeriodoComOProduto.Count() + "Somando:" + pedidosDoPeriodoComOProduto.Sum(x => x.Preco);


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
