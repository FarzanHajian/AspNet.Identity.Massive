using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using static Slapper.AutoMapper;

namespace AspNet.Identity.Massive
{
    /// <summary>
    /// Class that implements the key ASP.NET Identity role store iterfaces
    /// </summary>
    public class RoleStore<TRole> : IQueryableRoleStore<TRole, int>
        where TRole : IdentityRole
    {
        private RoleTable roleTable;
        public DbManager Database { get; private set; }

        public IQueryable<TRole> Roles => roleTable.All().Select(x => MapDynamic<TRole>(x)).Cast<TRole>().AsQueryable();

        /// <summary>
        /// Default constructor that initializes a new database
        /// instance using the Default Connection string
        /// </summary>
        public RoleStore()
        {
            new RoleStore<TRole>(new DbManager(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString));
        }

        /// <summary>
        /// Constructor that takes a dbmanager as argument
        /// </summary>
        /// <param name="database"></param>
        public RoleStore(DbManager database)
        {
            Database = database;
            roleTable = database.GetRoleTable();
        }

        public Task CreateAsync(TRole role)
        {
            if (role == null) throw new ArgumentNullException("role");
            roleTable.Insert(role);
            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null) throw new ArgumentNullException("user");
            roleTable.Delete(role.Id);
            return Task.FromResult<Object>(null);
        }

        public Task<TRole> FindByIdAsync(int roleId)
        {
            TRole result = roleTable.GetRoleById(roleId) as TRole;
            return Task.FromResult(result);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            TRole result = roleTable.GetRoleByName(roleName) as TRole;
            return Task.FromResult(result);
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null) throw new ArgumentNullException("user");
            roleTable.Update(role);
            return Task.FromResult<Object>(null);
        }

        public void Dispose()
        {
            if (Database != null) Database = null;
        }
    }
}