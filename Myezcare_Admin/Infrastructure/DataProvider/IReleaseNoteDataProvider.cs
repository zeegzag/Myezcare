using System;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;

namespace Myezcare_Admin.Infrastructure.DataProvider
{
    public interface IReleaseNoteDataProvider
    {
        ServiceResponse SetAddReleaseNotePage(long ReleaseNoteID);
        ServiceResponse SaveReleaseNote(ReleaseNote releaseNote, long loggedInUserId);
        ServiceResponse SetReleaseNoteListPage();
        ServiceResponse GetReleaseNoteList(SearchReleaseNoteListPage searchReleaseNoteListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteReleaseNote(SearchReleaseNoteListPage searchReleaseNoteListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserID);
        ServiceResponse ViewReleaseNote(long ReleaseNoteID);

    }
}
