using BusinessLogicLayer;
using DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationLayer.UserControls;
using System;
using System.Reflection;
using System.Windows.Controls;

namespace Tests
{
    [TestClass]
    public class EditProfileTests
    {
        private User _testUser;

        public void Setup()
        {
            _testUser = new User
            {
                Email = "old@example.com",
                Password = "OldPass123",
                ProfilePicture = null
            };
        }

        [TestMethod]
        public void ValidateEmail_ValidEmailFormat_ReturnTrue()
        {
            // Arrange
            var email = "test@gmail.com";
            //Act
            var userProfileService = new UserProfileServices();
            //Assert
            Assert.IsTrue(userProfileService.ValidateEmail(email));
        }

        [TestMethod]
        public void ValidateEmail_InvalidEmailFormat_ReturnFalse()
        {             
            // Arrange
            var email = "testgmail.com";
            //Act
            var userProfileService = new UserProfileServices();
            //Assert
            Assert.IsFalse(userProfileService.ValidateEmail(email));
        }

        [TestMethod]
        public void ValidatePassword_ValidPasswordFormat_ReturnTrue()
        {
            // Arrange
            var password = "TestPass123";
            //Act
            var userProfileService = new UserProfileServices();
            //Assert
            Assert.IsTrue(userProfileService.ValidatePassword(password));
        }

        [TestMethod]
        public void ValidatePassword_InvalidPasswordFormat_ReturnFalse()
        {
            // Arrange
            var password = "testpass";
            //Act
            var userProfileService = new UserProfileServices();
            //Assert
            Assert.IsFalse(userProfileService.ValidatePassword(password));
        }
    }
}
