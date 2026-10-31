using MvcMusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore.Controllers
{
    public class CartController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        // GET: /Cart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            return View(viewModel);
        }

        // GET: /Cart/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            var addedAlbum = storeDB.Albums.Single(album => album.AlbumId == id);

            var cart = ShoppingCart.GetCart(this);
            cart.AddToCart(addedAlbum);

            return RedirectToAction("Index");
        }

        // AJAX: /Cart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this);
            string albumName = storeDB.Carts.Single(item => item.RecordId == id).Album.Title;

            int itemCount = cart.RemoveFromCart(id);

            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(albumName) + " se eliminó de tu carrito.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCartItems().Count(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }

        // GET: /Cart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}