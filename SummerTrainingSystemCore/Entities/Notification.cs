using System;

namespace SummerTrainingSystemCore.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public DateTime When { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
