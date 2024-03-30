using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;
namespace DoAnCoSo.Areas.Admin.Controllers
{
    public class ConversationController : Controller
    {
        private AspNetUserRoleModel dbContext = new AspNetUserRoleModel();

        // Action để hiển thị danh sách các cuộc trò chuyện
        public ActionResult Index()
        {
            var conversations = dbContext.Messages.ToList();
            return View(conversations);
        }

        // Action để trả lời tin nhắn từ một người dùng cụ thể
        public ActionResult Reply(int messageId)
        {
            var message = dbContext.Messages.FirstOrDefault(m => m.Id == messageId);
            if (message == null)
            {
                return HttpNotFound();
            }

            return View(message);
        }


        [HttpPost]
        public ActionResult Reply(Message message)
        {
            if (ModelState.IsValid)
            {
                using (var dbContext = new AspNetUserRoleModel())
                {
                    var existingMessage = dbContext.Messages.Find(message.Id);
                    if (existingMessage != null)
                    {
                        existingMessage.ReplyMessage = message.ReplyMessage;
                        dbContext.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }

            // Nếu dữ liệu không hợp lệ, quay trở lại view "Reply" với model để hiển thị thông báo lỗi
            return View(message);
        }

    }
}