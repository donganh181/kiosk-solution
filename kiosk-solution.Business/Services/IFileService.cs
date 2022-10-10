using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Services
{
    public interface IFileService
    {
        public Task<string> UploadImageToFirebase(string image, string type, string cateName, Guid id, string name);
    }
}
