using AspNet.Identity.Massive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class RoleUnitTest
    {
        private DbManager database;
        private RoleTable roleTable;

        [TestInitialize]
        public void Initialize()
        {
            database = new DbManager("DefaultConnection");
            roleTable = database.GetRoleTable();
        }

        [TestMethod]
        public void InsertRole()
        {
            dynamic role = new IdentityRole { Name = "Admin" };
            role = roleTable.Insert(role);
            int count = roleTable.All("Name = 'Admin'").ToList().Count;
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void UpdateRole()
        {
            dynamic role = new IdentityRole { Name = "Admins" };
            role = roleTable.Insert(role);
            role.Name = "Administrators";
            roleTable.Update(role);
            string name = roleTable.Single(role.Id).Name;
            Assert.AreEqual("Administrators", name);
        }

        [TestMethod]
        public void DeleteRole()
        {
            dynamic role = new IdentityRole { Name = "PowerUsers" };
            role = roleTable.Insert(role);
            roleTable.Delete(where: "Name = 'PowerUsers'");
            int count = roleTable.All("Name = 'PowerUsers'").ToList().Count;
            Assert.AreEqual(0, count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            roleTable.Delete();
            database.Dispose();
        }
    }
}