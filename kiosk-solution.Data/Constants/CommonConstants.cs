using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.Constants
{
    public class CommonConstants
    {
        /// <summary>
        /// Paging zone
        /// </summary>
        public const int DefaultPaging = 250;
        public const int LimitPaging = 100;
        public const int DefaultPage = 1;

        /// <summary>
        /// Event Zone
        /// </summary>
        public static string SERVER_TYPE = "server";
        public static string LOCAL_TYPE = "local";

        /// <summary>
        /// Upload File Zone
        /// </summary>
        public static string APP_IMAGE = "app_image";
        public static string CATE_IMAGE = "category_image";
        public static string EVENT_IMAGE = "event_image";
        public static string POI_IMAGE = "poi_image";

        public static string THUMBNAIL = "thumbnail";
        public static string SOURCE_IMAGE = "source_image";
    }
}
