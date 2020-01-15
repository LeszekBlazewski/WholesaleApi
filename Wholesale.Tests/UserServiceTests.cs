using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using Wholesale.DAL;
using Wholesale.BL.Models;
using Wholesale.BL.Services;
using Wholesale.DAL.Repositories;
using Wholesale.BL.Enums;
using System.Threading.Tasks;
using System.Linq;

namespace Wholesale.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DeliveriesContext>()
                .UseInMemoryDatabase(databaseName: "WholesaleDB" + DateTime.Now.ToFileTimeUtc())
                .Options;

            var context = new DeliveriesContext(options);

            var repo = new UserRepository(context);

            _userService = new UserService(repo);
        }

        [Test]
        [TestCase("")]
        [TestCase("    ")]
        [TestCase("\t\t")]
        [TestCase(null)]
        public void Create_passwordIsNullOrWhiteSpace_InvalidOperationThrownAndUserNotAddedToDatabase(string password)
        {
            // Arrange
            var user = new User() { Address = new Address(), CompanyName = "Comp", Email = "email" };

            // Action && Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _userService.Create(user, password));
        }

        [Test]
        public async Task Create_emailIsAlreadyTaken_InvalidOperationThrownAndUserNotAddedToDatabase()
        {
            // Arrange
            var password = "test";
            var user = new User() { Address = new Address(), CompanyName = "Comp", Email = "email@gmail.com" };
            await _userService.Create(user, password);

            // Action && Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _userService.Create(user, password));
        }

        [Test]
        public async Task Create_validEmailAndPassword_UserIsAddedToDatabase()
        {
            // Arrange
            var password = "test";
            var user = new User() { Address = new Address(), CompanyName = "Comp", Email = "email@gmail.com", FirstName = "John", Role = UserRole.Client };

            // Action 
            await _userService.Create(user, password);

            //Assert
            var usersInDb = await _userService.GetAll();
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(usersInDb);
                Assert.That(usersInDb, Has.Count.EqualTo(1));
                Assert.AreEqual(usersInDb.First().FirstName, user.FirstName);
            });
        }

    }
}