namespace DrinkWater.LogReg
{
    /// <summary>
    /// Class for passing a user id through app.
    /// </summary>
    public class SessionUser
    {
        private long userId;
        private string username;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionUser"/> class.
        /// </summary>
        public SessionUser()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionUser"/> class.
        /// </summary>
        /// <param name="userId">Id of logged in user.</param>
        /// <param name="username"> Name of logged ib User.</param>
        public SessionUser(long userId, string username)
        {
            UserId = userId;
            Username = username;
        }

        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public long UserId { get => userId; set => userId = value; }

        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public string Username { get => username; set => username = value; }
    }
}
