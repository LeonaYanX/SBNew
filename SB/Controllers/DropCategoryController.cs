using Microsoft.AspNetCore.Mvc;

namespace SB.Controllers
{
    public class DropCategoryController : Controller
    {
        public IActionResult _CategoryPartial()
        {
            Catalog catalog = new Catalog();
            return View(catalog.Value);
        }
    }
}
