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


        private IDBRepository _dbRepository;
        private IUserService _userService;

        IConfiguration _configuration;
        public UserController(
            IHttpContextAccessor accessor,
            IDBRepository repository,
            IUserService userService,
        IConfiguration configuration) : base(accessor)
        {
            _userService = userService;
            _dbRepository = repository;

        }
        private static readonly SiasunLogger Logger = SiasunLogger.GetInstance(typeof(UserController));



        [HttpGet("User")]
        public ActionResult Index(Guid Id)
        {
            var res = _userService.DeleteUserById(Id, Id);

            return Json(res);
        }
    }


}
