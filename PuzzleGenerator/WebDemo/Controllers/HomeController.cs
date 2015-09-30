using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using PuzzleGenerator;

namespace WebDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        static readonly Dictionary<string, List<Bitmap>> ImagesDictionary = new Dictionary<string, List<Bitmap>>();

        public JsonResult UploadImage(string uri, int x, int y)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return null;
            }

            var image = new Bitmap(Image.FromStream(new MemoryStream((new WebClient()).DownloadData(uri))));
            var generator = new PuzzleCreator(image);
            var images = generator.GetPartsOfImage(x, y).ToList();
            
            string puzzleKey = Guid.NewGuid().ToString();
            ImagesDictionary[puzzleKey] = images;
            
            var count = 0;
            var imageUrls = images.Select(item => Url.Action("GenerateImage", "Home", new RouteValueDictionary() { { "key", puzzleKey }, { "id", count++ } }));

            JsonResult result = Json(imageUrls, JsonRequestBehavior.AllowGet);
            return result;
        }

        public ActionResult GenerateImage(string key, int id)
        {
            FileContentResult result;

            using (var memStream = new MemoryStream())
            {
                List<Bitmap> images = ImagesDictionary[key];
                Bitmap bitmap = images[id];
                bitmap.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                result = this.File(memStream.GetBuffer(), "image/jpeg");
            }

            return result;
        }
    }
}
