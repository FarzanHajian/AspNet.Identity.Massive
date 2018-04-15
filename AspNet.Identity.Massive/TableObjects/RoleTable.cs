namespace AspNet.Identity.Massive
{
    /// <summary>
    /// Class that represents the Role table in the Database
    /// </summary>
    public class RoleTable : TableBase
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="connStringName"></param>
        public RoleTable(string connStringName) : base(connStringName, "Roles")
        {
        }

        /// <summary>
        /// Returns a role name given the roleId
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns>Role name</returns>
        public string GetRoleName(int roleId) => Single(roleId, columns: "Name")?.Name;

        /// <summary>
        /// Returns the role Id given a role name
        /// </summary>
        /// <param name="roleName">Role's name</param>
        /// <returns>Role's Id</returns>
        public int GetRoleId(string roleName)
        {
            var role = Single(where: "where Name = @0", args: new object[] { roleName });
            return (role?.Id) ?? 0;
        }

        /// <summary>
        /// Gets the IdentityRole given the role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IdentityRole GetRoleById(int roleId)
        {
            var roleName = GetRoleName(roleId);
            return (roleName != null ? new IdentityRole(roleName, roleId) : null);
        }

        /// <summary>
        /// Gets the IdentityRole given the role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IdentityRole GetRoleByName(string roleName)
        {
            var roleId = GetRoleId(roleName);
            return (roleId > 0 ? new IdentityRole(roleName, roleId) : null);
        }

        /// <summary>
        /// Updates the given role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public void Update(IdentityRole role) => Update(role, role.Id);
    }
}