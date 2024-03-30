using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;
using PagedList.Mvc;
using PagedList;

namespace DoAnCoSo.Controllers
{
    public class StylesController : Controller
    {
        // GET: Styles
        public ActionResult Index(int? page)
        {
            var context = new BookingModelContext();
            var styles = context.Styles.ToList();

            int pageNumber = page ?? 1; // Số trang hiện tại, mặc định là 1
            int pageSize = 3; // Số mục trên mỗi trang

            // Sử dụng PagedList để phân trang danh sách styles
            IPagedList<Styles> pagedStyles = styles.ToPagedList(pageNumber, pageSize);

            // Tạo danh sách số trang
            var pageNumbers = Enumerable.Range(1, pagedStyles.PageCount);

            // Truyền danh sách số trang vào ViewBag
            ViewBag.PageNumbers = pageNumbers;

            return View(pagedStyles);
        }
    }
}
