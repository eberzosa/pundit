using System.Net;
using System.Security.Authentication;
using System.Web.Mvc;
using Pundit.Core.Model;
using Pundit.Core.Server.Model;
using Pundit.Core.Utils;

namespace Pundit.Web.Controllers
{
   /// <summary>
   /// HTTP REST repository web service
   /// </summary>
   public class RemoteRepositoryController : Controller
   {
      private readonly IRemoteRepository _remote;
      private readonly IUserRepository _userRepository;
      private NetworkCredential _cred;
      private User _user;

      public RemoteRepositoryController(IRemoteRepository remote, IUserRepository userRepository)
      {
         _remote = remote;
         _userRepository = userRepository;
      }

      [HttpPost]
      public void Publish()
      {
         try
         {
            _cred = RequestSigning.GetCredential(Request.Headers);
         }
         catch(AuthenticationException ex)
         {
            Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            Response.StatusDescription = ex.Message;
         }

         if (_cred != null)
         {
            _user = _userRepository.GetUser(_cred.UserName);
            if (_user == null)
            {
               Response.StatusCode = (int)HttpStatusCode.NotFound;
               Response.StatusDescription = "user '" + _cred.UserName + "' not found";
            }
            else
            {
               try
               {
                  RequestSigning.ValidateSignature(Request.Url, Request.Headers, _user.ApiKey);
               }
               catch(AuthenticationException ex)
               {
                  Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                  Response.StatusDescription = ex.Message;                  
               }

               _remote.Publish(Request.InputStream);
            }
         }
      }

      // allow unrestricted downloads
      [HttpGet]
      public FileResult Download(string platform, string packageId, string version)
      {
         return File(_remote.Download(platform, packageId, version), "pundit/package-zip");
      }

      // allow unrestricted snapshotting
      [HttpGet]
      public ContentResult GetSnapshot(string delta)
      {
         RemoteSnapshot snapshot = _remote.GetSnapshot(delta);
         string xml = snapshot.ToXml();
         return Content(xml, "text/xml");
      }

   }
}
