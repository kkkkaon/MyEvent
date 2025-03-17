using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GoodStore.Filters
{
    //寫完要去program註冊服務
    public class EHLoginFilter:IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //檢查是否已登入(Session有值)
            if (context.HttpContext.Session.GetString("EventHolderID") == null)
            {
                //尚未登入,導向登入頁面
                context.Result = new RedirectToActionResult("Login", "EHLogin", null);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
