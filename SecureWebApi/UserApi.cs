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

        [FunctionName("Login")]
        public async Task<IActionResult> PostLogin([HttpTrigger(AuthorizationLevel.Function, "Post", Route = "v1/login")] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var login = JsonConvert.DeserializeObject<LoginModel>(requestBody);

            var accessToken = userService.Login(login.Username, login.Password);

            if (accessToken == null) return new BadRequestResult();

            return new OkObjectResult(accessToken);
        }

        [FunctionName("User")]
        public async Task<IActionResult> GetUser([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/user")] HttpRequest req, [AccessTokenBinding] AccessTokenModel token, ILogger log)
        {
            if (token.TokenState == AccessTokenModel.State.Invalid) return new UnauthorizedResult();

            var user = userService.GetUserById(token.UserId);
            user.Clean();

            return new OkObjectResult(user);
        }
    }
}
