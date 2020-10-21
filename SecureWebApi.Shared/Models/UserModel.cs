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
        public string RefreshToken { get; set; }
        public List<Role> Roles { get; set; }
        public string ActivationCode { get; set; }
        public string ReferralCode { get; set; }

        public override string ToString()
        {
            return $"{this.Username}({this.Email})";
        }

        public void Clean()
        {
            this.Password = string.Empty;
            this.RefreshToken = string.Empty;                                               // This will break in memory implementation of the user service.
        }

        public bool IsActivated() => string.IsNullOrEmpty(ActivationCode);

        
    }
}
