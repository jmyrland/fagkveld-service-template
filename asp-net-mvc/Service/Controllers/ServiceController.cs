using LiteDB;
using Newtonsoft.Json;
using Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Service.Controllers
{
    public class Item
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Text { get; set; }
    }

    public class ItemInputModel
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }

    public class ServiceController : Controller
    {
        private const string DB_PATH = @"C:\Temp\ItemDatabase.db";

        [Route("~/", Name = "default")]
        public JsonResult Index()
        {
            // http://www.litedb.org/
            using (var db = new LiteDatabase(DB_PATH))
            {
                var items = db.GetCollection<Item>("items");
                var results = items.FindAll().ToList().Select(i => new
                {
                    id = i.Id,
                    timestamp = i.Timestamp.ToJavaScriptMilliseconds()
                });
                return Json(results, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("~/")]
        public JsonResult Create(ItemInputModel input)
        {
            // :: Basic validation
            if (string.IsNullOrEmpty(input.Text))
            {
                Response.StatusCode = 409;
                return Json(new { error = "Invalid input." });
            }

            // http://www.litedb.org/
            using (var db = new LiteDatabase(DB_PATH))
            {
                var items = db.GetCollection<Item>("items");

                var item = new Item()
                {
                    Timestamp = DateTime.Now,
                    Text = input.Text,
                };
                items.Insert(item);

                var result = new
                {
                    id = item.Id,
                    text = item.Text,
                    timestamp = item.Timestamp.ToJavaScriptMilliseconds()
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("~/{id}")]
        public ActionResult GetItem(int id)
        {
            // http://www.litedb.org/
            using (var db = new LiteDatabase(DB_PATH))
            {
                var items = db.GetCollection<Item>("items");
                var item = items.FindOne(i => i.Id == id);

                if (item == null)
                {
                    return HttpNotFound();
                }
                
                var result = new
                {
                    id = item.Id,
                    text = item.Text,
                    timestamp = item.Timestamp.ToJavaScriptMilliseconds()
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        // Example of consuming other services
        [Route("~/first-post")]
        public ActionResult ServiceExample()
        {
            var postService = ServiceHelper.GetService("post");

            // ::Get all posts
            var posts = postService.Get<IEnumerable<Dictionary<string, object>>>("/list");
            var firstPost = posts.FirstOrDefault();

            return Json(firstPost, JsonRequestBehavior.AllowGet);
        }
    }
}