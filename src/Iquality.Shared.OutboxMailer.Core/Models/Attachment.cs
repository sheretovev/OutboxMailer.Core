using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iquality.Shared.OutboxMailer.Core.Models
{
    public class Attachment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid AttachmentId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public Guid OutboxMessageId { get; set; }
        public OutboxMessage OutboxMessages { get; set; }
    }
}
