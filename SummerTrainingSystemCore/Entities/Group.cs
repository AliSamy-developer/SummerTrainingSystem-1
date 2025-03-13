using System.Collections.Generic;

namespace SummerTrainingSystemCore.Entities
{
    public class Group
    {
        public Group()
        {
            Messages = new List<Message>();
            Notifications = new List<Notification>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Message> Messages { get; set; }
        public List<Notification> Notifications { get; set; }

    }
}
