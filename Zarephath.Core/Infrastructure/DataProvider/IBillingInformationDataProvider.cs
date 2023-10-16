﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IBillingInformationDataProvider
    {
        ServiceResponse AddBillingDetail(BillingInformationModel addBillingInformationModel);
        ServiceResponse GetBillingDetail(BillingInformationModel addBillingInformationModel);
    }
}