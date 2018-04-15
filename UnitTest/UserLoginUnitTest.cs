using AspNet.Identity.Massive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class UserLoginUnitTest
    {
        private DbManager database;
        private UserLoginTable userLoginTable;
        private UserTable<IdentityUser> userTable;

        [TestInitialize]
        public void Initialize()
        {
            database = new DbManager("DefaultConnection");
            userLoginTable = database.GetUserLoginTable();
            userTable = database.GetUserTable<IdentityUser>();
        }

        [TestMethod]
        public void InsertUserLogin()
        {
            IdentityUser user = new IdentityUser { UserName = "user1", Email = "user3@example.com", PhoneNumber = "1234567890", PasswordHash = "@#$rfgt$##WE", EmailConfirmed = true, PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0 };
            user = userTable.Insert(user);
            object login = new { LoginProvider = "Facebook", Providerkey = "FRtg$56466#@#$#$%", UserId = user.Id };
            login = userLoginTable.Insert(login);
            int count = userLoginTable.All("LoginProvider= 'Facebook'").ToList().Count;
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void UpdateUserLogin()
        {
            IdentityUser user = new IdentityUser { UserName = "user2", Email = "user2@example.com", PhoneNumber = "1234567890", PasswordHash = "@#$rfgt$##WE", EmailConfirmed = true, PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0 };
            user = userTable.Insert(user);
            dynamic login = new { LoginProvider = "Facebook", Providerkey = "FRtg$56466#@#$#$%", UserId = user.Id };
            login = userLoginTable.Insert(login);
            login.LoginProvider = "Google";
            userLoginTable.Update(login);
            string value = userLoginTable.Single(where: $"UserId={user.Id}").LoginProvider;
            Assert.AreEqual("Google", value);
        }

        [TestMethod]
        public void DeleteUserLogin()
        {
            IdentityUser user = new IdentityUser { UserName = "user3", Email = "user3@example.com", PhoneNumber = "1234567890", PasswordHash = "@#$rfgt$##WE", EmailConfirmed = true, PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0 };
            user = userTable.Insert(user);
            object login = new { LoginProvider = "Facebook", Providerkey = "FRtg$56466#@#$#$%", UserId = user.Id };
            login = userLoginTable.Insert(login);
            userLoginTable.Delete(where: "LoginProvider = 'Facebook'");
            int count = userLoginTable.All("LoginProvider = 'Facebook'").ToList().Count;
            Assert.AreEqual(0, count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            userLoginTable.Delete();
            userTable.Delete();
            database.Dispose();
        }
    }
}