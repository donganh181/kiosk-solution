using System;
using System.Net.Mail;
using System.Threading.Tasks;
using kiosk_solution.Data.Constants;

namespace kiosk_solution.Data.ViewModels
{
    public class EmailUtil
    {
        private static readonly string _email = "capstoneprojectfu2021@gmail.com";
        private static readonly string _pass = "capstone123";

        public static async Task SendEmail(string sendto, string subject, string content)
        {
            //sendto: Email receiver (người nhận)
            //subject: Tiêu đề email
            //content: Nội dung của email, bạn có thể viết mã HTML
            //Nếu gửi email thành công, sẽ trả về kết quả: OK, không thành công sẽ trả về thông tin l�-i
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(_email);
                mail.To.Add(sendto);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = content;

                smtpServer.Host = "smtp.gmail.com";
                smtpServer.Port = 587;
                smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpServer.EnableSsl = true;
                smtpServer.UseDefaultCredentials = false;
                smtpServer.Credentials = new System.Net.NetworkCredential(_email, _pass);

                await smtpServer.SendMailAsync(mail);
                Console.WriteLine("Email sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Email sending failed");
            }
        }

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
    }
}