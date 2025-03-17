using GoodStore.Filters;
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

        [ServiceFilter(typeof(EHLoginFilter))]
        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetString("EventHolderID");
            var EH = await _context.EventHolder.Include(m => m.ECredentials).FirstOrDefaultAsync();

            ViewBag.Events = await _context.Event.Include(v => v.Venue).Where(o => o.EventHolderID == id).ToListAsync();
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

                return RedirectToAction("Index", "EHLogin");
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

            return RedirectToAction("Index", "Home");
        }

        [ServiceFilter(typeof(EHLoginFilter))]
        public async Task<IActionResult> CheckOrder(string id)
        {
            var orders = await _context.Order.Include(o => o.Member).Include(t=>t.Payment).Include(o => o.Event).Where(o => o.EventID == id).ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> OrderIndex()
        {
            //var myEventContext = _context.OrderDetail.Include(o => o.Order).Include(o => o.Seat);
            var myEventContext = _context.Order.Include(e => e.Event).Include(m => m.Member);
            return View(await myEventContext.ToListAsync());
        }
    }
}
