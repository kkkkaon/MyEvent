using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;
using MyEvent.ViewModels;
using System.Text.Json;
using Newtonsoft.Json;
using GoodStore.Filters;

namespace MyEvent.Controllers
{
    [ServiceFilter(typeof(AdminLoginFilter))]
    public class MembersController : Controller
    {
        private readonly MyEventContext _context;

        public MembersController(MyEventContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(await _context.Member.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            callViewBagData();

            ViewBag.RoleList = await _context.RoleList
                .Select(r => new SelectListItem
                {
                    Value = r.RoleID.ToString(),
                    Text = r.RoleName
                }).ToListAsync();
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
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
            var newMemberID = lastMember != null ? (int.Parse(lastMember.MemberID) + 1).ToString() : memberID + "0001";
            member.MemberID = newMemberID;
            ModelState.Remove("MemberID");

            string role = HttpContext.Session.GetString("Role");

            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("MemberID", member.MemberID);
                HttpContext.Session.SetString("Member", member.Credentials.Account);
                string? eventId = HttpContext.Session.GetString("EventID");
                if (eventId != null)
                {
                    //瀏覽的演出
                    return RedirectToAction("Details", "Browse", new { id = eventId });
                }
                return RedirectToAction("Index", "Members"); 
            }

            //要寫City跟Area跟角色 (如果沒有value)顯示errormessage

            return View(member);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.MemberID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            callViewBagData();
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            ViewBag.RoleList = await _context.RoleList
            .Select(r => new SelectListItem
            {
                Value = r.RoleID.ToString(),
                Text = r.RoleName
            }).ToListAsync();

            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Member member, DateOnly? Birthday)
        {
            if (id != member.MemberID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 先從資料庫查詢出原本的資料
                    var existingMember = await _context.Member.FindAsync(id);
                    if (existingMember == null)
                    {
                        return NotFound();
                    }
                    // 如果前端未傳遞 Birthday，則保持原本的值
                    if (Birthday.HasValue)
                    {
                        existingMember.Birthday = Birthday.Value;
                    }

                    existingMember.MemberName = member.MemberName;
                    existingMember.ZipCode = member.ZipCode;
                    existingMember.City = member.City;
                    existingMember.Area = member.Area;
                    existingMember.Address = member.Address;
                    existingMember.Role = member.Role;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.MemberID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var member = await _context.Member.FindAsync(id);
            if (member != null)
            {
                _context.Member.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(string id)
        {
            return _context.Member.Any(e => e.MemberID == id);
        }

        private void callViewBagData()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "area_data.json");
            var json = System.IO.File.ReadAllText(filePath);
            var areaData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, AreaInfo>>>(json);

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
