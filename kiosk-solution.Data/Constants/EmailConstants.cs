namespace kiosk_solution.Data.Constants
{
    public class EmailConstants
    {
        public static string CREATE_ACCOUNT_SUBJECT = "Cấp tài khoản Tika - Tourist Interact Kiosk Application";

        public static string CREATE_ACCOUNT_CONTENT_BASE = "Kính gửi quý đối tác,<br/>" +
                                                           "Chúng tôi chân thành cảm ơn đã hợp tác, bên dưới là tài khoản để đăng nhập vào hệ thống của bạn: <br/>" +
                                                           "username: EMAIL" +
                                                           "<br/>password: PASSWORD" +
                                                           "<br/><br/>Thanks and best regards," +
                                                           "<br/>Tika - Tourist Interact Kiosk Application";

        public static string UPATE_STATUS_SUBJECT_BASE = "STATUS tài khoản Tika - Tourist Interact Kiosk Application";

        public static string UPATE_STATUS_TO_DEACTIVE_CONTENT_BASE = "Kính gửi quý đối tác,<br/>" +
                                                                     "Chúng tôi chân thành cảm ơn đã hợp tác suốt thời gian qua, " +
                                                                     "do HỢP ĐỒNG HẾT HIỆU LỰC hoặc quý đối tác đã VI PHẠM ĐIỀU LỆ trong hợp đồng " +
                                                                     "nên chúng tôi quyết định khóa tài khoản [ EMAIL ]. <br/>" +
                                                                     "Nếu có sai sót xin hãy liên hệ với chúng tôi." +
                                                                     "<br/><br/>Thanks and best regards," +
                                                                     "<br/>Tika - Tourist Interact Kiosk Application";

        public static string UPATE_STATUS_TO_ACTIVE_CONTENT_BASE = "Kính gửi quý đối tác,<br/>" +
                                                                   "Chúng tôi chân thành cảm ơn đã hợp tác suốt thời gian qua, " +
                                                                   "tài khoản [ EMAIL ] đã hoạt động trở lại." +
                                                                   "<br/><br/>Thanks and best regards," +
                                                                   "<br/>Tika - Tourist Interact Kiosk Application";

        public static string CREATE_KIOSK_SUBJECT = "Tạo Kiosk thành công - Tourist Interact Kiosk Application";
        public static string CREATE_KIOSK_CONTENT = "Kính gửi quý đối tác,<br/>" +
                                                    "Chúng tôi chân thành cảm ơn đã hợp tác, chúng tôi đã tạo thành công sản phảm kiosk mà quý đối tác đã đặt mua. " +
                                                    "Quý đối tác vui lòng vào trang quản lý và đăng nhập vài tài khoản [ EMAIL ] để cài đặt thêm những thông tin cần thiết cho kiosk và kích hoạt kiosk. <br/>" +
                                                    "<br/><br/>Thanks and best regards," +
                                                    "<br/>Tika - Tourist Interact Kiosk Application";

        public static string STOP_APP_SUBJECT = "Tình trạng ứng dụng của quý khách - Tourist Interact Kiosk Application";
        public static string STOP_APP_CONTENT = "Kính gửi quý đối tác,<br/>" +
                                                "Chúng tôi chân thành cảm ơn đã hợp tác suốt thời gian qua, " +
                                                "do HỢP ĐỒNG HẾT HIỆU LỰC hoặc quý đối tác đã VI PHẠM ĐIỀU LỆ trong hợp đồng " +
                                                "nên chúng tôi quyết định vô hiệu hóa ứng dụng [ APP ]. <br/>" +
                                                "Nếu có sai sót xin hãy liên hệ với chúng tôi." +
                                                "<br/><br/>Thanks and best regards," +
                                                "<br/>Tika - Tourist Interact Kiosk Application";

        public static string PUBLISH_REQUEST_SUBJECT = "Yêu cầu đăng tải ứng dụng của quý khách - Tourist Interact Kiosk Application";
        public static string APPROVED_PUBLISH_REQUEST_CONTENT = "Kính gửi quý đối tác,<br/>" +
                                                "Chúng tôi chân thành cảm ơn đã hợp tác, " +
                                                "ứng dụng của quý đối tác phù hợp với tiêu chuẩn của chúng tôi " +
                                                "nên chúng tôi đã đăng tải ứng dụng [ APP ] của quý đối tác lên hệ thống của chúng tôi. <br/>" +
                                                "Nếu có sai sót xin hãy liên hệ với chúng tôi." +
                                                "<br/><br/>Thanks and best regards," +
                                                "<br/>Tika - Tourist Interact Kiosk Application";

        public static string DENIED_PUBLISH_REQUEST_CONTENT = "Kính gửi quý đối tác,<br/>" +
                                                "Chúng tôi chân thành cảm ơn đã hợp tác, " +
                                                "ứng dụng của quý đối tác không phù hợp với tiêu chuẩn của chúng tôi " +
                                                "nên chúng tôi không thể đăng tải ứng dụng [ APP ] của quý đối tác lên hệ thống của chúng tôi. <br/>" +
                                                "Nếu có sai sót xin hãy liên hệ với chúng tôi." +
                                                "<br/><br/>Thanks and best regards," +
                                                "<br/>Tika - Tourist Interact Kiosk Application";

        public static string FORGET_PASSWORD_SUBJECT = "Quên mật khẩu - Tourist Interact Kiosk Application";
        public static string FORGET_PASSWORD_LINK = "https://tikap.cf:9930/verify-pass?partyId=PARTY_ID&verifyCode=VERIFY_CODE";
        public static string FORGET_PASSWORD_CONTENT = "Kính gửi quý đối tác,<br/>"+
                                                       "Bên dưới là link để giúp quý đối tác cài lại mật khẩu, nếu có thắc mắc, sai sót hãy liên hệ chúng tôi để được hỗ trợ thêm.<br/>"+
                                                       "<a href='LINK_RESET'>Nhấn vào đây để reset mật khẩu</a>"+
                                                       "<br/><br/>Thanks and best regards," +
                                                       "<br/>Tika - Tourist Interact Kiosk Application";
        public static string RESET_PASSWORD_SUBJECT = "Cấp mật khẩu mới - Tourist Interact Kiosk Application";
        public static string RESET_PASSWORD_CONTENT = "Kính gửi quý đối tác,<br/>"+
                                                       "Chúng tôi chân thành cảm ơn đã hợp tác, chúng tôi đã cấp lại mật khẩu mới cho tài khoản của quý đối tác. Hiện mật khẩu mới là:<br/>"+
                                                       "<h2>NEW_PASSWORD</h2>"+
                                                       "<br/><br/>Thanks and best regards," +
                                                       "<br/>Tika - Tourist Interact Kiosk Application";
    }
}