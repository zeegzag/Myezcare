using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IEmailServiceDataProvider
    {
        #region Email Service
         
        ServiceResponse SendEmail();

        #endregion

    }
}
