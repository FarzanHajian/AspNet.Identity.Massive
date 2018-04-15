using AspNet.Identity.Massive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class UserUnitTest
    {
        private DbManager database;
        private UserTable<IdentityUser> userTable;

        [TestInitialize]
        public void Initialize()
        {
            database = new DbManager("DefaultConnection");
            userTable = database.GetUserTable<IdentityUser>();
        }

        [TestMethod]
        public void InsertUser()
        {
            dynamic user = new IdentityUser { UserName = "admin", Email = "admin@example.com", PhoneNumber = "1234567890", PasswordHash = "@#$rfgt$##WE", EmailConfirmed = true, PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0 };
            user = userTable.Insert(user);
            int count = userTable.All("UserName = 'admin'").ToList().Count;
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void UpdateUser()
        {
            dynamic user = new IdentityUser { UserName = "admin", Email = "admin@example.com", PhoneNumber = "1234567890", PasswordHash = "@#$rfgt$##WE", EmailConfirmed = true, PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0 };
            user = userTable.Insert(user);
            user.PhoneNumber = "1234512345";
            user.PhoneNumberConfirmed = false;
            userTable.Update(user);
            dynamic user2 = userTable.Single(user.Id);
            Assert.AreEqual("1234512345", user2.PhoneNumber);
            Assert.AreEqual(false, user2.PhoneNumberConfirmed);
        }

        [TestMethod]
        public void DeleteUser()
        {
            dynamic user = new IdentityUser { UserName = "admin", Email = "admin@example.com", PhoneNumber = "1234567890", PasswordHash = "@#$rfgt$##WE", EmailConfirmed = true, PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0 };
            user = userTable.Insert(user);
            userTable.Delete(where: "UserName = 'admin'");
            int count = userTable.All("UserName = 'admin'").ToList().Count;
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void FindUserByUserName()
        {
            dynamic user = new IdentityUser { UserName = "admin", Email = "admin@example.com", PhoneNumber = "1234567890", PasswordHash = "@#$rfgt$##WE", EmailConfirmed = true, PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0 };
            user = userTable.Insert(user);
            IdentityUser iUser = userTable.GetUserByName("admin");
            Assert.AreNotEqual(null, iUser);
        }

        [TestCleanup]
        public void Cleanup()
        {
            userTable.Delete();
            database.Dispose();
        }
    }
}