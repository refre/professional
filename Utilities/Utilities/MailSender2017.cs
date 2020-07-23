using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ista.Utilities
{
    public class MailSender2017
    {
        private const string _smtpHost = "be";
        private const int _smtpPort = 25;
        private const string _smtpUser = "notification";
        private const string _smtpPassword = "*****";
        private const string _smtpDefaultSender = "notification@xxxx.be";
        private string _messageBody;
        private string _to;

        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpDefaultSender { get; set; }
        public string Subject { get; set; }


        public MailSender2017(string to, string subject, string messagebody)
        {
            SmtpHost = _smtpHost;
            SmtpPort = _smtpPort;
            SmtpUser = _smtpUser;
            SmtpPassword = _smtpPassword;
            SmtpDefaultSender = _smtpDefaultSender;
            Subject = subject;
            _messageBody = messagebody;
            _to = to;

        }
        public MailSender2017(string smtpHost, int smtpPort, string smtpUser, string smtpPassword, string smtpDefaultSender, string to, string subject, string messagebody)
        {
            SmtpHost = smtpHost;
            SmtpPort = smtpPort;
            SmtpUser = smtpUser;
            SmtpPassword = smtpPassword;
            SmtpDefaultSender = smtpDefaultSender;
            Subject = subject;
            _messageBody = messagebody;
            _to = to;
        }

        public void Send()
        {
            MailMessage msg = new MailMessage();
            HTMLHelper helper = new HTMLHelper();

            if (string.IsNullOrEmpty(_to))
                return;

            if (_to.Contains(";"))
            {
                var recipents = _to.Split(new char[] { ';' });
                foreach (var toData in recipents)
                {
                    msg.To.Add(toData);
                }
            }
            else
                msg.To.Add(_to);


            msg.From = new MailAddress(SmtpDefaultSender);

            string body = string.Empty;
            body = helper.TextToHTML(_messageBody);
            msg.IsBodyHtml = true;
            msg.Body = body;
            msg.Subject = Subject;

            SmtpClient mSmtpClient = new SmtpClient
            {
                Host = SmtpHost,
                Port = SmtpPort,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(SmtpUser, SmtpPassword)
            };
            // On the new server security does not allow default credential usage
            mSmtpClient.Send(msg);
        }
    }
}
