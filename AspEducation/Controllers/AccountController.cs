using Microsoft.AspNetCore.Mvc;

namespace AspEducation.Controllers
{
    public class AccountController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}