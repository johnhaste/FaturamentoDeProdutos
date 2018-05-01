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
    public class ClientesController : Controller
    {
        private Entities db = new Entities();

        //GET: Buscando clientes por nome
        public ActionResult BuscarClientes(string textoBuscado)
        {

       
            //Verifica se um texto foi buscado
            if (!String.IsNullOrEmpty(textoBuscado))
            {
                return View(pnClientes.RetornaClientesPorNome(textoBuscado));
            }
            
            //Se não retorna todos os clientes
            return View(pnClientes.RetornaClientes());

        }

        //Clientes e busca
        public ActionResult Index(int? page, string textoBuscado, string metodo)
        {
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Cliente> clientes;

            

            //Verifica se um texto foi buscado
            if (!String.IsNullOrEmpty(textoBuscado))
            {
                if (metodo.Equals("inicial"))
                {
                    clientes = pnClientes.RetornaClientesPorInicial(textoBuscado).ToPagedList(pageIndex, pageSize);
                    return View(clientes);
                }
                else {
                    clientes = pnClientes.RetornaClientesPorNome(textoBuscado).ToPagedList(pageIndex, pageSize);
                    return View(clientes);
                }
                
            }

            clientes = pnClientes.RetornaClientesPorNome("").ToPagedList(pageIndex, pageSize);
            return View(clientes);

        }

        // GET: Clientes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            ViewBag.ClienteID = new SelectList(db.Enderecos, "ClienteID", "Rua");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClienteID,Nome")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.ClienteID = Guid.NewGuid();
                db.Clientes.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteID = new SelectList(db.Enderecos, "ClienteID", "Rua", cliente.ClienteID);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteID = new SelectList(db.Enderecos, "ClienteID", "Rua", cliente.ClienteID);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClienteID,Nome")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteID = new SelectList(db.Enderecos, "ClienteID", "Rua", cliente.ClienteID);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Cliente cliente = db.Clientes.Find(id);
            db.Clientes.Remove(cliente);
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
