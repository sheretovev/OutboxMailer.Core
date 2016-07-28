using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Iquality.Shared.Data.Models
{
    public class OutboxContext : DbContext
    {
        public OutboxContext(DbContextOptions<OutboxContext> options)
            : base(options)
        { }

        public DbSet<OutboxMessage> Messages { get; set; }    
    }

    public class OutboxMessage
    {
        public int OutboxId { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public List<Attachment> Attachments { get; set; }
    }

    public class Attachment
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public OutboxMessage Message { get; set; }
    }
}
