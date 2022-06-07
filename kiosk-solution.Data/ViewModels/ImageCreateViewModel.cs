using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class ImageCreateViewModel
    {
        public ImageCreateViewModel(string name, string image, Guid? keyId, string keyType, string keySubType)
        {
            Name = name;
            Image = image;
            KeyId = keyId;
            KeyType = keyType;
            KeySubType = keySubType;
        }

        public string Name { get; set; }
        public string Image { get; set; }
        public Guid? KeyId { get; set; }
        public string KeyType { get; set; }
        public string KeySubType { get; set; }
    }
}
