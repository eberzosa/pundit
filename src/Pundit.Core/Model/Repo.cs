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

      /// <summary>
      /// 
      /// </summary>
      /// <param name="id"></param>
      /// <param name="tag"></param>
      /// <param name="uri"></param>
      /// <exception cref="ArgumentNullException"></exception>
      /// <exception cref="ArgumentException"></exception>
      public Repo(long id, string tag, string uri)
      {
         if (tag == null) throw new ArgumentNullException("tag");
         if (uri == null) throw new ArgumentNullException("uri");
         if(id <= 0) throw new ArgumentException("invalid id: " + id, "id");

         _id = id;
         _tag = tag;
         _uri = uri;
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
      public long LastChangeId { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public bool IsEnabled { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public bool UseForPublishing { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public int Priority { get; set; }
   }
}
