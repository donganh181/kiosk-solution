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
        
        public static async Task SendCreateAccountEmail(string sendto)
        {
            //sendto: Email receiver (người nhận)
            //subject: Tiêu đề email
            //content: Nội dung của email, bạn có thể viết mã HTML
            //Nếu gửi email thành công, sẽ trả về kết quả: OK, không thành công sẽ trả về thông tin l�-i
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                string content = "Kính gửi quý đối tác,<br/>" +
                                 "Chúng tôi chân thành cảm ơn đã hợp tác, bên dưới là tài khoản để đăng nhập vào hệ thống của bạn: <br/>" +
                                  $"username: {sendto}" +
                                  $"<br/>password: {DefaultConstants.DEFAULT_PASSWORD}" +
                                  "<br/><br/>Thanks and Best regards," +
                                  "<br/>Tika - Tourist Interact Kiosk Application";
                string subject = "Cấp tài khoản Tika - Tourist Interact Kiosk Application";
                mail.From = new MailAddress(_email);
                mail.To.Add(sendto);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = content;
 
                mail.Priority = MailPriority.High;
 
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_email, _pass);
                SmtpServer.EnableSsl = true;
 
                await SmtpServer.SendMailAsync(mail);
                Console.WriteLine("Email sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email sending failed");
            }
        }
    }
}