using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Pundit.Core.Server.Model;

namespace Pundit.Core.Server.Application
{
   class MySqlUserRepository : MySqlRepositoryBase, IUserRepository
   {
      private const string UserTableName = "User";

      public MySqlUserRepository() : this(null)
      {
         
      }

      public MySqlUserRepository(string connectionString) : base(connectionString)
      {
      }

      public User CreateUser(User user)
      {
         throw new NotImplementedException();
      }

      public User GetUser(string login)
      {
         using(IDataReader reader = ExecuteReader(UserTableName, null,
            new[] { "Login=?P0"}, login))
         {
            if(reader.Read())
            {
               var u = new User(
                  reader.AsString("Login"),
                  reader.AsString("Role"),
                  reader.AsString("PasswordHash"),
                  reader.AsString("ApiKey"));
               return u;
            }
         }
         return null;
      }

      public void UpdateUser(User user)
      {
         throw new NotImplementedException();
      }

      public void DeleteUser(long userId)
      {
         throw new NotImplementedException();
      }
   }
}
