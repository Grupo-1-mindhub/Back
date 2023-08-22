using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
