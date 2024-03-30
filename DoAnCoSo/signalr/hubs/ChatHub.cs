using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAnCoSo.Models;
using System;

public class ChatHub : Hub
{
    public void Send(string name, string message, string phone)
    {
        using (var dbContext = new AspNetUserRoleModel())
        {
            // Tìm bản ghi tin nhắn hiện tại dựa trên người gửi và số điện thoại
            var existingMessage = dbContext.Messages
                .FirstOrDefault(m => m.Sender == name && m.Phone == phone);

            if (existingMessage != null)
            {
                // Cập nhật nội dung tin nhắn
                existingMessage.MessageText += ";" + message;
                existingMessage.SentTime = DateTime.Now; // Cập nhật thời gian gửi mới nhất

                dbContext.SaveChanges();
            }
            else
            {
                // Tạo bản ghi tin nhắn mới và lưu vào cơ sở dữ liệu
                var newMessage = new Message
                {
                    Sender = name,
                    MessageText = message,
                    SentTime = DateTime.Now,
                    Phone = phone
                };

                dbContext.Messages.Add(newMessage);
                dbContext.SaveChanges();
            }
        }

        // Gửi tin nhắn tới người dùng và Admin
        var connectionIds = GetUserConnectionIds(name);
        Clients.Clients(connectionIds).addNewMessageToPage(name, message);

        // Gửi tin nhắn tới người gửi (tự động cập nhật tin nhắn trên trang của người gửi)
        Clients.Caller.addNewMessageToPage(name, message);
    }



    private List<string> GetUserConnectionIds(string userName)
    {
        using (var dbContext = new AspNetUserRoleModel())
        {
            var user = dbContext.AspNetUsers.FirstOrDefault(u => u.UserName == userName);
            if (user != null)
            {
                var roleIds = user.AspNetUserRoles.Select(ur => ur.RoleId).ToList();
                var adminRole = dbContext.AspNetRoles.FirstOrDefault(r => r.Name == "Admin");
                if (adminRole != null)
                {
                    roleIds.Add(adminRole.Id);
                }

                var connectionIds = dbContext.AspNetUserRoles
                    .Where(ur => roleIds.Contains(ur.RoleId))
                    .Select(ur => ur.UserId)
                    .ToList();

                return connectionIds;
            }
        }

        return new List<string>();
    }
}
