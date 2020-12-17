namespace DrinkWater.LogReg
{
    /// <summary>
    /// Announces SessionUser сlass.
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
        /// <param name="userId">user id value.</param>
        /// <param name="username">username value.</param>
        public SessionUser(long userId, string username)
        {
            UserId = userId;
            Username = username;
        }

        /// <summary>
        /// Gets or sets user id property.
        /// </summary>
        public long UserId { get => userId; set => userId = value; }

        /// <summary>
        /// Gets or sets username property.
        /// </summary>
        public string Username { get => username; set => username = value; }
    }
}
