using System.Text.RegularExpressions;
using EverStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace EverStore.Controllers
{
    public class ApiController : Controller
    {
        [HttpPost]
        public IActionResult ListSkip(string key)
        {
            if (!Regex.IsMatch(key, "^[0-9]+$"))
            {
                return Forbid();
            }
            var value = DataProvider.GetInstance().ListSkip(int.Parse(key));
            return Ok(value);
        }
    }
}