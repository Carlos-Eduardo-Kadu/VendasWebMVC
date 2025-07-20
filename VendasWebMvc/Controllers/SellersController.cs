using Microsoft.AspNetCore.Mvc;

namespace VendasWebMvc.Controllers
{
    public class SellersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
