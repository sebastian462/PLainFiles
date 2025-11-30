using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainFiles.Core
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }

        public User(string username, string password, bool active)
        {
            Username = username;
            Password = password;
            Active = active;
        }
    }
}

