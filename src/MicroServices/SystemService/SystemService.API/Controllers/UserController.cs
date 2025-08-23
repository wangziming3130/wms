using Core.Service;
using Core.Utility;
using Core.Web;
using Microsoft.AspNetCore.Mvc;
using SystemService.Service;

namespace SystemService.API
{
    [ApiVersion("1")]
    public partial class UserController : APIBaseController
    {
        private static readonly SiasunLogger Logger = SiasunLogger.GetInstance(typeof(UserController));
        private ServiceFactory _sf;

        IConfiguration _configuration;
        public UserController(
            IHttpContextAccessor accessor,
            ServiceFactory sf,
        IConfiguration configuration) : base(accessor)
        {
            _sf = sf;

        }
        [HttpPost("user")]
        public ActionResult Delete(Guid Id)
        {
            var res = _sf.UserService.DeleteUserById(Id).Result;

            return Json(res);
        }
    }


}
