using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SecureWebApi.Models;
using SecureWebApi.Shared.Services;
using System.IO;
using System.Threading.Tasks;

namespace SecureWebApi
{
    public class UserApi
    {
        private IUserService userService;

        public UserApi(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Returns access and refresh tokens after validating user
        /// </summary>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/api/v1/Login</url>
        /// <param name="req">Http request</param>
        /// <param name="log">Azure functions log object</param>
        /// <returns>Returns access and refresh tokens</returns>
        /// <response code="200">Returns access and refresh tokens</response>
        [FunctionName("Login")]
        public async Task<IActionResult> Login([HttpTrigger(AuthorizationLevel.Function, "Post", Route = "v1/login")] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var login = JsonConvert.DeserializeObject<LoginModel>(requestBody);

            var tokens = userService.Login(login.Username, login.Password);

            if (tokens == null) return new BadRequestResult();

            return new OkObjectResult(tokens);
        }

        /// <summary>
        /// Returns the requester user object
        /// </summary>
        /// <verb>GET</verb>
        /// <url>http://localhost:7071/api/v1/User</url>
        /// <param name="req">Http request</param>
        /// <param name="token">Accesstoken</param>
        /// <param name="log">Azure functions log object</param>
        /// <returns>Returns the requester user object</returns>
        /// <response code="200">Returns the requester user object</response>
        /// <response code="401">UnauthorizedResult</response> 
        [FunctionName("User")]
        public async Task<IActionResult> GetUser([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/user")] HttpRequest req, [AccessTokenBinding] AccessTokenModel token, ILogger log)
        {
            if (token.TokenState == AccessTokenModel.State.Invalid) return new UnauthorizedResult();

            var user = userService.GetUserById(token.UserId);
            user.Clean();

            return new OkObjectResult(user);
        }

        /// <summary>
        /// Returns access and refresh tokens after adding user
        /// </summary>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/api/v1/Register</url>
        /// <param name="req">Http request</param>
        /// <param name="log">Azure functions log object</param>
        /// <param name="body">[FromBody]RegisterModel object</param>
        /// <returns>Returns access and refresh tokens</returns>
        /// <response code="200">Returns access and refresh tokens</response>
        [FunctionName("Register")]
        public async Task<IActionResult> RegisterUser([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/register")] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var requestData = JsonConvert.DeserializeObject<RegisterModel>(requestBody);

            var tokens = userService.CreateUser(new Shared.Models.UserModel
            {
                Username = requestData.Username,
                Password = requestData.Password,
                Email = requestData.Email,
                Roles = new System.Collections.Generic.List<Shared.Models.UserModel.Role> { Shared.Models.UserModel.Role.FreeUser }
            });

            return new OkObjectResult(tokens);
        }

        /// <summary>
        /// Returns access token and refreshed refresh token
        /// </summary>
        /// <verb>POST</verb>
        /// <url>http://localhost:7071/api/v1/RefreshToken</url>
        /// <param name="req">Http request</param>
        /// <param name="token">Accesstoken</param>
        /// <param name="log">Azure functions log object</param>
        /// <returns>Returns access token and refreshed refresh token</returns>
        /// <response code="200">Returns access token and refreshed refresh token</response>
        [FunctionName("RefreshToken")]
        public async Task<IActionResult> RefreshToken([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/refreshtoken")] HttpRequest req, [AccessTokenBinding] AccessTokenModel token, ILogger log)
        {
            if (token.TokenState == AccessTokenModel.State.Invalid) return new UnauthorizedResult();

            var user = userService.GetUserById(token.UserId);

            return new OkObjectResult(userService.RefreshToken(user));
        }
    }
}
