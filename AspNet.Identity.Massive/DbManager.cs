using System;
using System.Configuration;

namespace AspNet.Identity.Massive
{
    /// <summary>
    /// The database context object
    /// </summary>
    public class DbManager : IDisposable
    {
        private string connStringName;
        private object userTable;
        private RoleTable roleTable;
        private UserRoleTable userRoleTable;
        private UserClaimTable userClaimTable;
        private UserLoginTable userLoginTable;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="connStringName">The name of the connection string</param>
        public DbManager(string connStringName)
        {
            // Use first?
            this.connStringName = connStringName == "" ? ConfigurationManager.ConnectionStrings[0].Name : connStringName;
        }

        public UserTable<TUser> GetUserTable<TUser>() where TUser : IdentityUser
        {
            if (userTable == null) userTable = new UserTable<TUser>(connStringName);
            return (UserTable<TUser>)userTable;
        }

        public RoleTable GetRoleTable()
        {
            if (roleTable == null) roleTable = new RoleTable(connStringName);
            return roleTable;
        }

        public UserRoleTable GetUserRoleTable()
        {
            if (userRoleTable == null) userRoleTable = new UserRoleTable(connStringName);
            return userRoleTable;
        }

        public UserClaimTable GetUserClaimTable()
        {
            if (userClaimTable == null) userClaimTable = new UserClaimTable(connStringName);
            return userClaimTable;
        }

        public UserLoginTable GetUserLoginTable()
        {
            if (userLoginTable == null) userLoginTable = new UserLoginTable(connStringName);
            return userLoginTable;
        }

        /// <summary>
        ///Disposes the context
        /// </summary>
        public void Dispose()
        {
        }
    }
}