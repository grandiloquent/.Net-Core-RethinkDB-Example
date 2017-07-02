using System.Threading.Tasks;
using EverStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace EverStore.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewData["Articles"] = await DataProvider.GetInstance().ListLastArticles();

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Calculator()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}