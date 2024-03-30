using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using Common;
using DoAnCoSo.Models;
using Microsoft.AspNet.Identity;

namespace DoAnCoSo.Controllers
{
    public class AppointmentsController : Controller
    {
        private BookingModelContext _context = new BookingModelContext();
        private AspNetUserRoleModel _users = new AspNetUserRoleModel();

        [Authorize]
        // GET: Appointments/CreateStep1
        public ActionResult CreateStep1()
        {
            var services = _context.Services.ToList();
            var barbers = _context.Barbers.ToList();

            ViewBag.Services = new SelectList(services, "ServiceId", "ServiceName");
            ViewBag.Barbers = new SelectList(barbers, "BarberId", "BarberName");

            return View();
        }

        // POST: Appointments/CreateStep1
        // POST: Appointments/CreateStep1
        [HttpPost]
        public ActionResult CreateStep1(Appointments appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.UserId = User.Identity.GetUserId();
                Session["AppointmentStep1"] = appointment;
                return RedirectToAction("CreateStep2");
            }

            var services = _context.Services.ToList();
            var barbers = _context.Barbers.ToList();
            ViewBag.Services = new SelectList(services, "ServiceId", "ServiceName");
            ViewBag.Barbers = new SelectList(barbers, "BarberId", "BarberName");

            return View(appointment);
        }


        // GET: Appointments/CreateStep2
        public ActionResult CreateStep2()
        {
            if (Session["AppointmentStep1"] == null)
            {
                return RedirectToAction("CreateStep1");
            }

            var appointmentStep1 = (Appointments)Session["AppointmentStep1"];
            ViewBag.AppointmentStep1 = appointmentStep1;
            var seats = _context.Seats.ToList();
            //lay ra moc thoi gian BUSY 
           // var ListBooked = 
            ViewBag.ListBooked = _context.Appointments
                      .Where(p => p.BarberId == appointmentStep1.BarberId && p.StartDate == appointmentStep1.StartDate)
                      .ToDictionary(x => x.StartTime.Value.ToString(@"h\:mm"), x => x);

            ViewBag.Seats = new SelectList(seats, "SeatId", "SeatNumber");
            

            return View();
        }

        // POST: Appointments/CreateStep2
        [HttpPost]
        public ActionResult CreateStep2(Appointments appointment)
        {
            if (ModelState.IsValid)
            {
                Session["AppointmentStep2"] = appointment;
                return RedirectToAction("CreateStep3");
            }

            var appointmentStep1 = (Appointments)Session["AppointmentStep1"];
            ViewBag.AppointmentStep1 = appointmentStep1;

            return View(appointment);
        }

        // GET: Appointments/CreateStep39
        [HttpGet]
        public ActionResult CreateStep3()
        {
            if (Session["AppointmentStep1"] == null || Session["AppointmentStep2"] == null)
            {
                return RedirectToAction("CreateStep1");
            }

            var appointmentStep1 = (Appointments)Session["AppointmentStep1"];
            var appointmentStep2 = (Appointments)Session["AppointmentStep2"];

            var service = _context.Services.Find(appointmentStep1.ServiceId);
            var barber = _context.Barbers.Find(appointmentStep1.BarberId);
            var seat = _context.Seats.Find(appointmentStep2.SeatId);
            var seatNumber = seat != null ? seat.SeatNumber.ToString() : string.Empty;
            ViewBag.SeatNumber = seatNumber;

            ViewBag.AppointmentStep1 = appointmentStep1;
            ViewBag.AppointmentStep2 = appointmentStep2;
            ViewBag.ServiceName = service?.ServiceName;
            ViewBag.BarberName = barber?.BarberName;
            ViewBag.StartDate = appointmentStep1.StartDate?.ToString("dd/MM/yyyy");
            ViewBag.SeatId = appointmentStep2?.SeatId;
            ViewBag.Note = appointmentStep2?.Note;

            return PartialView("CreateStep3", appointmentStep1);
        }

