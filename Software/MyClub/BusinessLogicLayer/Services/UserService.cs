using DataAccessLayer;
using DataAccessLayer.EntityRepositories;
using System;
using System.Linq;

namespace BusinessLogicLayer.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository(); 
        }

        public bool RegisterUser(User user)
        {
            if (IsValidRegistration(user))
            {
                if (_userRepository.GetUserByEmail(user.Email).Any())
                {
                    return false; // Email already exists
                }

                user.StatusID = (int)UserStatus.Pending;
                return _userRepository.AddUser(user) > 0;
            }
            return false; // Invalid registration
        }

        public User AuthenticateUser(string email, string password)
        {
            var user = _userRepository.GetUserByEmail(email).FirstOrDefault();
            return (user != null && user.Password == password) ? user : null;
        }

        private bool IsValidRegistration(User user)
        {
            return !string.IsNullOrEmpty(user.FirstName) &&
                   !string.IsNullOrEmpty(user.LastName) &&
                   !string.IsNullOrEmpty(user.Email) &&
                   !string.IsNullOrEmpty(user.Username) &&
                   !string.IsNullOrEmpty(user.Password) &&
                   user.BirthDate != default(DateTime);
        }

        public bool ValidateLogin(string email, string password)
        {
            var user = _userRepository.GetUserByEmail(email).FirstOrDefault();
            return user != null && user.Password == password;
        }
    }
}
