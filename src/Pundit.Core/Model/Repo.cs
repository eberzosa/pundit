using System;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Repository information
   /// </summary>
   public class Repo
   {
      private readonly long _id;
      private readonly string _tag;
      private readonly string _uri;
      private readonly string _login;
      private readonly string _apiKey;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="tag"></param>
      /// <param name="uri"></param>
      /// <exception cref="ArgumentNullException"></exception>
      /// <exception cref="ArgumentException"></exception>
      public Repo(string tag, string uri)
      {
         if (tag == null) throw new ArgumentNullException("tag");
         if (uri == null) throw new ArgumentNullException("uri");

         _tag = tag;
         _uri = uri;
         IsEnabled = true;
         RefreshIntervalInHours = 1;
      }

      public Repo(long id, string tag, string uri) : this(tag, uri)
      {
         if(id <= 0) throw new ArgumentException("Invalid id: " + id);
         _id = id;
      }

      /// <summary>
      /// Internal Id
      /// </summary>
      public long Id
      {
         get { return _id; }
      }

      /// <summary>
      /// Tag (name)
      /// </summary>
      public string Tag
      {
         get { return _tag; }
      }

      /// <summary>
      /// Uri
      /// </summary>
      public string Uri
      {
         get { return _uri; }
      }

      /// <summary>
      /// Refresh interval
      /// </summary>
      public int RefreshIntervalInHours { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public DateTime LastRefreshed { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public string LastChangeId { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public bool IsEnabled { get; set; }

      /// <summary>
      /// Remote login for publishing
      /// </summary>
      public string Login { get; set; }

      /// <summary>
      /// API key for publishing
      /// </summary>
      public string ApiKey { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public int Priority { get; set; }
   }
}
