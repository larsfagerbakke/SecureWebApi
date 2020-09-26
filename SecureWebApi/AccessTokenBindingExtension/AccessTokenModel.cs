using System;
using System.Collections.Generic;

namespace SecureWebApi
{
    public class AccessTokenModel
    {
        public enum State
        {
            Valid,
            Invalid,
        }

        public enum Role
        {
            FreeUser = 1,
            User = 2,
            Admin = 3,
        }

        public Guid UserId { get; set; }
        public List<Role> Roles { get; set; }
        public State TokenState { get; set; }
    }
}