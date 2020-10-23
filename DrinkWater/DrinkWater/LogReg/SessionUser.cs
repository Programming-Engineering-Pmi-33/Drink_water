using System;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.Security.RightsManagement;
>>>>>>> Misha_branch
using System.Text;

namespace DrinkWater.LogReg
{
<<<<<<< HEAD
    public class SessionUser
    {

=======
   public class SessionUser
    {
        
>>>>>>> Misha_branch
        private long userId;
        private string username;
        public SessionUser() { }
        public SessionUser(long userId, string username)
        {
            UserId = userId;
            Username = username;
        }

        public long UserId { get => userId; set => userId = value; }
        public string Username { get => username; set => username = value; }
    }
}
