using System;
using Journal.Models;
using Microsoft.EntityFrameworkCore;

namespace Journal.Data
{
    public class JournalContext : DbContext
    {
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("Server=localhost; Port=3306; Database=mylocaldatabase; User=root; Password=testpass;");
        }
    }
}