        [HttpPost]
        public ActionResult CreateStep3Post(string email)
        {
            
            var appointmentStep1 = (Appointments)Session["AppointmentStep1"];
            var appointmentStep2 = (Appointments)Session["AppointmentStep2"];

            if (appointmentStep2 != null)
            {
                if (appointmentStep2.StartDate != null)
                {
                    appointmentStep1.StartDate = appointmentStep2.StartDate;
                }
                appointmentStep1.StartTime = appointmentStep2.StartTime;
                appointmentStep1.EndTime = appointmentStep2.EndTime;
                appointmentStep1.SeatId = appointmentStep2.SeatId;
                appointmentStep1.Note = appointmentStep2.Note;

                var validSeat = _context.Seats.Any(s => s.SeatId == appointmentStep1.SeatId);
                if (!validSeat)
                {
                    var seats = _context.Seats.ToList();
                    ViewBag.Seats = new SelectList(seats, "SeatId", "SeatNumber");
                    ViewBag.AppointmentStep1 = appointmentStep1;
                    ViewBag.AppointmentStep2 = appointmentStep2;
                    ViewBag.ServiceName = _context.Services.Find(appointmentStep1.ServiceId)?.ServiceName;
                    ViewBag.BarberName = _context.Barbers.Find(appointmentStep1.BarberId)?.BarberName;

                    ModelState.AddModelError("", "The selected seat is not valid. Please choose another seat.");
                    return PartialView("CreateStep3", appointmentStep1);
                }
                appointmentStep1.UserId = User.Identity.GetUserId();
                appointmentStep1.Status = 1;

                // Cập nhật trạng thái của ghế thành 1
                var seat = _context.Seats.Find(appointmentStep1.SeatId);
                if (seat != null)
                {
                    seat.Availability = 1;
                }

                _context.Appointments.Add(appointmentStep1);
                _context.SaveChanges();

                // Gửi thông tin lịch hẹn về email của người dùng
                SendAppointmentEmail(email);

                Session.Remove("AppointmentStep1");
                Session.Remove("AppointmentStep2");

                // Lưu dữ liệu thành công, hiển thị thông báo và chuyển hướng trang
                // alert('Đặt lịch thành công');
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("CreateStep2");
            }
        }
        private void SendAppointmentEmail(string email)
        {
            var appointmentStep1 = (Appointments)Session["AppointmentStep1"];
            var appointmentStep2 = (Appointments)Session["AppointmentStep2"];

            if (appointmentStep1 != null && appointmentStep2 != null)
            {
                string templatePath = Server.MapPath("~/Content/template/newbooking.html");
                string content = System.IO.File.ReadAllText(templatePath);

                content = content.Replace("{{SendMail}}", DateTime.Now.ToString("dd/MM/yyyy-hh:mm"));
                content = content.Replace("{{Number}}", appointmentStep1.AppointmentId.ToString());

                var user = _users.AspNetUsers.Find(appointmentStep1.UserId);
                content = content.Replace("{{Name}}", user?.UserName);

                var service = _context.Services.Find(appointmentStep1.BarberId);
                content = content.Replace("{{DichVu}}", service.ServiceName.ToString());

                var barber = _context.Barbers.Find(appointmentStep1.BarberId);
                content = content.Replace("{{Barber}}", barber?.BarberName);

                content = content.Replace("{{NgayBatDau}}", appointmentStep1.StartDate?.ToString("dd/MM/yyyy"));
                content = content.Replace("{{GioBatDau}}", appointmentStep2.StartTime.Value.ToString("hh\\:mm"));

                var seat = _context.Seats.Find(appointmentStep2.SeatId);
                content = content.Replace("{{Ghe}}", seat?.SeatNumber.ToString());

                content = content.Replace("{{GhiChu}}", appointmentStep2.Note);

                MailHelper mailHelper = new MailHelper();
                mailHelper.SendMail(email, "Thông tin lịch hẹn", content);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}