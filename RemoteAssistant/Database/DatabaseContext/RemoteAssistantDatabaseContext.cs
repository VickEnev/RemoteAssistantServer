using Microsoft.EntityFrameworkCore;
using RemoteAssistantStorage.Models;
using System;

namespace RemoteAssistantStorage.DatabaseContext
{
    internal class RemoteAssistantDatabaseContext : DbContext
    {
        public DbSet<UserDevice> UserDevices   { get; set; }
        public DbSet<LogSystem> LogSystem      { get; set; }

        public RemoteAssistantDatabaseContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=remote_assistant_database.db");
        }
    }
}
