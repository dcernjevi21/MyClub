using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityRepositories
{
    public class UserRepository
    {
        public int AddUser(User user)
        {
            return 1;
        }

        public int UpdateUser(User user)
        {
            return 1;
        }

        public int DeleteUser(User user)
        {
            return 1;
        }

        public List<User> GetAllUsers()
        {
            return new List<User>();
        }

        public User GetUserByEmail(string email)
        {
            return new User();
        }

        public List<User> GetAllPendingUsers(User user)
        {
            return new List<User>();
        }
    }
}
