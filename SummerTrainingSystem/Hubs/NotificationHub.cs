using Microsoft.AspNetCore.SignalR;
using SummerTrainingSystemCore.Entities;
using SummerTrainingSystemEF.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SummerTrainingSystem.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public NotificationHub(ApplicationDbContext context)
        {
            _context = context;
        }
        public override Task OnConnectedAsync()
        {
            var currentGroup = _context.Groups.FirstOrDefault(c => c.Name == Context.User.Identity.Name);
            if (currentGroup == null)
            {
                Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
                var group = new Group
                {
                    Name = Context.User.Identity.Name
                };
                _context.Groups.Add(group);
                _context.SaveChanges();
            }
            else
            {
                Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            }
            return base.OnConnectedAsync();
        }

        public Task SendNotificationToGroup(string receiver, string content)
        {
            // send message to specific group
            var group = _context.Groups.FirstOrDefault(g => g.Name == receiver);
            if (group != null)
            {
                var notificationToAdd = new Notification
                {
                    Content = content,
                    GroupId = group.Id,
                    SenderEmail = Context.User.Identity.Name,
                    ReceiverEmail = receiver,
                    When = DateTime.Now
                };
                group.Notifications.Add(notificationToAdd);
                _context.SaveChanges();
            }
            Clients.Group(Context.User.Identity.Name).SendAsync("ReceivePrivateNotification", Context.User.Identity.Name, content, DateTime.Now.ToString());
            Clients.Group(receiver).SendAsync("ReceivePrivateNotification", Context.User.Identity.Name, content, DateTime.Now.ToString());

            return Task.CompletedTask;
        }
    }
}
