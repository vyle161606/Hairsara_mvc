using DoAnCoSo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCoSo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var context = new BookingModelContext();
            HomeModels obj = new HomeModels
            {
                listServices = context.Services.ToList(),
                listNews = context.News.ToList()
            };
            return View(obj);
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}