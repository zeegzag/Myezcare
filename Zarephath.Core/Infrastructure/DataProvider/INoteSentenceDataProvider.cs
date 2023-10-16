using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface INoteSentenceDataProvider
    {

        #region Add DX Code

        ServiceResponse SetAddNoteSentencePage(long noteSentenceId);
        ServiceResponse AddNoteSentence(NoteSentence noteSentence, long loggedInId);

        #endregion Add DX Code

        #region DX Code List

        ServiceResponse SetAddNoteSentenceListPage();
        ServiceResponse GetNoteSentenceList(SearchNoteSentenceListPage searchNoteSentenceListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection);
        ServiceResponse DeleteNoteSentence(SearchNoteSentenceListPage searchNoteSentenceListPage, int pageIndex, int pageSize, string sortIndex, string sortDirection, long loggedInUserId);

        #endregion DX Code List

      


    }
}
