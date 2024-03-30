using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;

namespace DoAnCoSo.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "Admin")]
        // GET: Admin/Home
        public ActionResult Index()
        {
            var context = new BookingModelContext();
            var account = new AspNetUserRoleModel();
            
            // Truyền số lượng User vào ViewBag
            var numberOfUsers = account.AspNetUsers.Count();
            ViewBag.NumberOfUsers = numberOfUsers;

            // Truyền số lượng News vào ViewBag
            var numberOfNews = context.News.Count();
            ViewBag.NumberOfNews = numberOfNews;

            // Truyền số lượng Services vào ViewBag
            var numberOfServices = context.Services.Count();
            ViewBag.NumberOfServices = numberOfServices;

            // Truyền số lượng barbers vào ViewBag
            var numberOfStyles = context.Styles.Count();
            ViewBag.NumberOfStyles = numberOfStyles;

            // Truyền số lượng barbers vào ViewBag
            var numberOfBarbers = context.Barbers.Count();
            ViewBag.NumberOfBarbers = numberOfBarbers;

            // Truyền số lượng Seat vào ViewBag
            var numberOfSeats = context.Seats.Count();
            ViewBag.NumberOfSeats = numberOfSeats;

            // Truyền số lượng Rating vào ViewBag
            var numberOfRatings = context.Ratings.Count();
            ViewBag.NumberOfRatings = numberOfRatings;

            // Truyền số lượng Rating vào ViewBag
            var numberOfAppointments = context.Appointments.Count();
            ViewBag.NumberOfAppointments = numberOfAppointments;

            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetAppointmentCount()
        {
            var context = new BookingModelContext();
            var numberOfAppointmentsStatus1 = context.Appointments.Count(a => a.Status == 1);          

            return Json(numberOfAppointmentsStatus1, JsonRequestBehavior.AllowGet);
        }

    }
}
