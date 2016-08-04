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
       [HttpPut("")]
        public void Put([FromBody]OutboxMessage body)
        {
            if (body == null) throw new ArgumentNullException($"{nameof(OutboxMessage)} was not provided from a body. Please use JSON format to provide valid object.");
            body.CreatedDate = DateTime.UtcNow;
            OutboxContext.RunInDb(context => context.Set<OutboxMessage>().Add(body));                       
        }
        
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
