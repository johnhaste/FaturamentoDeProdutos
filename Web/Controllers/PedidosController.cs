using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.PN;
using PagedList;

namespace Web.Controllers
{
    public class PedidosController : Controller
    {
        private Entities db = new Entities();

        /*
        // GET: Pedidos
        public ActionResult Index()
        {
            var pedidos = db.Pedidos.Include(p => p.Cliente).Include(p => p.DetalhesPedido);
            return View(pedidos.ToList());
        }
        */

        //Clientes e busca
        public ActionResult Index(int? page, Guid clienteID)
        {
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            IPagedList<Pedido> pedidos;

            //Verifica se um texto foi buscado
            if (!String.IsNullOrEmpty(clienteID.ToString()))
            {
                pedidos = pnPedidos.RetornaPedidosDoCliente(clienteID).ToPagedList(pageIndex, pageSize);
                return View(pedidos);
            }

            pedidos = pnPedidos.RetornaPedidos().ToPagedList(pageIndex, pageSize);
            return View(pedidos);

        }

        // GET: Pedidos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // GET: Pedidos/Create
        public ActionResult Create()
        {
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome");
            ViewBag.NroPedido = new SelectList(db.DetalhesPedidoes, "NroPedido", "NroPedido");
            return View();
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClienteID,NroPedido,Data")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Pedidos.Add(pedido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", pedido.ClienteID);
            ViewBag.NroPedido = new SelectList(db.DetalhesPedidoes, "NroPedido", "NroPedido", pedido.NroPedido);
            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", pedido.ClienteID);
            ViewBag.NroPedido = new SelectList(db.DetalhesPedidoes, "NroPedido", "NroPedido", pedido.NroPedido);
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClienteID,NroPedido,Data")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", pedido.ClienteID);
            ViewBag.NroPedido = new SelectList(db.DetalhesPedidoes, "NroPedido", "NroPedido", pedido.NroPedido);
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedidos.Find(id);
            db.Pedidos.Remove(pedido);
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
