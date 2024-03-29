using System;
namespace Journal.Models
{
    public class JournalEntry
    {
        public int id { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; } = new User();
    }
}

