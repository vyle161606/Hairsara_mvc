using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;
namespace DoAnCoSo.Controllers
{
    public class BarbersController : Controller
    {
        // GET: Barbers
        public ActionResult Index()
        {
            var context = new BookingModelContext();
            var barbers = context.Barbers.ToList();
            return View(barbers);
        }
    }
}