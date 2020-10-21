using SecureWebApi.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecureWebApi.Shared.Services
{
    public interface IUserService
    {
        List<UserModel> GetUsers();

        UserModel GetUserById(Guid id);

        UserModel GetUserByActivationCode(string code);

        string CreateUser(UserModel u);

        bool UpdateUser(UserModel u);

        bool DeleteUser(Guid id);

        bool ActivateUser(Guid id);

        string Login(string username, string password);

        string RefreshToken(UserModel u);
    }

    public class MemoryUserService : IUserService
    {
        private readonly ConfigurationService config;

        private static List<UserModel> users = new List<UserModel>
        {
            new UserModel { Id = new Guid("474d5198-4467-4e48-93a0-77a57d835fca"), Username = "user1", Password = "24c9e15e52afc47c225b757e7bee1f9d".ToUpper(), Roles = new List<UserModel.Role>{ UserModel.Role.FreeUser } },
            new UserModel { Id = new Guid("b6342620-5a61-460d-9b96-753f8cabb143"), ActivationCode = "asd", Username = "user2", Password = "7e58d63b60197ceb55a1c487989a3720".ToUpper(), Roles = new List<UserModel.Role>{ UserModel.Role.FreeUser, UserModel.Role.Admin } },
        };

        public MemoryUserService(ConfigurationService config)
        {
            this.config = config;
        }

        public string CreateUser(UserModel u)
        {
            u.Id = Guid.NewGuid();
            u.Username = $"{u.Username}.{Helpers.Util.Math.GenerateNumberBetween(10000, 99999)}";
            u.Password = Helpers.Crypto.MD5.CreateMD5(u.Password);
            u.RefreshToken = Helpers.Util.Util.GenerateRandomString(32);
            u.ActivationCode = Helpers.Util.Util.GenerateRandomString(32);
            u.ReferralCode = Helpers.Util.Util.GenerateRandomString(10).ToUpper();       // Should really have a guaranteed unique referral code

            users.Add(u);

            var accessToken = Helpers.Authentication.TokenHelper.CreateToken(u, config.jwtKey, config.jwtIssuer, config.jwtAudience);

            return $"{accessToken}:{u.RefreshToken}";
        }

        public bool DeleteUser(Guid id)
        {
            users = users.Where(x => x.Id != id).ToList();

            return true;
        }

        public UserModel GetUserById(Guid id)
        {
            return users.Single(x => x.Id == id);
        }

        public List<UserModel> GetUsers()
        {
            return users;
        }

        public string Login(string username, string password)
        {
            var hashedPassword = Helpers.Crypto.MD5.CreateMD5(password);

            var user = users.Where(x => x.Username == username && x.Password == hashedPassword).FirstOrDefault();
            user.RefreshToken = Helpers.Util.Util.GenerateRandomString(32);

            if (user == null)
                return "";

            var accessToken = Helpers.Authentication.TokenHelper.CreateToken(user, config.jwtKey, config.jwtIssuer, config.jwtAudience);

            return $"{accessToken}:{user.RefreshToken}";
        }

        public bool UpdateUser(UserModel u)
        {
            var user = users.Where(x => x.Id == u.Id).FirstOrDefault();
            user = u;

            return true;
        }

        public string RefreshToken(UserModel u)
        {
            u.RefreshToken = Helpers.Util.Util.GenerateRandomString(32);

            var accessToken = Helpers.Authentication.TokenHelper.CreateToken(u, config.jwtKey, config.jwtIssuer, config.jwtAudience);

            return $"{accessToken}:{u.RefreshToken}";
        }

        public UserModel GetUserByActivationCode(string code)
        {
            return GetUsers().Where(x => x.ActivationCode != null && x.ActivationCode.Equals(code)).FirstOrDefault();
        }

        public bool ActivateUser(Guid id)
        {
            var user = GetUserById(id);
            user.ActivationCode = null;
            
            return true;
        }
    }
}
