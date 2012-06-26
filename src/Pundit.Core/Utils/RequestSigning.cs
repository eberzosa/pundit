using System;
using System.Collections.Specialized;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace Pundit.Core.Utils
{
   /// <summary>
   /// Basic utilities for signing and validating web requests
   /// </summary>
   public static class RequestSigning
   {
      private const string DateHeader = "x-pdt-dt";
      private const string RequestIdHeader = "x-pdt-id";

      private static string CreateSignature(string apiKey, string date, string requestId, Uri requestUri)
      {
         var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(apiKey));

         //third part (PathAndQuery) doesn't include the host path!!!
         string signature = Convert.ToBase64String(
            hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(date + ":" + requestId + ":" + requestUri.PathAndQuery)));

         return signature;
      }

      /// <summary>
      /// Sign the outgoing request
      /// </summary>
      /// <param name="request"></param>
      /// <param name="login"></param>
      /// <param name="apiKey"></param>
      public static void Sign(HttpWebRequest request, string login, string apiKey)
      {
         if (request != null && apiKey != null && login != null)
         {
            string requestId = Guid.NewGuid().ToString();
            //string dt = DateTime.Now.ToUniversalTime().ToString("ddd, dd MMM yyyy HH:mm:ss UTC");
            string dt = DateTime.UtcNow.Ticks.ToString();
            string signature = CreateSignature(apiKey, dt, requestId, request.RequestUri);
            string header = string.Format("{0}:{1}",
                                          Convert.ToBase64String(Encoding.UTF8.GetBytes(login)),
                                          signature);

            request.Headers[DateHeader] = dt;
            request.Headers[RequestIdHeader] = requestId;
            request.Headers[HttpRequestHeader.Authorization] = header;
         }
      }

      /// <summary>
      /// Validate incoming request and throw <see cref="AuthenticationException"/> if signature doesn't match
      /// </summary>
      /// <param name="requestUri"></param>
      /// <param name="headers"></param>
      /// <param name="apiKey"></param>
      public static void ValidateSignature(Uri requestUri, NameValueCollection headers, string apiKey)
      {
         if (requestUri == null) throw new ArgumentNullException("requestUri");
         if (headers == null) throw new ArgumentNullException("headers");
         if (apiKey == null) throw new ArgumentNullException("apiKey");

         string signatureHeader = headers[HttpRequestHeader.Authorization.ToString()];
         string incomingDate = headers[DateHeader];
         string incomingRequestId = headers[RequestIdHeader];
         if (string.IsNullOrEmpty(signatureHeader) || string.IsNullOrEmpty(incomingDate) ||
            string.IsNullOrEmpty(incomingRequestId)) throw new AuthenticationException("request not fully signed");
         var cred = GetCredential(headers);
         if (string.IsNullOrEmpty(cred.UserName) || string.IsNullOrEmpty(cred.Password))
            throw new AuthenticationException("invalid signature header");

         string signature = CreateSignature(apiKey, incomingDate, incomingRequestId, requestUri);
         if(signature != cred.Password) throw new AuthenticationException("invalid signature");
      }

      /// <summary>
      /// Gets login and signature from headers
      /// </summary>
      /// <param name="headers"></param>
      /// <returns></returns>
      public static NetworkCredential GetCredential(NameValueCollection headers)
      {
         if(headers == null) throw new AuthenticationException("no headers");
         string shead = headers[HttpRequestHeader.Authorization.ToString()];
         if(shead == null) throw new AuthenticationException("no authorization header");
         string[] head = shead.Split(':');
         if(head.Length != 2) throw new AuthenticationException("wrong authorization header format");
         string login = Encoding.UTF8.GetString(Convert.FromBase64String(head[0]));
         string signature = head[1];
         return new NetworkCredential(login, signature);
      }
   }
}
