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

        // GET: DetalhesPedido
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

        //Faturamentos por Mês e ano
        public ActionResult Faturamento(int anoEscolhido)
        {

            //Cria a ViewModel
            vmFaturamentoTotal FaturamentoGeral = new vmFaturamentoTotal();

            //Faturamentos que aparecerão na ViewModel
            List<Faturamento> faturamentos = new List<Faturamento>();
            
            //12 meses
            for (int i = 1; i < 13; i++) {

                //Pegar todos os pedidos de janeiro
                List<Pedido> pedidosDoMes = pnPedidos.RetornaPedidosPorPeriodo(anoEscolhido, i);

                faturamentos.Add(new Faturamento(i)
                {
                    ano = anoEscolhido,
                    mes = i,
                    valorTotal = pnPedidos.RetornaValorTotalDeListaDePedido(pedidosDoMes)
                });


            }
            
            FaturamentoGeral.Faturamentos = faturamentos;

            return View(FaturamentoGeral);
            
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
