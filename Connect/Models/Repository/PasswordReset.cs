using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Connect.Models.Repository {
    public class PasswordReset {
        #region Send password reset link
      public  static void SendPasswordResetEmail(string toEmail, string userName, string uid) {
            //From Address 
            string FromAddress = "lpuconnects@gmail.com";
            string FromAdressTitle = "LpuConnect";

            //To Address 
            string ToAddress = toEmail;
            string ToAdressTitle = userName + " " + "(" + toEmail + ")";
            string Subject = "[LpuConnect] Please reset your password";
            string BodyContent = "We heard that you lost your LPU Connect password. Sorry about that!<br/>" + "But don’t worry! You can use the following link within the next day to reset your password:<br/><br/>" + "<a href=http://localhost:8073/LpuConnect/Reset/ChangePassword.aspx?uid=" + uid + ">Reset</a>" + "<br/>If you don’t use this link within 24 hours, it will expire.<br/><br/><br/>" + "Thanks,<br/>" + "Your friends at LpuConnect";

            //Smtp Server 
            string SmtpServer = "smtp.gmail.com";
            //Smtp Port Number 
            int SmtpPortNumber = 587;

            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
            mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, ToAddress));
            mimeMessage.Subject = Subject;
            mimeMessage.Body = new TextPart("html") {
                Text = BodyContent
            };
            using (var client = new SmtpClient()) {
                client.Connect(SmtpServer, SmtpPortNumber, false);
                client.Authenticate("lpuconnects@gmail.com", "Lpu@123@");
                client.Send(mimeMessage);
                client.Disconnect(true);
            }
        }
        #endregion
    }
}