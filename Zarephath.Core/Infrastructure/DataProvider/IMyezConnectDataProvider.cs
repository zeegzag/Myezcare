using System;
using Zarephath.Core.Models;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface IMyezConnectDataProvider
    {
        ServiceResponse ViewReleaseNote(long ReleaseNoteID);
        ServiceResponse UpdateSiteCache(string id);
    }
}
