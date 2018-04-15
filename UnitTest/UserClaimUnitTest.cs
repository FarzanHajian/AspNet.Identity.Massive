using AspNet.Identity.Massive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Security.Claims;

namespace UnitTest
{
    [TestClass]
    public class UserClaimUnitTest
    {
        private DbManager database;
        private UserClaimTable userClaimTable;
        private UserTable<IdentityUser> userTable;

        [TestInitialize]
        public void Initialize()
        {
            database = new DbManager("DefaultConnection");
            userClaimTable = database.GetUserClaimTable();
            userTable = database.GetUserTable<IdentityUser>();
        }

        [TestMethod]
        public void InsertUserClaim()
        {
            IdentityUser user = new IdentityUser { UserName = "user1", Email = "user3@example.com", PhoneNumber = "1234567890", PasswordHash = "@#$rfgt$##WE", EmailConfirmed = true, PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0 };
            user = userTable.Insert(user);
            object claim = new { UserId = user.Id, ClaimType = "Claim1", ClaimValue = "Value1" };
            claim = userClaimTable.Insert(claim);
            int count = userClaimTable.All("ClaimType= 'Claim1'").ToList().Count;
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void UpdateUserClaim()
        {
            IdentityUser user = new IdentityUser { UserName = "user2", Email = "user2@example.com", PhoneNumber = "1234567890", PasswordHash = "@#$rfgt$##WE", EmailConfirmed = true, PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0 };
            user = userTable.Insert(user);
            dynamic claim = new { UserId = user.Id, ClaimType = "Claim1", ClaimValue = "Value1" };
            claim = userClaimTable.Insert(claim);
            claim.ClaimValue = "Value2";
            userClaimTable.Update(claim);
            string value = userClaimTable.Single(claim.Id).ClaimValue;
            Assert.AreEqual("Value2", value);
        }

        [TestMethod]
        public void DeleteUserClaim()
        {
            IdentityUser user = new IdentityUser { UserName = "user3", Email = "user3@example.com", PhoneNumber = "1234567890", PasswordHash = "@#$rfgt$##WE", EmailConfirmed = true, PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0 };
            user = userTable.Insert(user);
            object claim = new { UserId = user.Id, ClaimType = "Claim1", ClaimValue = "Value1" };
            claim = userClaimTable.Insert(claim);
            userClaimTable.Delete(claim);
            userClaimTable.Delete(where: "ClaimType = 'Claim1'");
            int count = userClaimTable.All("ClaimType = 'Claim1'").ToList().Count;
            Assert.AreEqual(0, count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            userClaimTable.Delete();
            userTable.Delete();
            database.Dispose();
        }
    }
}