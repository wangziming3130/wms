using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public enum EventID
    {
        None = 0,

        [Description("Upload file error")]
        UploadFileError = 56021,

        [Description("The file type is not supported")]
        UnsupportFile = 56022
    }
}
