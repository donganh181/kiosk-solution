using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Data.ViewModels
{
    public class ImageUpdateViewModel
    {
        public ImageUpdateViewModel(Guid id, string name, string image, string keySubType)
        {
            Id = id;
            Name = name;
            Image = image;
            KeySubType = keySubType;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string KeySubType { get; set; }
    }
}
