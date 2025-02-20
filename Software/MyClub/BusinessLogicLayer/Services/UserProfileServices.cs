using DataAccessLayer;
using DataAccessLayer.EntityRepositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class UserProfileServices
    {
        public bool ValidateName(string name)
        {
            string namePattern = @"^[A-Z][a-z]+(?: [A-Z][a-z]+)?$";

            bool isNameValid = string.IsNullOrEmpty(name) || Regex.IsMatch(name, namePattern);

            return isNameValid;
        }


        public bool ValidateEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        public bool ValidatePassword(string password)
        {
            if (password.Length < 8)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);

            return hasUpper && hasLower && hasDigit;
        }

        public bool UpdateUser(User user)
        {
            bool isSuccessful = false;
            using (var repo = new UserRepository())
            {
                int affectedRows = repo.Update(user);
                isSuccessful = affectedRows > 0;
            }
            return isSuccessful;
        }

        public User GetUserByEmail(string email)
        {
            using (var repo = new UserRepository())
            {
                return repo.GetUserByEmail(email).FirstOrDefault();
            }
        }

        public List<User> GetUsersByRoleId(int roleId)
        {
            using (var repo = new UserRepository())
            {
                return repo.GetUsersByRoleId(roleId).ToList();
            }
        }
    }
}
