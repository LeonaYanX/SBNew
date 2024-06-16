using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace SB.Controllers
{
    public class BaseController : Controller
    {
        protected bool IsUserLogged()
        {
            return HttpContext.Session.GetInt32("user").HasValue;
        }

        protected int GetUserId()
        {
            
                return HttpContext.Session.GetInt32("user").Value;
            
        }

       
    }
}
