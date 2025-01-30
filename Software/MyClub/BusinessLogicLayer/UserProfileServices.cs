using DataAccessLayer.EntityRepositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class UserProfileServices
    {
        public bool ValidateEmail(string email)
        {
            return true;
        }

        public bool ValidatePassword(string password)
        {
            return true;
        }

        public bool ChangeEmail(User user)
        {
            bool isSuccessful = false;
            using (var repo = new UserRepository())
            {
                int affectedRows = repo.Update(user);
                isSuccessful = affectedRows > 0;
            }
            return isSuccessful;
        }

        public bool ChangePassword()
        {
            bool isSuccessful = false;
            using (var repo = new UserRepository())
            {
                int affectedRows = repo.Update(new User());
                isSuccessful = affectedRows > 0;
            }
            return isSuccessful;
        }

        public List<User> GetUserByEmail(string email)
        {
            using (var repo = new UserRepository())
            {
                return repo.GetUserByEmail(email).ToList();
            }
        }
    }
}
