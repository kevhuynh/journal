using System;
using System.Collections.Generic;
using Journal.Models;
using Microsoft.EntityFrameworkCore;

namespace Journal.Data
{
    public class JournalDb : JournalData
    {
        private JournalContext context;

        public JournalDb(JournalContext context)
        {
            this.context = context;
        }

        public JournalEntry AddJournalEntry(string title, string body, int userId)
        {
            JournalEntry newEntry = new JournalEntry()
            {
                Date = DateTime.Now,
                Title = title,
                Body = body,
                UserId = userId
            };

            context.JournalEntries.Add(newEntry);
            context.SaveChanges();

            return newEntry;
        }

        public User? FindUser(string username)
        {
            User? user = context.Users
                .Where(s => s.name == username)
                .AsNoTracking()
                .FirstOrDefault();

            return user;
        }

        public User? AddUser(string username)
        {
            List<User> users = context.Users
                .Where(s => s.name == username)
                .ToList();

            if (users.Count > 0)
            {
                return null;
            }

            User newUser = new User()
            {
                name = username
            };

            context.Users.Add(newUser);
            context.SaveChanges();

            return newUser;
        }


        public List<JournalEntry> GetAllEntries()
        {
            return context.JournalEntries
                .AsNoTracking()
                .ToList();
        }

        public List<JournalEntry> FilterEntries(FilterField field, string phrase)
        {
            switch (field)
            {
                case FilterField.Title:
                    return context.JournalEntries
                        .Where(s => s.Title.Contains(phrase))
                        .AsNoTracking()
                        .ToList();
                case FilterField.Body:
                    return context.JournalEntries
                        .Where(s => s.Body.Contains(phrase))
                        .AsNoTracking()
                        .ToList();
                default:
                    return context.JournalEntries
                        .Where(s => s.Body.Contains(phrase) || s.Title.Contains(phrase))
                        .AsNoTracking()
                        .ToList();
            }
        }

        public List<JournalEntry> SortEntries(SortField sortField, SortOrder order)
        {
            DbSet<JournalEntry> dbSetEntries = context.JournalEntries;
            IQueryable<JournalEntry> queryJE;

            if (order == SortOrder.Desc)
            {
                switch (sortField)
                {
                    case SortField.Title:
                        queryJE = dbSetEntries.OrderByDescending(s => s.Title);
                        break;
                    case SortField.Body:
                        queryJE = dbSetEntries.OrderByDescending(s => s.Body);
                        break;
                    default:
                        queryJE = dbSetEntries.OrderByDescending(s => s.Date);
                        break;
                }
            }
            else
            {
                switch (sortField)
                {
                    case SortField.Title:
                        queryJE = dbSetEntries.OrderBy(s => s.Title);
                        break;
                    case SortField.Body:
                        queryJE = dbSetEntries.OrderBy(s => s.Body);
                        break;
                    default:
                        queryJE = dbSetEntries.OrderBy(s => s.Date);
                        break;
                }
            }

            return queryJE.AsNoTracking().ToList();
        }

        public List<JournalEntry> GetByDateTime(DateGetOptions dateOptions, DateTime dateTime)
        {
            DbSet<JournalEntry> dbSetEntries = context.JournalEntries;
            IQueryable<JournalEntry> queryJE;

            switch (dateOptions)
            {
                case DateGetOptions.DayHourMinute:
                    queryJE = dbSetEntries.Where(s => s.Date.Date == dateTime.Date
                    && s.Date.Hour == dateTime.Hour
                    && s.Date.Minute == dateTime.Minute);
                    break;
                case DateGetOptions.Before:
                    queryJE = dbSetEntries.Where(s => s.Date < dateTime);
                    break;
                default:
                    queryJE = dbSetEntries.Where(s => s.Date > dateTime);
                    break;
            }

            return queryJE.AsNoTracking().ToList();
        }
    }
}