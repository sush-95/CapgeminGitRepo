using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FadvTestingApp
{
    public class Email_Processor
    {
        #region Private Variables

        MailMessage message;
        SmtpClient client;

        string imap; string userId; string password; string mailBox; string CCMailIDs;   string fromMailid; string toMailID;
        string hostName; string networkuname; string networkpassword; string Port; 
       
        #endregion
        public Email_Processor()
        {
            this.imap = (ConfigurationManager.AppSettings["IMAP"]).ToString();
            this.userId = (ConfigurationManager.AppSettings["UserID"]).ToString();
            this.password = ConfigurationManager.AppSettings["Password"];
            this.mailBox = ConfigurationManager.AppSettings["MailBox"];
            this.fromMailid = ConfigurationManager.AppSettings["FromMailID"];
            this.toMailID = ConfigurationManager.AppSettings["ToMailID"];
            this.CCMailIDs = ConfigurationManager.AppSettings["CCMailIDs"];
            this.hostName = ConfigurationManager.AppSettings["HostName"];
            this.networkuname = ConfigurationManager.AppSettings["NetworkUserName"];
            this.networkpassword = ConfigurationManager.AppSettings["NetworkPassword"];           
            this.Port = ConfigurationManager.AppSettings["Port"];
        }
        public void SendMail(string attachmentFilename,string subject,string body)
        {
            try
            {
                client = new SmtpClient(hostName, Convert.ToInt32(Port));
                message = new MailMessage(this.fromMailid, this.toMailID, subject, body);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(this.fromMailid, this.password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                message.From = new MailAddress(fromMailid);
                message.Subject = "Test";
                message.Body = "Body";

                if (!string.IsNullOrEmpty(attachmentFilename.Trim()))
                    message.Attachments.Add(new Attachment(attachmentFilename));

                client.Send(message);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
        }

    }
}
