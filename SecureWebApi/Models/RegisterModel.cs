using System;
using System.Collections.Generic;
using System.Text;

namespace SecureWebApi.Models
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
