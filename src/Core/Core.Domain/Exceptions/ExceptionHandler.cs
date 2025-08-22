using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{

    public enum ExceptionMessage
    {
        [Description("Sorry, you don't have the permission to this site.")]
        CF_NoAuthorize_NoPermission_Message = 1,
        [Description("Sorry, currently you are in simulation mode and limited to read only access.")]
        CF_NoAuthorize_Simulation_Message,
        [Description("Sorry, currently the module is a draft module, please check later.")]
        CF_NoAuthorize_DraftModule_Message
    }
}
