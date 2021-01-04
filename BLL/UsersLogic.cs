using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMSDesktop.DAL.Model;
using AMSDesktop.DAL.Repository;
using AMSDesktop.Helpers;

namespace AMSDesktop.BLL
{
    public class UsersLogic
    {
        public List<User> GetUsers()
        {
            return new UsersRepository().GetUsers();
        }
        public User IsAuthenticatedUser(string username, string password)
        {
            User user = new UsersRepository().GetUser(username);
            if (user != null)
            {
                if (user.Password == new CryptographyHelper().GetHashedString(password + user.Salt))
                {
                    return user;
                }

                return null;
            }

            return null;
        }

        public void AddUser(User user)
        {
            new UsersRepository().AddUser(user);
        }

        public void UpdateUser(User user)
        {
            new UsersRepository().UpdateUser(user);
        }

        public void DeleteUser(User user)
        {
            new UsersRepository().DeleteUser(user);
        }

        public void ChangePassword(User user)
        {
            new UsersRepository().ChangePassword(user);
        }
    }
}
