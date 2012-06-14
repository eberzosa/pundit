using System;
using System.Web.Security;

namespace Pundit.Core.Server.Application.AspNet
{
   public class PunditRoleProvider : RoleProvider
   {
      #region [ RoleProvider ]

      public override bool IsUserInRole(string username, string roleName)
      {
         return username != null && username == "root" && roleName != null && roleName == "admins";
      }

      public override string[] GetRolesForUser(string username)
      {
         if (username == "root") return new[] {"admins"};

         return null;
      }

      public override void CreateRole(string roleName)
      {
         throw new NotImplementedException();
      }

      public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
      {
         throw new NotImplementedException();
      }

      public override bool RoleExists(string roleName)
      {
         throw new NotImplementedException();
      }

      public override void AddUsersToRoles(string[] usernames, string[] roleNames)
      {
         throw new NotImplementedException();
      }

      public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
      {
         throw new NotImplementedException();
      }

      public override string[] GetUsersInRole(string roleName)
      {
         throw new NotImplementedException();
      }

      public override string[] GetAllRoles()
      {
         throw new NotImplementedException();
      }

      public override string[] FindUsersInRole(string roleName, string usernameToMatch)
      {
         throw new NotImplementedException();
      }

      public override string ApplicationName
      {
         get { throw new NotImplementedException(); }
         set { throw new NotImplementedException(); }
      }

      #endregion
   }
}
