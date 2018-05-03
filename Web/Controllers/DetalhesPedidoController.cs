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
        public ActionResult Faturamento()
        {

            //Cria a ViewModel
            vmFaturamentoTotal FaturamentoGeral = new vmFaturamentoTotal();

            List<Faturamento> faturamentos = new List<Faturamento>();

            Faturamento primeiroFaturamento = new Faturamento(1) {ano = 2018, mes = 1, valorTotal = 1000 };
            Faturamento segundoFaturamento = new Faturamento(2) { ano = 2018, mes = 2, valorTotal = 1200 };

            faturamentos.Add(primeiroFaturamento);
            faturamentos.Add(segundoFaturamento);

            FaturamentoGeral.Faturamentos = faturamentos;

            return View(FaturamentoGeral);

            /*
            //Cria um evento que irá receber as informações básicas da classe
            Eventos Evento = pnEventos.RetornaEventoPorId(id);

            //Preenche as informações e listas do evento
            EventoCompleto.id = Guid.Parse(id.ToString());
            EventoCompleto.Nome = Evento.Nome;
            EventoCompleto.Descricao = Evento.Descricao;
            EventoCompleto.Categoria = pnEventos.RetornaCategoriaPorId(Evento.Categoria).Nome;
            EventoCompleto.DataDeInicio = Evento.DataDeInicio;
            EventoCompleto.DataDeTermino = Evento.DataDeTermino;
            EventoCompleto.Posts = pnEventos.RetornaPostsPorIdEvento(id);
            EventoCompleto.InscritosNoEvento = pnUsuario.RetornaInscritosPorIdEvento(id);
            EventoCompleto.TwitterLink = pnEventos.RetornaLinkTwitterPorDescricao(Evento.Nome, Evento.Descricao);
            */
            
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
