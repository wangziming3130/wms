using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{

    public enum AuthType
    {
        Invalid = 0,
        AAD,
        Local,
        Windows,
        Simulate,
        Switch
    }
    public enum AccountMode
    {
        Normal = 0,
        Simulate,
        Switch
    }

    public enum UserType
    {
        Invalid = 0,
        Student,
        Staff
    }
}
