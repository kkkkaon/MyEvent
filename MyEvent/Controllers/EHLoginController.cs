using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MyEvent.Models;
using MyEvent.ViewModels;
using System.Text.Json;


namespace MyEvent.Controllers
{
    public class EHLoginController : Controller
    {
        private readonly MyEventContext _context;

        public EHLoginController(MyEventContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetString("EventHolderID");
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Login", "EHLogin"); // 未登入，導向登入頁
            }

            var EH = await _context.EventHolder.Include(m => m.ECredentials).Include(m => m.Event).Where(o => o.EventHolderID == id).FirstOrDefaultAsync();

            ViewBag.Events = await _context.Event.Where(o => o.EventHolderID == id).Select(e => e.EventID).ToListAsync();
            return View(EH);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(ECredentials login)
        {
            var result = await _context.ECredentials.Where(m => m.Account == login.Account && m.Password == login.Password).Include(m => m.EventHolder).FirstOrDefaultAsync();


            if (result != null)
            {
                HttpContext.Session.SetString("EventHolderID", result.EventHolder.EventHolderID);

                return RedirectToAction("Index", "Events");
                ;
            }
            else
            {
                ViewData["Message"] = "帳號或密碼錯誤";
            }
            return View(login);
        }

        public IActionResult Logout()
        {
            //5.4.2
            //HttpContext.Session.Clear():清除所有session
            HttpContext.Session.Remove("EventHolderID");
            HttpContext.Session.Remove("Role");

            return RedirectToAction("Index", "Home");
        }
    }
}
