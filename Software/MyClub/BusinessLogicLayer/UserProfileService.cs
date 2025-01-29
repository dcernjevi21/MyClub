using DataAccessLayer;
using DataAccessLayer.EntityRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class UserProfileService
    {
        public bool ValidateEmail()
        {
            return true;
        }

        public bool ValidatePassword()
        {
            return true;
        }

        public void ChangeEmail()
        {

        }

        public void ChangePassword()
        {

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
