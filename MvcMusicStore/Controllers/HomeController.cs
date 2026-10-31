using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        public ActionResult Index()
        {
            ViewBag.Genres = storeDB.Genres.OrderBy(g => g.Name).ToList();

            // Los 5 álbumes más vendidos
            var albums = GetTopSellingAlbums(5);
            return View(albums);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private List<Album> GetTopSellingAlbums(int count)
        {
            // Ids de los álbumes con más unidades vendidas
            var topIds = storeDB.OrderDetails
                .GroupBy(od => od.AlbumId)
                .OrderByDescending(g => g.Sum(od => od.Quantity))
                .Select(g => g.Key)
                .Take(count)
                .ToList();

            var top = storeDB.Albums.Include("Artist").Include("Genre")
                .Where(a => topIds.Contains(a.AlbumId))
                .ToList()
                .OrderBy(a => topIds.IndexOf(a.AlbumId))
                .ToList();

            // Si hay menos de 5 con ventas, se completa con los álbumes más nuevos
            if (top.Count < count)
            {
                var extra = storeDB.Albums.Include("Artist").Include("Genre")
                    .Where(a => !topIds.Contains(a.AlbumId))
                    .OrderByDescending(a => a.AlbumId)
                    .Take(count - top.Count)
                    .ToList();

                top.AddRange(extra);
            }

            return top;
        }
    }
}