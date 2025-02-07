using DataAccessLayer;
using DataAccessLayer.EntityRepositories;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLogicLayer.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public User AuthenticateUser(string email, string password)
        {
            var user = _userRepository.GetUserByEmail(email).FirstOrDefault();
            if (user != null && VerifyPassword(password, user.Password))
            {
                return user;
            }
            return null;
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return enteredPassword == storedPassword;
        }

        public bool RegisterUser(User user, out string errorMessage)
        {
            if (!IsValidRegistration(user))
            {
                errorMessage = "Invalid registration data!";
                return false;
            }

            if (IsEmailInUse(user.Email))
            {
                errorMessage = "Email is already in use.";
                return false;
            }

            return SaveUser(user, out errorMessage);
        }

        private bool IsValidRegistration(User user)
        {
            return !string.IsNullOrEmpty(user.FirstName) &&
                   !string.IsNullOrEmpty(user.LastName) &&
                   !string.IsNullOrEmpty(user.Email) && IsValidEmail(user.Email) &&
                   !string.IsNullOrEmpty(user.Username) &&
                   !string.IsNullOrEmpty(user.Password) &&
                   user.BirthDate != default;
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsEmailInUse(string email)
        {
            return _userRepository.GetUserByEmail(email).Any();
        }

        private bool SaveUser(User user, out string errorMessage)
        {
            user.StatusID = (int)UserStatus.Pending;
            bool result = _userRepository.AddUser(user) > 0;
            errorMessage = result ? string.Empty : "Registration failed due to a database error.";
            return result;
        }

        public List<User> GetPendingUsers()
        {
            return _userRepository.GetAllPendingUsers().ToList();
        }

        public bool AcceptUser(User user)
        {
            user.StatusID = (int)UserStatus.Accepted;
            return _userRepository.Update(user) > 0;
        }

        public bool RejectUser(User user)
        {
            user.StatusID = (int)UserStatus.Rejected;
            return _userRepository.Update(user) > 0;
        }

        public List<User> GetUsersFromTeam(int teamId)
        {
            using (var repo = new UserRepository())
            {
                return repo.GetUsersByTeamId(teamId).Where(u => u.RoleID == 3).ToList();
            }
        }
    }
}
