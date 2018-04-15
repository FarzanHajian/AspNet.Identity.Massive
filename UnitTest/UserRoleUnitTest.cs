using AspNet.Identity.Massive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class UserRoleUnitTest
    {
        private DbManager database;
        private UserRoleTable userRoleTable;
        private RoleTable roleTable;
        private UserTable<IdentityUser> userTable;

        [TestInitialize]
        public void Initialize()
        {
            database = new DbManager("DefaultConnection");
            userRoleTable = database.GetUserRoleTable();
            userTable = database.GetUserTable<IdentityUser>();
            roleTable = database.GetRoleTable();
        }

        [TestMethod]
        public void InsertUserRole()
        {
            IdentityUser user = new IdentityUser { UserName = "user1", Email = "user1@example.com", PhoneNumber = "1234567890", PasswordHash = "@#$rfgt$##WE", EmailConfirmed = true, PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0 };
            user = userTable.Insert(user);
            dynamic role = new { Name = "Admin" };
            role = roleTable.Insert(role);
            object userRole = new { UserId = user.Id, RoleId = role.Id };
            userRole = userRoleTable.Insert(userRole);
            int count = userRoleTable.All($"UserId={user.Id} and RoleId={role.Id}").ToList().Count;
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void UpdateUserRole()
        {
            IdentityUser user = new IdentityUser { UserName = "user1", Email = "user1@example.com", PhoneNumber = "1234567890", PasswordHash = "@#$rfgt$##WE", EmailConfirmed = true, PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0 };
            user = userTable.Insert(user);
            IdentityUser user2 = new IdentityUser { UserName = "user2", Email = "user2@example.com", PhoneNumber = "1234567890", PasswordHash = "@#$rfgt$##WE", EmailConfirmed = true, PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0 };
            user2 = userTable.Insert(user2);
            dynamic role = new { Name = "Admin" };
            role = roleTable.Insert(role);
            dynamic userRole = new { UserId = user.Id, RoleId = role.Id };
            userRole = userRoleTable.Insert(userRole);
            userRole.UserId = user2.Id;
            userRoleTable.Update(userRole);
            int userId = userRoleTable.Single(where: $"RoleId={role.Id}").UserId;
            Assert.AreEqual(user2.Id, userId);
        }

        [TestMethod]
        public void DeleteUserRole()
        {
            IdentityUser user = new IdentityUser { UserName = "user3", Email = "user3@example.com", PhoneNumber = "1234567890", PasswordHash = "@#$rfgt$##WE", EmailConfirmed = true, PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0 };
            user = userTable.Insert(user);
            object role = new { Name = "Admin" };
            role = roleTable.Insert(role);
            userRoleTable.Delete(user.Id);
            int count = userRoleTable.All($"UserId={user.Id}").ToList().Count;
            Assert.AreEqual(0, count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            userRoleTable.Delete();
            userTable.Delete();
            roleTable.Delete();
            database.Dispose();
        }
    }
}