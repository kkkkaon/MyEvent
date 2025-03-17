using GoodStore.Filters;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyEventContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("MyEventConnection")));


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); 
builder.Services.AddDistributedMemoryCache();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/MemberLogin/Login"; // 設定登入頁面
});

//過濾器
builder.Services.AddScoped<AdminLoginFilter>();
builder.Services.AddScoped<MemberLoginFilter>();
builder.Services.AddScoped<EHLoginFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSession();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
