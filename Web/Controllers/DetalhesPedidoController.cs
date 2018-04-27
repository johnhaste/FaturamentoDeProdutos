﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.DAO;

namespace Web.Controllers
{
    public class DetalhesPedidoController : Controller
    {
        private Entities db = new Entities();

        // GET: DetalhesPedido
        public ActionResult Index()
        {
            var detalhesPedidoes = db.DetalhesPedidoes.Include(d => d.Pedido).Include(d => d.Produto);
            return View(detalhesPedidoes.ToList());
        }

        // GET: DetalhesPedido/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalhesPedido detalhesPedido = db.DetalhesPedidoes.Find(id);
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
                db.DetalhesPedidoes.Add(detalhesPedido);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            DetalhesPedido detalhesPedido = db.DetalhesPedidoes.Find(id);
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
            DetalhesPedido detalhesPedido = db.DetalhesPedidoes.Find(id);
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
            DetalhesPedido detalhesPedido = db.DetalhesPedidoes.Find(id);
            db.DetalhesPedidoes.Remove(detalhesPedido);
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
