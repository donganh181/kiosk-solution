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

namespace kiosk_solution.Business.Services.impl
{
    public class FirebaseStorageService : IFileService
    {
        private readonly ILogger<IFileService> _logger;

        public FirebaseStorageService(ILogger<IFileService> logger)
        {
            _logger = logger;
        }

        public async Task<string> UploadImageToFirebase(string image, string type, string cateName, Guid id, string name)
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
                        var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConstants.KEY));
                        var a = await auth.SignInWithEmailAndPasswordAsync(FirebaseConstants.ADMIN_USERNAME, FirebaseConstants.ADMIN_PASSWORD);

                        var cancellation = new CancellationTokenSource();

                        var upload = new FirebaseStorage(
                                    FirebaseConstants.BUCKET,
                                    new FirebaseStorageOptions
                                    {
                                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                                        ThrowOnCancel = true
                                    }).Child("assets")
                                    .Child($"{type}")
                                    .Child($"{cateName}")
                                    .Child($"{id}")
                                    .Child($"{name}.jpg")
                                    .PutAsync(memStream, cancellation.Token);
                        string url = await upload;
                        return url;
                    }
                    catch (Exception)
                    {
                        _logger.LogInformation("Firebase error.");
                        throw new ErrorResponse((int)HttpStatusCode.InternalServerError, "Firebase error.");
                    }

                }
            }
        }
    }
}
