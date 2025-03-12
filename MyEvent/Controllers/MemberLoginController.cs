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
    public class MemberLoginController : Controller
    {
        private readonly MyEventContext _context;

        public MemberLoginController(MyEventContext context)
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

            var member = await _context.Member.Include(m => m.MemberTel).Include(m => m.CreditCard).Include(m => m.Credentials).Include(m => m.Order).Where(o => o.MemberID == id).FirstOrDefaultAsync();


            ViewBag.Orders = await _context.Order.Where(o => o.MemberID == id).Include(o => o.Event).ToListAsync();
            return View(member);
        }

        public async Task<IActionResult> Create()
        {
            callViewBagData();

            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            callViewBagData();

            if (_context.Credentials.Find(member.Credentials.Account) != null)
            {
                ViewData["ErrorMessage"] = "帳號已存在";
                return View(member);
            }

            member.JoinDate = DateTime.Now;
            member.Role = "1";
            var memberID = DateTime.Now.ToString("yyyyMM");
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            var lastMember = _context.Member.Where(m => m.JoinDate.Year == currentYear && m.JoinDate.Month == currentMonth).OrderByDescending(m => m.MemberID).FirstOrDefault();
            var newMemberID = lastMember != null ? (int.Parse(lastMember.MemberID) + 1).ToString() : DateTime.Now.ToString("yyyyMM") + "0001";
            member.MemberID = newMemberID;
            ModelState.Remove("MemberID");

            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("MemberID", member.MemberID);                
                string? eventId = HttpContext.Session.GetString("EventID");
                if (eventId != null)
                {
                    //瀏覽的演出
                    return RedirectToAction("Details", "Browse", new { id = eventId });
                }
                return RedirectToAction("Index", "Browse"); 
                ;

            }

            //要寫City跟Area跟角色 (如果沒有value)顯示errormessage


            return View(member);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Credentials login)
        {
            var result = await _context.Credentials.Where(m => m.Account == login.Account && m.Password == login.Password).Include(m => m.Member).FirstOrDefaultAsync();


            if (result != null)
            {
                HttpContext.Session.SetString("MemberID", result.Member.MemberID);
                HttpContext.Session.SetString("Role", result.Member.Role);
                string? eventId = HttpContext.Session.GetString("EventID");
                string? role = HttpContext.Session.GetString("Role");
                if(role == "2")
                {
                    return RedirectToAction("Index", "Members");
                }
                if (eventId != null)
                {
                    //瀏覽的演出
                    return RedirectToAction("Details", "Browse", new { id = eventId });
                }
                return RedirectToAction("Index", "Browse");
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
            HttpContext.Session.Remove("MemberID");
            HttpContext.Session.Remove("Role");

            return RedirectToAction("Index", "Home");
        }
        public bool MemberAccountCheck(string account)
        {
            var result = _context.Credentials.Where(m => m.Account == account).FirstOrDefault();

            return result == null;
            //帳號不重複回傳true
        }

        private void callViewBagData()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "area_data.json");
            var json = System.IO.File.ReadAllText(filePath);
            var areaData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, AreaInfo>>>(json);

            ViewBag.AreaData = areaData;

            var cities = areaData.Keys.ToList();
            ViewBag.Cities = cities;

            var districts = areaData
            .SelectMany(city => city.Value.Keys)
            .ToList();

            ViewBag.Districts = districts;
            
        }

    }
}
