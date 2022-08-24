using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Xml.Integration.Data.Helper;
using Xml.Integration.Data.Models;
using Xml.Integration.Data.Tiger;

namespace Xml.Integration.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();

        }

        public ActionResult Start()
        {
            ViewBag.Model = new RootDto();
            return View();

        }

        [HttpPost]
        public ActionResult FileRead(HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.Error = "Dosya seçimi zorunludur.";
                return View("ExcelDealer");
            }

            try
            {
                var document = new XmlDocument();
                document.Load(file.InputStream);


                var json = JsonConvert.SerializeXmlNode(document);

                var dto = JsonConvert.DeserializeObject<RootDto>(json);
                //var resp = OrderStoreService.CreateSalesOrder();
                //var list = dto.Root;
                ViewBag.Model = dto;

            }
            catch (Exception e)
            {
                ViewBag.Error = "Yükleme esnasında hata oluştu.Lütfen xml kontrol ediniz. Err = " + e.Message;
            }

            return View("Start");
        }


        [HttpPost]
        public JsonResult CreaateInvoice(SalesInvoiceHeader satir)
        {
            var sonuc = new InvoiceStoreService().CreateSalesInvoice(satir);
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
    }
}