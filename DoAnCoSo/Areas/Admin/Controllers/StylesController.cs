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
    public class StylesController : Controller
    {
        private BookingModelContext db = new BookingModelContext();
        [Authorize(Roles = "Admin")]
        // GET: Admin/Styles
        public ActionResult Index()
        {
            return View(db.Styles.ToList());
        }
        [Authorize(Roles = "Admin")]
        // GET: Admin/Styles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Styles styles = db.Styles.Find(id);
            if (styles == null)
            {
                return HttpNotFound();
            }
            return View(styles);
        }
        [Authorize(Roles = "Admin")]
        // GET: Admin/Styles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Styles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StyleId,StyleName,Image,Description,Price")] Styles styles, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/ListImages/Styles"), fileName);
                    imageFile.SaveAs(filePath);

                    styles.Image = fileName; // Lưu đường dẫn ảnh trong đối tượng Barbers
                }
                db.Styles.Add(styles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(styles);
        }
        [Authorize(Roles = "Admin")]
        // GET: Admin/Styles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Styles styles = db.Styles.Find(id);
            if (styles == null)
            {
                return HttpNotFound();
            }
            return View(styles);
        }

        // POST: Admin/Styles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StyleId,StyleName,Image,Description,Price")] Styles styles, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/ListImages/Styles"), fileName);
                    imageFile.SaveAs(filePath);

                    styles.Image = fileName; // Lưu đường dẫn ảnh trong đối tượng Barbers
                }
                db.Entry(styles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(styles);
        }
        [Authorize(Roles = "Admin")]
        // GET: Admin/Styles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Styles styles = db.Styles.Find(id);
            if (styles == null)
            {
                return HttpNotFound();
            }
            return View(styles);
        }

        // POST: Admin/Styles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Styles styles = db.Styles.Find(id);
            db.Styles.Remove(styles);
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
