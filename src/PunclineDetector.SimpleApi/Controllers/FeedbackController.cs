using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;


namespace PunclineDetector.SimpleApi.Controllers
{
    public class FeedbackController : ApiController
    {
        public void Post([FromBody]string id)
        {
            var message = new MailMessage("punchline@jeppek.dk","jeppe@jeppek.dk");

            message.Subject = "Feedback";
            message.Body = id;

            var client = new SmtpClient();
            client.EnableSsl = true;
            client.Send(message);
        }
    }
}