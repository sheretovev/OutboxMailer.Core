using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Script.Serialization;

namespace ServiceCallExampleFw4
{
    class Program
    {
        static void Main(string[] args)
        {            
            new Program().Send("to@mailinator.com", "cc@mailinator.com", "bcc@mailinator.com", "from@mailinator.com", "subject", "bodytext", null);
        }

        public class OutboxMessage
        {
            public string FromAddress { get; set; }
            public string ToAddress { get; set; }
            public string CcAddress { get; set; }
            public string BccAddress { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
        }

        // TODO: process cc, bcc, attachments.        
        public bool Send(string to, string cc, string bcc, string from, string subject, string body, ICollection<Attachment> attachmens)
        {
            var url = "http://localhost:5000/api/outboxmessages";
            return SendPutRequest(url, new OutboxMessage
            {
                FromAddress = from,
                ToAddress = to,
                CcAddress = cc,
                BccAddress = bcc,
                Subject = subject,
                Body = body
            });
        }

        private static bool SendPutRequest(string url, OutboxMessage bodyMessage)
        {
            // Create a request using a URL that can receive a post. 
            var request = WebRequest.Create(url);
            var postData = new JavaScriptSerializer().Serialize(bodyMessage);
            var byteArray = Encoding.UTF8.GetBytes(postData);

            request.Method = "PUT";
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            var response = request.GetResponse();
            // Open the stream using a StreamReader for easy access.
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var responseFromServer = reader.ReadToEnd();
                // Clean up the streams.
                reader.Close();
                response.Close();
            }
            return true;
        }
    }
}
