using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;

namespace DoAnCoSo.Areas.Admin.Controllers
{
    public class AppointmentsController : Controller
    {
        private BookingModelContext db = new BookingModelContext();

        [Authorize(Roles = "Admin")]

        // GET: Admin/Appointments
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.Barbers).Include(a => a.Services).ToList();

            // Lặp qua danh sách appointments để gán UserName tương ứng với UserId
            foreach (var appointment in appointments)
            {
                var userId = appointment.UserId;
                var user = db.AspNetUsers.FirstOrDefault(u => u.Id == userId);
                var userName = user != null ? user.UserName : string.Empty;
                appointment.UserName = userName;
            }

            return View(appointments);
        }
        [Authorize(Roles = "Admin")]
        public JsonResult UpdateStatus(int appointmentId)
        {
            // Tìm đối tượng Appointment dựa trên appointmentId
            var appointment = db.Appointments.Find(appointmentId);

            if (appointment != null)
            {
                // Cập nhật trạng thái thành 2
                appointment.Status = 2;

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                return Json(new { result = "Success" });
            }

            return Json(new { result = "Error" });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult Complete(int appointmentId)
        {
            using (var db = new BookingModelContext())
            {
                var appointment = db.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
                if (appointment != null)
                {
                    appointment.Status = 3; // Cập nhật trạng thái thành 3
                    appointment.EndTime = DateTime.Now; // Cập nhật giá trị EndTime là ngày giờ hiện tại
                    db.SaveChanges();

                    // Trả về kết quả thành công và giá trị EndTime
                    return Json(new { result = "Success", endTime = appointment.EndTime?.ToString("dd/MM/yyyy-HH:mm") });
                }
                else
                {
                    // Xử lý khi không tìm thấy appointment trong cơ sở dữ liệu
                    return Json(new { result = "Error" });
                }
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: Admin/Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointments appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }
            return View(appointments);
        }

        // POST: Admin/Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointments appointments = db.Appointments.Find(id);
            db.Appointments.Remove(appointments);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
