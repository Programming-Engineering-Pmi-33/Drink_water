namespace DrinkWater.Services
{
    using System.Linq;
    using System.Runtime.InteropServices.ComTypes;

    /// <summary>
    /// Announces UserService сlass.
    /// </summary>
    public class UsersService
    {
        private readonly dfkg9ojh16b4rdContext db = null;

        private static UsersService instance = null;

        private UsersService()
        {
            db = new dfkg9ojh16b4rdContext();
        }

        /// <summary>
        ///  Gets only one instance of user service.
        /// </summary>
        public static UsersService GetService
        {
            get
            {
                if (instance == null)
                {
                    instance = new UsersService();
                }

                return instance;
            }
        }

        /// <summary>
        /// Checks if username is in database.
        /// </summary>
        /// <param name="username">username value.</param>
        /// <returns>bool value.</returns>
        public bool UsernameExists(string username)
        {
            var resultName = (from data in db.Users
                              where data.Username == username
                              select data.Username).ToList();

            return resultName.Count > 0;
        }

        /// <summary>
        /// Checks if email is in database.
        /// </summary>
        /// <param name="email">email value.</param>
        /// <returns>bool value.</returns>
        public bool EmailExists(string email)
        {
            var resultEmail = (from data in db.Users
                               where data.Email == email
                               select data.Email).ToList();

            return resultEmail.Count > 0;
        }

        /// <summary>
        /// Registers user.
        /// </summary>
        /// <param name="user">user instance.</param>
        public void RegisterUser(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        /// <summary>
        /// Deletes user from database.
        /// </summary>
        /// <param name="user">user instance.</param>
        public void DeleteUser(User user)
        {
            db.Users.Remove(user);
            db.SaveChanges();
        }

        /// <summary>
        /// Gets user salt from database.
        /// </summary>
        /// <param name="username"> username value.</param>
        /// <returns>salt or 0 if username is not found.</returns>
        public long GetUserSalt(string username)
        {
            var salt = (from data in db.Users
                        where data.Username == username
                        select data.Salt).FirstOrDefault();

            return salt;
        }

        /// <summary>
        /// Gets user id from database.
        /// </summary>
        /// <param name="username">username value.</param>
        /// <param name="password">password value.</param>
        /// <param name="salt">salt value.</param>
        /// <returns>id or 0 if username is not found.</returns>
        public int GetUserId(string username, string password, long salt)
        {
            string hashedPassword = EncryptionService.ComputeSaltedHash(password, salt);

            var userId = (from data in db.Users
                          where data.Username != null && data.Username == username && data.Password == hashedPassword
                          select data.UserId).FirstOrDefault();

            return userId;
        }
    }
}