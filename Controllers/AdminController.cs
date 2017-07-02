using System;
using System.Linq;
using System.Threading.Tasks;
using EverStore.Libraries;
using EverStore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace EverStore.Controllers
{
    public class AdminController : Controller
    {
        private const string PASSWORD = "secret";


        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] JObject value)
        {
            var password = value.GetValue("password").ToString();
            if (password != PASSWORD)
            {
                return Forbid();
            }

            var article = new Article
            {
                Id = Shortid.GetInstace().Generate(),
                Title = value.GetValue("title").ToString(),
                Content = value.GetValue("content").ToString(),
                Image = value.GetValue("image").ToString(),
                Tags = value.GetValue("tags").Select(i => i.ToString()).ToArray(),
                CreateAt = DataProvider.GetTimestamp()
            };

            article.Id = Shortid.GetInstace().Generate();
            var result = await DataProvider.GetInstance().Insert(article);
            return Json(result);
        }

        public async Task<IActionResult> Update(string value)
        {

            Console.WriteLine(value);

            return Ok("OK");

        }
    }
}