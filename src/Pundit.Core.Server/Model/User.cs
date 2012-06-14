using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Pundit.Core.Server.Application.AspNet;

namespace Pundit.Core.Server.Model
{
   public class User
   {
      public long Id { get; private set; }

      public string Login { get; set; }

      public string Role { get; set; }

      public string PasswordHash { get; set; }

      public string ApiKey { get; set; }

      public static explicit operator MembershipUser(User u)
      {
         return new MembershipUser(
            PunditMembershipProvider.ProviderName,
            u.Login, u.Id, null, null, null, true, false,
            DateTime.MinValue, DateTime.MinValue, DateTime.Now,
            DateTime.MinValue, DateTime.MinValue);
      }

      public static string HashPassword(string clearTextPassword)
      {
         return clearTextPassword;  //todo: write hashing function
      }
   }
}
