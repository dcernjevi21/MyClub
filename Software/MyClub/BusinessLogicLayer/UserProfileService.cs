using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class UserProfileService
    {
        private bool ValidateEmail()
        {
            return true;
        }

        private bool ValidatePassword()
        {
            return true;
        }

        private void ChangeEmail()
        {
        }

        private void ChangePassword()
        {

        }

        private User GetUserByEmail(string email)
        {
            return new User();
        }
    }
}
