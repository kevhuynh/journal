using System;
using Journal.Data;
using Journal.Models;


namespace Journal
{
    public class Application
    {
        JournalData data;

        //Use dependency injection
        public Application(JournalData data)
        {
            this.data = data;
        }

        public void CreateJournalEntry(string title, string body, string username)
        {
            User? user = data.FindUser(username);

            if (user == null)
            {
                //Two ways to think about this... and I went with option 2
                //1) Automatically create user if it doesn't exist. Might make things easier.
                //2) Do not create user. Could've been a typo and the user sees that.
                Console.WriteLine("Could not find user with that username. Please add user and then create journal entry");
                return;
            }

            JournalEntry entry = data.AddJournalEntry(title, body, user.id);

            Console.WriteLine("Journal entry created for \"" + title + "\" on " + entry.Date);
        }

        public void CreateUser(string username)
        {
            User? newUser = data.AddUser(username);
            if (newUser == null)
            {
                Console.WriteLine(username + " already taken. Please try a different username.");
            }
            else
            {
                Console.WriteLine(username + " successfully created");
            }
        }

        public void ListEntries()
        {
            List<JournalEntry> entries = data.GetAllEntries();

            if (entries.Count == 0)
            {
                Console.WriteLine("There are no journal entries");
            }

            foreach (JournalEntry entry in entries)
            {
                Console.WriteLine(entry.id + ", " + entry.Date + ", \"" + entry.Title + "\", \"" + entry.Body + "\"");
            }
        }

        public void FilterEntries(FilterField filter, string phrase)
        {
            List<JournalEntry> entries = data.FilterEntries(filter, phrase);

            if (entries.Count == 0)
            {
                Console.WriteLine("There are no journal entries that have a field with that phrase");
            }

            foreach (JournalEntry entry in entries)
            {
                Console.WriteLine(entry.id + ", " + entry.Date + ", \"" + entry.Title + "\", \"" + entry.Body + "\"");
            }
        }

        public void SortEntries(SortField sortField, SortOrder sortOrder)
        {
            List<JournalEntry> entries = data.SortEntries(sortField, sortOrder);

            if (entries.Count == 0)
            {
                Console.WriteLine("There are no journal entries.");
            }

            foreach (JournalEntry entry in entries)
            {
                Console.WriteLine(entry.id + ", " + entry.Date + ", \"" + entry.Title + "\", \"" + entry.Body + "\"");
            }
        }

        public void GetEntriesByDateTime(DateGetOptions dateOption, DateTime dateTime)
        {
            List<JournalEntry> entries = data.GetByDateTime(dateOption, dateTime);

            if (entries.Count == 0)
            {
                Console.WriteLine("There are no journal entries.");
            }

            foreach (JournalEntry entry in entries)
            {
                Console.WriteLine(entry.id + ", " + entry.Date + ", \"" + entry.Title + "\", \"" + entry.Body + "\"");
            }
        }
    }
}

