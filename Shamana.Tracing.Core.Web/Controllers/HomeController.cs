using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shamana.Tracing.Core.Web.Models;

namespace Shamana.Tracing.Core.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var webModel = TransactionProxy<IWebModel>.Create(this.HttpContext, new WebModel());
            webModel.PublicArgs(string.Empty, 0);
            webModel.PublicMethod();
            webModel.PublicProperty = string.Empty;
            string test;
            webModel.PublicArgs(out test);
            return View();
        }

        public ActionResult About()
        {
            throw new Exception();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            var webModel = TransactionProxy<MarshalModel>.Create(this.HttpContext, new MarshalModel());
            webModel.InitMethod();
            webModel.PublicArgs(string.Empty, 0);
            webModel.PublicMethod();
            webModel.PublicProperty = string.Empty;
            string test;
            webModel.PublicArgs(out test);
            return View();
        }
    }
}