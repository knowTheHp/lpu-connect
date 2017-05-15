using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace Connect.Models.Repository {
    public class EmailVerification {
         public static void SendEmail(long userId,string toEmail, string userName, string uid) {
            try {
                //From Address 
                string FromAddress = "lpuconnects@gmail.com";
                string FromAdressTitle = "LpuConnect";
                //To Address 
                string ToAddress = toEmail;
                string ToAdressTitle = userName + " " + "(" + toEmail + ")";
                string Subject = "[LpuConnect] Verify your email address.";
                string BodyContent = "Hi " + userName + "<br/>" + "Help us secure your LpuConnect account by verifying your email address" + "(" + ToAddress + ")" + ".<br/><br/>" + "<a href=http://localhost:9101/Account/VerifyEmail/"+userId+"?uid=" + uid + "/>Verify email address</a><br/><br/><br/>" + "You’re receiving this email because you recently created a new LpuConnect account or added a new email address. If this wasn’t you, please ignore this email.";

                //Smtp Server 
                string SmtpServer = "smtp.gmail.com";
                //Smtp Port Number 
                int SmtpPortNumber = 587;

                var mimeMessage = new MimeMessage();
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
            } catch (Exception ex) {
                //
            }
        }
    }
}