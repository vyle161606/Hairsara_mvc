using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;

namespace DoAnCoSo.Areas.Admin.Controllers
{
    public class BarbersController : Controller
    {
        private BookingModelContext db = new BookingModelContext();

        // GET: Admin/Barbers
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Barbers.ToList());
        }
        [Authorize(Roles = "Admin")]
        // GET: Admin/Barbers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Barbers barbers = db.Barbers.Find(id);
            if (barbers == null)
            {
                return HttpNotFound();
            }
            return View(barbers);
        }
        [Authorize(Roles = "Admin")]
        // GET: Admin/Barbers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Barbers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BarberId,BarberName,Image,Description")] Barbers barbers, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/ListImages/Barbers"), fileName);
                    imageFile.SaveAs(filePath);

                    barbers.Image = fileName; // Lưu đường dẫn ảnh trong đối tượng Barbers
                }

                db.Barbers.Add(barbers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(barbers);
        }

        [Authorize(Roles = "Admin")]
        // GET: Admin/Barbers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Barbers barbers = db.Barbers.Find(id);
            if (barbers == null)
            {
                return HttpNotFound();
            }
            return View(barbers);
        }

        // POST: Admin/Barbers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BarberId,BarberName,Image,Description")] Barbers barbers, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/ListImages/Barbers"), fileName);
                    imageFile.SaveAs(filePath);

                    barbers.Image = fileName; // Lưu đường dẫn ảnh mới trong đối tượng Barbers
                }

                db.Entry(barbers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(barbers);
        }

        [Authorize(Roles = "Admin")]
        // GET: Admin/Barbers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Barbers barbers = db.Barbers.Find(id);
            if (barbers == null)
            {
                return HttpNotFound();
            }
            return View(barbers);
        }

        // POST: Admin/Barbers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Barbers barbers = db.Barbers.Find(id);
            if (barbers == null)
            {
                return HttpNotFound();
            }
            db.Barbers.Remove(barbers);
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
