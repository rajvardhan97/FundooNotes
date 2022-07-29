using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Model
{
    public class MSMQmodel
    {
        MessageQueue messgaeQueue = new MessageQueue();

        public void sendData2Queue(string Token)
        {
            messgaeQueue.Path = @".\private$\Token";
            if(!MessageQueue.Exists(messgaeQueue.Path))
            {
                MessageQueue.Create(messgaeQueue.Path);
            }

            messgaeQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            messgaeQueue.ReceiveCompleted += MessgaeQueue_ReceiveCompleted;
            messgaeQueue.Send(Token);
            messgaeQueue.BeginReceive();
            messgaeQueue.Close();
        }

        private void MessgaeQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var msg = messgaeQueue.EndReceive(e.AsyncResult);
            string Token = msg.Body.ToString();
            string subject = "Fundoo Notes Reset Link";
            string Body = Token;

            var SMTP = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("rajvardhansingh2609@gmail.com", "xkjdxufujqonoyrf"),
                EnableSsl = true
            };
            SMTP.Send("rajvardhansingh2609@gmail.com", "rajvardhansingh2609@gmail.com", subject, Body);
            messgaeQueue.BeginReceive();

        }

      
    }
}
