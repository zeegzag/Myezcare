using System;
using System.Collections.Generic;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class SetNoteSentenceListPage
    {
        public SetNoteSentenceListPage()
        {
            SearchNoteSentenceListPage = new SearchNoteSentenceListPage();
            DeleteFilter = new List<NameValueData>();
        }
        public SearchNoteSentenceListPage SearchNoteSentenceListPage { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class SearchNoteSentenceListPage
    {
        public string NoteSentenceTitle { get; set; }
        public string NoteSentenceDetails { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }

    }


}
