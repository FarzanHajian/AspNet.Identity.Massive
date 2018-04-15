using System.Security.Claims;

namespace AspNet.Identity.Massive
{
    /// <summary>
    /// Class that represents the UserClaims table in the Database
    /// </summary>
    public class UserClaimTable : TableBase
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="connStringName"></param>
        public UserClaimTable(string connStringName) : base(connStringName, "UserClaims")
        {
        }

        /// <summary>
        /// Returns a ClaimsIdentity instance given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public ClaimsIdentity FindByUserId(int userId)
        {
            ClaimsIdentity claims = new ClaimsIdentity();

            foreach (dynamic c in Query("Select ClaimType, ClaimValue from UserClaims where UserId=@0", userId))
            {
                claims.AddClaim(new Claim(c.ClaimType, c.ClaimValue));
            }

            return claims;
        }

        /// <summary>
        /// Deletes all claims from a user given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public void Delete(int userId) => Delete(where: "where UserId = @userId", args: new object[] { userId });

        /// <summary>
        /// Inserts a new claim in UserClaims table
        /// </summary>
        /// <param name="claim">User's claim to be added</param>
        /// <param name="userId">User's id</param>
        /// <returns></returns>
        public void Insert(Claim claim, int userId)
        {
            Insert(new { ClaimValue = claim.Value, ClaimType = claim.Type, UserId = userId });
        }

        /// <summary>
        /// Deletes a claim from a user
        /// </summary>
        /// <param name="user">The user to have a claim deleted</param>
        /// <param name="claim">A claim to be deleted from user</param>
        /// <returns></returns>
        public void Delete(IdentityUser user, Claim claim)
        {
            Delete(
                null,
                "where UserId = @0 and ClaimValue = @1 and ClaimType = @2",
                 user.Id,
                 claim.Value,
                 claim.Type
            );
        }
    }
}