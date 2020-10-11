using AtsenergoGrabber.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AtsenergoGrabber.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Download(string date)
        {

            try
            {


                var url = Helper.GetUrl(date);
                var html = string.Empty;

                WebClient client = new WebClient();
                html = await client.DownloadStringTaskAsync(url);

                if (!string.IsNullOrEmpty(html))
                {
                    var q = Helper.GetQueryString(html);
                    if (!string.IsNullOrEmpty(q))
                    {


                        var urlFile = Helper.GetUrlFile(q);

                        var dir = Server.MapPath(Helper.GetUploadDir());

                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        var localFile = Server.MapPath(Helper.GetLocalFile(date));

                        await client.DownloadFileTaskAsync(urlFile, localFile);
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Файл  на сайте https://www.atsenergo.ru/nreport?rname=losses_regions не найден" });
                    }
                }

            }

            catch (Exception ex)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error("Date: {0} {1} \n {2}", date, ex.Message, ex);
            }

            return Json(new { success = false, message = "При загрузке файла произошла ошибка, обратитель к разработчикам" });
        }


    }
}