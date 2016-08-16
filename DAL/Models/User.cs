using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public List<Task> Tasks { get; set; }

        public List<UserToken> Tokens { get; set; }
    }
}