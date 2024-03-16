using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskNexus.DAL.DB;
using TaskNexus.DAL.Interfaces;
using TaskNexus.Models.ApplicationUser;
using TaskNexus.Models.Enum;
using TaskNexus.Models.Response;
using TaskNexus.Models.ViewModel.User;
using TaskNexus.Service.ImplementationsService;
using TaskNexus.Service.InterfaceService;

namespace TaskNexusUnitTest
{
   
    public class ApplicationUserServiceTest
    {



        [Fact]
        public async Task Register_WhenUserDoesNotExist_ShouldReturnTrue()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);

            var applicationUserRepositoryMock = new Mock<IApplicationUserRepository>();

            var service = new ApplicationUserService(userManagerMock.Object, signInManagerMock.Object, applicationUserRepositoryMock.Object);

            var viewModel = new RegisterViewModel
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "password123"
            };

           
            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await service.Register(viewModel);

            // Assert
            Assert.True(result.Data); // Expecting user to be registered successfully
            Assert.Equal(StatusCode.OK, result.StatusCode); // Expecting StatusCode.OK
        }
        [Fact]
        public async Task Register_WhenUserCreationFails_ShouldReturnFalse()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);

            var applicationUserRepositoryMock = new Mock<IApplicationUserRepository>();

            var service = new ApplicationUserService(userManagerMock.Object, signInManagerMock.Object, applicationUserRepositoryMock.Object);

            // Set up UserManager to return null, indicating the user does not exist
            userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            var viewModel = new RegisterViewModel
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "password123"
            };

            // Set up UserManager to return failure result
            var identityError = new IdentityError { Description = "Failed to create user" };
            var failedResult = IdentityResult.Failed(identityError);
            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(failedResult);

            // Act
            var result = await service.Register(viewModel);

            // Assert
            Assert.False(result.Data); // Expecting user registration to fail
            Assert.Equal(StatusCode.BadRequest, result.StatusCode); // Expecting BadRequest StatusCode
        }



        [Fact]
        public async Task DeleteUser_WhenUserExists_ShouldReturnTrue()
        {
            // Arrange
            var userId = "1"; // Припустимий ідентифікатор користувача
            var userRepositoryMock = new Mock<IApplicationUserRepository>();
            userRepositoryMock.Setup(m => m.Get(userId)).ReturnsAsync(new ApplicationUser()); // Повертаємо не null, оскільки користувач з таким ідентифікатором існує

            var service = new ApplicationUserService(null, null, userRepositoryMock.Object);

            // Act
            var result = await service.DeleteUser(userId);

            // Assert
            Assert.True(result.Data); // Очікуємо, що користувач буде видалений успішно
            Assert.Equal(StatusCode.OK, result.StatusCode); // Очікуємо StatusCode.OK
        }

        [Fact]
        public async Task DeleteUser_WhenUserDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var userId = "1"; // Припустимий ідентифікатор користувача
            var userRepositoryMock = new Mock<IApplicationUserRepository>();
            userRepositoryMock.Setup(m => m.Get(userId)).ReturnsAsync((ApplicationUser)null); // Повертаємо null, оскільки користувач з таким ідентифікатором не існує

            var service = new ApplicationUserService(null, null, userRepositoryMock.Object);

            // Act
            var result = await service.DeleteUser(userId);

            // Assert
            Assert.False(result.Data); // Очікуємо, що користувач не буде видалений, оскільки його не існує
            Assert.Equal(StatusCode.NullEntity, result.StatusCode); // Очікуємо StatusCode.NullEntity
        }

        [Fact]
        public async Task DeleteUser_WhenRepositoryThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            var userId = "1"; // Припустимий ідентифікатор користувача
            var userRepositoryMock = new Mock<IApplicationUserRepository>();
            userRepositoryMock.Setup(m => m.Get(userId)).ThrowsAsync(new Exception("Repository error")); // Моделюємо виняток з репозиторію

            var service = new ApplicationUserService(null, null, userRepositoryMock.Object);

            // Act
            var result = await service.DeleteUser(userId);

            // Assert
            Assert.False(result.Data); // Очікуємо, що користувач не буде видалений через помилку репозиторію
            Assert.Equal(StatusCode.InternalServerError, result.StatusCode); // Очікуємо StatusCode.InternalServerError
        }
        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnTrue()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);

            var service = new ApplicationUserService(userManagerMock.Object, signInManagerMock.Object, null);

            var viewModel = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var user = new ApplicationUser { Email = viewModel.Email };
            userManagerMock.Setup(m => m.FindByEmailAsync(viewModel.Email)).ReturnsAsync(user);
            signInManagerMock.Setup(m => m.PasswordSignInAsync(user, viewModel.Password, false, false))
                            .ReturnsAsync(SignInResult.Success);

            // Act
            var result = await service.Login(viewModel);

            // Assert
            Assert.True(result.Data); // Expecting user to be logged in successfully
            Assert.Equal(StatusCode.OK, result.StatusCode); // Expecting StatusCode.OK
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldReturnFalse()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);

            var service = new ApplicationUserService(userManagerMock.Object, signInManagerMock.Object, null);

            var viewModel = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var user = new ApplicationUser { Email = viewModel.Email };
            userManagerMock.Setup(m => m.FindByEmailAsync(viewModel.Email)).ReturnsAsync(user);
            signInManagerMock.Setup(m => m.PasswordSignInAsync(user, viewModel.Password, false, false))
                            .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await service.Login(viewModel);

            // Assert
            Assert.False(result.Data); // Expecting user login attempt to fail
            Assert.Equal(StatusCode.Unauthorized, result.StatusCode); // Expecting StatusCode.Unauthorized
        }

        [Fact]
        public async Task Login_WithNonexistentUser_ShouldReturnFalse()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);

            var service = new ApplicationUserService(userManagerMock.Object, signInManagerMock.Object, null);

            var viewModel = new LoginViewModel
            {
                Email = "nonexistent@example.com",
                Password = "password123"
            };

            userManagerMock.Setup(m => m.FindByEmailAsync(viewModel.Email)).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await service.Login(viewModel);

            // Assert
            Assert.False(result.Data); // Expecting user login attempt to fail
            Assert.Equal(StatusCode.Unauthorized, result.StatusCode); // Expecting StatusCode.Unauthorized
        }

        [Fact]
        public async void UpdateUser_WhenUserExists_ShouldReturnUpdatedUser()
        {
            // Arrange
            var userId = "user123";
            var updatedUserName = "updatedUserName";

            var userToUpdate = new ApplicationUser
            {
                Id = userId,
                UserName = "initialUserName"
            };

            var updatedUser = new ApplicationUser
            {
                Id = userId,
                UserName = updatedUserName
            };

            var userRepositoryMock = new Mock<IApplicationUserRepository>();
            userRepositoryMock.Setup(m => m.Get(userId)).ReturnsAsync(userToUpdate);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<ApplicationUser>())).ReturnsAsync(updatedUser);

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
              Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);

            var service = new ApplicationUserService(userManagerMock.Object, signInManagerMock.Object, userRepositoryMock.Object);

            // Act
            var result = await service.UpdateUser(userId, updatedUser);

            // Assert
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Equal(updatedUserName, result.Data.UserName);
        }
        [Fact]
        public async Task UpdateUser_WhenUserExistsButUpdateFails_ShouldReturnNoUpdate()
        {
            // Arrange
            var userId = "user123";
            var updatedUserName = "updatedUserName";

            var userToUpdate = new ApplicationUser
            {
                Id = userId,
                UserName = "initialUserName"
            };

            var updatedUser = new ApplicationUser
            {
                Id = userId,
                UserName = updatedUserName
            };

            var userRepositoryMock = new Mock<IApplicationUserRepository>();
            userRepositoryMock.Setup(m => m.Get(userId)).ReturnsAsync(userToUpdate);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<ApplicationUser>())).ReturnsAsync((ApplicationUser)null);

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
              Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);

            var service = new ApplicationUserService(userManagerMock.Object, signInManagerMock.Object, userRepositoryMock.Object);

            // Act
            var result = await service.UpdateUser(userId, updatedUser);

            // Assert
            Assert.Equal(StatusCode.NoUpdate, result.StatusCode);
            Assert.Equal("NoUpdate", result.Description);
        }
    }
}
