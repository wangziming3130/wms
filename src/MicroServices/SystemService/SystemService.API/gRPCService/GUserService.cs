using Core.Grpc.Protos;
using Core.Utility;
using Grpc.Core;
using SystemService.Service;

namespace SystemService.API
{
    //[AllowAnonymous]
    public class GUserService : GUser.GUserBase
    {
        private static readonly SiasunLogger logger = SiasunLogger.GetInstance(typeof(GUserService));
        private readonly ServiceFactory _sf;


        public GUserService(
            ServiceFactory sf
            )
        {
            _sf = sf;
        }
        public override async Task<GHandleGetUserNameByIdMsg> GetUserNameById(GHandleGetUserNameByIdParamMsg request, ServerCallContext context)
        {
            Guid userId = new Guid(request.UserId);
            var user = await _sf.UserService.GetUserById(userId);
            GHandleGetUserNameByIdMsg result = new GHandleGetUserNameByIdMsg() { UserName = user.USER_NAME };
            return result;
        }

    }
}
