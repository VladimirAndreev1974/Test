using AtsenergoGrabber.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AtsenergoGrabber.Controllers
{
    public class EntityController : Controller
    {
        private Repository repository;

        public EntityController()
        {
            repository = new Repository(new ReportContext());
        }


        [HttpPost]
        public async Task<JsonResult> Add(string date)
        {

            try
            {

                var localFile = Server.MapPath(Core.Helper.GetLocalFile(date));

                if (System.IO.File.Exists(localFile))
                {

                    var data = await ExcelParser.Parse(localFile);
                    var count = data.Count();
                    if (count > 0)
                    {
                        await repository.Add(data, date);
                    }

                    return Json(new { success = true, count = data.Count() });
                }
                else
                {
                    return Json(new { success = false, message = "Файл не найден, скачайте его" });
                }
            }
            catch (Exception ex)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error("Date: {0} {1} \n {2}", date, ex.Message, ex);
            }

            return Json(new { success = false, message = "При обработке файла произошла ошибка, обратитесь к разработчикам" });
        }


        [HttpPost]
        public async Task<JsonResult> Get(string date)
        {
            try
            {
                var losses = await repository.GetByDate(date);
                return Json(new { success = true, data = losses });
            }
            catch (Exception ex)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error("Date: {0} {1} \n {2}", date, ex.Message, ex);
            }

            return Json(new { success = false, message = "При работе с БД произошла ошибка, обратитесь к разработчикам" });


        }
    }
}