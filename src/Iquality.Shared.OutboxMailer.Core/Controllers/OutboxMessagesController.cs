using Microsoft.AspNetCore.Mvc;
using Iquality.Shared.OutboxMailer.Core.Models;
using System.Collections.Generic;
using Iquality.Shared.OutboxMailer.Core.Mailer;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Iquality.Shared.OutboxMailer.Core.Controllers
{
    [Route("api/[controller]")]
    public class OutboxMessagesController : Controller
    {        
        private readonly ILogger _logger;

        public OutboxMessagesController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<OutboxMessagesController>();            
        }

        // GET api/ouboxmessages
        [HttpGet]
        public IEnumerable<OutboxMessage> Get()
        {            
            return OutboxContext.RunInDb(context => context.Set<OutboxMessage>().ToList());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public OutboxMessage Get(int id)
        {
            return new OutboxMessage { Body = "body", ToAddress = "to", FromAddress = "from", Subject = "title" };
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }

        // PUT api/ouboxmessages
       [HttpPut("{to}/{from}/{subject}")]
        public void Put(string to, string from, string subject, [FromBody]string body)
        {
            OutboxContext.RunInDb(context => context.Set<OutboxMessage>().Add(new OutboxMessage
            {
                Body = body,
                Subject = subject,
                FromAddress = from,
                ToAddress = to,
                CreatedDate = DateTime.UtcNow
            }));                       
        }
        
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
