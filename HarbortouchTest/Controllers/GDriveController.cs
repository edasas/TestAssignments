using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using HarbortouchTest.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HarbortouchTest.Controllers
{
    public class GDriveController : Controller
    {
        //asuming files haven't been uploaded via other sources
        static string[] Scopes = new string[] { DriveService.Scope.DriveFile };
        static string ApplicationName = "Harbortouch Rentals";
        static GDriveItems viewItems = new GDriveItems();
        const string jsonPath = @"client_secret.json";
        static DriveService _service;

        DriveService Service
        {
            get
            {
                if (_service == null)
                {
                    UserCredential credential = GetGDriveCredentals();

                    // Create Drive API service.
                    _service = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = ApplicationName,
                    });
                }

                return _service;
            }
            set
            {
                _service = value;
            }
        }
        
        // GET: GDrive
        public ActionResult Index()
        {
            viewItems.Items = GetFiles(Service);
            return View(viewItems);
        }

        public FileResult Download(string id, string name)
        {            
            var stream = new System.IO.MemoryStream();
            if (!String.IsNullOrEmpty(id))
            {
                try
                {
                    var request = Service.Files.Get(id);
                    request.DownloadWithStatus(stream);
                }
                catch
                {
                    viewItems.Messages.Add(new ViewMessage()
                    {
                        MessageType = MessageTypes.Danger,
                        BoldMessage = string.Format("Can't download {0}", name),
                    });
                    RedirectToAction("Index");
                }
            }
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }

        public ActionResult Delete(string id, string name)
        {            
            if (String.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);                
            }
            try
            {
                FilesResource.DeleteRequest request = Service.Files.Delete(id);
                request.Execute();
                viewItems.Messages.Add(new ViewMessage()
                {
                    MessageType = MessageTypes.Success,
                    BoldMessage = string.Format("{0} deleted", name),
                });

            }
            catch(Exception ex)
            {
                viewItems.Messages.Add(new ViewMessage()
                {
                    MessageType = MessageTypes.Danger,
                    BoldMessage = string.Format("{0} deletion failed", name),
                    Message = ex.Message,
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<HttpPostedFileBase> files)
        {            
            foreach (var file in files)
            {
                if (file != null)
                {
                    try
                    {
                        var body = new Google.Apis.Drive.v3.Data.File();
                        body.Name = file.FileName;

                        FilesResource.CreateMediaUpload request = Service.Files.Create(body, file.InputStream, file.ContentType);

                        request.Upload();

                        viewItems.Messages.Add(new ViewMessage()
                        {
                            MessageType = MessageTypes.Success,
                            BoldMessage = string.Format("{0} uploaded file", file.FileName)
                        });
                    }
                    catch(Exception ex)
                    {
                        viewItems.Messages.Add(new ViewMessage()
                        {
                            MessageType = MessageTypes.Danger,
                            BoldMessage = string.Format("{0} upload failed", file.FileName),
                            Message = ex.Message,
                        });
                    }
                }
            }
            return RedirectToAction("Index");
        }

        private UserCredential GetGDriveCredentals()
        {
            UserCredential toReturn;

            using (var stream =
                new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/gdrive_credentials.json");

                toReturn = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            return toReturn;
        }

        private IList<Google.Apis.Drive.v3.Data.File> GetFiles(DriveService service)
        {
            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1000;
            listRequest.Fields = "nextPageToken, files(id, name, webContentLink)";

            // List files.
            return listRequest.Execute()
                .Files;
        }
    }
}