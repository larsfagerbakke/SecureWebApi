using System;
using System.Collections.Generic;

namespace SecureWebApi.Shared.Models
{
    public class UserModel
    {
        public enum Role
        {
            FreeUser = 1,
            User = 2,
            Admin = 3
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Role> Roles { get; set; }

        public void Clean()
        {
            this.Password = string.Empty;
        }
    }
}
