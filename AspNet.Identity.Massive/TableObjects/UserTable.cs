using static Slapper.AutoMapper;

namespace AspNet.Identity.Massive
{
    /// <summary>
    /// Class that represents the Users table in the Database
    /// </summary>
    public class UserTable<TUser> : TableBase
        where TUser : IdentityUser
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="connStringName"></param>
        public UserTable(string connStringName) : base(connStringName, "Users")
        {
        }

        /// <summary>
        /// Returns the User's name given a User id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserName(int userId) => Single(userId, "UserName")?.Name;

        /// <summary>
        /// Returns a User ID given a User name
        /// </summary>
        /// <param name="userName">The User's name</param>
        /// <returns></returns>
        public int GetUserId(string userName) => Single(where: "where UserName=@0", args: new object[] { userName })?.Id;

        /// <summary>
        /// Returns an TUser given the User's id
        /// </summary>
        /// <param name="userId">The User's id</param>
        /// <returns></returns>
        public TUser GetUserById(int userId)
        {
            dynamic user = Single(userId);
            return (user == null ? null : MapDynamic<TUser>(user, keepCache: false));
        }

        /// <summary>
        /// Returns an TUser given the User name
        /// /// </summary>
        /// <param name="userName">User's name</param>
        /// <returns></returns>
        public TUser GetUserByName(string userName)
        {
            dynamic user = Single(where: "where UserName = @0", args: new object[] { userName });
            return (user == null ? null : MapDynamic<TUser>(user, keepCache: false));
        }

        /// <summary>
        /// Returns an TUser given the User; Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public TUser GetUserByEmail(string email)
        {
            dynamic user = Single(where: "where Email = @0", args: new object[] { email });
            return (user == null ? null : MapDynamic<TUser>(user, keepCache: false));
        }

        /// <summary>
        /// Return the User's password hash
        /// </summary>
        /// <param name="userId">The User's id</param>
        /// <returns></returns>
        public string GetPasswordHash(int userId) => Single(userId, "PasswordHash")?.PasswordHash;

        /// <summary>
        /// Sets the User's password hash
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public void SetPasswordHash(int userId, string passwordHash)
        {
            Update(new { PasswordHash = passwordHash }, userId);
        }

        /// <summary>
        /// Returns the User's security stamp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSecurityStamp(int userId) => Single(userId, "SecurityStamp")?.SecurityStamp;

        /// <summary>
        /// Updates a User in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public void Update(TUser user) => Update(user, user.Id);
    }
}