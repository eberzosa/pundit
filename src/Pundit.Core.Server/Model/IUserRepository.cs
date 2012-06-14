using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Server.Model
{
   public interface IUserRepository
   {
      User CreateUser(User user);

      User GetUser(string login);

      void UpdateUser(User user);

      void DeleteUser(long userId);
   }
}
