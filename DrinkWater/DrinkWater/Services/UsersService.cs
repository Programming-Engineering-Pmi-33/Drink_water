namespace DrinkWater.Services
{
    using System.Linq;

    public class UsersService
    {
        private readonly dfkg9ojh16b4rdContext db = null;

        private static UsersService instance = null;

        // constructor
        private UsersService()
        {
            db = new dfkg9ojh16b4rdContext();
        }

        // getting only one instance of user service
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

        // checking if username is in database
        public bool UsernameExists(string username)
        {
            var resultName = (from data in db.Users
                              where data.Username == username
                              select data.Username).ToList();

            return resultName.Count > 0;
        }

        // checking if email is in database
        public bool EmailExists(string email)
        {
            var resultEmail = (from data in db.Users
                               where data.Email == email
                               select data.Email).ToList();

            return resultEmail.Count > 0;
        }

        // registration of user
        public void RegisterUser(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            db.Users.Remove(user);
            db.SaveChanges();
        }

        // getting user salt from database
        public long GetUserSalt(string username)
        {
            var salt = (from data in db.Users
                        where data.Username != null && data.Username == username
                        select data.Salt).FirstOrDefault();
            if (salt == null)
            {
                return 0;
            }

            return (long)salt;
        }

        // getting user id from database
        public int GetUserId(string username, string password,  long salt)
        {
            string hashedPassword = EncryptionService.ComputeSaltedHash(password, salt);

            var userId = (from data in db.Users
                          where data.Username != null && data.Username == username && data.Password == hashedPassword
                          select data.UserId).FirstOrDefault();

            return userId;
        }
    }
}
