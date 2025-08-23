using Core.Service;
using Core.Utility;
using Core.Web;
using Microsoft.AspNetCore.Mvc;
using WareHouseService.Service;
using WareHouseService.Service;

namespace WareHouseService.API
{
    [ApiVersion("1")]
    public partial class WareHouseController : APIBaseController
    {
        private ServiceFactory _sf;

        IConfiguration _configuration;
        public WareHouseController(
            IHttpContextAccessor accessor,
            ServiceFactory sf,
        IConfiguration configuration) : base(accessor)
        {
            _sf = sf;

        }
        private static readonly SiasunLogger Logger = SiasunLogger.GetInstance(typeof(WareHouseController));

        [HttpPost("warehouse")]
        public async Task<ActionResult> Add()
        {
            var res = await _sf.WHService.AddWareHouseAndArea();

            return Json(res);
        }
        [HttpGet("warehouse")]
        public async Task<ActionResult> Get(Guid id)
        {
            var res = await _sf.WHService.GetAreaById(id);

            return Json(res);
        }
    }


}
