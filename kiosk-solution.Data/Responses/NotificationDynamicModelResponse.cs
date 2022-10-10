using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.Responses
{
    [Serializable]
    public class NotificationDynamicModelResponse<T>
    {
        public NotificationPagingMetaData Metadata { get; set; }
        public List<T> Data { get; set; }
    }
    [Serializable]
    public class NotificationPagingMetaData
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
        public int UnseenNoti { get; set; }
    }
}
