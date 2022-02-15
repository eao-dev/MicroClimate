using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MicroClimateControllSystem.Models;

namespace MicroClimateControllSystem.Controllers
{
    public class AuthController : Controller
    {
        private ClimateContext db = new ClimateContext();

        [HttpGet]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated) // Если пользователь авторизован
            {
                return RedirectToAction("Index", "Main");
            }
            return View();
        }

        public bool checkAccount(Auth auth, ref Auth authBase)
        {
            if (ModelState.IsValid)
            {
                authBase = null;
                // Проверка корректности учётной записи
                try
                {
                    authBase = db.Auth.First(p => p.Login == auth.Login && p.Password == auth.Password);
                }
                catch
                {
                    return false;
                }

                if (authBase != null)
                    return true;
            }
            return false;
        }

        [HttpPost]
        public ActionResult Index(Auth auth)
        {
            Auth authBase = new Auth();

            if (checkAccount(auth, ref authBase))
            {
                // Успешная аутентификация
                FormsAuthentication.SetAuthCookie(authBase.Login, true);
                return RedirectToAction("Index", "Main");
            } 
            else  
              ViewBag.Error = "Неверный логин/пароль";
              return View("~/Views/Shared/Message.cshtml");
        }

        // Регистрация
        [HttpGet]
        public ActionResult Reg()
        {
            return View();
        }

        public bool addAccount(ref Auth auth)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Auth.Add(auth);
                    db.SaveChanges();
                   
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        // Регистрация
        //[ExceptionAttribute]
        [HttpPost]
        public ActionResult Reg(Auth auth)
        {
            if (addAccount(ref auth))
            {
                ViewBag.Message = "Учётная запись успешно добавлена";
                return View("~/Views/Shared/Message.cshtml");
            }
            else 
            {
                ViewBag.Error = "Не удалось добавить учётную запись!";
                return View("~/Views/Shared/Message.cshtml");
            }
        }
    }
}