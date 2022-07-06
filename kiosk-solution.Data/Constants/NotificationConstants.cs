using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.Constants
{
    public class NotificationConstants
    {
        //Status zone
        public static string UNSEEN = "unseen";
        public static string SEEN = "seen";

        //Publish request zone
        public static string APPROVED_TITLE = "Request to Publish Application [APP] - Approved";
        public static string DENIED_TITLE = "Request to Publish Application [APP] - Denied";
        public static string APPROVED_CONTENT = "Your Application [APP] has been approved to publish in our system.";
        public static string DENIED_CONTENT = "Your Application [APP] does not meet requirement to publish in our system.";
    }
}
