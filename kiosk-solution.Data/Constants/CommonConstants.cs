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
        /// Upload File Zone
        /// </summary>
        public static string APP_IMAGE = "app_image";
        public static string CATE_IMAGE = "category_image";
        public static string EVENT_IMAGE = "event_image";
        public static string POI_IMAGE = "poi_image";
        public static string POI_CATE_IMAGE = "poi_cate_image";
        public static string LOCATION_IMAGE = "location_image";
        public static string BANNER_IMAGE = "banner_image";
        public static string BANNER_EVENT = "events";
        public static string BANNER_POI = "pois";
        public static string BANNER_APP = "apps";

        

        public static string THUMBNAIL = "thumbnail";
        public static string SOURCE_IMAGE = "source_image";
    }
}
