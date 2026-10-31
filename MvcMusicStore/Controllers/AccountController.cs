using System;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public class AccountController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        //
        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(string userName, string password, string returnUrl)
        {
            EnsureAdmin();

            var user = storeDB.Usuarios.FirstOrDefault(u => u.UserName == userName);
            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                SignIn(user);

                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }

        //
        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(string userName, string email, string password, string confirmPassword)
        {
            EnsureAdmin();

            if (string.IsNullOrWhiteSpace(userName))
                ViewBag.Error = "El usuario es obligatorio.";
            else if (string.IsNullOrEmpty(password) || password.Length < 7 || password.All(char.IsLetterOrDigit))
                ViewBag.Error = "La contraseña debe tener al menos 7 caracteres e incluir un carácter no alfanumérico.";
            else if (password != confirmPassword)
                ViewBag.Error = "Las contraseñas no coinciden.";
            else if (storeDB.Usuarios.Any(u => u.UserName == userName))
                ViewBag.Error = "Ese nombre de usuario ya existe.";

            if (ViewBag.Error != null)
                return View();

            var user = new Usuario
            {
                UserName = userName.Trim(),
                Email = email,
                PasswordHash = HashPassword(password),
                Role = "Usuario"
            };
            storeDB.Usuarios.Add(user);
            storeDB.SaveChanges();

            SignIn(user);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // ---------- Auxiliares ----------

        private void SignIn(Usuario user)
        {
            MigrateShoppingCart(user.UserName);

            var ticket = new FormsAuthenticationTicket(
                1, user.UserName, DateTime.Now, DateTime.Now.AddMinutes(30),
                false, user.Role);

            Response.Cookies.Add(new HttpCookie(
                FormsAuthentication.FormsCookieName,
                FormsAuthentication.Encrypt(ticket)));
        }

        private void MigrateShoppingCart(string UserName)
        {
            // Asocia los items del carrito anónimo con el usuario que inició sesión
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.MigrateCart(UserName);
            Session[ShoppingCart.CartSessionKey] = UserName;
        }

        // Crea el usuario administrador si todavía no existe
        private void EnsureAdmin()
        {
            if (!storeDB.Usuarios.Any(u => u.UserName == "admin"))
            {
                storeDB.Usuarios.Add(new Usuario
                {
                    UserName = "admin",
                    Email = "admin@musicstore.com",
                    PasswordHash = HashPassword("password123!"),
                    Role = "Administrator"
                });
                storeDB.SaveChanges();
            }
        }

        private static string HashPassword(string password)
        {
            var salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(salt);

            var hash = new Rfc2898DeriveBytes(password, salt, 10000).GetBytes(32);
            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        private static bool VerifyPassword(string password, string stored)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(stored)) return false;

            var parts = stored.Split(':');
            if (parts.Length != 2) return false;

            var salt = Convert.FromBase64String(parts[0]);
            var expected = Convert.FromBase64String(parts[1]);
            var actual = new Rfc2898DeriveBytes(password, salt, 10000).GetBytes(32);
            return actual.SequenceEqual(expected);
        }
    }
}