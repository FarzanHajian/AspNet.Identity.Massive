using Massive;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace AspNet.Identity.Massive
{
    /// <summary>
    /// Class that represents the UserRoles table in the Database
    /// </summary>
    public class UserRoleTable : TableBase
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="connStringName"></param>
        public UserRoleTable(string connStringName) : base(connStringName, "UserRoles")
        {
        }

        /// <summary>
        /// Returns a list of user's roles
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<string> FindByUserId(int userId)
        {
            return Query(
                "Select Roles.Name from UserRoles, Roles where UserRoles.UserId=@0 and UserRoles.RoleId=Roles.Id",
                userId
           )
           .Select(x => (x?.Name) as string)
           .ToList();
        }

        /// <summary>
        /// Deletes all roles from a user in the UserRoles table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public void Delete(int userId)
        {
            using (DbConnection conn = OpenConnection())
            {
                Execute(CreateDeleteCommand(where: $"UserId={userId}"));
            }
        }

        /// <summary>
        /// Inserts a new role for a user in the UserRoles table
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="roleId">The Role's id</param>
        /// <returns></returns>
        public void Insert(IdentityUser member, int roleId)
        {
            Insert(new { UserId = member.Id, RoleId = roleId });
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