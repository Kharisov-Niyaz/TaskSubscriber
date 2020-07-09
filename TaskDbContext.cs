using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskSubscriber
{
    class TaskDbContext : DbContext
    {
        public DbSet<Packet> packets { get; set; }
        public TaskDbContext()
        {            
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server =.\SQLEXPRESS02; Database = taskDbSubscriber; Trusted_Connection = True;");
            optionsBuilder.UseSqlite("Data Source=taskDbSubscriber.db");
        }
    }
}
