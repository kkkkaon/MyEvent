using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;
using MyEvent.ViewModels;

namespace MyEvent.Controllers
{
    public class MemberManageController : Controller
    {
        private readonly MyEventContext _context;

        public MemberManageController(MyEventContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetString("MemberID");
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Login", "MemberLogin"); // 未登入，導向登入頁
            }

            var member =await _context.Member.Include(m => m.MemberTel).Include(m => m.CreditCard).Include(m => m.Credentials).Include(m => m.Order).Where(o => o.MemberID == id).FirstOrDefaultAsync();

            

            ViewBag.Orders = await _context.Order.Where(o => o.MemberID == id).Include(o => o.Event).ToListAsync();
            return View(member);
        }
    }
}
