using Microsoft.AspNetCore.Mvc;
using Iquality.Shared.OutboxMailer.Core.Models;
using System.Collections.Generic;

namespace Iquality.Shared.OutboxMailer.Core.Controllers
{
    [Route("api/[controller]")]
    public class OutboxMessagesController : Controller
    {
        private OutboxContext _context = new OutboxContext();
        //public OutboxMessagesController(OutboxContext context)
        //{
        //    _context = context;
        //}
        public OutboxMessagesController()
        {
            //_context = context;
        }

        // GET api/ouboxmessages
        [HttpGet]
        public IEnumerable<OutboxMessage> Get()
        {
            return new [] { new OutboxMessage { Body = "body", ToAddress = "to", FromAddress = "from", Subject = "title" } ,
              new OutboxMessage { Body = "body", ToAddress = "to", FromAddress = "from", Subject = "title" }, 
              new OutboxMessage { Body = "body", ToAddress = "to", FromAddress = "from", Subject = "title" } };
            //return _context.Messages;
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
        //[HttpPut("{to}{from}{subject}")]
        //public void Put(string to, string from, string subject, [FromBody]string body)
        //{
        //    _context.Messages.Add(new OutboxMessage
        //    {
        //        Body = body,
        //        Subject = subject,
        //        FromAddress = from,
        //        ToAddress = to
        //    }
        //    );
        //}


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }


        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
