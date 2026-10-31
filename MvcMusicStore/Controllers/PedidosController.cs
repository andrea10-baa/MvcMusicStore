using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        // GET: /Pedidos/
        public ActionResult Index()
        {
            bool isAdmin = User.IsInRole("Administrator");

            var query = storeDB.Orders.AsQueryable();
            if (!isAdmin)
            {
                string userName = User.Identity.Name;
                query = query.Where(o => o.Username == userName);
            }

            var orders = query.OrderByDescending(o => o.OrderDate).ToList();
            var ids = orders.Select(o => o.OrderId).ToList();
            var details = storeDB.OrderDetails.Where(d => ids.Contains(d.OrderId)).ToList();

            var model = orders.Select(o => new PedidoResumen
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                Username = o.Username,
                Items = details.Where(d => d.OrderId == o.OrderId).Sum(d => d.Quantity),
                Total = details.Where(d => d.OrderId == o.OrderId).Sum(d => d.Quantity * d.UnitPrice)
            }).ToList();

            return View(model);
        }

        // GET: /Pedidos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var order = storeDB.Orders.Find(id.Value);
            if (order == null)
                return HttpNotFound();

            // Un cliente solo puede ver sus propios pedidos
            if (!User.IsInRole("Administrator") && order.Username != User.Identity.Name)
                return RedirectToAction("Index");

            var items = storeDB.OrderDetails
                .Include("Album")
                .Where(d => d.OrderId == id.Value)
                .ToList();

            ViewBag.Order = order;
            return View(items);
        }
    }
}