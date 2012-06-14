using System;
using System.Web.Security;
using Pundit.Core.Server.Model;

namespace Pundit.Core.Server.Application.AspNet
{
   public class PunditRoleProvider : RoleProvider
   {
      private IUserRepository _repository;

      #region [ RoleProvider ]

      public override bool IsUserInRole(string username, string roleName)
      {
         User u = _repository.GetUser(username);
         return u != null && u.Role == roleName;
      }

      public override string[] GetRolesForUser(string username)
      {
         User u = _repository.GetUser(username);
         return u == null ? null : new[] {u.Role};
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
