using System;
using Journal.Models;

namespace Journal.Data
{
    public interface JournalData
    {
        public JournalEntry AddJournalEntry(string title, string body, int userId);

        public User? AddUser(string username);

        public User? FindUser(string username);

        public List<JournalEntry> GetAllEntries();

        public List<JournalEntry> FilterEntries(FilterField field, string phrase);

        public List<JournalEntry> SortEntries(SortField sortField, SortOrder order);

        public List<JournalEntry> GetByDateTime(DateGetOptions dateOptions, DateTime dateTime);
    }
}
