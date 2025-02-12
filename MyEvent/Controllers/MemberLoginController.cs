﻿using Microsoft.AspNetCore.Mvc;
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


        public IActionResult Create()
        {
            ViewBag.Credential = new Credentials();
            //var member = _context.Member.Include(m => m.MemberTel).Include(m => m.CreditCard).Include(m => m.Credentials).Include(m => m.Order).FirstOrDefault();

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

            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            //member.Credentials = new Credentials();
            member.JoinDate = DateTime.Now;
            var memberID = DateTime.Now.ToString("yyyyMM");
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            var lastMember = _context.Member.Where(m => m.JoinDate.Year == currentYear && m.JoinDate.Month == currentMonth).OrderByDescending(m => m.MemberID).FirstOrDefault();
            var newMemberID = lastMember != null ? (int.Parse(lastMember.MemberID) + 1).ToString() : DateTime.Now.ToString("yyyyMM") + "0001";
            member.MemberID = newMemberID;
            ModelState.Remove("MemberID");

            if (_context.Credentials.Find(member.Credentials.Account) != null)
            {
                ViewData["ErrorMessage"] = "帳號已存在";
                return View(member);
            }

            //要寫City跟Area必填的errormessage

            if (ModelState.IsValid)
            {

                _context.Member.Add(member);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("MemberID", member.MemberID);
                HttpContext.Session.SetString("Member", member.Credentials.Account);
                string? eventId = HttpContext.Session.GetString("EventID");
                if (eventId != null)
                {
                    //瀏覽的演出
                    return RedirectToAction("Details", "Browse", new { id = eventId });
                }
                return RedirectToAction("Index", "Browse");
                ;

            }

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
                HttpContext.Session.SetString("Member", result.Account);
                string? eventId = HttpContext.Session.GetString("EventID");
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
            return View(result);
        }

        public IActionResult Logout()
        {
            //5.4.2
            //HttpContext.Session.Clear():清除所有session
            HttpContext.Session.Remove("Member");

            return RedirectToAction("Index", "Home");
        }
    }
}
