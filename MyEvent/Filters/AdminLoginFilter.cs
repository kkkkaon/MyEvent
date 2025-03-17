using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GoodStore.Filters
{
    public class AdminLoginFilter:IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //檢查是否已登入(Session有值)
            if (context.HttpContext.Session.GetString("Role") != "2")
            {
                //尚未登入,導向登入頁面
                context.Result = new RedirectToActionResult("Login", "MemberLogin", null);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
