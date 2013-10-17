using System;
using System.Data.Entity;
using System.Linq;

namespace PittyLove.Model
{
    public sealed class UserRepository : EfRepository<User>, IUserRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository{T}"/> class.
        /// </summary>
        public UserRepository(DbContext context) : base(context)
        {
          
        }

        #endregion
     

        public User GetByCredentials(string username, string password)
        {
            return Set.FirstOrDefault(item 
                => item.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase) 
                && !string.IsNullOrEmpty(password));
        }
    }
}
