using Firebase.Auth;
using Firebase.Storage;
using ImageMagick;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kiosk_solution.Business.Utilities
{
    public interface IFirebaseUtil
    {
        Task<string> UploadImageToFirebase(string image, string type, Guid id, string name);
    }
    public class FirebaseUtil : IFirebaseUtil
    {
       
        private readonly ILogger<FirebaseUtil> logger;

        public static string key = "AIzaSyBC_Q1n9Veg-TcTu6FVC0ZEQY5-Gy8z9X0";
        public static string bucket = "kiosk-solution.appspot.com";

        public async Task<string> UploadImageToFirebase(string image, string type, Guid id, string name)
        {
            if (image == null) return null;
            if (image.Length <= 0) return null;

            byte[] data = System.Convert.FromBase64String(image);

            using (MagickImage magicImage = new MagickImage(data))
            {
                magicImage.Format = MagickFormat.Jpg; // Get or Set the format of the image.
                magicImage.Quality = 75; // This is the Compression level.
                using (MemoryStream memStream = new MemoryStream())
                {
                    magicImage.Write(memStream);
                    memStream.Position = 0;
                    try
                    {
                        var auth = new FirebaseAuthProvider(new FirebaseConfig(key));
                        var a = await auth.SignInWithEmailAndPasswordAsync(FirebaseConstants.ADMIN_USERNAME, FirebaseConstants.ADMIN_PASSWORD);

                        var cancellation = new CancellationTokenSource();

                        var upload = new FirebaseStorage(
                                    bucket,
                                    new FirebaseStorageOptions
                                    {
                                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                                        ThrowOnCancel = true
                                    }).Child("assets")
                                    .Child($"{type}")
                                    .Child($"{id}")
                                    .Child($"{name}.jpg")
                                    .PutAsync(memStream, cancellation.Token);
                        string url = await upload;
                        return url;
                    }
                    catch (Exception)
                    {
                        logger.LogInformation("Firebase error.");
                        throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Firebase error.");
                    }
                    
                }
            }
        }
    }
}
