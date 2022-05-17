namespace kiosk_solution.Data.Constants
{
    public class EmailConstants
    {
        public static string CREATE_ACCOUNT_SUBJECT = "Cấp tài khoản Tika - Tourist Interact Kiosk Application";

        public static string CREATE_ACCOUNT_CONTENT_BASE = "Kính gửi quý đối tác,<br/>" +
                                                           "Chúng tôi chân thành cảm ơn đã hợp tác, bên dưới là tài khoản để đăng nhập vào hệ thống của bạn: <br/>" +
                                                           "username: EMAIL" +
                                                           "<br/>password: PASSWORD" +
                                                           "<br/><br/>Thanks and Best regards," +
                                                           "<br/>Tika - Tourist Interact Kiosk Application";

        public static string UPATE_STATUS_SUBJECT_BASE = "STATUS tài khoản Tika - Tourist Interact Kiosk Application";

        public static string UPATE_STATUS_TO_DEACTIVE_CONTENT_BASE = "Kính gửi quý đối tác,<br/>" +
                                                                     "Chúng tôi chân thành cảm ơn đã hợp tác suốt thời gian qua, " +
                                                                     "do HỢP ĐỒNG HẾT HIỆU LỰC hoặc quý đối tác đã VI PHẠM ĐIỀU LỆ trong hợp đồng " +
                                                                     "nên chúng tôi quyết định khóa tài khoản [ EMAIL ]. <br/>" +
                                                                     "Nếu có sai sót xin hãy liên hệ với chúng tôi." +
                                                                     "<br/><br/>Thanks and Best regards," +
                                                                     "<br/>Tika - Tourist Interact Kiosk Application";

        public static string UPATE_STATUS_TO_ACTIVE_CONTENT_BASE = "Kính gửi quý đối tác,<br/>" +
                                                                   "Chúng tôi chân thành cảm ơn đã hợp tác suốt thời gian qua, " +
                                                                   "tài khoản [ EMAIL ] đã hoạt động trở lại." +
                                                                   "<br/><br/>Thanks and Best regards," +
                                                                   "<br/>Tika - Tourist Interact Kiosk Application";
    }
}