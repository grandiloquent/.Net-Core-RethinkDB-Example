using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EverStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace EverStore.Controllers
{
    //[Route("/t")]
    public class TagController : Controller
    {
        public async Task<IActionResult> Index(string tag)
        {
            ViewData["Articles"] = await DataProvider.GetInstance().ListArticlesByTag(tag);

            if (tag == "foods")
            {
                ViewData["MenuActive"] = 2;
            }
            else if (tag == "encyclopedia")
            {
                ViewData["MenuActive"] = 3;
            }
            return View();
        }
    }
}