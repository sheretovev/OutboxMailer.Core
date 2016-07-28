using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Iquality.Shared.OutboxMailer.Core.Models
{
    public class OutboxContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbDir = @"c:\temp\OuboxMailer\";
            if (!Directory.Exists(dbDir))
                Directory.CreateDirectory(dbDir);
            var dbPath = Path.Combine(dbDir, @"SQLiteData.db");
            optionsBuilder.UseSqlite("Data Source=" + dbPath);
        }

        public OutboxContext()  { }
        public void Init()
        {
            Database.Migrate();
        }

        public DbSet<OutboxMessage> Messages { get; set; }

        public static T RunInDb<T>(Func<DbContext, T> func)
        {
            T result = default(T);
            try
            {
                using (var db = new OutboxContext())
                {
                    result = func(db);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var logger = DependencyResolver.Services.GetService<ILogger>();
                logger.LogError("DB Error", ex);
            }
            return result;
        }

        public static void RunInDb(Action<DbContext> func)
        {
            try
            {
                using (var db = new OutboxContext())
                {
                    func(db);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var logger = DependencyResolver.Services.GetService<ILogger>();
                logger.LogError("DB Error", ex);
            }
        }
    }
}
