using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace ista.Utilities
{
    /// <summary>
    /// This Class is used to send email with ISTA configuration.
    /// </summary>
    public class MailSender
    {
        /// <summary>
        /// Private variable: default smpt host
        /// </summary>
        private const string _defautSmptHost = "be.root.net";
        /// <summary>
        /// Private variable: list of recipiant.
        /// </summary>
        private string _toListInput;
        /// <summary>
        /// Private variable: list of recipiant in copy.
        /// </summary>
        private string _cCListInput;
        /// <summary>
        /// Gets or sets the list of recipiant.
        /// </summary>
        public List<string> _toList;
        /// <summary>
        /// Gets or sets the list of recipiant in copy.
        /// </summary>
        public List<string> _cCList;
        /// <summary>
        /// Gets or sets
        /// </summary>
        public bool HTML { get; set; }
        /// <summary>
        /// Gets or sets whether the email's got an high importance.
        /// </summary>
        public bool High { get; set; }
        /// <summary>
        /// Gets or sets whether the email's got an low importance.
        /// </summary>
        public bool Low { get; set; }
        /// <summary>
        /// Gets or sets the body of the message.
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Gets or sets whether the subject of the message.
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Gets or sets the SMTPHost for the mail.
        /// </summary>
        public string SMTPHost { get; set; }
        /// <summary>
        /// Gets or sets the SMTPPort for the mail.
        /// </summary>
        public int? SMTPPort { get; set; }
        /// <summary>
        /// Gets or sets who send the email.
        /// </summary>
        public string From { get; set; }
        private List<string> _columName;
        private List<List<string>> _dataLine;
        private string _headerMessage;
        private string _endMessage;
        private string _messagebody;

        // The interface offer 2 way to add Recipient/CC, either by using AddRecipient/AddCC or directly
        // pass a semi-colon separated list of email address using ToList/CCList properties.
        /// <summary>
        /// Sets the list of recipiants.
        /// </summary>
        public string ToList
        {
            set
            {
                _toListInput = value;
                string[] Recipients;
                Recipients = _toListInput.Split(';');
                foreach (string Recipient in Recipients)
                {
                    AddRecipient(Recipient);
                }
            }
        }
        /// <summary>
        /// Sets the list of recipiants in copy.
        /// </summary>
        public string CCList
        {
            set
            {
                _cCListInput = value;
                string[] Recipients;
                Recipients = _cCListInput.Split(';');
                foreach (string Recipient in Recipients)
                {
                    AddCC(Recipient);
                }
            }
        }
        /// <summary>
        /// Constructor(s)
        /// </summary>
        public MailSender()
        {
            _toList = new List<string>();
            _cCList = new List<string>();
        }
        /// <summary>
        /// Constructor(s), just invoke send method.
        /// </summary>
        /// <param name="to">list of recipiant. this list is semi colon (;) separated.</param>
        /// <param name="cc">list of recipiant in copy. this list is semi colon (;) separated.</param>
        /// <param name="subject">subject of the email</param>
        /// <param name="messagebody">Message</param>
        public MailSender(string to, string cc, string subject, string messagebody)
            : this()
        {
            AddRecipient(to);
            AddCC(cc);
            Subject = subject;
            _messagebody = messagebody;
        }
        public MailSender(string to, string cc, string subject, string headerMessage, string endMessage,List<string> columName,List<List<string>> dataLine):this()
        {
            AddRecipient(to);
            AddCC(cc);
            Subject        = subject;
            _columName     = columName;
            _dataLine      = dataLine;
            _headerMessage = headerMessage;
            _endMessage    = endMessage;
        }


        /// <summary>
        /// This method add recipient to the list
        /// </summary>
        /// <param name="sAddress"></param>
        public void AddRecipient(string sAddress)
        {
            _toList.Add(sAddress);
        }
        /// <summary>
        /// This method add recipient in copy.
        /// </summary>
        /// <param name="sAddress"></param>
        public void AddCC(string sAddress)
        {
            _cCList.Add(sAddress);
        }
        /// <summary>
        /// Send the email
        /// </summary>
        /// <param name="columnName">Name of the columns for the table header</param>
        /// <param name="data">Data into the header.</param>
        /// <param name="footer">Add a footer at the end of the message.</param>
        public void Send(bool footer = true)
        {
            MailMessage message = new MailMessage();

            // Remove path
            string version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            string exeFileFullName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string exeFileName = exeFileFullName;
            if (exeFileName.Contains("\\"))
            {
                exeFileName = exeFileName.Substring(exeFileName.LastIndexOf("\\") + 1);
            }
            // Remove white spaces
            exeFileName = exeFileName.Trim();

            if (From != null && From != "")
            {
                message.From = new MailAddress(From);
            }
            else
            {
                // When the send is not defined, retrieve the name of the executable and add "@ista.be"
                message.From = new MailAddress(exeFileName.Trim().Replace(' ', '_') + "@ista.be");
            }
            foreach (string address in _toList)
            {
                if (address != "")
                {
                    message.To.Add(address);
                }
            }
            foreach (string address in _cCList)
            {
                if (address != "")
                {
                    message.CC.Add(address);
                }
            }
            message.Subject = Subject;

            if (High)
            {
                message.Priority = MailPriority.High;
            }
            else if (Low)
            {
                message.Priority = MailPriority.Low;
            }

            string body = string.Empty;
            // Add a standard footer
            HTMLHelper helper = new HTMLHelper();

            if (_messagebody != null)
            {
                body = helper.TextToHTML(_messagebody);
            }
            else
            {
                StringBuilder htmlbody = new StringBuilder();
                htmlbody.Append(helper.HtmlText(_headerMessage));
                htmlbody.Append(helper.TableHeader(_columName));
                htmlbody.Append(helper.TableData(_dataLine));
                htmlbody.Append(helper.TableFooter());
                htmlbody.Append(helper.HtmlEnd(_endMessage));

                if (footer)
                {
                    htmlbody.Append(string.Format("<p><i>Generated on {0} by {1} <br/>({2} - V{3})</i></p>", System.Environment.MachineName, Environment.UserName, exeFileFullName, version));
                }

                body = htmlbody.ToString();
            }

            message.Body = body;

            message.IsBodyHtml = true;

            // Instantiate a new instance of SmtpClient
            SmtpClient mSmtpClient            = new SmtpClient();
            mSmtpClient.Host                  = SMTPHost ?? _defautSmptHost;
            mSmtpClient.Port                  = SMTPPort ?? mSmtpClient.Port;
            mSmtpClient.UseDefaultCredentials = true;

            // Send the mail message
            mSmtpClient.Send(message);
        }
        /// <summary>
        /// Send the email
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="footer">Add a footer at the end of the message.</param>
        public void Send(string appName, bool footer = true)
        {
            MailMessage message = new MailMessage();

            string exeFileName = appName;
            if (exeFileName.Contains("\\"))
            {
                exeFileName = exeFileName.Substring(exeFileName.LastIndexOf("\\") + 1);
            }
            // Remove white spaces
            exeFileName = exeFileName.Trim();

            if (From != null && From != "")
            {
                message.From = new MailAddress(From);
            }
            else
            {
                // When the send is not defined, retrieve the name of the executable and add "@ista.be"
                message.From = new MailAddress(exeFileName.Trim().Replace(' ', '_') + "@ista.be");
            }
            foreach (string address in _toList)
            {
                if (address != "")
                {
                    message.To.Add(address);
                }
            }
            foreach (string address in _cCList)
            {
                if (address != "")
                {
                    message.CC.Add(address);
                }
            }
            message.Subject = Subject;

            if (High)
            {
                message.Priority = MailPriority.High;
            }
            else if (Low)
            {
                message.Priority = MailPriority.Low;
            }

            string body = string.Empty;
            // Add a standard footer
            HTMLHelper helper = new HTMLHelper();

            if (_messagebody != null)
            {
                body = helper.TextToHTML(_messagebody);
            }
            else
            {
                StringBuilder htmlbody = new StringBuilder();
                htmlbody.Append(helper.HtmlText(_headerMessage));
                htmlbody.Append(helper.TableHeader(_columName));
                htmlbody.Append(helper.TableData(_dataLine));
                htmlbody.Append(helper.TableFooter());
                htmlbody.Append(helper.HtmlEnd(_endMessage));

                if (footer)
                {
                    htmlbody.Append(string.Format("<p><i>Generated on {0} by {1} <br/>({2} - V{3})</i></p>", System.Environment.MachineName, Environment.UserName, appName));
                }

                body = htmlbody.ToString();
            }

            message.Body = body;

            message.IsBodyHtml = true;

            // Instantiate a new instance of SmtpClient
            SmtpClient mSmtpClient = new SmtpClient();
            mSmtpClient.Host = SMTPHost ?? _defautSmptHost;
            mSmtpClient.Port = SMTPPort ?? mSmtpClient.Port;
            mSmtpClient.UseDefaultCredentials = true;

            // Send the mail message
            mSmtpClient.Send(message);
        }
        /// <summary>
        /// Send the email
        /// </summary>
        /// <param name="noFooter">Add a footer at the end of the message.</param>
        //public void Send(Boolean noFooter = false)
        //{
        //    MailMessage message = new MailMessage();

        //    // Remove path
        //    string version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
        //    string exeFileFullName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        //    string exeFileName = exeFileFullName;
        //    if (exeFileName.Contains("\\"))
        //    {
        //        exeFileName = exeFileName.Substring(exeFileName.LastIndexOf("\\") + 1);
        //    }
        //    // Remove white spaces
        //    exeFileName = exeFileName.Trim();

        //    if (From != null && From != "")
        //    {
        //        message.From = new MailAddress(From);
        //    }
        //    else
        //    {
        //        // When the send is not defined, retrieve the name of the executable and add "@ista.be"
        //        message.From = new MailAddress(exeFileName.Trim().Replace(' ', '_') + "@ista.be");
        //    }
        //    foreach (string address in _toList)
        //    {
        //        if (address != "")
        //        {
        //            message.To.Add(address);
        //        }
        //    }
        //    foreach (string address in _cCList)
        //    {
        //        if (address != "")
        //        {
        //            message.CC.Add(address);
        //        }
        //    }
        //    message.Body = Body;
        //    message.Subject = Subject;

        //    if (High)
        //    {
        //        message.Priority = MailPriority.High;
        //    }
        //    else if (Low)
        //    {
        //        message.Priority = MailPriority.Low;
        //    }

        //    HTMLHelper helper = new HTMLHelper();

        //    Body = helper.TextToHTML(Body);

        //    // Add a standard footer
        //    if (!noFooter)
        //    {
        //        Body += string.Format("<p><i>Generated on {0} by {1} <br/>({2} - V{3})</i></p>", System.Environment.MachineName, Environment.UserName, exeFileFullName, version);
        //    }

        //    message.Body = Body;

        //    message.IsBodyHtml = true;

        //    // Instantiate a new instance of SmtpClient
        //    SmtpClient mSmtpClient = new SmtpClient();
        //    mSmtpClient.Host = SMTPHost ?? _defautSmptHost;
        //    mSmtpClient.Port = SMTPPort ?? mSmtpClient.Port;
        //    mSmtpClient.UseDefaultCredentials = true;

        //    // Send the mail message
        //    mSmtpClient.Send(message);

        //}
    }
}
