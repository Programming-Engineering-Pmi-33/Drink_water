using System;
using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Text;

namespace DrinkWater.LogReg
{
    public class SessionUser
    {
     
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
