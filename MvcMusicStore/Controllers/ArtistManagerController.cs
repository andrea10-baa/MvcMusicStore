using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    // [Authorize(...)]  <- poné aquí el mismo que usa StoreManagerController
    public class ArtistManagerController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        // GET: /ArtistManager
        public ActionResult Index()
        {
            return View(storeDB.Artists.OrderBy(a => a.Name).ToList());
        }

        // GET: /ArtistManager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ArtistManager/Create
        [HttpPost]
        public ActionResult Create(Artist artist)
        {
            if (string.IsNullOrWhiteSpace(artist.Name))
                ModelState.AddModelError("Name", "El nombre es obligatorio");

            if (ModelState.IsValid)
            {
                storeDB.Artists.Add(artist);
                storeDB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artist);
        }
    }
}