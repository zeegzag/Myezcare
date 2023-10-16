using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Infrastructure.Utility.Fcm
{
    public interface IResponse
    {
        HttpWebResponse HttpWebResponse
        {
            get;
            set;
        }

        WebRequest WebRequest
        {
            get;
            set;
        }
    }
}
