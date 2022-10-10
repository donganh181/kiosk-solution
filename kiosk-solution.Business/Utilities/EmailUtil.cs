using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using kiosk_solution.Data.Constants;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace kiosk_solution.Data.ViewModels
{
    public class EmailUtil
    {
        private static readonly string _email = "capstoneprojectfu2021@gmail.com";
        private static readonly string _pass = "capstone123";

        public static async Task SendEmail(string sendto, string subject, string content)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("capstoneprojectfu2021@gmail.com", "Kiosk-Platform");
            var to = new EmailAddress(sendto, sendto);
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = content;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            switch (response.StatusCode)
            {
                case HttpStatusCode.Accepted:
                case HttpStatusCode.OK:
                    Console.WriteLine("Send email success");
                    break;
            }
            var body = string.Empty;
            using (var reader = new StreamReader(response.Body.ReadAsStream()))
            {
                //Request.Body.Seek(0, SeekOrigin.Begin);
                //body = reader.ReadToEnd();
                body = reader.ReadToEnd();
            }

            Console.WriteLine(body);
        }

        // public static async Task SendEmail(string sendto, string subject, string content)
        // {
        //     //sendto: Email receiver (người nhận)
        //     //subject: Tiêu đề email
        //     //content: Nội dung của email, bạn có thể viết mã HTML
        //     //Nếu gửi email thành công, sẽ trả về kết quả: OK, không thành công sẽ trả về thông tin l�-i
        //     try
        //     {
        //         MailMessage mail = new MailMessage();
        //         SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        //
        //         mail.From = new MailAddress(_email);
        //         mail.To.Add(sendto);
        //         mail.Subject = subject;
        //         mail.IsBodyHtml = true;
        //         mail.Body = content;
        //
        //         smtpServer.Host = "smtp.gmail.com";
        //         smtpServer.Port = 587;
        //         smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        //         smtpServer.EnableSsl = true;
        //         smtpServer.UseDefaultCredentials = false;
        //         smtpServer.Credentials = new System.Net.NetworkCredential(_email, _pass);
        //
        //         await smtpServer.SendMailAsync(mail);
        //         Console.WriteLine("Email sent successfully");
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //         Console.WriteLine("Email sending failed");
        //     }
        // }

        public static string getCreateAccountContent(string email)
        {
            string result = EmailConstants.CREATE_ACCOUNT_CONTENT_BASE.Replace("EMAIL", email)
                .Replace("PASSWORD", DefaultConstants.DEFAULT_PASSWORD);
            return result;
        }

        public static string getCreateKioskContent(string email)
        {
            string result = EmailConstants.CREATE_KIOSK_CONTENT.Replace("EMAIL", email);
            return result;
        }

        public static string getUpdateStatusSubject(bool isActive)
        {
            string status;
            if (isActive)
                status = "Mở";
            else
                status = "Khóa";
            string result = EmailConstants.UPATE_STATUS_SUBJECT_BASE.Replace("STATUS", status);
            return result;
        }

        public static string getUpdateStatusContent(string email, bool isActive)
        {
            string result;
            if (isActive)
                result = EmailConstants.UPATE_STATUS_TO_ACTIVE_CONTENT_BASE.Replace("EMAIL", email);
            else
                result = EmailConstants.UPATE_STATUS_TO_DEACTIVE_CONTENT_BASE.Replace("EMAIL", email);
            return result;
        }

        public static string GetStopAppContent(string appName)
        {
            var content = EmailConstants.STOP_APP_CONTENT.Replace("APP", appName);
            return content;
        }

        public static string GetApprovedPublishAppContent(string appName)
        {
            var content = EmailConstants.APPROVED_PUBLISH_REQUEST_CONTENT.Replace("APP", appName);
            return content;
        }

        public static string GetDeniedPublishAppContent(string appName)
        {
            var content = EmailConstants.DENIED_PUBLISH_REQUEST_CONTENT.Replace("APP", appName);
            return content;
        }

        public static string GetForgetPasswordContent(string link)
        {
            var content = EmailConstants.FORGET_PASSWORD_CONTENT.Replace("LINK_RESET", link);
            return content;
        }
        public static string GetResetPasswordContent(string newPassword)
        {
            var content = EmailConstants.RESET_PASSWORD_CONTENT.Replace("NEW_PASSWORD", newPassword);
            return content;
        }
    }
}