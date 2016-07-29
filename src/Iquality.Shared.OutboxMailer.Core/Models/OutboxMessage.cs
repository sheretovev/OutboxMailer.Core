using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Iquality.Shared.OutboxMailer.Core.Models
{
    public class OutboxMessage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid OutboxMessageId { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public List<Attachment> Attachments { get; set; }
    }
}
