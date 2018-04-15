using Massive;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace AspNet.Identity.Massive
{
    /// <summary>
    /// Class that represents the UserLogins table in the Database
    /// </summary>
    public class UserLoginTable : TableBase
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="connStringName"></param>
        public UserLoginTable(string connStringName) : base(connStringName, "UserLogins")
        {
        }

        /// <summary>
        /// Deletes a login from a user in the UserLogins table
        /// </summary>
        /// <param name="user">User to have login deleted</param>
        /// <param name="login">Login to be deleted from user</param>
        /// <returns></returns>
        public void Delete(IdentityUser user, UserLoginInfo login)
        {
            Delete(
                null,
                "where UserId = @0 and LoginProvider = @1 and ProviderKey = @2",
                user.Id,
                login.LoginProvider,
                login.ProviderKey
            );
        }

        /// <summary>
        /// Deletes all Logins from a user in the UserLogins table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public void Delete(int userId)
        {
            using (DbConnection conn = OpenConnection())
            {
                Execute(CreateDeleteCommand(where: "UserId=@0", args: new object[] { userId }));
            }
        }

        /// <summary>
        /// Inserts a new login in the UserLogins table
        /// </summary>
        /// <param name="user">User to have new login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public void Insert(IdentityUser user, UserLoginInfo login)
        {
            Insert(new { LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey, UserId = user.Id });
        }

        /// <summary>
        /// Return a userId given a user's login
        /// </summary>
        /// <param name="userLogin">The user's login info</param>
        /// <returns></returns>
        public int FindUserIdByLogin(UserLoginInfo userLogin)
        {
            dynamic user = Single(
                where: "LoginProvider = @0 and ProviderKey = @1",
                args: new object[] { userLogin.LoginProvider, userLogin.ProviderKey }
            );
            return (user?.UserId) ?? 0;
        }

        /// <summary>
        /// Returns a list of user's logins
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<UserLoginInfo> FindByUserId(int userId)
        {
            return All(
                where: "UserId = @0",
                columns: "LoginProvider, ProviderKey",
                args: new object[] { userId }
            ).Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey))
            .ToList();
        }

        public override dynamic Insert(object o)
        {
            var oAsExpando = o.ToExpando();
            if (!IsValid(oAsExpando))
            {
                throw new InvalidOperationException("Can't insert: " + string.Join("; ", Errors.ToArray()));
            }
            if (BeforeSave(oAsExpando))
            {
                using (var conn = OpenConnection())
                {
                    using (DbCommand cmd = CreateInsertCommand(oAsExpando))
                    {
                        cmd.Connection = conn;
                        cmd.ExecuteNonQuery();
                    }
                    Inserted(oAsExpando);
                    conn.Close();
                }
                return oAsExpando;
            }
            return null;
        }
    }
}